# ===========================================================================
# Spaces Object Storage Module - Ecosystem Digital Ocean Infrastructure
# ===========================================================================

resource "digitalocean_spaces_bucket" "main" {
  name   = var.name
  region = var.region
  acl    = var.acl

  force_destroy = var.force_destroy

  versioning {
    enabled = var.versioning_enabled
  }

  lifecycle {
    prevent_destroy = false
  }
}

# CORS como recurso separado (recomendado por DO)
resource "digitalocean_spaces_bucket_cors_configuration" "main" {
  count  = length(var.cors_allowed_origins) > 0 ? 1 : 0
  bucket = digitalocean_spaces_bucket.main.id
  region = var.region

  cors_rule {
    allowed_headers = var.cors_allowed_headers
    allowed_methods = var.cors_allowed_methods
    allowed_origins = var.cors_allowed_origins
    max_age_seconds = var.cors_max_age_seconds
  }
}

# CDN para acceso público (opcional)
resource "digitalocean_cdn" "spaces_cdn" {
  count  = var.enable_cdn ? 1 : 0
  origin = digitalocean_spaces_bucket.main.bucket_domain_name
  ttl    = var.cdn_ttl
}
