using Microsoft.EntityFrameworkCore;
using Athenaeum_API.Models;

namespace Athenaeum_API.Data
{
    public class APIContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public APIContext(DbContextOptions<APIContext> options)
            : base(options)
        {

        }

    }
}

/*   Scaffold-DbContext “Server={DESKTOP-SQ8RU2R}; Database={master};Trusted_Connection=True;” Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

“ConnectionStrings”:{
 “DefaultConnection” : “Server ={DESKTOP-SQ8RU2R}; Database ={master}; Trusted_Connection = True;”
}
*/