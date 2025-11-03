using System.Globalization;
using Microsoft.Maui.Graphics;

namespace Valedourado.Supervisor.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        // MELHORIA: O conversor agora busca as cores do ResourceDictionary da aplicação
        // e atribui cores específicas para diferentes status, tornando a UI mais rica.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string statusKey = value as string;

            // Usa um switch para facilitar a adição de novos status no futuro.
            string resourceKey = statusKey switch
            {
                "Aberto" => "SuccessColor", // Verde
                "Fechado" => "Primary",      // Laranja (cor da marca)
                "Cancelado" => "DangerColor",// Vermelho
                _ => "Gray500"               // Cinza para qualquer outro status
            };

            // Tenta buscar a cor a partir da chave do recurso.
            if (Application.Current.Resources.TryGetValue(resourceKey, out var colorValue) && colorValue is Color color)
            {
                return color;
            }

            // Retorna uma cor de fallback caso a chave não seja encontrada.
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}