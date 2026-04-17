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
