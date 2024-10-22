# Azure Function for Sending Sensor Data to MQTT Server (C# with Docker)

This Azure Function reads sensor data from various sources (e.g., HTTP trigger, IoT Hub) and sends the data to an MQTT server. The project includes Docker support for containerized deployment.

## Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Setup](#setup)
- [Configuration](#configuration)
- [Docker](#docker)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Overview

This project uses an Azure Function written in C# to handle incoming sensor data and publish it to an MQTT broker. It is set up for both regular Azure deployment and containerized deployment using Docker.

## Prerequisites

Before you begin, make sure you have the following:

- An **Azure Subscription** with an active Function App.
- **MQTT Broker** credentials (MQTT broker URL, username, password, etc.).
- **Azure Functions Core Tools** for local development.
- **.NET 6.0 SDK** installed.
- **Docker** installed on your local machine.
- Optional: Access to a source of sensor data or a mock dataset.

## Setup

### 1. Clone the repository

Clone the project repository to your local machine:

```bash
git clone https://github.com/stevehellier/SensorStream
cd SensorStream
```

### 2. Restore dependencies

To restore project dependencies:

```bash
dotnet restore
```
**or**

to restore and build the project:
```bash
dotnet build
```

### 3. Configure MQTT Broker Settings

In `local.settings.json`, or through the Azure portal's "App Settings," configure the following settings:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "<Azure-Storage-Connection-String>",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "MQTT_BROKER_URL": "<your-mqtt-broker-url>",
    "MQTT_PORT": "1883",
    "MQTT_USERNAME": "<your-mqtt-username>",
    "MQTT_PASSWORD": "<your-mqtt-password>",
  }
}
```

These values are used by the function to connect to the MQTT broker.

## Docker

This project includes a `Dockerfile` to package and run the Azure Function inside a container. This allows you to deploy the function as a container in any environment that supports Docker, including Azure's **Container Instances**, **Azure Kubernetes Service (AKS)**, or your local machine.

### Docker Setup

#### 1. Build the Docker Image

To build the Docker image, run the following command from the root of the project directory (where the `Dockerfile` is located):

```bash
docker build -t <container-name> .
```

#### 1. Run the Docker Container

After building the image, you can run the container locally to test the function:

```bash
docker run -p 8080:80 -e AzureWebJobsStorage="<Azure-Storage-Connection-String>" \
    -e MQTT__BROKER_URL="<your-mqtt-broker-url>" \
    -e MQTT__PORT="1883" \
    -e MQTT__USERNAME="<your-mqtt-username>" \
    -e MQTT__PASSWORD="<your-mqtt-password>" \
    <container-name>
```

This will run the Azure Function inside a container and map port `8080` of your local machine to port `80` inside the container.

For Azure Container Registry (ACR):

```bash
# Log in to your ACR
az acr login --name <your-registry-name>

# Tag the image with the ACR repository name
docker tag <container-name> <your-registry-name>.azurecr.io/<container-name>

# Push the image to ACR
docker push <your-registry-name>.azurecr.io/<container-name>
```

#### 5. Deploy to Azure

To deploy your Docker image to an Azure Function App (with container support), create an Azure Function App with a custom container and specify the container image:

```bash
az functionapp create --resource-group <<container-name>
  --plan <your-app-service-plan> \
  --name <your-function-app-name> \
  --deployment-container-image-name <your-registry-name>.azurecr.io/<container-name>
```

## Configuration

### Function Settings

| Setting           | Description                                        |
|-------------------|----------------------------------------------------|
| `MQTT_BROKER_URL` | The URL of your MQTT broker.                       |
| `MQTT_PORT`       | The port used to connect to the MQTT broker.       |
| `MQTT_USERNAME`   | MQTT broker username (if authentication is required).|
| `MQTT_PASSWORD`   | MQTT broker password (if authentication is required).|

These values can be configured in `local.settings.json` for local testing or in the Azure App Settings for production.
### Function Trigger

This function can be triggered by various input sources, such as:

- **HTTP Trigger**: Send sensor data to the function via an HTTP POST request.
- **Event Grid** or **IoT Hub Trigger**: Set up to handle real-time sensor data from IoT devices.

## Usage

Once the function is deployed, it will automatically receive sensor data and publish it to the MQTT server.

To test the function locally with an HTTP trigger, run the function (either in Docker or locally) and use the following command:

```bash
curl -X POST http://localhost:8080/api/sensors/Office?temperature=23.5&humidity=55&heatIndex=23.5
```

This will publish the data to the configured MQTT server with the topic `sensor/office`.

### Example

 `deviceName` will be the same as the sensor name in the URL.

```json
{
  "deviceName": "Office",
  "temperature": 23.5,
  "humidity": 55,
  "heatIndex": 23.5
}
```

## Folder structure

```bash
SensorStream/
├── SensorStream.sln
├── Dockerfile
├── Function/
│   └── Properties
│       └── launchSettings.cs
│   └── SensorDataFunction.cs
│   └── SensorDataFunctions.csproj
│   └── AppConfig.cs
│   └── Program.cs
│   └── host.json
│   └── local.settings.json
├── Interfaces/
│   └── IMessageSender.cs
│   └── Interfaces.csproj
├── Models/
│   └── SensorData.cs
│   └── Models.csproj
├── MessageService/
│   └── MessageSender.cs
│   └── MessageService.csproj
├── README.md
└── .gitignore
```

## Contributing

Contributions are welcome! Feel free to:

1. Fork the repository.
2. Create a new branch for your feature (`git checkout -b feature/your-feature`).
3. Commit your changes (`git commit -m 'Add your feature'`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Create a pull request.

## License

This project is licensed under the MIT License. See the `LICENSE` file for more details.

---