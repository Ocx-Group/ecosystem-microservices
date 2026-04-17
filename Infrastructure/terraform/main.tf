# =============================================================================
# Ecosystem Microservices - Infrastructure
# =============================================================================

terraform {
  required_version = ">= 1.5.0"

  required_providers {
    digitalocean = {
      source  = "digitalocean/digitalocean"
      version = "~> 2.40"
    }
  }

  # Backend remoto - descomentar y configurar para estado compartido
  # backend "s3" {
  #   endpoint                    = "https://nyc3.digitaloceanspaces.com"
  #   bucket                      = "ecosystem-terraform-state"
  #   key                         = "prod/terraform.tfstate"
  #   region                      = "us-east-1"
  #   skip_credentials_validation = true
  #   skip_metadata_api_check     = true
  #   skip_requesting_account_id  = true
  #   skip_s3_checksum            = true
  # }
}

provider "digitalocean" {
  token             = var.do_token
  spaces_access_id  = var.spaces_access_key
  spaces_secret_key = var.spaces_secret_key
}

# =============================================================================
# VPC
# =============================================================================

data "digitalocean_vpc" "main" {
  id = var.vpc_uuid
}

# =============================================================================
# Database Module
# =============================================================================

module "database" {
  source = "./modules/database"

  name       = "db-postgresql-ocx-group"
  region     = var.region
  size       = var.db_size
  node_count = var.db_node_count
  version_pg = var.db_version
  vpc_uuid   = data.digitalocean_vpc.main.id

  database_name = "ocx_group"

  tags = var.tags
}

# =============================================================================
# Spaces (Object Storage) Module
# =============================================================================

module "spaces" {
  source = "./modules/spaces"

  name          = var.spaces_bucket_name
  region        = var.region
  acl           = var.spaces_acl
  force_destroy = var.spaces_force_destroy

  cors_allowed_origins = var.spaces_cors_origins
}

# =============================================================================
# Container Registry Module
# =============================================================================

module "registry" {
  source = "./modules/registry"

  name              = var.registry_name
  subscription_tier = var.registry_subscription_tier
  region            = var.registry_region
}
