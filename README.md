# Dotnet and Open Telemetry  (.NET 10)

## Overview

This solution demonstrates a microservices architecture using .NET 10. It consists of two independent microservices, each designed to showcase dependency injection, caching, and distributed tracing with OpenTelemetry.

## Projects

### MicroserviceA

- **Purpose:**  
  MicroserviceA provides endpoints for managing and retrieving information about people. It demonstrates:
  - RESTful API design using ASP.NET Core.
  - In-memory caching for fast data retrieval.
  - Distributed tracing with OpenTelemetry, exporting traces to Jaeger, OTLP, and console.
  - Layered architecture with clear separation between endpoints, application logic, and infrastructure.

### MicroserviceB

- **Purpose:**  
  MicroserviceB provides endpoints for retrieving country information. It demonstrates:
  - RESTful API design using ASP.NET Core.
  - In-memory caching for country data.
  - Extensible infrastructure for future enhancements.

## Features

- **.NET 10:**  
  Both microservices target .NET 10, leveraging the latest language and runtime features.

- **OpenTelemetry Integration:**  
  MicroserviceA is instrumented with OpenTelemetry for distributed tracing, supporting Jaeger, OTLP, and console exporters.

- **Caching:**  
  Both services use in-memory caching to optimize data access.

- **Layered Architecture:**  
  Each microservice is organized into layers (Endpoints, Application, Infrastructure, Models) for maintainability and scalability.

## How to Run

1. **Restore NuGet Packages:**  
   Open the solution in Visual Studio Insiders and restore all NuGet dependencies.

2. **Build the Solution:**  
   Build both projects to ensure all dependencies are resolved.

3. **Run Microservices:**  
   Start each microservice project. By default, they will run on different ports.

4. **Access APIs:**  
   - MicroserviceA: `/people` endpoints for people data.
   - MicroserviceB: `/countries` endpoints for country data.

5. **Tracing:**  
   - Ensure Jaeger is running locally to view distributed traces.
   - Traces are also exported to OTLP and console for debugging and analysis.

## Running Jaeger with Docker

To run Jaeger locally using the all-in-one Docker image, execute the following command:

```bash
docker run -d --name jaeger 
-e COLLECTOR_ZIPKIN_HOST_PORT=:9411 
-p 5775:5775/udp 
-p 6831:6831/udp 
-p 6832:6832/udp 
-p 5778:5778 
-p 16686:16686 
-p 14268:14268 
-p 14250:14250 
-p 9411:9411 
jaegertracing/all-in-one:latest