# ServerDownloadGuard
## Table of contents
* [General info](#general-info)
* [Technolgies](#technolgies)
* [Setup](#setup)
## General info
This is server side for Download Guard project. The main goal of app is to allow users to inform others about network usage.
## Technologies
App is build using ASP.NET Core 3.1 and PostgreSQL
## Setup
Open Visual Studio 19 and clone this repository, then follow [docs](https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-iis?view=aspnetcore-5.0&tabs=visual-studio#publish-and-deploy-the-app) to publish and deploy app. Next you will have to add bindings in your IIS Manager to allow other users in network access server. Type should be http, ip adress should be your IPv4 local adress and port should be set to 5000. If everything work correctly you will be able to see "Hello Word" under http://localhost:5000 and http://your-local-adress:5000. Also don't forget about client side you can find [here](https://github.com/Luminatione/ClientDownloadGuard).
