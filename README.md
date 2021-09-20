# Freeturilo Backend

The repository contains all applications essential to run backend part of Freeturilo.

## NextBike Data Parser

Library providing classes for proper XML deserialization of data from NextBike API.

## NextBike Data Presenter

Simple React application presenting changes notified by Redis Worker Service.

## Redis Worker Service

Worker service responsible for updating numbers of available bikes. 

# Requirements
* `ASP.NET Core 5.0`  
* `.NET SDK 5.0` <br/>
    Only if not running in docker (https://dotnet.microsoft.com/download/dotnet/5.0).
* `xsd` <br/>
    Supposing your current working directory is FreeturiloBackend run:
    ```
    cd ./NextBikeDataParser
    xsd ./markers.xsd /classes /namespace:NextBikeDataParser
    cd ..
    ```

* `Docker` https://www.docker.com
* `Docker compose` https://docs.docker.com/compose <br/>
    For running in docker.


# Running in Docker

Docker support is provided. To launch for the first time run:

```
docker-compose up --build
```

Next time one can run:
```
docker-compose up
```