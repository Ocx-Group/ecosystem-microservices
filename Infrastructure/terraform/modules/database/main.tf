# ===========================================================================
# Database PostgreSQL Module - Ecosystemfx Digital Ocean Infrastructure
# ===========================================================================


resource "digitalocean_database_cluster" "main" {
  name       = var.name
  engine     = "pg"
  version    = var.version_pg
  size       = var.size
  region     = var.region
  node_count = var.node_count

  # Private VPC
  private_network_uuid = var.vpc_uuid

  #Tags para identificar
  tags = var.tags

  lifecycle {
    prevent_destroy = true
    ignore_changes = [
      tags,
      maintenance_window,
    ]
  }
}

# La base de datos principal - los schemas de cada microservicio
# se gestionan via EF Core Migrations, no desde Terraform
resource "digitalocean_database_db" "main" {
  cluster_id = digitalocean_database_cluster.main.id
  name       = var.database_name
}
