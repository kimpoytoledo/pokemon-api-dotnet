
# Pokémon API

## Project Scope
This project provides a simple REST API to fetch Pokémon details using .NET 6. It utilizes the PokéAPI to retrieve information about a Pokémon when given a Pokémon name. The project demonstrates the use of Dependency Injection, caching with Redis, Docker containers, and unit testing in a .NET 6 application.

## Project Structure
- `PokemonAPI`: Main project directory containing the source code.
  - `Controllers`: Contains the `PokemonController` which handles incoming HTTP requests.
  - `Models`: Contains the `PokemonInfo` model representing the data structure of a Pokémon.
  - `Services`: Contains service interfaces and implementations for fetching Pokémon data and caching.
- `PokemonAPITests`: Contains unit tests for the `PokemonController` and `PokemonService`.
- `Dockerfile`: Docker file to build a Docker image for the application.
- `docker-compose.yml`: Docker Compose file to define and run multi-container Docker applications.

## Architecture
The project follows a simple architecture pattern:
- **Controller**: Handles incoming HTTP requests, invokes services, and returns HTTP responses.
- **Services**: Contains business logic for fetching and caching Pokémon data.
- **Models**: Defines the data structure.

## Prerequisites
- .NET 6 SDK
- Docker (if you want to run the application in a container)
- Redis (for caching, can be run as a Docker container)
- A modern IDE such as Visual Studio or Visual Studio Code.

## Usage
1. **Running the application**:
   - Use the command line or an IDE to run the application.
   ```bash
   dotnet run --project PokemonAPI/PokemonAPI.csproj
   ```

2. **Accessing the API**:
   - Once the application is running, you can access the API at `http://localhost:5000/pokemon/{name}` replacing `{name}` with the name of the Pokémon you want to look up.

3. **Running with Docker**:
   - Build and run the application using Docker Compose.
   ```bash
   docker-compose up --build
   ```

4. **Running the tests**:
   - Use the command line to run the unit tests.
   ```bash
   dotnet test
   ```
