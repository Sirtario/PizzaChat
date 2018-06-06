using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PIZZA.Client
{
    /// <summary>
    /// Interaktionslogik für GetText.xaml
    /// </summary>
    public partial class GetText : Window
    {
        public GetText(string title, string windowTitle)
        {
            InitializeComponent();

            _label.Content = title;
            Title = windowTitle;
        }

        public string Value => _textBox.Text;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Accept();
        }

        private void Accept()
        {
            DialogResult = true;

            Close();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Accept();
            }
        }
    }
}
