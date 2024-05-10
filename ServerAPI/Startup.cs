using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ServerAPI.Data;


namespace ServerAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("crudCS")));
        }
    }
}
