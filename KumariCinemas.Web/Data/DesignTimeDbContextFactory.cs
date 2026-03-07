using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KumariCinemas.Web.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CinemaDbContext>
{
    public CinemaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CinemaDbContext>();

        var oracleConnectionString =
            "User Id=YOUR_USER;Password=YOUR_PASSWORD;Data Source=localhost:1521/XEPDB1;";

        optionsBuilder.UseOracle(oracleConnectionString);

        return new CinemaDbContext(optionsBuilder.Options);
    }
}
