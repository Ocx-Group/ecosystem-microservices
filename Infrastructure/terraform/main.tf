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
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "~> 2.35"
    }
    helm = {
      source  = "hashicorp/helm"
      version = "~> 2.17"
    }
  }

  # Backend remoto - descomentar y configurar para estado compartido
  # backend "s3" {
  #   endpoint                    = "https://nyc3.digitaloceanspaces.com"
  #   bucket                      = "ecosystem-terraform-state"
  #   key                         = "prod/terraform.tfstate"
  #   region                      = "us-east-1"
  #   skip_credentials_validation = true
  #   skip_metadata_api_check     = true
  #   skip_requesting_account_id  = true
  #   skip_s3_checksum            = true
  # }
}

provider "digitalocean" {
  token             = var.do_token
  spaces_access_id  = var.spaces_access_key
  spaces_secret_key = var.spaces_secret_key
}

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

  name              = var.registry_name
  subscription_tier = var.registry_subscription_tier
  region            = var.registry_region
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
