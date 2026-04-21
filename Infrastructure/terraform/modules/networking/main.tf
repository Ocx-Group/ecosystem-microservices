# ===========================================================================
# Networking Module - Ecosystem Digital Ocean Infrastructure
# ===========================================================================

# Dominio principal en DigitalOcean
resource "digitalocean_domain" "main" {
  name = var.domain
}

# Record A - raíz del dominio apuntando al LB de K8s
# DESHABILITADO temporalmente: la IP actual sirve el front legacy.
# Descomentar cuando el front se migre a K8s.
# resource "digitalocean_record" "root" {
#   domain = digitalocean_domain.main.id
#   type   = "A"
#   name   = "@"
#   value  = var.k8s_lb_ip
#   ttl    = var.ttl
# }

# Record A - www
# DESHABILITADO temporalmente (mismo motivo que root).
# resource "digitalocean_record" "www" {
#   domain = digitalocean_domain.main.id
#   type   = "A"
#   name   = "www"
#   value  = var.k8s_lb_ip
#   ttl    = var.ttl
# }

# Record A - api (para los microservicios)
resource "digitalocean_record" "api" {
  domain = digitalocean_domain.main.id
  type   = "A"
  name   = "api"
  value  = var.k8s_lb_ip
  ttl    = var.ttl
}

# Record A - argocd (acceso al panel de ArgoCD)
resource "digitalocean_record" "argocd" {
  domain = digitalocean_domain.main.id
  type   = "A"
  name   = "argocd"
  value  = var.k8s_lb_ip
  ttl    = var.ttl
}

# Subdominio por microservicio (opcional)
resource "digitalocean_record" "microservices" {
  for_each = toset(var.microservice_subdomains)

  domain = digitalocean_domain.main.id
  type   = "A"
  name   = each.value
  value  = var.k8s_lb_ip
  ttl    = var.ttl
}
