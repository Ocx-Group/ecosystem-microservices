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
  token = var.do_token
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
