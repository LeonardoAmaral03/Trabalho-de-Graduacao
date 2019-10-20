using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerMaintenance.Context;
using ComputerMaintenance.Services;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ComputerMaintenance
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
            services.AddDbContext<AppContextModel>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ComputerMaintenance")));

            // Add Hangfire services
            services.AddHangfire(s => s.UseSqlServerStorage(Configuration.GetConnectionString("HangFire")));
            services.AddHangfireServer();

            services.AddScoped<StatusSchedule>();

            // Make sure you call this previous to AddMvc
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddMvc()
                            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                            .AddJsonOptions(options => {
                                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                            });

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Hangfire
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire");

            RecurringJob.AddOrUpdate<StatusSchedule>(checkStatus => checkStatus.CheckStatus(), Cron.Daily, TimeZoneInfo.Local);

            app.UseHttpsRedirection();

            // Make sure you call this before calling app.UseMvc()
            app.UseCors("ApiCorsPolicy");

            app.UseMvc();
        }
    }
}
