using Domain.Contract;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Repositories;
using Services;
using Services.Abstraction.Contract;
using Services.Implementations;
using Shards.Settings;
using System.Text;

namespace inventory_system
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services Configuration

            // Add Controllers
            builder.Services.AddControllers();

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();

            // Database Configuration
            builder.Services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null
                        );
                    }
                )
            );

            // ? JWT Settings Configuration
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("JWT")
            );

           
            // ? Identity Configuration
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;

                // Sign in settings
                options.SignIn.RequireConfirmedEmail = false; // ? Set to true if using email verification
            })
            .AddEntityFrameworkStores<InventoryDbContext>()
            .AddDefaultTokenProviders();

            // ? JWT Authentication Configuration
            var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false; // Set to true in production
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
                    ),
                    ClockSkew = TimeSpan.Zero // Remove delay of token expiration
                };
            });

            #region Product Service 
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<Func<IProductService>>(provider =>

            () => provider.GetRequiredService<IProductService>()

            );
            #endregion
            #region Customer Service 
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<Func<ICustomerService>>(provider =>

            () => provider.GetRequiredService<ICustomerService>()

            );
            #endregion
            #region Category service 
            builder.Services.AddScoped<ICategoryServicecs, CategoryService>();
            builder.Services.AddScoped<Func<ICategoryServicecs>>(provider =>

            () => provider.GetRequiredService<ICategoryServicecs>()

            );
            #endregion
            #region Supplier Service 
            builder.Services.AddScoped<ISupplierServicecs, SupplierService>();
            builder.Services.AddScoped<Func<ISupplierServicecs>>(provider =>

            () => provider.GetRequiredService<ISupplierServicecs>()

            );
            #endregion
            #region Sales Order Service 
            builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
            builder.Services.AddScoped<Func<ISalesOrderService>>(provider =>

            () => provider.GetRequiredService<ISalesOrderService>()

            );
            #endregion
            #region Purchase Order Service 
            builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            builder.Services.AddScoped<Func<IPurchaseOrderService>>(provider =>

            () => provider.GetRequiredService<IPurchaseOrderService>()

            );
            #endregion

            // ? Register Application Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // ? AutoMapper
            builder.Services.AddAutoMapper(cfg => { }, typeof(AssemplyReference).Assembly);

            #endregion

            var app = builder.Build();

            #region Database Seeding

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Starting database seeding...");

                    var dataSeeding = services.GetRequiredService<IDataSeeding>();

                    // Seed Identity data first
                    await dataSeeding.SeedIdentityDataAsync();
                    logger.LogInformation("Identity data seeded successfully");

                    // Then seed business data
                    await dataSeeding.SeedDataAsync();
                    logger.LogInformation("Business data seeded successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");

                    if (app.Environment.IsDevelopment())
                    {
                        throw;
                    }
                }
            }

            #endregion

            #region Middleware Pipeline

            if (app.Environment.IsDevelopment())
            {
               
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

           

            // ? Authentication & Authorization
            app.UseAuthentication(); // Must be before UseAuthorization
            app.UseAuthorization();

            app.MapControllers();

            #endregion
            var appLogger = app.Services.GetRequiredService<ILogger<Program>>();
            appLogger.LogInformation("Application started successfully");

            await app.RunAsync();
        }
    }
}

