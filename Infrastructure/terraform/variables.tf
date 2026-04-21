# =============================================================================
# Root Variables
# =============================================================================

variable "do_token" {
  description = "Token de API de DigitalOcean"
  type        = string
  sensitive   = true
}

variable "spaces_access_id" {
  description = "Access Key ID para DigitalOcean Spaces"
  type        = string
  sensitive   = true
}

variable "spaces_secret_key" {
  description = "Secret Key para DigitalOcean Spaces"
  type        = string
  sensitive   = true
}

variable "project_name" {
  description = "Nombre del proyecto"
  type        = string
  default     = "ecosystem"
}

variable "environment" {
  description = "Ambiente (dev, staging, prod)"
  type        = string
  default     = "prod"
}

variable "region" {
  description = "Región de DigitalOcean"
  type        = string
  default     = "nyc3"
}

# VPC
variable "vpc_uuid" {
  description = "UUID de la VPC existente"
  type        = string
}

# Database
variable "db_size" {
  description = "Tamaño del cluster de BD"
  type        = string
  default     = "db-s-2vcpu-4gb"
}

variable "db_node_count" {
  description = "Número de nodos de BD (Primary + Standby)"
  type        = number
  default     = 2
}

variable "db_version" {
  description = "Versión de PostgreSQL"
  type        = string
  default     = "16"
}

# Kubernetes
variable "k8s_version" {
  description = "Versión de Kubernetes"
  type        = string
  default     = "1.32.10-do.5"
}

variable "k8s_node_size" {
  description = "Tamaño de nodos K8s"
  type        = string
  default     = "s-4vcpu-8gb"
}

variable "k8s_node_count" {
  description = "Número de nodos K8s"
  type        = number
  default     = 2
}

variable "k8s_auto_scale" {
  description = "Habilitar auto-scaling"
  type        = bool
  default     = true
}

variable "k8s_min_nodes" {
  description = "Mínimo de nodos K8s"
  type        = number
  default     = 2
}

variable "k8s_max_nodes" {
  description = "Máximo de nodos K8s"
  type        = number
  default     = 5
}

# Registry
variable "registry_name" {
  description = "Nombre del container registry"
  type        = string
  default     = "ocx-registry"
}

variable "registry_subscription_tier" {
  description = "Tier del registry de contenedores"
  type        = string
  default     = "professional"
}

variable "registry_region" {
  description = "Región del registry"
  type        = string
  default     = "syd1"
}

# Spaces
variable "spaces_bucket_name" {
  description = "Nombre del bucket de Spaces"
  type        = string
}

variable "spaces_acl" {
  description = "ACL del bucket (private, public-read)"
  type        = string
  default     = "private"
}

variable "spaces_force_destroy" {
  description = "Permitir destruir bucket con contenido"
  type        = bool
  default     = false
}

variable "spaces_cors_origins" {
  description = "Orígenes permitidos para CORS en Spaces"
  type        = list(string)
  default     = []
}

# Domain
variable "domain" {
  description = "Dominio principal"
  type        = string
  default     = "ecosystemfx.net"
}

# Tags
variable "tags" {
  description = "Tags para los recursos"
  type        = list(string)
  default     = ["ecosystem", "prod", "terraform-managed"]
}

# ArgoCD
variable "argocd_chart_version" {
  description = "Version del chart de ArgoCD"
  type        = string
  default     = "7.8.26"
}

variable "argocd_service_type" {
  description = "Tipo de servicio para ArgoCD server"
  type        = string
  default     = "ClusterIP"
}

variable "argocd_image_updater_chart_version" {
  description = "Version del chart de ArgoCD Image Updater"
  type        = string
  default     = "0.12.3"
}

# NGINX Ingress
variable "nginx_ingress_chart_version" {
  description = "Version del chart de NGINX Ingress Controller"
  type        = string
  default     = "4.12.1"
}

# cert-manager
variable "cert_manager_chart_version" {
  description = "Version del chart de cert-manager"
  type        = string
  default     = "1.17.2"
}

# Sealed Secrets
variable "sealed_secrets_chart_version" {
  description = "Version del chart de Sealed Secrets"
  type        = string
  default     = "2.17.1"
}

# Networking / Domain
# Cloudflare
variable "cloudflare_api_token" {
  description = "API Token de Cloudflare (permisos: Zone DNS Edit, Zone Settings Edit). Opcional: solo se usa si el módulo cloudflare está habilitado."
  type        = string
  sensitive   = true
  default     = ""
}

variable "cloudflare_zone_id" {
  description = "Zone ID de ecosystemfx.net en Cloudflare. Opcional: solo se usa si el módulo cloudflare está habilitado."
  type        = string
  sensitive   = true
  default     = ""
}

variable "cors_allowed_origins" {
  description = "Dominios permitidos para CORS en el Gateway"
  type        = list(string)
  default     = []
}

# Networking / Domain
variable "k8s_lb_ip" {
  description = "IP del Load Balancer de K8s (disponible tras crear el cluster)"
  type        = string
  default     = ""
}

variable "dns_ttl" {
  description = "TTL de records DNS en segundos"
  type        = number
  default     = 300
}

variable "microservice_subdomains" {
  description = "Subdominios adicionales para microservicios"
  type        = list(string)
  default     = []
}
