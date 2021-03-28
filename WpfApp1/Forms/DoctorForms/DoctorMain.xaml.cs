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
using WFAEntity.API;
using WpfApp1.Classes;
using WpfApp1.Forms.RegistratorForms;

namespace WpfApp1.Forms.DoctorForms
{
    /// <summary>
    /// Логика взаимодействия для DoctorMain.xaml
    /// </summary>
    public partial class DoctorMain : Window
    {
        public void UpdateGrid()
        {
                ClientsGrid.ItemsSource = MyDBContext.DBContext.Clients.ToList();
                ClientsGrid.Columns[0].Header = "ID Клиента";
                ClientsGrid.Columns[1].Header = "Имя";
                ClientsGrid.Columns[2].Header = "Фамилия";
                ClientsGrid.Columns[3].Header = "Отчество";
                ClientsGrid.Columns[4].Header = "Дата рождения";
                ClientsGrid.Columns[5].Header = "Адрес";
                ClientsGrid.Columns[6].Header = "Телефон";
                ClientsGrid.Columns[7].Header = "Пол";
                ClientsGrid.Columns[8].Visibility = Visibility.Hidden;
        }
        public DoctorMain(string Name, string Surname, string Lastname, string Specialize)
        {
            InitializeComponent();
            this.Title = "Врач: " + Name + " " + Surname + " " + Lastname;
            this.InfoLabel.Content = "Вы вошли как: " + Specialize;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void ClientsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ClientsGrid.SelectedItem != null)
            {
                DoctorTicketsList tickets = new DoctorTicketsList((Client)ClientsGrid.SelectedItem);
                tickets.ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
