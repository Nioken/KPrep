using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
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
using WpfApp1.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminForm.xaml
    /// </summary>
    public partial class AdminForm : System.Windows.Window
    {
        public void UpdateGrid()
        {
            WorkersGrid.ItemsSource = MyDBContext.DBContext.Workers.ToList();
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
        public void UpdateGridAfterSearch(List<Worker> Source)
        {
            WorkersGrid.ItemsSource = Source;
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
        public AdminForm(string Name,string Surname,string Lastname,string Specialize)
        {
            InitializeComponent();
            this.Title = "Администратор: " + Name + " " + Surname + " " + Lastname;
            this.InfoLabel.Content = "Вы вошли как: " + Specialize;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGrid();
            SearchBox.Text = "Поиск по ФИО";
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
            if (WorkersGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника для удаления!");
                return;
            }
            try
            {
                var itm = (Worker)WorkersGrid.SelectedItem;
                var tmp = (
        from tmpWorker in MyDBContext.DBContext.Workers.ToList<Worker>()
        where tmpWorker.WorkerID.CompareTo(itm.WorkerID) == 0
        select tmpWorker
              ).ToList();
                MyDBContext.DBContext.Workers.Remove(tmp[0]);
                MyDBContext.DBContext.SaveChanges();
                UpdateGrid();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                MessageBox.Show(ex.Message + "\n\nВозможно запись которую вы пытаетесь удалить, имеет связанные записи в БД.");
            }
}

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkersGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите строку для редактирования!");
                return;
            }
            var itm = (Worker)WorkersGrid.SelectedItem;
            Worker EditWorker = MyDBContext.DBContext.Workers.Find(itm.WorkerID);
            WorkerAddForm workerAddForm = new WorkerAddForm(this,EditWorker);
            workerAddForm.ShowDialog();
        }

        public struct WorkerSearchInfo
        {
            public int ID { get; set; }
            public string NamesInfo { get; set; }
            public WorkerSearchInfo(int ID, string NamesInfo) :this()
            {
                this.ID = ID;
                this.NamesInfo = NamesInfo;
            } 
        }
        public List<Worker> SearchWorkers(string SearchValue)
        {
            var DefaultWorkersList = MyDBContext.DBContext.Workers.ToList();
            List<WorkerSearchInfo> workerSearchInfos = new List<WorkerSearchInfo>();
            List<Worker> SearchedWorkers = new List<Worker>();
            for(int i = 0; i < DefaultWorkersList.Count; i++)
            {
                workerSearchInfos.Add(new WorkerSearchInfo(DefaultWorkersList[i].WorkerID, DefaultWorkersList[i].Surname + " " + DefaultWorkersList[i].Name + " " + DefaultWorkersList[i].Lastname));
            }
            for(int i = 0; i < workerSearchInfos.Count; i++)
            {
                if (workerSearchInfos[i].NamesInfo.Contains(SearchValue))
                {
                    SearchedWorkers.Add(MyDBContext.DBContext.Workers.Find(workerSearchInfos[i].ID));
                }
            }
            return SearchedWorkers;
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(SearchBox.Text.Length >= 3 && SearchBox.Text != "Поиск по ФИО")
            {
                List<Worker> SearchedWorkers = new List<Worker>();
                string SearchValue = SearchBox.Text;
                Thread SearchThread = new Thread(() => SearchedWorkers = SearchWorkers(SearchValue));
                SearchThread.Start();
                SearchThread.Join();
                UpdateGridAfterSearch(SearchedWorkers);
            }
            else
            {
                UpdateGrid();
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = String.Empty;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "Поиск по ФИО";
        }
        public void exportToXls()
        {
            Excel._Application exApp = new Excel.Application();
            exApp.Workbooks.Add();
            Worksheet workSheet = (Worksheet)exApp.ActiveSheet;
            workSheet.Cells[1].EntireRow.Font.Bold = true;
            workSheet.Cells.EntireRow.Font.Size = 14;
            workSheet.Cells.EntireRow.Font.Name = "Arial";
            workSheet.Cells[1, 1] = "Фамилия";
            workSheet.Cells[1, 2] = "Имя";
            workSheet.Cells[1, 3] = "Отчество";
            workSheet.Cells[1, 4] = "Должность";
            workSheet.Cells[1, 5] = "Дата рождения";
            workSheet.Cells[1, 6] = "Стаж";
            workSheet.Cells[1, 7] = "Телефон";
            workSheet.Cells[1, 8] = "Логин";
            workSheet.Cells[1, 9] = "Пароль";
            int i = 2;

            foreach (Worker worker in MyDBContext.DBContext.Workers.ToList())
            {
                workSheet.Cells[i, 1] = worker.Surname;
                workSheet.Cells[i, 2] = worker.Name;
                workSheet.Cells[i, 3] = worker.Lastname;
                workSheet.Cells[i, 4] = worker.Specialize;
                workSheet.Cells[i, 5] = worker.Birthday.ToShortDateString();
                workSheet.Cells[i, 6] = worker.Expirience;
                workSheet.Cells[i, 7] = worker.Phone;
                workSheet.Cells[i, 8] = worker.Login;
                workSheet.Cells[i, 9] = worker.Password;
                i++;
            }
            string pathToXlsFile = Environment.CurrentDirectory + "\\WorkersReport.xls";
            workSheet.SaveAs(pathToXlsFile);
            exApp.Quit();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            exportToXls();
        }
    }

}
