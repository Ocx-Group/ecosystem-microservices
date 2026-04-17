# =============================================================================
# Spaces Module Variables
# =============================================================================

variable "name" {
  description = "Nombre del bucket"
  type        = string
}

variable "region" {
  description = "Región de DigitalOcean"
  type        = string
}

variable "acl" {
  description = "Control de acceso (private, public-read)"
  type        = string
  default     = "private"
}

variable "force_destroy" {
  description = "Permitir destruir bucket con contenido"
  type        = bool
  default     = false
}

variable "versioning_enabled" {
  description = "Habilitar versionado de objetos"
  type        = bool
  default     = false
}

# CORS
variable "cors_allowed_origins" {
  description = "Orígenes permitidos para CORS"
  type        = list(string)
  default     = []
}

variable "cors_allowed_methods" {
  description = "Métodos HTTP permitidos para CORS"
  type        = list(string)
  default     = ["GET", "PUT", "POST", "DELETE"]
}

variable "cors_allowed_headers" {
  description = "Headers permitidos para CORS"
  type        = list(string)
  default     = ["*"]
}

variable "cors_max_age_seconds" {
  description = "Tiempo máximo de cache para CORS preflight"
  type        = number
  default     = 3600
}

# CDN
variable "enable_cdn" {
  description = "Habilitar CDN para el bucket"
  type        = bool
  default     = false
}

variable "cdn_ttl" {
  description = "TTL del CDN en segundos"
  type        = number
  default     = 3600
}
