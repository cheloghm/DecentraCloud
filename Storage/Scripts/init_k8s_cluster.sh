#!/bin/bash

set -e

# Initialize the Kubernetes cluster using kubeadm
sudo kubeadm init --pod-network-cidr=10.244.0.0/16

# Set up kubeconfig for the root user
mkdir -p $HOME/.kube
sudo cp -i /etc/kubernetes/admin.conf $HOME/.kube/config
sudo chown $(id -u):$(id -g) $HOME/.kube/config

# Apply Flannel CNI plugin (you can change to Calico if preferred)
kubectl apply -f https://raw.githubusercontent.com/coreos/flannel/master/Documentation/kube-flannel.yml

# Taint control plane to act as both control plane and worker node
kubectl taint nodes --all node-role.kubernetes.io/master-

echo "Kubernetes cluster initialized and configured."
