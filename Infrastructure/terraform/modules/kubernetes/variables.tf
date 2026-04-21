# =============================================================================
# Kubernetes Module Variables
# =============================================================================

variable "name" {
  description = "Nombre del cluster K8s"
  type        = string
}

variable "region" {
  description = "Region de DigitalOcean"
  type        = string
}

variable "k8s_version" {
  description = "Version de Kubernetes"
  type        = string
}

variable "vpc_uuid" {
  description = "UUID de la VPC"
  type        = string
}

variable "registry_integration" {
  description = "Habilitar integracion con Container Registry"
  type        = bool
  default     = true
}

variable "node_size" {
  description = "Tamano de los nodos"
  type        = string
  default     = "s-4vcpu-8gb"
}

variable "node_count" {
  description = "Numero de nodos (si auto_scale = false)"
  type        = number
  default     = 2
}

variable "auto_scale" {
  description = "Habilitar auto-scaling"
  type        = bool
  default     = true
}

variable "min_nodes" {
  description = "Minimo de nodos con auto-scaling"
  type        = number
  default     = 2
}

variable "max_nodes" {
  description = "Maximo de nodos con auto-scaling"
  type        = number
  default     = 5
}

variable "tags" {
  description = "Tags para los nodos"
  type        = list(string)
  default     = []
}

variable "maintenance_day" {
  description = "Dia de mantenimiento"
  type        = string
  default     = "sunday"
}

variable "maintenance_start_time" {
  description = "Hora de inicio de mantenimiento (UTC)"
  type        = string
  default     = "04:00"
}