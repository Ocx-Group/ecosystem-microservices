# =============================================================================
# Spaces Module Outputs
# =============================================================================

output "bucket_name" {
  description = "Nombre del bucket"
  value       = digitalocean_spaces_bucket.main.name
}

output "bucket_urn" {
  description = "URN del bucket"
  value       = digitalocean_spaces_bucket.main.urn
}

output "bucket_domain_name" {
  description = "Dominio del bucket"
  value       = digitalocean_spaces_bucket.main.bucket_domain_name
}

output "endpoint" {
  description = "Endpoint del bucket"
  value       = digitalocean_spaces_bucket.main.endpoint
}

output "region" {
  description = "Región del bucket"
  value       = digitalocean_spaces_bucket.main.region
}

output "cdn_endpoint" {
  description = "Endpoint del CDN (si está habilitado)"
  value       = var.enable_cdn ? digitalocean_cdn.spaces_cdn[0].endpoint : null
}
