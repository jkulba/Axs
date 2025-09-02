# Instructions to Create a Podman Pod for Hosting a Blazor WASM Application with NGINX

## Prerequisites
Ensure that you have Podman installed on your Linux Mint PC and that your Blazor WASM application has been built and the output files are available in the `dist` directory.

## Step 1: Build the Docker Image
Navigate to the root of your project directory and build the Docker image using the following command:

```bash
podman build -t volcano-client .
```

This command uses the `Dockerfile` in the current directory to create an image named `volcano-client`.

## Step 2: Create a Pod
Create a new Podman pod to host the NGINX server:

```bash
podman pod create --name volcano-client-pod -p 8080:80
```

This command creates a pod named `volcano-client-pod` and maps port 8080 on your host to port 80 on the container.

## Step 3: Run the NGINX Container
Run the NGINX container within the pod using the following command:

```bash
podman run -d --pod volcano-client-pod --name volcano-client-container volcano-client
```

This command starts the NGINX container in detached mode (`-d`) within the `volcano-client-pod`.

## Step 4: Access the Application
You can now access your Blazor WASM application by navigating to `http://localhost:8080` in your web browser.

## Step 5: Stop the Pod
To stop the pod and all its containers, use the following command:

```bash
podman pod stop volcano-client-pod
```

## Step 6: Remove the Pod
If you want to remove the pod after stopping it, use:

```bash
podman pod rm volcano-client-pod
```

These instructions will help you set up and manage a Podman pod for hosting your Blazor WASM application using NGINX.