using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlaylistManagementSystem.BLL;
using PlaylistManagementSystem.DAL;

namespace PlaylistManagementSystem
{
    //  your class needs to be public so it can be used outside of 
    //      this project
    //  this class also needs to be static
    public static class PlaylistManagementExtension
    {
        //  method name can be anything, however it must match
        //  the builder.Services.XXXX(options =>...)
        //      statement in your program.cs

        //  the first parameter is the class that you are attempting to extend
        //  the second parameter is the options value in your call statement
        //  is is receiving the connection string for your application
        public static void AddBackendDependencies(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            //  register the DBContext class in Chinook2018 with the service collection
            services.AddDbContext<PlaylistManagementContext>(options);

            //  adding any services that you create in the class library
            //  using .AddTransient<t>(...)
            services.AddTransient<PlaylistTrackServices>((ServiceProvider) =>
            {
                var context = ServiceProvider.GetService<PlaylistManagementContext>();
                return new PlaylistTrackServices(context);
            });
        }
    }
}
