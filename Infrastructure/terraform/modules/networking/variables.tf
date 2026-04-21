# =============================================================================
# Networking Module Variables
# =============================================================================

variable "domain" {
  description = "Dominio principal"
  type        = string
}

variable "k8s_lb_ip" {
  description = "IP del Load Balancer del cluster K8s"
  type        = string
}

variable "ttl" {
  description = "TTL de los records DNS en segundos"
  type        = number
  default     = 300
}

variable "microservice_subdomains" {
  description = "Subdominios adicionales para microservicios (ej: account, wallet)"
  type        = list(string)
  default     = []
}
