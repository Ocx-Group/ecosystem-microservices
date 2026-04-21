# ===========================================================================
# Kubernetes (DOKS) Module - Ecosystem Digital Ocean Infrastructure
# ===========================================================================

resource "digitalocean_kubernetes_cluster" "main" {
  name    = var.name
  region  = var.region
  version = var.k8s_version

  vpc_uuid = var.vpc_uuid

  registry_integration = var.registry_integration

  node_pool {
    name       = "${var.name}-pool"
    size       = var.node_size
    node_count = var.auto_scale ? null : var.node_count
    auto_scale = var.auto_scale
    min_nodes  = var.auto_scale ? var.min_nodes : null
    max_nodes  = var.auto_scale ? var.max_nodes : null
    tags       = var.tags
  }

  maintenance_policy {
    day        = var.maintenance_day
    start_time = var.maintenance_start_time
  }

  lifecycle {
    prevent_destroy = true
    ignore_changes = [
      node_pool[0].node_count,
    ]
  }
}