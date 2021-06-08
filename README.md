# Work planner Backend Web API

This is my second project in which i studied web api with microservice architecture.

The main idea of the project is a work execution planner.
The security system is made using identity server 4. There is a system of users with registration, authorization.
After registration, the user will be sent an email for confirmation.

Users can create rooms and tasks within them.
They can then invite other users to join their rooms. There is a role system inside the rooms.

All access to microservices is implemented through the api gateway. Also, each microservice is located in a container.

| Status | Master | Develop |
| ------ | ------ | ------- |
|  Build | [![CI](https://github.com/Inozpavel/WorkPlanner.WebApi/actions/workflows/dotnet.yml/badge.svg?branch=master&event=push)](https://github.com/Inozpavel/WorkPlanner.WebApi/actions/workflows/dotnet.yml) | [![CI](https://github.com/Inozpavel/WorkPlanner.WebApi/actions/workflows/dotnet.yml/badge.svg?branch=dev&event=push)](https://github.com/Inozpavel/WorkPlanner.WebApi/actions/workflows/dotnet.yml) |

## Prerequisites for launching

[Docker Desktop](https://www.docker.com/products/docker-desktop)

## Run system locally using Docker images

To run project open console in folder with [docker-compose.yml](docker-compose.yml?raw=true) file and run this commands

1. Pull services images from Docker Hub

```cmd
docker-compose pull
```

2. Run services

```cmd
docker-compose up -d
```

After starting API will be available on [localhost:4000](http://localhost:4000)

Documentation will be available on [localhost:4000/swagger](http://localhost:4000/swagger)

## Project Information

### Project technologies stack

Main framework

- [ASP.Net Core](https://dotnet.microsoft.com/apps/aspnet)

Database
- [PostgreSQL](https://www.postgresql.org/)

Security
- [Identity Server 4](https://identityserver4.readthedocs.io/en/latest/)

ORM
- [Entity Framework Core for PostgreSQL](https://docs.microsoft.com/ru-ru/ef/core/)

API Gateway
- [Ocelot](https://github.com/ThreeMammals/Ocelot)

Message broker
- [RabbitMQ (MassTransit)](https://masstransit-project.com/usage/transports/rabbitmq.html)

Documentation
- [Swagger](https://swagger.io/)

Packages
- [AutoMapper](https://github.com/AutoMapper/AutoMapper)
- [AspNetCore.Identity](https://docs.microsoft.com/ru-ru/aspnet/identity/overview/getting-started/introduction-to-aspnet-identity)

## Project Architecture
[[!Architecture](Architecture.png)]

Patterns
- UnitOfWork
- Repository
- SOA