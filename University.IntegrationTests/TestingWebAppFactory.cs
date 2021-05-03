using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using University.Db;
using University.Repositories;
using University.Repositories.Interfaces;

namespace University.IntegrationTests
{
    public sealed class TestingWebAppFactory<T> : WebApplicationFactory<Startup>
    {
        private IServiceProvider _serviceProvider;
        public IRepository Resolve<IRepository>() => _serviceProvider.GetRequiredService<IRepository>();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<UniversityContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<UniversityContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryEmployeeTest");
                    options.UseInternalServiceProvider(serviceProvider);
                });
                services.AddScoped<ICourseRepository, CourseRepository>();
                services.AddScoped<IGroupRepository, GroupRepository>();
                services.AddScoped<IStudentRepository, StudentRepository>();

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                using var appContext = scope.ServiceProvider.GetRequiredService<UniversityContext>();
                try
                {
                    appContext.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    throw;
                }

                _serviceProvider = sp;
            });
        }
    }
}
