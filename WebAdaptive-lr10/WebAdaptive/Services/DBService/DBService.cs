using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAdaptive.Services.DBService
{
    public class DBService : DbContext
    {
        public DBService(DbContextOptions<DBService> options) : base(options)
        {

        }
    }
}
