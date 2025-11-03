using CommunityToolkit.Maui;
// PASSO 1: Adicione estes usings no topo do arquivo
using Valedourado.Services;
using Valedourado.ViewModels;
using Valedourado.Views;

namespace Valedourado
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // =================================================================
            // ========= INÍCIO DO CÓDIGO DE CAPTURA GLOBAL DE ERRO ============
            // =================================================================

            // Captura exceções não tratadas no código .NET em geral
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                LogUnhandledException(args.ExceptionObject as Exception, "AppDomain");
            };

            // Captura exceções de tarefas (async/await) que não foram observadas
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                LogUnhandledException(args.Exception, "TaskScheduler");
            };

            // Captura exceções específicas da plataforma Android
#if ANDROID
            Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
            {
                LogUnhandledException(args.Exception, "AndroidEnvironment");
                args.Handled = true; // Impede o crash imediato para tentarmos mostrar o alerta
            };
#endif

            // =================================================================
            // ========== FIM DO CÓDIGO DE CAPTURA GLOBAL DE ERRO ==============
            // =================================================================


#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Seu código de registro de serviços continua normal
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddHttpClient<IApiService, ApiService>(client =>
            {
                client.BaseAddress = new Uri("https://api-producao.valedourado.com.br:8087");
            });

            // Registro de ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<AberturaViewModel>();
            builder.Services.AddTransient<OpSelectionViewModel>();
            builder.Services.AddTransient<DetalhamentoViewModel>();
            builder.Services.AddTransient<LossOpSelectionViewModel>();
            builder.Services.AddTransient<RecordLossesViewModel>();
            builder.Services.AddTransient<EfficiencyOpSelectionViewModel>();
            builder.Services.AddTransient<RecordEfficiencyViewModel>();

            // Registro de Páginas (Views)
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<AberturaPage>();
            builder.Services.AddTransient<OpSelectionPage>();
            builder.Services.AddTransient<DetalhamentoPage>();
            builder.Services.AddTransient<LossOpSelectionPage>();
            builder.Services.AddTransient<RecordLossesPage>();
            builder.Services.AddTransient<EfficiencyOpSelectionPage>();
            builder.Services.AddTransient<RecordEfficiencyPage>();


            return builder.Build();
        }

        // =================================================================
        // ===== MÉTODO AUXILIAR PARA LOGAR E EXIBIR O ERRO CAPTURADO ======
        // =================================================================
        private static void LogUnhandledException(Exception ex, string source)
        {
            // Tenta desembrulhar a exceção para pegar o erro real
            if (ex is System.Reflection.TargetInvocationException tie && tie.InnerException != null)
            {
                ex = tie.InnerException;
            }

            if (ex == null) return;

            // Constrói a mensagem de erro detalhada
            string errorMessage = $"Timestamp: {DateTime.Now}\n";
            errorMessage += $"Fonte do Erro: {source}\n\n";
            errorMessage += $"Tipo da Exceção: {ex.GetType().FullName}\n";
            errorMessage += $"Mensagem: {ex.Message}\n\n";
            errorMessage += $"Stack Trace:\n{ex.StackTrace}\n\n";

            // Se houver uma exceção interna, adiciona os detalhes dela também
            if (ex.InnerException != null)
            {
                errorMessage += $"--- DETALHES DA EXCEÇÃO INTERNA ---\n";
                errorMessage += $"Tipo: {ex.InnerException.GetType().FullName}\n";
                errorMessage += $"Mensagem: {ex.InnerException.Message}\n\n";
                errorMessage += $"Stack Trace:\n{ex.InnerException.StackTrace}\n\n";
            }

            errorMessage += "===================================\n\n";

            // Escreve o erro no console de depuração
            System.Diagnostics.Debug.WriteLine("ERRO FATAL CAPTURADO:");
            System.Diagnostics.Debug.WriteLine(errorMessage);

            // Tenta salvar o erro em um arquivo de log no dispositivo
            try
            {
                string logFileName = "Valedourado_FatalErrors.log";
                string logFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, logFileName);

                System.IO.File.AppendAllText(logFilePath, errorMessage);

                System.Diagnostics.Debug.WriteLine($"Erro salvo em: {logFilePath}");
            }
            catch (Exception logEx)
            {
                System.Diagnostics.Debug.WriteLine($"Falha ao escrever no arquivo de log: {logEx.Message}");
            }
        }
    }
}