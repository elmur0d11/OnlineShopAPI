# ASP.NET CORE WEBAPI Online Shop API

### Commit 12/30/2024
> [!NOTE]
> Added Unit Tests to Controllers
### Commit 12/28/2024
> [!NOTE]
> Added Hangfire for auto get data about cache. I Add it just for FUN )

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
>
> xUnit


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


