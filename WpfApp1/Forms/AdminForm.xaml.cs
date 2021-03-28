using System;
using System.Collections.Generic;
using System.Data;
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
using WpfApp1.Forms;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminForm.xaml
    /// </summary>
    public partial class AdminForm : Window
    {
        public void UpdateGrid()
        {
            WorkersGrid.ItemsSource = MyDBContext.Workers.ToList();
            WorkersGrid.Columns[0].Header = "ID сотрудника";
            WorkersGrid.Columns[1].Header = "Имя";
            WorkersGrid.Columns[2].Header = "Фамилия";
            WorkersGrid.Columns[3].Header = "Отчество";
            WorkersGrid.Columns[4].Header = "Дата рождения";
            WorkersGrid.Columns[5].Header = "Стаж";
            WorkersGrid.Columns[6].Header = "Специальность";
            WorkersGrid.Columns[7].Header = "Номер телефона";
            WorkersGrid.Columns[8].Visibility = Visibility.Hidden;
            WorkersGrid.Columns[9].Visibility = Visibility.Hidden;
            WorkersGrid.Columns[10].Visibility = Visibility.Hidden;
            WorkersGrid.Columns[11].Visibility = Visibility.Hidden;
        }
        WFAEntity.API.MyDBContext MyDBContext = new WFAEntity.API.MyDBContext();
        public AdminForm(string Name,string Surname,string Lastname,string Specialize)
        {
            InitializeComponent();
            this.Title = "Администратор: " + Name + " " + Surname + " " + Lastname;
            this.InfoLabel.Content = "Вы вошли как: " + Specialize;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WorkerAddForm workerAddForm = new WorkerAddForm(this);
            workerAddForm.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ServicesWindow sw = new ServicesWindow();
            sw.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var itm = (Worker)WorkersGrid.SelectedItem;
            var tmp = (
    from tmpWorker in MyDBContext.Workers.ToList<Worker>()
    where tmpWorker.WorkerID.CompareTo(itm.WorkerID) == 0
    select tmpWorker
          ).ToList();
            MyDBContext.Workers.Remove(tmp[0]);
            MyDBContext.SaveChanges();
            UpdateGrid();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var itm = (Worker)WorkersGrid.SelectedItem;
            Worker EditWorker = MyDBContext.Workers.Find(itm.WorkerID);
            WorkerAddForm workerAddForm = new WorkerAddForm(this,EditWorker);
            workerAddForm.ShowDialog();
        }
    }
}
