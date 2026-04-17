# Production Environment Variables - Ecosystem

# Uso:
#     terraform init -backend-config="key=prod/terraform.tfstate"
#     terraform apply -var-file="environments/prod.tfvars"

# Project
project_name = "ecosystem"
environment  = "prod"
region       = "nyc3"

# VPC
vpc_ip_range = "10.10.0.0/16"

# Kubernetes - Started Configuration 
k8s_version    = "1.32.10-do.5"
k8s_node_size  = "s-4vcpu-8gb"
k8s_node_count = 2
k8s_auto_scale = true
k8s_min_nodes  = 2
k8s_max_nodes  = 5

# Databas - HA
db_size       = "db-s-2vcpu-4gb"
db_node_count = 2
db_version    = "16"

# Registry
registry_subscription_tier = "professional"

# Spaces
spaces_bucket_name   = ""
spaces_acl           = ""
spaces_force_destroy = false # CRITICO: nunca true en prod

# Domain
domain = "api.ecosystemfx.com"

# Cloudflare 
enable_cloudflare_dns = true
cloudflare_proxied    = true

# TAG
tags = ["ecosystem", "prod", "terraform-managed", "critical"]
