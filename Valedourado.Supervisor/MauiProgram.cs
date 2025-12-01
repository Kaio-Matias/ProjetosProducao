using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage; // Adicione este using se ele não existir
using Microsoft.Extensions.Logging;
using Valedourado.Supervisor.Services;
using Valedourado.Supervisor.ViewModels;
using Valedourado.Supervisor.Views;
using System.Net.Http;

namespace Valedourado.Supervisor;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit() // Essencial para os serviços do Toolkit
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font Awesome 7 Free-Solid-900.otf", "FontAwesome");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // --- Configuração dos Serviços ---
        string baseApiUrl = "https://api-producao.valedourado.com.br:8087";

        builder.Services.AddSingleton<IAuthService, AuthService>();

        // --- CORREÇÃO APLICADA: Registro do IFileSaver ---
        // O serviço IFileSaver do Community Toolkit precisa ser registrado.
        builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);

        var httpClientBuilder = builder.Services.AddHttpClient<IApiService, ApiService>(client =>
        {
            client.BaseAddress = new Uri(baseApiUrl);
            client.Timeout = TimeSpan.FromSeconds(20);
        });

#if DEBUG
        httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientDevHandler());
#endif

        // --- Registro de ViewModels ---
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<HistoricoViewModel>();
        builder.Services.AddTransient<OpDetailViewModel>();
        builder.Services.AddTransient<GestaoViewModel>();
        builder.Services.AddTransient<AberturaViewModel>();
        // --- Registro de Views (Páginas) ---
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<HistoricoPage>();
        builder.Services.AddTransient<OpDetailPage>();
        builder.Services.AddTransient<GestaoPage>();
        builder.Services.AddTransient<AberturaPage>();

        return builder.Build();
    }
}

#if DEBUG
internal class HttpClientDevHandler : HttpClientHandler
{
    public HttpClientDevHandler()
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    }
}
#endif