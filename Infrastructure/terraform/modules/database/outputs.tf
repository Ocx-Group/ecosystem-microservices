# =============================================================================
# Database Module Outputs
# =============================================================================

locals {
  db_user     = digitalocean_database_cluster.main.user != null && digitalocean_database_cluster.main.user != "" ? digitalocean_database_cluster.main.user : "doadmin"
  db_password = digitalocean_database_cluster.main.password != null ? digitalocean_database_cluster.main.password : ""
  db_port     = tostring(digitalocean_database_cluster.main.port)
}

output "cluster_id" {
  description = "ID del cluster de base de datos"
  value       = digitalocean_database_cluster.main.id
}

output "cluster_urn" {
  description = "URN del cluster de base de datos"
  value       = digitalocean_database_cluster.main.urn
}

output "host" {
  description = "Host de la base de datos"
  value       = digitalocean_database_cluster.main.host
}

output "private_host" {
  description = "Host privado de la base de datos (Dentro de la vpc)"
  value       = digitalocean_database_cluster.main.private_host
}

output "port" {
  description = "Puerto de la base de datos"
  value       = digitalocean_database_cluster.main.port
}

output "user" {
  description = "Usuario administrador"
  value       = digitalocean_database_cluster.main.user
}

output "password" {
  description = "Contraseña del usuario administrador"
  value       = digitalocean_database_cluster.main.password
  sensitive   = true
}

output "database" {
  description = "Nombre de la base de datos por defecto"
  value       = digitalocean_database_cluster.main.database
}

output "connection_uri" {
  description = "URI de conexión completa"
  value       = digitalocean_database_cluster.main.uri
  sensitive   = true
}

output "private_uri" {
  description = "URI de conexión privada (dentro de la VPC)"
  value       = digitalocean_database_cluster.main.private_uri
  sensitive   = true
}

output "database_names" {
  description = "Nombre de la base de datos"
  value       = digitalocean_database_db.main.name
}

# Connection string para .NET
output "connection_string" {
  description = "Connection string formato .NET"
  value       = "Host=${digitalocean_database_cluster.main.private_host};Port=${local.db_port};Database=${var.database_name};Username=${local.db_user};Password=${local.db_password};SSL Mode=Require;Trust Server Certificate=true"
  sensitive   = true
}

