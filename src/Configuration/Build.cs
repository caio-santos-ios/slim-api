using System.Text;
using api_slim.src.Handlers;
using api_slim.src.Interfaces;
using api_slim.src.Repository;
using api_slim.src.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace api_slim.src.Configuration
{
    public static class Build
    {
        public static void AddBuilderConfiguration(this WebApplicationBuilder builder)
        {
            AppDbContext.ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? ""; 
            AppDbContext.DatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? ""; 
            bool IsSSL;
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IS_SSL")))
            {
                IsSSL = Convert.ToBoolean(Environment.GetEnvironmentVariable("IS_SSL"));
            }
            else
            {
                IsSSL = false;
            }

            AppDbContext.IsSSL = IsSSL;
        }
        public static void AddBuilderAuthentication(this WebApplicationBuilder builder)
        {
            string? SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? "";
            string? Issuer = Environment.GetEnvironmentVariable("ISSUER") ?? "";
            string? Audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? "";
            
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(SecretKey)
                    )
                };
            });
        }
        public static void AddContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<AppDbContext>();
        }
        public static void AddBuilderServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IAuthService, AuthService>();                  
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();    
            builder.Services.AddTransient<IAccountsReceivableService, AccountsReceivableService>();
            builder.Services.AddTransient<IAccountsReceivableRepository, AccountsReceivableRepository>();    
            builder.Services.AddTransient<IGenericTableService, GenericTableService>();
            builder.Services.AddTransient<IGenericTableRepository, GenericTableRepository>();                       
            builder.Services.AddTransient<IAccreditedNetworkService, AccreditedNetworkService>();
            builder.Services.AddTransient<IAccreditedNetworkRepository, AccreditedNetworkRepository>();                        
            builder.Services.AddTransient<IAddressService, AddressService>();
            builder.Services.AddTransient<IAddressRepository, AddressRepository>();                        
            builder.Services.AddTransient<IContactService, ContactService>();
            builder.Services.AddTransient<IContactRepository, ContactRepository>();                        
            builder.Services.AddTransient<ISellerRepresentativeService, SellerRepresentativeService>();
            builder.Services.AddTransient<ISellerRepresentativeRepository, SellerRepresentativeRepository>();      
            builder.Services.AddTransient<IPlanService, PlanService>();
            builder.Services.AddTransient<IPlanRepository, PlanRepository>();
            builder.Services.AddTransient<IProcedureService, ProcedureService>();
            builder.Services.AddTransient<IProcedureRepository, ProcedureRepository>();
            builder.Services.AddTransient<IBillingService, BillingService>();
            builder.Services.AddTransient<IBillingRepository, BillingRepository>();
            builder.Services.AddTransient<ISellerService, SellerService>();
            builder.Services.AddTransient<ISellerRepository, SellerRepository>();
            builder.Services.AddTransient<ICommissionService, CommissionService>();
            builder.Services.AddTransient<ICommissionRepository, CommissionRepository>();
            builder.Services.AddTransient<IServiceModuleService, ServiceModuleService>();
            builder.Services.AddTransient<IServiceModuleRepository, ServiceModuleRepository>();                  
            builder.Services.AddTransient<IProfessionalService, ProfessionalService>();
            builder.Services.AddTransient<IProfessionalRepository, ProfessionalRepository>();                  

            // Handlers
            builder.Services.AddTransient<SmsHandler>();
            builder.Services.AddTransient<MailHandler>();
            builder.Services.AddTransient<CloudinaryHandler>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}