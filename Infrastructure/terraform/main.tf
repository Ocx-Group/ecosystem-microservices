# =============================================================================
# Ecosystem Microservices - Infrastructure
# =============================================================================

terraform {
  required_version = ">= 1.5.0"

  required_providers {
    digitalocean = {
      source  = "digitalocean/digitalocean"
      version = "~> 2.40"
    }
    cloudflare = {
      source  = "cloudflare/cloudflare"
      version = "~> 4.0"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "~> 2.35"
    }
    helm = {
      source  = "hashicorp/helm"
      version = "~> 2.17"
    }
  }

  # Estado remoto en DigitalOcean Spaces (S3-compatible)
  # Credenciales via env vars: AWS_ACCESS_KEY_ID / AWS_SECRET_ACCESS_KEY
  # Primera vez: terraform init -migrate-state
  backend "s3" {
    endpoint                    = "https://nyc3.digitaloceanspaces.com"
    bucket                      = "ecosystem-storage"
    key                         = "terraform/prod/terraform.tfstate"
    region                      = "us-east-1"
    skip_credentials_validation = true
    skip_metadata_api_check     = true
    skip_requesting_account_id  = true
    skip_s3_checksum            = true
  }
}

provider "digitalocean" {
  token             = var.do_token
  spaces_access_id  = var.spaces_access_id
  spaces_secret_key = var.spaces_secret_key
}

provider "cloudflare" {
  api_token = var.cloudflare_api_token
}

# NOTA: Los providers kubernetes/helm requieren que el cluster exista primero.
# Aplicar en dos pasos:
#   1. terraform apply -target=module.kubernetes -var-file="environments/prod.tfvars"
#   2. terraform apply -var-file="environments/prod.tfvars"
provider "kubernetes" {
  host  = module.kubernetes.endpoint
  token = module.kubernetes.kube_token
  cluster_ca_certificate = base64decode(
    module.kubernetes.cluster_ca_certificate
  )
}

provider "helm" {
  kubernetes {
    host  = module.kubernetes.endpoint
    token = module.kubernetes.kube_token
    cluster_ca_certificate = base64decode(
      module.kubernetes.cluster_ca_certificate
    )
  }
}

# =============================================================================
# VPC
# =============================================================================

data "digitalocean_vpc" "main" {
  id = var.vpc_uuid
}

# =============================================================================
# Database Module
# =============================================================================

module "database" {
  source = "./modules/database"

  name       = "db-postgresql-ocx-group"
  region     = var.region
  size       = var.db_size
  node_count = var.db_node_count
  version_pg = var.db_version
  vpc_uuid   = data.digitalocean_vpc.main.id

  database_name = "ocx_group"

  tags = var.tags
}

# =============================================================================
# Spaces (Object Storage) Module
# =============================================================================

module "spaces" {
  source = "./modules/spaces"

  name          = var.spaces_bucket_name
  region        = var.region
  acl           = var.spaces_acl
  force_destroy = var.spaces_force_destroy

  cors_allowed_origins = var.spaces_cors_origins
}

# =============================================================================
# Container Registry Module
# =============================================================================

module "registry" {
  source = "./modules/registry"

  name                     = var.registry_name
  subscription_tier        = var.registry_subscription_tier
  region                   = var.registry_region
  generate_k8s_credentials = true
}

# =============================================================================
# Kubernetes (DOKS) Module
# =============================================================================

module "kubernetes" {
  source = "./modules/kubernetes"

  name        = "${var.project_name}-${var.environment}-k8s"
  region      = var.region
  k8s_version = var.k8s_version
  vpc_uuid    = data.digitalocean_vpc.main.id

  node_size  = var.k8s_node_size
  auto_scale = var.k8s_auto_scale
  min_nodes  = var.k8s_min_nodes
  max_nodes  = var.k8s_max_nodes
  node_count = var.k8s_node_count

  registry_integration = true

  tags = var.tags
}

# =============================================================================
# Platform Tools - ArgoCD & Sealed Secrets
# =============================================================================

resource "kubernetes_namespace" "argocd" {
  metadata {
    name = "argocd"
  }

  depends_on = [module.kubernetes]
}

resource "kubernetes_namespace" "sealed_secrets" {
  metadata {
    name = "sealed-secrets"
  }

  depends_on = [module.kubernetes]
}

resource "helm_release" "argocd" {
  name       = "argocd"
  repository = "https://argoproj.github.io/argo-helm"
  chart      = "argo-cd"
  version    = var.argocd_chart_version
  namespace  = kubernetes_namespace.argocd.metadata[0].name

  set {
    name  = "server.service.type"
    value = var.argocd_service_type
  }

  set {
    name  = "configs.params.server\\.insecure"
    value = "true"
  }

  timeout = 600
}

# -----------------------------------------------------------------------------
# ArgoCD Image Updater
# Detecta nuevas imágenes en DOCR y actualiza las Applications de ArgoCD
# directamente vía API (write-back-method = argocd) — sin commits a git.
# -----------------------------------------------------------------------------
resource "helm_release" "argocd_image_updater" {
  name       = "argocd-image-updater"
  repository = "https://argoproj.github.io/argo-helm"
  chart      = "argocd-image-updater"
  version    = var.argocd_image_updater_chart_version
  namespace  = kubernetes_namespace.argocd.metadata[0].name

  set {
    name  = "config.argocd.grpcWeb"
    value = "true"
  }

  set {
    name  = "config.argocd.serverAddress"
    value = "argocd-server.argocd.svc.cluster.local"
  }

  set {
    name  = "config.argocd.insecure"
    value = "true"
  }

  set {
    name  = "config.argocd.plaintext"
    value = "true"
  }

  # Intervalo de polling al registry (default 2m)
  set {
    name  = "config.applicationsAPIKind"
    value = "kubernetes"
  }

  timeout    = 300
  depends_on = [helm_release.argocd]
}

resource "helm_release" "sealed_secrets" {
  name       = "sealed-secrets"
  repository = "https://bitnami-labs.github.io/sealed-secrets"
  chart      = "sealed-secrets"
  version    = var.sealed_secrets_chart_version
  namespace  = kubernetes_namespace.sealed_secrets.metadata[0].name

  set {
    name  = "fullnameOverride"
    value = "sealed-secrets-controller"
  }

  timeout = 300
}

# =============================================================================
# NGINX Ingress Controller
# =============================================================================

resource "kubernetes_namespace" "ingress_nginx" {
  metadata {
    name = "ingress-nginx"
  }
  depends_on = [module.kubernetes]
}

resource "helm_release" "nginx_ingress" {
  name       = "ingress-nginx"
  repository = "https://kubernetes.github.io/ingress-nginx"
  chart      = "ingress-nginx"
  version    = var.nginx_ingress_chart_version
  namespace  = kubernetes_namespace.ingress_nginx.metadata[0].name

  set {
    name  = "controller.service.type"
    value = "LoadBalancer"
  }

  set {
    name  = "controller.service.annotations.service\\.beta\\.kubernetes\\.io/do-loadbalancer-name"
    value = "ecosystem-prod-lb"
  }

  timeout = 600
}

# =============================================================================
# cert-manager (TLS con Let's Encrypt)
# =============================================================================

resource "kubernetes_namespace" "cert_manager" {
  metadata {
    name = "cert-manager"
  }
  depends_on = [module.kubernetes]
}

resource "helm_release" "cert_manager" {
  name       = "cert-manager"
  repository = "https://charts.jetstack.io"
  chart      = "cert-manager"
  version    = var.cert_manager_chart_version
  namespace  = kubernetes_namespace.cert_manager.metadata[0].name

  set {
    name  = "crds.enabled"
    value = "true"
  }

  timeout = 300
}

# =============================================================================
# Namespace: ecosystem (prod)
# =============================================================================

resource "kubernetes_namespace" "ecosystem" {
  metadata {
    name = "ecosystem"
    labels = {
      environment = "prod"
      managed-by  = "terraform"
    }
  }
  depends_on = [module.kubernetes]
}

# =============================================================================
# Cloudflare Module
# DESHABILITADO: Los nameservers del dominio están en DigitalOcean.
# Para activar Cloudflare (proxy/WAF/cache) se requiere migrar los NS del
# dominio a Cloudflare. Una vez migrados, descomentar el bloque siguiente
# y eliminar (o comentar) los records duplicados del módulo `networking`.
# =============================================================================

# module "cloudflare" {
#   count  = var.k8s_lb_ip != "" ? 1 : 0
#   source = "./modules/cloudflare"
#
#   zone_id         = var.cloudflare_zone_id
#   lb_ip           = var.k8s_lb_ip
#   api_subdomain   = "api"
#   allowed_origins = var.cors_allowed_origins
# }

# =============================================================================
# Networking / Domain Module
# NOTA: Solo se aplica cuando k8s_lb_ip tenga valor (después del cluster K8s)
# =============================================================================

module "networking" {
  count  = var.k8s_lb_ip != "" ? 1 : 0
  source = "./modules/networking"

  domain    = var.domain
  k8s_lb_ip = var.k8s_lb_ip
  ttl       = var.dns_ttl

  microservice_subdomains = var.microservice_subdomains
}
