# =============================================================================
# Root Variables
# =============================================================================

variable "do_token" {
  description = "Token de API de DigitalOcean"
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
variable "registry_subscription_tier" {
  description = "Tier del registry de contenedores"
  type        = string
  default     = "professional"
}

# Spaces
variable "spaces_bucket_name" {
  description = "Nombre del bucket de Spaces"
  type        = string
  default     = ""
}

variable "spaces_acl" {
  description = "ACL del bucket"
  type        = string
  default     = ""
}

variable "spaces_force_destroy" {
  description = "Permitir destruir bucket con contenido"
  type        = bool
  default     = false
}

# Domain
variable "domain" {
  description = "Dominio principal"
  type        = string
  default     = "api.ecosystemfx.com"
}

# Cloudflare
variable "enable_cloudflare_dns" {
  description = "Usar Cloudflare para DNS"
  type        = bool
  default     = true
}

variable "cloudflare_proxied" {
  description = "Proxy de Cloudflare habilitado"
  type        = bool
  default     = true
}

# Tags
variable "tags" {
  description = "Tags para los recursos"
  type        = list(string)
  default     = ["ecosystem", "prod", "terraform-managed"]
}
