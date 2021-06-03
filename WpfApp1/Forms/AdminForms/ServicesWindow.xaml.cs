using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace WpfApp1.Forms
{
    /// <summary>
    /// Логика взаимодействия для ServicesWindow.xaml
    /// </summary>
    public partial class ServicesWindow : Window
    {
        public void UpdateGrid()
        {
            List<ShowServiceStruct> serviceStructs = new List<ShowServiceStruct>();
            var tmp = MyDBContext.DBContext.PaidServices.ToList();
            for (int i = 0; i < tmp.Count; i++)
            {
                serviceStructs.Add(new ShowServiceStruct(tmp[i].ServiceID, tmp[i].Name, tmp[i].Price, tmp[i].Worker.Name + " " + tmp[i].Worker.Surname + " " + tmp[i].Worker.Lastname));
            }
            ServicesGrid.ItemsSource = serviceStructs;
            ServicesGrid.Columns[0].Header = "ID Услуги";
            ServicesGrid.Columns[1].Header = "Наименование";
            ServicesGrid.Columns[2].Header = "Цена";
            ServicesGrid.Columns[3].Header = "Врач";
        }
        
        public struct ShowServiceStruct {
            public int ID { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public string DoctorName { get; set; }
            public ShowServiceStruct(int ID,string Name,double Price,string DoctorName) : this()
            {
                this.ID = ID;
                this.Name = Name;
                this.Price = Price;
                this.DoctorName = DoctorName;
            }
         }
 
        public ServicesWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddService add = new AddService(this);
            add.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ServicesGrid.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите строку для удаления!");
                return;
            }
            try
            {
                var row = (DataGridRow)ServicesGrid.ItemContainerGenerator.ContainerFromIndex(ServicesGrid.SelectedIndex);
                //var SelectedService = row.Item as ShowServiceStruct;
                var itm = (ShowServiceStruct)ServicesGrid.SelectedItem;
                var tmp = (
        from tmpService in MyDBContext.DBContext.PaidServices.ToList<PaidServices>()
        where tmpService.ServiceID.CompareTo(itm.ID) == 0
        select tmpService
              ).ToList();
                MyDBContext.DBContext.PaidServices.Remove(tmp[0]);
                MyDBContext.DBContext.SaveChanges();
                UpdateGrid();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                MessageBox.Show(ex.Message + "\n\nВозможно запись которую вы пытаетесь удалить, имеет связанные записи в БД.");
            }
        }

        //edit
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (ServicesGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите строку для редактирования!");
                return;
            }
            var itm = (ShowServiceStruct)ServicesGrid.SelectedItem;
            PaidServices EditService = MyDBContext.DBContext.PaidServices.Find(itm.ID);
            AddService AddForm = new AddService(this, EditService);
            AddForm.ShowDialog();
        }
    }
}
