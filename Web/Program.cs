using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneAuth.UI.Pages;
using OneAuth.Web.Components;
using OneAuth.Web.Components.Account;
using OneAuth.Web.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<OneAuthDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<OneAuthDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
                .AddInMemoryClients(new Client[] {
                    new Client
                    {
                        ClientId = "client",
                        AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
                        ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())},
                        RedirectUris = { "https://localhost:5002/signin-oidc" },
                        PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
                        FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",
                        AllowedScopes = { "openid", "profile", "email", "phone" },


                    }
                })
                .AddInMemoryIdentityResources(new IdentityResource[] {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email(),
                    new IdentityResources.Phone(),
                })
                .AddAspNetIdentity<ApplicationUser>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting();
app.UseAntiforgery();
app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(OneAuth.UI._Imports).Assembly);

app.MapAdditionalIdentityEndpoints();

app.Run();
