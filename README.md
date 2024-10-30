# StarWars.API

## Configuration

Update your `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Jwt": {
    "Key": "your-secure-key-at-least-16-characters",
    "Issuer": "https://localhost:7001",
    "Audience": "http://localhost:3000"
  },
  "CacheSettings": {
    "StarshipsCacheMinutes": 60,
    "ManufacturersCacheMinutes": 120
  }
}
```

## Features

- JWT Authentication
- In-memory caching for SWAPI responses
- Starships listing with manufacturer filtering
- CORS configuration for frontend applications
- Error handling and logging
- Swagger documentation

## API Endpoints

### Authentication

```
POST /api/auth/login
```
Request body:
```json
{
  "username": "demo",
  "password": "demo123"
}
```

### Starships

```
GET /api/starships
GET /api/starships?manufacturer={manufacturerName}
GET /api/starships/manufacturers
```

All endpoints require a valid JWT token in the Authorization header:
```
Authorization: Bearer {your-jwt-token}
```

## Running the Application

1. Development:
```bash
cd StarWarsBFF.API
dotnet run
```

2. Docker (optional):
```bash
docker build -t starwars-bff .
docker run -p 7001:80 starwars-bff
```

The API will be available at:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5832`
- Swagger UI: `https://localhost:7001/swagger`

## Development


## Security Considerations

1. Update the JWT key in production
2. Configure CORS for your production frontend URL
3. Use HTTPS in production
4. Consider rate limiting for the SWAPI calls
5. Implement proper logging in production

## Error Handling

The API uses standard HTTP status codes:
- 200: Success
- 400: Bad Request
- 401: Unauthorized
- 404: Not Found
- 500: Internal Server Error

## Caching

The API implements in-memory caching for:
- Starships data (60 minutes by default)
- Manufacturers list (120 minutes by default)
