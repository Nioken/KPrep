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
using Xceed.Wpf.Toolkit;

namespace WpfApp1.Forms.RegistratorForms
{
    /// <summary>
    /// Логика взаимодействия для TicketAdd.xaml
    /// </summary>
    public partial class TicketAdd : Window
    {
        Client SelectedClient;
        public TicketAdd(Client SelectedClient)
        {
            this.SelectedClient = SelectedClient;
            InitializeComponent();
            var ServLsit = MyDBContext.DBContext.PaidServices.ToList();
            List<string> Names = new List<string>();
            for (int i = 0; i < ServLsit.Count; i++)
            {
                Names.Add(ServLsit[i].Name);
            }
            ServicesCombo.ItemsSource = Names;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (WorkDatePicker.SelectedDate != null)
            {
                Ticket ticket = new Ticket();
                ticket.TicketID = MyDBContext.DBContext.Tickets.Count();
                ticket.TicketID++;
                ticket.Client = SelectedClient;
                ticket.TicketNumber = ticket.TicketID;
                ticket.WorkDate = WorkDatePicker.SelectedDate.Value.ToShortDateString();
                ticket.WorkTime = WorkTimePicker.Text;
                var tmp = (
                    from tmpService in MyDBContext.DBContext.PaidServices.ToList<PaidServices>()
                    where tmpService.Name.CompareTo(ServicesCombo.Text) == 0
                    select tmpService
                    ).ToList();
                ticket.PaidServices = tmp[0];
                MyDBContext.DBContext.Tickets.Add(ticket);
                MyDBContext.DBContext.SaveChanges();
                this.Close();
            }
        }
    }
}
