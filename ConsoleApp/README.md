# Battleships

## How to run locally

### Start development database
```bash
# Use -d for detatched
docker-compose up [-d]
```
### Run your app
Running from IDE works just fine or `./path/to/executable`

## How to develop

### Create migrations
```bash
dotnet ef migrations add InitialMigration --project DAL --startup-project ConsoleGame
```