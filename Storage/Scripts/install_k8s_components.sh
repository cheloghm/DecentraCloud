#!/bin/bash

set -e

# Determine OS type
OS=$(uname | tr '[:upper:]' '[:lower:]')

# Install Docker or containerd based on OS
install_container_runtime() {
  if [[ "$OS" == "linux" ]]; then
    # Install Docker
    echo "Installing Docker..."
    sudo apt-get update
    sudo apt-get install -y apt-transport-https ca-certificates curl gnupg lsb-release
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
    echo "deb [arch=$(dpkg --print-architecture signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
    sudo apt-get update
    sudo apt-get install -y docker-ce docker-ce-cli containerd.io
    sudo systemctl start docker
    sudo systemctl enable docker
  elif [[ "$OS" == "darwin" ]]; then
    echo "Please install Docker Desktop manually on macOS."
  elif [[ "$OS" == "windows" ]]; then
    echo "Please install Docker Desktop manually on Windows."
  else
    echo "Unsupported OS: $OS"
    exit 1
  fi
}

# Install Kubernetes components
install_k8s_components() {
  if [[ "$OS" == "linux" ]]; then
    echo "Installing Kubernetes components..."
    sudo apt-get update
    sudo apt-get install -y apt-transport-https ca-certificates curl
    curl -s https://packages.cloud.google.com/apt/doc/apt-key.gpg | sudo apt-key add -
    sudo bash -c 'cat <<EOF >/etc/apt/sources.list.d/kubernetes.list
deb https://apt.kubernetes.io/ kubernetes-xenial main
EOF'
    sudo apt-get update
    sudo apt-get install -y kubelet kubeadm kubectl
    sudo apt-mark hold kubelet kubeadm kubectl
  elif [[ "$OS" == "darwin" ]]; then
    echo "Installing Kubernetes components on macOS..."
    brew install kubectl
    brew install kubeadm
    brew install kubelet
  elif [[ "$OS" == "windows" ]]; then
    echo "Please install Kubernetes components manually on Windows."
  else
    echo "Unsupported OS: $OS"
    exit 1
  fi
}

# Run installation functions
install_container_runtime
install_k8s_components

echo "Installation of Kubernetes components and container runtime completed."
