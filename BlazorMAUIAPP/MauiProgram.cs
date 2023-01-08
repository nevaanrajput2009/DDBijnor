using Blazored.LocalStorage;
using DD_SharedUI.Serivce;
using DD_SharedUI.Serivce.IService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using MudBlazor;
using MudBlazor.Services;

namespace BlazorMAUIAPP;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});
        var configuration = builder.Configuration;
        var baseServerUrlConfig = new Dictionary<string, string>
        {
            { "BaseServerUrl", "https://ddbijnor.com" }
        };
        configuration.AddInMemoryCollection(baseServerUrlConfig);

        builder.Services.AddMauiBlazorWebView();
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddMudServices();
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = 
            new Uri("https://ddapi.ddbijnor.com") });

        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;

            config.SnackbarConfiguration.PreventDuplicates = true;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();


        return builder.Build();
	}
}
