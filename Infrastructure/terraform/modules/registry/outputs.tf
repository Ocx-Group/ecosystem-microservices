# =============================================================================
# Container Registry Module Outputs
# =============================================================================

output "name" {
  description = "Nombre del registry"
  value       = digitalocean_container_registry.main.name
}

output "endpoint" {
  description = "Endpoint del registry"
  value       = digitalocean_container_registry.main.endpoint
}

output "server_url" {
  description = "URL del servidor del registry"
  value       = digitalocean_container_registry.main.server_url
}

output "docker_credentials" {
  description = "Credenciales Docker para K8s (base64)"
  value       = var.generate_k8s_credentials ? digitalocean_container_registry_docker_credentials.main[0].docker_credentials : null
  sensitive   = true
}
