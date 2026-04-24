# Production Environment Variables - Ecosystem

# Uso:
#     terraform init -backend-config="key=prod/terraform.tfstate"
#     terraform apply -var-file="environments/prod.tfvars"

# Project
project_name = "ecosystem"
environment  = "prod"
region       = "nyc3"

# VPC
vpc_uuid = "50a6b3b3-fa0a-4a48-bd99-905a25759089"

# Kubernetes - Cost-optimized for ~$200/mo budget
# Nodes: 2 min, 4 max (s-2vcpu-4gb = $24/mo each)
# Pod-level autoscaling se controla con HPA por servicio (min 2, max 5)
k8s_version    = "1.35.1-do.3"
k8s_node_size  = "s-2vcpu-4gb"
k8s_node_count = 2
k8s_auto_scale = true
k8s_min_nodes  = 2
k8s_max_nodes  = 4

# Database - HA
db_size       = "db-amd-1vcpu-2gb"
db_node_count = 2
db_version    = "16"

# Registry
registry_name              = "ocx-registry"
registry_subscription_tier = "professional"
registry_region            = "syd1"

# Spaces
spaces_bucket_name   = "ecosystem-storage"
spaces_acl           = "private"
spaces_force_destroy = false # CRITICO: nunca true en prod
spaces_cors_origins  = ["https://www.ecosystemfx.net"]

# Domain
domain    = "ecosystemfx.net"
k8s_lb_ip = "129.212.135.183"

# CORS - Orígenes permitidos para el Gateway
cors_allowed_origins = [
  "https://ecosystemfx.net",
  "https://recybotia.com",
  "https://recycoin.net"
]

# DNS
dns_ttl                 = 300
microservice_subdomains = ["account", "wallet", "inventory", "configuration", "notification"]

# TAG (compartido por todos los recursos: cluster K8s, BD, etc.)
tags = ["ecosystem", "prod", "terraform-managed"]
