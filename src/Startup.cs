﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StroopTest.Configuration;
using StroopTest.Interfaces;
using StroopTest.Services;

namespace StroopTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISessionStorage, SessionStorage>();
            services.AddSingleton<IColorProvider, ColorProvider>();
            services.AddSingleton<IColorRepository, ColorRepository>();
            services.AddSingleton<IRandomGenerator, RandomGenerator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            var config = new StroopTestSettings();
            Configuration.Bind("TestSettings", config);
            services.AddSingleton(config);

            // services.Configure<StroopTestSettings>(
            //     Configuration.GetSection("TestSettings"));

            services.AddMvc()
                .AddSessionStateTempDataProvider();

            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=StroopTest}/{action=Index}/{id?}");
            });
        }
    }
}
