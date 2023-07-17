using AbsolCase.Configurations;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyCaseApi.Controllers;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using MyCaseApi.Repositories;
using MyCaseApi.ViewModels;
using Newtonsoft.Json.Converters;
using System;
using System.Diagnostics;
using System.IO;

namespace MyCaseApi
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
            services.AddIdentity<User, IdentityRole>(options =>
            {
                // Configure identity options here.
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddEntityFrameworkStores<ApiDbContext>()
            .AddDefaultTokenProviders();


            services.AddScoped<IJobTestService, JobTestService>();
            string sConnectionString = Configuration.GetConnectionString("ConnectionString");
            services.AddHangfire(x => x.UseSqlServerStorage(sConnectionString));
            services.AddHangfireServer();
            
            
            services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(3));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JWTBearer";
                options.DefaultChallengeScheme = "JWTBearer";
            })
            .AddJwtBearer("JWTBearer", JWToptions =>
            {
                JWToptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = UserManagementController.signinKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(5)
                };
            });
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMvc()
            .AddSessionStateTempDataProvider();
            services.AddSession();
            services.AddMemoryCache();
            services.AddSwaggerDocument();

            // register repositeries
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IPricePlanService, PricePlanService>();
            services.AddTransient<IConfigManagement, ConfigManagement>();
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<ICaseService, CaseService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<ILeadService, LeadService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<ITimeLineService, TimeLineService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IFinancialDetailsService, FinancialDetailsService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IClientTransactionService, ClientTransactionService>();
            services.AddTransient<IRequestUserService, RequestUserService>();
            services.AddTransient<IActivityService, ActivityServices>();
            services.AddTransient<IFirmService, FirmService>();
            services.AddTransient<INotesService, NotesServices>();
            services.AddTransient<IEventsService, EventsServices>();
            services.AddTransient<IExpenseService, ExpenseServices>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IDecumentService, DecumentServices>();
            services.AddTransient<IFaqServices, FaqServices>();
            services.AddTransient<IContactUsService, ContactUsService>();
            services.AddTransient<IAttorneyAdmin, AttorneyAdmin>();
            services.AddTransient<IDMSService, DMSService>();
            services.AddTransient<IWorkflowService, WorkflowService>();
            services.AddScoped<EncryptDecrypt>();
            services.AddScoped<EmailService>();
            services.AddScoped<DocuSignHub>();
            services.AddDbContext<ApiDbContext>(item => item.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyCaseApi", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
             Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/Images"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
             Path.Combine(Directory.GetCurrentDirectory(), "UploadDecuments")),
                RequestPath = "/UploadDecuments"
            });

            app.UseSession();
            app.UseAuthentication();
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "MyCaseApi v1"));

            }
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("MyPolicy");
            app.UseHangfireDashboard("/mydashboard");
            //app.UseHangfireServer();
            //RecurringJob.AddOrUpdate(
            //    () => Debug.WriteLine("Minutely Job"), Cron.Minutely);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
