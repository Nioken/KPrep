using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
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
using WpfApp1.Forms.RegistratorForms;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp1.Forms.DoctorForms
{
    /// <summary>
    /// Логика взаимодействия для DoctorMain.xaml
    /// </summary>
    public partial class DoctorMain : System.Windows.Window
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
        public void UpdateGridAfterSearch(List<Client> Source)
        {
            ClientsGrid.ItemsSource = Source;
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

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = String.Empty;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "Поиск по ФИО";
        }

        public struct ClientSearchInfo
        {
            public int ID { get; set; }
            public string NamesInfo { get; set; }
            public ClientSearchInfo(int ID, string NamesInfo) : this()
            {
                this.ID = ID;
                this.NamesInfo = NamesInfo;
            }
        }
        public List<Client> SearchWorkers(string SearchValue)
        {
            var DefaultClientsList = MyDBContext.DBContext.Clients.ToList();
            List<ClientSearchInfo> clientSearchInfos = new List<ClientSearchInfo>();
            List<Client> SearchedClients = new List<Client>();
            for (int i = 0; i < DefaultClientsList.Count; i++)
            {
                clientSearchInfos.Add(new ClientSearchInfo(DefaultClientsList[i].ClientID, DefaultClientsList[i].Surname + " " + DefaultClientsList[i].Name + " " + DefaultClientsList[i].Lastname));
            }
            for (int i = 0; i < clientSearchInfos.Count; i++)
            {
                if (clientSearchInfos[i].NamesInfo.Contains(SearchValue))
                {
                    SearchedClients.Add(MyDBContext.DBContext.Clients.Find(clientSearchInfos[i].ID));
                }
            }
            return SearchedClients;
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text.Length >= 3 && SearchBox.Text != "Поиск по ФИО")
            {
                List<Client> SearchedClients = new List<Client>();
                string SearchValue = SearchBox.Text;
                Thread SearchThread = new Thread(() => SearchedClients = SearchWorkers(SearchValue));
                SearchThread.Start();
                SearchThread.Join();
                UpdateGridAfterSearch(SearchedClients);
            }
            else
            {
                UpdateGrid();
            }
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
            workSheet.Cells[1, 4] = "Адрес";
            workSheet.Cells[1, 5] = "Дата рождения";
            workSheet.Cells[1, 6] = "Телефон";
            workSheet.Cells[1, 7] = "Пол";
            int i = 2;

            foreach (Client client in MyDBContext.DBContext.Clients.ToList())
            {
                workSheet.Cells[i, 1] = client.Surname;
                workSheet.Cells[i, 2] = client.Name;
                workSheet.Cells[i, 3] = client.Lastname;
                workSheet.Cells[i, 4] = client.Adress;
                workSheet.Cells[i, 5] = client.Birthday.ToShortDateString();
                workSheet.Cells[i, 6] = client.Phone;
                workSheet.Cells[i, 7] = client.Gender;
                i++;
            }
            string pathToXlsFile = Environment.CurrentDirectory + "\\ClientsReport.xls";
            workSheet.SaveAs(pathToXlsFile);
            exApp.Quit();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            exportToXls();
        }
    }
}
