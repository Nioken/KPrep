using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    /// Логика взаимодействия для ClientAdd.xaml
    /// </summary>
    public partial class ClientAdd : Window
    {
        RegistratorWindow RW;
        Client EditClient;
        bool isEdit = false;
        public ClientAdd(RegistratorWindow RW)
        {
            this.RW = RW;
            InitializeComponent();
        }
        public ClientAdd(RegistratorWindow RW, Client EditClient)
        {
            this.RW = RW;
            this.EditClient = EditClient;
            isEdit = true;
            InitializeComponent();
            this.AddButton.Content = "Сохранить";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isEdit)
                {
                    if (!String.IsNullOrWhiteSpace(NameBox.Text) && !String.IsNullOrWhiteSpace(SurnameBox.Text) &&
                        !String.IsNullOrWhiteSpace(LastNameBox.Text) && !String.IsNullOrWhiteSpace(AdressBox.Text) &&
                        ClientDatePicker.SelectedDate != null && !String.IsNullOrWhiteSpace(PhoneBox.Text))
                    {
                        Client client = new Client();
                        client.ClientID = MyDBContext.DBContext.Clients.Count();
                        client.ClientID++;
                        client.Name = NameBox.Text;
                        client.Surname = SurnameBox.Text;
                        client.Lastname = LastNameBox.Text;
                        client.Adress = AdressBox.Text;
                        client.Birthday = (DateTime)ClientDatePicker.SelectedDate;
                        client.Phone = PhoneBox.Text;
                        if ((bool)MaleRadio.IsChecked)
                        {
                            client.Gender = "Мужской";
                        }
                        else
                        {
                            client.Gender = "Женский";
                        }
                        MyDBContext.DBContext.Clients.Add(client);
                        MyDBContext.DBContext.SaveChanges();
                        RW.UpdateGrid();
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля!");
                    }
                }
                else
                {
                    EditClient.Name = NameBox.Text;
                    EditClient.Surname = SurnameBox.Text;
                    EditClient.Lastname = LastNameBox.Text;
                    EditClient.Adress = AdressBox.Text;
                    EditClient.Phone = PhoneBox.Text;
                    EditClient.Birthday = (DateTime)ClientDatePicker.SelectedDate;
                    if (MaleRadio.IsChecked == true)
                    {
                        EditClient.Gender = "Мужской";
                    }
                    else
                    {
                        EditClient.Gender = "Женский";
                    }
                    MyDBContext.DBContext.Clients.AddOrUpdate(EditClient);
                    RW.UpdateGrid();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddButton_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isEdit)
            {
                NameBox.Text = EditClient.Name;
                SurnameBox.Text = EditClient.Surname;
                LastNameBox.Text = EditClient.Lastname;
                AdressBox.Text = EditClient.Adress;
                PhoneBox.Text = EditClient.Phone;
                ClientDatePicker.SelectedDate = EditClient.Birthday;
                if(EditClient.Gender == "Мужской")
                {
                    MaleRadio.IsChecked = true;
                }
                else
                {
                    FemaleRadio.IsChecked = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
