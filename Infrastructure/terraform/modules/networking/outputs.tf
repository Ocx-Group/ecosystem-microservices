# =============================================================================
# Networking Module Outputs
# =============================================================================

output "domain_urn" {
  description = "URN del dominio"
  value       = digitalocean_domain.main.urn
}

output "domain_name" {
  description = "Nombre del dominio"
  value       = digitalocean_domain.main.name
}

output "root_record_fqdn" {
  description = "FQDN del record raiz (deshabilitado hasta migrar el front)"
  value       = null
}

output "www_fqdn" {
  description = "FQDN del subdominio www (deshabilitado hasta migrar el front)"
  value       = null
}

output "api_fqdn" {
  description = "FQDN del subdominio api"
  value       = digitalocean_record.api.fqdn
}

output "argocd_fqdn" {
  description = "FQDN del subdominio argocd"
  value       = digitalocean_record.argocd.fqdn
}
