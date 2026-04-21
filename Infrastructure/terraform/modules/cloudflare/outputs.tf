# =============================================================================
# Outputs - Cloudflare Module
# =============================================================================

output "api_hostname" {
  description = "FQDN del API"
  value       = cloudflare_record.api.hostname
}

output "api_record_id" {
  description = "ID del DNS record"
  value       = cloudflare_record.api.id
}
