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

namespace WpfApp1.Forms.DoctorForms
{
    /// <summary>
    /// Логика взаимодействия для DoctorTicketsList.xaml
    /// </summary>
    public partial class DoctorTicketsList : Window
    {
        Client SelectedClient;
        public DoctorTicketsList(Client SelectedClient)
        {
            InitializeComponent();
            this.SelectedClient = SelectedClient;
        }
        public void UpdateList()
        {
            var tmp = (
    from tmpTicker in MyDBContext.DBContext.Tickets.ToList<Ticket>()
    where tmpTicker.Client == SelectedClient
    select tmpTicker
          ).ToList();
            List<string> TicketsStrings = new List<string>();
            for (int i = 0; i < tmp.Count; i++)
            {
                TicketsStrings.Add(tmp[i].TicketID + " | " + tmp[i].WorkDate + " | " + tmp[i].WorkTime + " | " + tmp[i].Client.Surname + " " + tmp[i].Client.Name + " " + tmp[i].Client.Lastname + " | " + tmp[i].PaidServices.Name);
            }
            TicketListBox.ItemsSource = TicketsStrings;
        }

        private void TicketListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var itm = TicketListBox.SelectedItem.ToString();
            string[] temp = itm.Split(' ');
            var tmp = (
    from tmpTicker in MyDBContext.DBContext.Tickets.ToList<Ticket>()
    where tmpTicker.TicketID.CompareTo(Convert.ToInt32(temp[0])) == 0
    select tmpTicker
          ).ToList();
            ResultForm RF = new ResultForm(tmp[0]);
            RF.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            UpdateList();
        }
    }
}
