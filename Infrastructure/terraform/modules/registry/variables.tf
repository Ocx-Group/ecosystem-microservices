# =============================================================================
# Container Registry Module Variables
# =============================================================================

variable "name" {
  description = "Nombre del container registry"
  type        = string
}

variable "subscription_tier" {
  description = "Tier de suscripción (starter, basic, professional)"
  type        = string
  default     = "professional"
}

variable "region" {
  description = "Región del registry"
  type        = string
}

variable "generate_k8s_credentials" {
  description = "Generar credenciales Docker para integración con K8s"
  type        = bool
  default     = false
}

variable "credentials_expiry_seconds" {
  description = "Tiempo de expiración de credenciales Docker"
  type        = number
  default     = 1576800000 # ~50 años, no expiran en la práctica
}
