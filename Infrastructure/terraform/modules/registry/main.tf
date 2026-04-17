# ===========================================================================
# Container Registry Module - Ecosystem Digital Ocean Infrastructure
# ===========================================================================

resource "digitalocean_container_registry" "main" {
  name                   = var.name
  subscription_tier_slug = var.subscription_tier
  region                 = var.region

  lifecycle {
    prevent_destroy = true
  }
}

# Credenciales Docker para integrar con K8s
resource "digitalocean_container_registry_docker_credentials" "main" {
  count          = var.generate_k8s_credentials ? 1 : 0
  registry_name  = digitalocean_container_registry.main.name
  write          = false
  expiry_seconds = var.credentials_expiry_seconds
}
