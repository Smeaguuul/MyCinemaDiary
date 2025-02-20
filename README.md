# MyCinemaDiary

To get it running, you need to acquire a TVDB api-key.
Then add a secrets.json file in the root directory. It should be setup like this:
{
    "tvdb_api_key": "example12355",
}
User API
Overview

The User API provides endpoints for user registration and authentication. Users can create an account and log in to receive a JWT token for secure access.
Endpoints
1. Register a New User

    Endpoint: POST /register
    Description: Registers a new user in the system.

Request

    Content-Type: application/json
    Body:
        name (string): The full name of the user.
        username (string): The desired username for the user.
        password (string): The password for the user account.

Responses

    200 OK: User registered successfully.
        Example:

json

    {
      "message": "User registered successfully"
    }

400 Bad Request: If registration fails due to validation errors or other issues.

    Example:

json

        {
          "error": "Error message describing the issue"
        }

2. User Login

    Endpoint: POST /login
    Description: Authenticates a user and returns a JWT token.

Request

    Content-Type: application/json
    Body:
        username (string): The username of the user.
        password (string): The password of the user.

Responses

    200 OK: Successful login, returns a JWT token.
        Example:

json

    {
      "token": "your_jwt_token_here"
    }

401 Unauthorized: If login fails due to invalid credentials.

    Example:

json

{
  "error": "Invalid username or password"
}
