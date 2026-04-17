# =============================================================================
# Root Outputs
# =============================================================================

# Database
output "db_private_host" {
  description = "Host privado de la BD (dentro de la VPC)"
  value       = module.database.private_host
}

output "db_port" {
  description = "Puerto de la BD"
  value       = module.database.port
}

output "db_connection_string" {
  description = "Connection string para .NET"
  value       = module.database.connection_string
  sensitive   = true
}

output "db_database_name" {
  description = "Base de datos principal"
  value       = module.database.database_names
}

# Spaces
output "spaces_bucket_name" {
  description = "Nombre del bucket"
  value       = module.spaces.bucket_name
}

output "spaces_endpoint" {
  description = "Endpoint del bucket"
  value       = module.spaces.endpoint
}

# Registry
output "registry_endpoint" {
  description = "Endpoint del registry"
  value       = module.registry.endpoint
}

output "registry_server_url" {
  description = "URL del servidor del registry"
  value       = module.registry.server_url
}
