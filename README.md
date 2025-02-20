<h1>MyCinemaDiary</h1>

Should be used in conjunction with the [frontend](https://github.com/Smeaguuul/MyCinemaDiary-Frontend)

To get it running, you need to acquire a TVDB api-key.
Then add a secrets.json file in the root directory. It should be setup like this:
{
    "tvdb_api_key": "example12355",
}

An adjustment is also needed in the appsettings.json, to add your own DB connection. It is currently setup for Postgres.
