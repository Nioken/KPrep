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
using WpfApp1.Classes;

namespace WpfApp1.Forms
{
    /// <summary>
    /// Логика взаимодействия для AddService.xaml
    /// </summary>
    public partial class AddService : Window
    {
        WFAEntity.API.MyDBContext MyDBContext = new WFAEntity.API.MyDBContext();
        ServicesWindow SW;
        PaidServices EditService;
        bool isEdit = false;
        public AddService(ServicesWindow SW)
        {
            InitializeComponent();
            this.SW = SW;
        }
        public AddService(ServicesWindow SW, PaidServices EditService)
        {
            isEdit = true;
            this.EditService = EditService;
            InitializeComponent();
            this.SW = SW;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> DoctorsName = new List<string>();
            var tmp = MyDBContext.Workers.ToList();
            for(int i = 0; i < tmp.Count; i++)
            {
                DoctorsName.Add(tmp[i].Surname + " " + tmp[i].Name + " " + tmp[i].Lastname);
            }
            DoctorsCombo.ItemsSource = DoctorsName;
            if (isEdit)
            {
                PriceBox.Text = EditService.Price.ToString();
                NameBox.Text = EditService.Name;
                DoctorsCombo.Text = EditService.Worker.Surname + " " + EditService.Worker.Name + " " + EditService.Worker.Lastname;
                AddButton.Content = "Сохранить";
            }
        }
        public Worker GetNeedWorker(List<Worker> List)
        {
            string[] Names = DoctorsCombo.Text.Split(' ');
            for(int i = 0; i < List.Count; i++)
            {
                if(List[i].Name == Names[1] && List[i].Surname == Names[0] && List[i].Name == Names[2])
                {
                    return List[i];
                }
            }
            return List[0];
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isEdit)
            {
                if (!String.IsNullOrWhiteSpace(NameBox.Text) && !String.IsNullOrWhiteSpace(PriceBox.Text))
                {
                    PaidServices ps = new PaidServices();
                    ps.ServiceID = MyDBContext.PaidServices.Count();
                    ps.ServiceID++;
                    ps.Name = NameBox.Text;
                    ps.Price = Convert.ToDouble(PriceBox.Text);
                    ps.Worker = GetNeedWorker(MyDBContext.Workers.ToList());
                    MyDBContext.PaidServices.Add(ps);
                    MyDBContext.SaveChanges();
                    NameBox.Text = String.Empty;
                    PriceBox.Text = String.Empty;
                    SW.UpdateGrid();
                }
                else
                {
                    MessageBox.Show("Заполните все поля: ");
                }
            }
            else
            {
                EditService.Price = Convert.ToDouble(PriceBox.Text);
                EditService.Name = NameBox.Text;
                EditService.Worker = GetNeedWorker(MyDBContext.Workers.ToList());
                MyDBContext.PaidServices.AddOrUpdate(EditService);
                MyDBContext.SaveChanges();
                SW.UpdateGrid();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
