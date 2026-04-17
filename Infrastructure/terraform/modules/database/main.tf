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

# Bases de datos para cada microservicio
resource "digitalocean_database_db" "databases" {
  for_each = toset(var.databases)

  cluster_id = digitalocean_database_cluster.main.id
  name       = each.value
}
