# =============================================================================
# Kubernetes Module Outputs
# =============================================================================

output "cluster_id" {
  description = "ID del cluster K8s"
  value       = digitalocean_kubernetes_cluster.main.id
}

output "cluster_urn" {
  description = "URN del cluster K8s"
  value       = digitalocean_kubernetes_cluster.main.urn
}

output "cluster_name" {
  description = "Nombre del cluster"
  value       = digitalocean_kubernetes_cluster.main.name
}

output "endpoint" {
  description = "Endpoint del API server"
  value       = digitalocean_kubernetes_cluster.main.endpoint
}

output "kubeconfig" {
  description = "Kubeconfig raw del cluster"
  value       = digitalocean_kubernetes_cluster.main.kube_config[0].raw_config
  sensitive   = true
}

output "cluster_ca_certificate" {
  description = "CA certificate del cluster (base64)"
  value       = digitalocean_kubernetes_cluster.main.kube_config[0].cluster_ca_certificate
  sensitive   = true
}

output "kube_token" {
  description = "Token de autenticacion del cluster"
  value       = digitalocean_kubernetes_cluster.main.kube_config[0].token
  sensitive   = true
}

output "cluster_subnet" {
  description = "Subnet del cluster"
  value       = digitalocean_kubernetes_cluster.main.cluster_subnet
}

output "service_subnet" {
  description = "Subnet de servicios"
  value       = digitalocean_kubernetes_cluster.main.service_subnet
}

output "ipv4_address" {
  description = "IP publica del cluster"
  value       = digitalocean_kubernetes_cluster.main.ipv4_address
}