# TestServer

TestServer is a Dockerized service with a SignalR hub and REST API. It can be used as a generic service to test SignalR clients before connecting to a real service.

## Features

- SignalR hub for real-time communication with connected clients
- Minimal POST REST API to send messages to the SignalR hub
- Configurable CORS support for allowed origins
- Swagger documentation for the API

### Additional Features
- Additional API endpoints to simulate HTTP responses with custom status codes

## Prerequisites

- Docker

## Getting Started

1. Clone this repository:

   ```shell
   git clone https://github.com/mcgoohan/TestServer.git
   ```

2. Navigate to the project directory:

   ```shell
   cd TestServer
   ```

3. Set the environment variables in the `docker-compose.yml` file:

   - `ALLOWED_ORIGINS`: Comma-separated list of allowed CORS origins.
   - `HUB_PATH`: The URL path for the SignalR hub.

4. Build and run the Docker container:

   ```shell
   docker-compose up --build
   ```

5. The TestServer service will be available at [http://localhost](http://localhost).

6. You can change the port that the TestServer service runs on by editing the `docker-compose.yml` file:
    ```javascript
    ports:
    - "80:80" // change left hand side to the port you want. e.g. for port 5000 change to "5000:80" 
    ```

## API Reference

The TestServer API is documented using Swagger. Once the service is running, you can access the Swagger UI at [http://localhost/swagger](http://localhost/swagger) to explore the available endpoints.

### Sending a message to the SignalR hub

Endpoint: `POST /api/messages/{methodName}`

Parameters:

- `methodName` (URL parameter): The name of the client method on the SignalR hub to invoke.
- Request body: The message to send to the SignalR hub.

Example cURL command:

```shell
curl -X POST -H "Content-Type: application/json" -d '{"message": "Hello"}' http://localhos/api/messages/clients.receiveMessage
```

### SignalR Hub

The SignalR hub allows real-time communication with connected clients. Clients can listen for specific events and receive messages from the hub.

Example SignalR client code:

```javascript
const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost/hub/v1/notifications")
  .build();

connection.on("receiveMessage", (message) => {
  console.log("Received message:", message);
});

connection.start()
  .then(() => {
    console.log("Connection established.");
  })
  .catch((error) => {
    console.error("Error connecting to the hub:", error);
  });
```

### Simulating HTTP responses

The TestServer also provides additional API endpoints to simulate HTTP responses with custom status codes. These endpoints allow you to test how your application handles different HTTP responses.

- `GET /responses/{statusCode}`: Returns a response with the specified status code.
- `POST /responses/{statusCode}`: Returns a response with the specified status code.
- `PUT /responses/{statusCode}`: Returns a response with the specified status code.
- `DELETE /responses/{statusCode}`: Returns a response with the specified status code.

Replace `{statusCode}` with the desired HTTP status code.

## License

This project is licensed under the [MIT License](LICENSE).
```