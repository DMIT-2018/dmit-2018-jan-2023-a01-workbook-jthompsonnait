#nullable disable
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlaylistManagementSystem.BLL;
using PlaylistManagementSystem.DAL;

namespace PlaylistManagementSystem
{
    //your class needs to be public so it can be used outside of this project
    //this class also needs to be static
    public static class PlaylistManagementExtension
    {
        //  method name can be anything, it must match
        //      the match the builder.Services.xxxxx(options => ..
        //      statement in your Program.cs

        //  the first parameter is the class that you are attempting
        //      to extend

        //  the second parameter is the options value in your call statement
        //  it is receiving the connectionstring for your application
        public static void PlaylistManagementDependencies(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            //  Register the DBContext class in Chinook2018 with the service collection
            services.AddDbContext<PlaylistManagementContext>(options);

            //  Add any services that you create in the class library
            //  using .AddTransient<t>(...)
            services.AddTransient<PlaylistTrackService>((ServiceProvider) =>
            {
                var context = ServiceProvider.GetService<PlaylistManagementContext>();
                return new PlaylistTrackService(context);
            });
        }
    }
}
