
using Biblioteca.Areas.Identity.Data;
using Biblioteca.Data;
using Microsoft.EntityFrameworkCore;
//using Biblioteca.Models;
//using Microsoft.AspNetCore.Mvc;

namespace Biblioteca
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            //services.AddMvc(x => x.EnableEndpointRouting = false);
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<BibliotecaContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("BibliotecaContextConnection")));



            //var connectionString = Configuration.GetConnectionString("BibliotecaContextConnection");
            //services.AddDbContext<BibliotecaContext>(options =>
            // options.UseSqlServer(connectionString)); 


            services.AddDefaultIdentity<BibliotecaUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                })
              .AddEntityFrameworkStores<BibliotecaContext>();

            //services.AddTransient<IRepository<BibliotecaLivro>, RepositorioEF<BibliotecaLivro>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

          

        }
    }
}