# =============================================================================
# Cloudflare Module - ecosystemfx.net
# Gestiona DNS, SSL y Cache Rules para el API Gateway
# =============================================================================

# DNS A record: api.ecosystemfx.net → IP del LB (proxied por Cloudflare)
resource "cloudflare_record" "api" {
  zone_id = var.zone_id
  name    = var.api_subdomain
  content = var.lb_ip
  type    = "A"
  ttl     = var.ttl
  proxied = true  # Tráfico pasa por Cloudflare (WAF, cache, SSL)
}

# SSL/TLS: Full (Strict), HTTPS siempre, TLS 1.2 mínimo, HTTP/2
resource "cloudflare_zone_settings_override" "main" {
  zone_id = var.zone_id

  settings {
    ssl              = "strict"
    always_use_https = "on"
    min_tls_version  = "1.2"
    http2            = "on"
    tls_1_3          = "on"
  }
}

# Cache Rule: Bypass para el API (no cachear respuestas del backend)
resource "cloudflare_ruleset" "cache_bypass_api" {
  zone_id     = var.zone_id
  name        = "Bypass cache for API"
  description = "No cachear respuestas de api.ecosystemfx.net"
  kind        = "zone"
  phase       = "http_request_cache_settings"

  rules {
    action = "set_cache_settings"
    action_parameters {
      cache = false
    }
    expression  = "(http.host eq \"${var.api_subdomain}.ecosystemfx.net\")"
    description = "Bypass cache for API subdomain"
    enabled     = true
  }
}

# WAF: Managed ruleset de Cloudflare (bloquea ataques comunes)
resource "cloudflare_ruleset" "waf_managed" {
  zone_id     = var.zone_id
  name        = "Managed WAF rules"
  description = "Cloudflare Managed Ruleset"
  kind        = "zone"
  phase       = "http_request_firewall_managed"

  rules {
    action      = "execute"
    action_parameters {
      id = "efb7b8c949ac4650a09736fc376e9aee"  # Cloudflare Managed Ruleset
    }
    expression  = "true"
    description = "Execute Cloudflare Managed Ruleset"
    enabled     = true
  }
}
