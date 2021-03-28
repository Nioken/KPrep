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

namespace WpfApp1.Forms.RegistratorForms
{
    /// <summary>
    /// Логика взаимодействия для RegistratorWindow.xaml
    /// </summary>
    public partial class RegistratorWindow : Window
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ClientAdd form = new ClientAdd(this);
            form.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var itm = (Client)ClientsGrid.SelectedItem;
            MyDBContext.DBContext.Clients.Remove(itm);
            MyDBContext.DBContext.SaveChanges();
            UpdateGrid();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var itm = (Client)ClientsGrid.SelectedItem;
            ClientAdd EditForm = new ClientAdd(this, itm);
            EditForm.ShowDialog();
        }

        private void ClientsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(ClientsGrid.SelectedItem != null){
                TicketAdd ticketAdd = new TicketAdd((Client)ClientsGrid.SelectedItem);
                ticketAdd.ShowDialog();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TicketListForm ListForm = new TicketListForm();
            ListForm.ShowDialog();
        }
    }
}
