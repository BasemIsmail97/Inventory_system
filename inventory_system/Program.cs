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

            #region MVC & Controllers Configuration
            // Register MVC controllers for API endpoints
            builder.Services.AddControllers();

            // Register MVC controllers with views for web pages
            builder.Services.AddControllersWithViews();
            #endregion

            #region Database Configuration
            // Configure Entity Framework Core with SQL Server
            builder.Services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        // Enable automatic retry on transient failures
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null
                        );
                    }
                )
            );
            #endregion

            #region JWT Settings Configuration
            // Bind JWT settings from appsettings.json
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("JWT")
            );
            #endregion

            #region Identity Configuration
            // Configure ASP.NET Core Identity for user management
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password requirements
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // Account lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;

                // Sign in settings
                options.SignIn.RequireConfirmedEmail = false; // Set to true for email verification
            })
            .AddEntityFrameworkStores<InventoryDbContext>()
            .AddDefaultTokenProviders();
            #endregion
            // 1. Add Session & Cookie Configuration (BEFORE Build)
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                // ??? ???? ??? Localhost
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

            // 2. Configure Antiforgery (BEFORE Build)
            builder.Services.AddAntiforgery(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });


            #region JWT Authentication Configuration
            // Get JWT settings from configuration
            var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>();

            // Configure JWT Bearer authentication
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
            #endregion

            #region Authorization Policies
            // Define role-based authorization policies
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
                options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
                options.AddPolicy("AdminOrManager", policy => policy.RequireRole("Admin", "Manager"));
                options.AddPolicy("AllRoles", policy => policy.RequireRole("Admin", "Manager", "Employee"));
            });
            #endregion

            #region Service Manager Registration
            // Register Service Manager - manages all business services
            builder.Services.AddScoped<IServiceManger, ServiceMangerWithFactoryDelegate>();
            
            #endregion

            #region Business Services Registration
            // Register Product Service
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<Func<IProductService>>(provider =>
                () => provider.GetRequiredService<IProductService>()
            );

            // Register Customer Service
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<Func<ICustomerService>>(provider =>
                () => provider.GetRequiredService<ICustomerService>()
            );

            // Register Category Service
            builder.Services.AddScoped<ICategoryServicecs, CategoryService>();
            builder.Services.AddScoped<Func<ICategoryServicecs>>(provider =>
                () => provider.GetRequiredService<ICategoryServicecs>()
            );

            // Register Supplier Service
            builder.Services.AddScoped<ISupplierServicecs, SupplierService>();
            builder.Services.AddScoped<Func<ISupplierServicecs>>(provider =>
                () => provider.GetRequiredService<ISupplierServicecs>()
            );

            // Register Sales Order Service
            builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
            builder.Services.AddScoped<Func<ISalesOrderService>>(provider =>
                () => provider.GetRequiredService<ISalesOrderService>()
            );

            // Register Purchase Order Service
            builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            builder.Services.AddScoped<Func<IPurchaseOrderService>>(provider =>
                () => provider.GetRequiredService<IPurchaseOrderService>()
            );

            // Register Authentication Service
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<Func<IAuthService>>(provider =>
                () => provider.GetRequiredService<IAuthService>()
            );
            // Register Authentication Token Service
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<Func<ITokenService>>(provider =>
                () => provider.GetRequiredService<ITokenService>()
            );
            #endregion

            #region Infrastructure Services Registration
            // Register Unit of Work pattern
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Data Seeding service
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();

            // Register Generic Repository pattern
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            #endregion

            #region AutoMapper Configuration
            // Register AutoMapper for object-to-object mapping
            builder.Services.AddAutoMapper(cfg => { }, typeof(AssemplyReference).Assembly);
            #endregion

            // Build the application
            var app = builder.Build();

            #region Database Seeding
            // Seed initial data into database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Starting database seeding...");
                    var dataSeeding = services.GetRequiredService<IDataSeeding>();

                    // Seed Identity data first (users and roles)
                    await dataSeeding.SeedIdentityDataAsync();
                    logger.LogInformation("Identity data seeded successfully");

                    // Then seed business data (products, categories, etc.)
                    await dataSeeding.SeedDataAsync();
                    logger.LogInformation("Business data seeded successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    if (app.Environment.IsDevelopment())
                    {
                        throw; // Re-throw in development for debugging
                    }
                }
            }
            #endregion

            #region Middleware Pipeline Configuration
            // Configure error handling based on environment
            if (app.Environment.IsDevelopment())
            {
                // Show detailed error page in development
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Use custom error page in production
                app.UseExceptionHandler("/Home/Error");
                // Enable HTTP Strict Transport Security
                app.UseHsts();
            }

            // Redirect HTTP to HTTPS
            app.UseHttpsRedirection();

            // Enable static file serving (CSS, JS, images)
            app.UseStaticFiles();

            // Enable routing
            app.UseRouting();
            #endregion

            #region Routing Configuration
            // Configure Area routing for Admin, Manager, Employee
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
            app.UseSession();

            // Enable authentication middleware - must come before authorization
            app.UseAuthentication();

            // Enable authorization middleware
            app.UseAuthorization();

            app.UseAntiforgery();
            // Map API controllers
            // Redirect root URL to Admin Login page
            app.MapGet("/", context =>
            {
                context.Response.Redirect("/Admin/Auth/Login");
                return Task.CompletedTask;
            });

            app.MapControllers();
            #endregion

            #region Application Startup
            // Log successful application start
            var appLogger = app.Services.GetRequiredService<ILogger<Program>>();
            appLogger.LogInformation("Application started successfully");

            // Run the application
            await app.RunAsync();
            #endregion
        }
    }
}
