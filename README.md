# UserSystem

A minimalist user system with registration, sign in and role based authentication.

## Usage

1. Set JWT secret and Postgres connection string in config `src/UserSystem.Api/appsettings.local.json`:
    ```sh
    # copy structure from the default config
    cp src/UserSystem.Api/appsettings.json src/UserSystem.Api/appsettings.local.json
    ```

2. Start API service and navigate to Swagger UI: [http://localhost:9000/swagger/index.html](http://localhost:5000/swagger/index.html)

