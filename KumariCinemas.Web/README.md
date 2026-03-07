# Kumari Cinemas - Milestone 2 (ASP.NET Core MVC)

## Implemented modules
- Movies: full CRUD
- Showtimes: full CRUD
- Users: full CRUD
- Halls: full CRUD
- Bookings: list + create + edit + delete with hall-capacity check
- Reports: bookings per movie and revenue per showtime

## Data provider modes
Configuration is in `appsettings.json`:
- `DataProvider: InMemory` (default, runs without DB setup)
- `DataProvider: Oracle` (connects to Oracle using `ConnectionStrings:OracleDb`)

Example Oracle connection string:
```
User Id=YOUR_USER;Password=YOUR_PASSWORD;Data Source=localhost:1521/XEPDB1;
```

When switching to Oracle:
1. Set `DataProvider` to `Oracle` in `appsettings.json`
2. Set a valid `ConnectionStrings:OracleDb`
3. Make sure your Oracle schema already contains the tables from `KumariCinemas.sql`

## Run the app
From workspace root:
```
dotnet run --project KumariCinemas.Web/KumariCinemas.Web.csproj
```
Then open the local URL shown in terminal.

## EF migration commands (optional)
If your teacher asks for EF migration evidence:
```
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialOracle --project KumariCinemas.Web/KumariCinemas.Web.csproj
dotnet ef database update --project KumariCinemas.Web/KumariCinemas.Web.csproj
```

Note: Current startup auto-creates only for InMemory mode. Oracle mode expects a real Oracle schema.

## Suggested milestone evidence
- Screenshot each module list page and at least one Create/Edit form
- Show one failed delete due to referential safety check
- Show booking creation and resulting booking list
- Include your existing SQL script (`KumariCinemas.sql`) and query output (`Query_Results.txt`)
- Include `KumariCinemas_Milestone2.sql` and filled `Milestone2_Query_Results_Template.txt`
