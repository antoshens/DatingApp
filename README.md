# DatingApp
This is my pet project of creating Dating App (PWA).
The app is the dating web application that allows user to search for his/her couple nearby
by the different parameters and preferences.

The app consists of 2 solution (instances) that are deployed separatelly.
First and main one is *DatingApp.API* solution which takes and HTTPS/HTTPS requests from the client (PWA app - TBD)
and applies business logic and communicates with the DB instanse.
The service is based on .NET Core 6 WepAPI.
The service builds on an n-layer architecture and used a DDD approach.
It contains different projects to implements ADL layer, Domain (Business) layer, Infrastructure project that contains
base interfaces and classes that are used across solution and DI container.

Also, the repo has another soultion *PhotoService*. Which is another service that stands only for saving user photos
into photo cloud provider (_Cloudinary_ in this case).

The microservices communicates with each other via the *RabbitMQ* message bus.

The UI is planned to be done as a .NET 6 Blazor app with Progressive Web App (PWA) approach.
