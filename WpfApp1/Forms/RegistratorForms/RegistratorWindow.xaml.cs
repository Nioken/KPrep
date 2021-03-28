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

namespace WpfApp1.Forms.RegistratorForms
{
    /// <summary>
    /// Логика взаимодействия для RegistratorWindow.xaml
    /// </summary>
    public partial class RegistratorWindow : Window
    {
        public RegistratorWindow(string Name, string Surname, string Lastname, string Specialize)
        {
            InitializeComponent();
            this.Title = "Регистратор: " + Name + " " + Surname + " " + Lastname;
            this.InfoLabel.Content = "Вы вошли как: " + Specialize;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
