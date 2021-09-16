# Freeturilo Backend

The repository contains all aplications essential to run backend part of Freeturilo.

## NextBike Data Parser

Library provided

## Redis Worker Service

Worker service responsible for updating Redis database. More at https://github.com/hiryleu0/FreeturiloRedisWorkerService

# Running in Docker
Before launching make sure you have gone through README files of all above repositories.

Docker support is provided for linux platform. To launch for the first time run:

```
docker-compose up --build
```

Next times you can skip `--build` parameter.