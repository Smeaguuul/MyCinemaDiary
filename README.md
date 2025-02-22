# MyCinemaDiary

MyCinemaDiary is a backend service designed to work in conjunction with the [frontend](https://github.com/Smeaguuul/MyCinemaDiary-Frontend).

## Getting Started

To get the application running, follow these steps:

1. **Acquire a TVDB API Key**: You will need a valid TVDB API key to access the necessary data.

2. **Create a `secrets.json` File**: In the root directory of the project, create a file named `secrets.json` with the following structure:

    ```json
    {
        "tvdb_api_key": "example12355"
    }
    ```

3. **Configure Database Connection**: You will also need to adjust the `appsettings.json` file to include your own database connection. The application is currently set up for PostgreSQL.

## Notes
Make sure to replace `"example12355"` with your actual TVDB API key and update the database connection settings as needed.
