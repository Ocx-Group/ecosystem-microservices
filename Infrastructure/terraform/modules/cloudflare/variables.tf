# =============================================================================
# Variables - Cloudflare Module
# =============================================================================

variable "zone_id" {
  description = "Cloudflare Zone ID de ecosystemfx.net"
  type        = string
  sensitive   = true
}

variable "lb_ip" {
  description = "IP publica del Load Balancer de K8s"
  type        = string
}

variable "api_subdomain" {
  description = "Subdominio del API"
  type        = string
  default     = "api"
}

variable "ttl" {
  description = "TTL del DNS record (1 = auto con proxy activo)"
  type        = number
  default     = 1
}

variable "allowed_origins" {
  description = "Lista de dominios permitidos para CORS"
  type        = list(string)
  default     = []
}
