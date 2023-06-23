using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SharpMessenger.DbInteraction;
using SharpMessenger.UsersApi.Authentication;
using SharpMessenger.UsersApi.Hubs;
using SharpMessenger.UsersApi.Hubs.Interfaces;
using System.Text;
using Microsoft.AspNetCore.ResponseCompression;

namespace TestUsersApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtAuthenticationManager.JWT_SECURITY_KEY)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorizationCore();

            services.AddCors(setup => setup.AddPolicy("UI_Policy", policy =>
            {
                policy
                    .SetIsOriginAllowed(origin => true)
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }));

            services.AddSignalR();
           

            services.AddSwaggerGen();

            // dependencies from infrastructure layer
            services.AddInteractionWithListDB();
            services.AddUserRepository();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseCors("UI_Policy");

            app.UseRouting();
            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute
                (
                    name: "default",
                    pattern: "{controller=Default}/{action=GetSome}"
                );

                endpoints.MapHub<NotificationHub>("/notification");
            });
        }
    }
}
