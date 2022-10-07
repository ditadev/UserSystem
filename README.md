# UserSystem

A minimalist user system with registration and sign in.

## Usage

1. Set JWT secret and Postgres connection string in config `src/UserSystem.Api/appsettings.local.json`:
    ```sh
    # copy structure from the default config
    cp src/UserSystem.Api/appsettings.json src/UserSystem.Api/appsettings.local.json
    ```

2. Start API service and navigate to Swagger UI: [http://localhost:5254/swagger/index.html](http://localhost:5254/swagger/index.html)

