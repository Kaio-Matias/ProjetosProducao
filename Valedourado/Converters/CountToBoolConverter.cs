using System.Globalization;

namespace Valedourado.Converters
{
    // Este conversor transforma um número (como a contagem de itens de uma lista)
    // em um booleano (true se o número for maior que 0, false caso contrário).
    // Usamos isso para esconder/mostrar a secção de quantidades nas telas de Perdas e Eficiência.
    public class CountToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                return count > 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}