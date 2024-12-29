# ASP.NET CORE WEBAPI Online Shop API

> [!TIP]
> Added Hangfire for auto get data from db and set it to the cache . I Add it just for FUN )

## USED
> C#
>
> ASP.NET CORE
>
> EntityFramework
>
> IMemoryCache
>
> Serilog
>
> Hangfire

## Before run project!
```
docker-compose up
docker-compose down
docker-compose up -d
```
**Docker used for Seq. For manage Log Files** :shipit:

### To Create PostgreSQL Db
```
Update-Database
```

> [!NOTE]
> If you dont want use log u can easly remove Serilog NuGet package and Remove file Services/Logging/Serilog.cs after that you should cahnge you appsetting.json configurations to >>>

```
"Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
```

> [!TIP]
> You can change configurations of the Serilog on appsettings.json

> [!TIP]
> You can change configurations of the Docker Compose in docker-compose.yml


