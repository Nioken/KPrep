using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp1.Forms.AdminForms
{
    /// <summary>
    /// Логика взаимодействия для BDConfigForm.xaml
    /// </summary>
    public partial class BDConfigForm : Window
    {
        public BDConfigForm()
        {
            InitializeComponent();
        }
        public struct WorkerStruct
        {
            public int WorkerID { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Lastname { get; set; }
            public DateTime Birthday { get; set; }
            public int Expirience { get; set; }
            public string Specialize { get; set; }
            public string Phone { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public int AccessLevelID { get; set; }
            public WorkerStruct(int WorkerID, string Name, string Surname, string Lastname, DateTime Birthday, int Expirience,
                string Specialize, string Phone, string Login, string Password, int AccessLevelID) :this()
            {
                this.WorkerID = WorkerID;
                this.Name = Name;
                this.Surname = Surname;
                this.Lastname = Lastname;
                this.Birthday = Birthday;
                this.Expirience = Expirience;
                this.Specialize = Specialize;
                this.Phone = Phone;
                this.Login = Login;
                this.Password = Password;
                this.AccessLevelID = AccessLevelID;
            }
        }
        public struct PaidServicesStruct
        {
            public int ServiceID { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public int WorkerID { get; set; }
            public PaidServicesStruct(int ServiceID, string Name, double Price, int WorkerID) :this()
            {
                this.ServiceID = ServiceID;
                this.Name = Name;
                this.Price = Price;
                this.WorkerID = WorkerID;
            }
        }
        public struct TicketStruct
        {
            public int TicketID { get; set; }
            public int TicketNumber { get; set; }
            public string WorkDate { get; set; }
            public string WorkTime { get; set; }
            public int ClientID { get; set; }
            public int PaidServicesID { get; set; }
            public TicketStruct(int TicketID,int TicketNumber, string WorkDate,string WorkTime,int ClientID,int PaidServicesID):this()
            {
                this.TicketID = TicketID;
                this.TicketNumber = TicketNumber;
                this.WorkDate = WorkDate;
                this.WorkTime = WorkTime;
                this.ClientID = ClientID;
                this.PaidServicesID = PaidServicesID;
            }
        }
        public struct ResultStruct
        {
            public int ResultID { get; set; }
            public string ResultText { get; set; }
            public int TicketID { get; set; }
            public ResultStruct(int ResultID,string ResultText,int TicketID) : this()
            {
                this.ResultID = ResultID;
                this.ResultText = ResultText;
                this.TicketID = TicketID;
            }
        }
        public void ChangeProgress(double value)
        {
            PortProgress.Value = value;
        }
        public void ExportData()
        {

            Dispatcher.Invoke((Action)(()=>ChangeProgress(0)));
            Microsoft.Win32.SaveFileDialog fileDialog = new Microsoft.Win32.SaveFileDialog();
            var res = (bool)fileDialog.ShowDialog();
            if (!res) return;
            string FilePath = fileDialog.FileName;
            using (StreamWriter file = new StreamWriter(FilePath))
            {
                file.Write('[');
                List<WorkerStruct> Workers = new List<WorkerStruct>();
                var Wrks = MyDBContext.DBContext.Workers.ToList();
                for (int i = 0; i < Wrks.Count; i++)
                {
                    Workers.Add(new WorkerStruct(Wrks[i].WorkerID, Wrks[i].Name, Wrks[i].Surname, Wrks[i].Lastname, Wrks[i].Birthday, Wrks[i].Expirience,
                        Wrks[i].Specialize, Wrks[i].Phone, Wrks[i].Login, Wrks[i].Password, Wrks[i].AccessLevel.AccessLevelID));
                }
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, MyDBContext.DBContext.AccessLevels.ToList());
                file.Write(',');
                Dispatcher.Invoke((Action)(() => ChangeProgress(16.3)));
                serializer.Serialize(file, Workers);
                file.Write(',');
                Dispatcher.Invoke((Action)(() => ChangeProgress(32.3)));
                serializer.Serialize(file, MyDBContext.DBContext.Clients);
                file.Write(',');
                Dispatcher.Invoke((Action)(() => ChangeProgress(48.6)));
                List<PaidServicesStruct> PaidServicesStructs = new List<PaidServicesStruct>();
                var Services = MyDBContext.DBContext.PaidServices.ToList();
                for (int i = 0; i < Services.Count; i++)
                {
                    PaidServicesStructs.Add(new PaidServicesStruct(Services[i].ServiceID, Services[i].Name, Services[i].Price, Services[i].Worker.WorkerID));
                }
                serializer.Serialize(file, PaidServicesStructs);
                file.Write(',');
                Dispatcher.Invoke((Action)(() => ChangeProgress(64.9)));
                List<TicketStruct> ticketStructs = new List<TicketStruct>();
                var Tics = MyDBContext.DBContext.Tickets.ToList();
                for (int i = 0; i < Tics.Count; i++)
                {
                    ticketStructs.Add(new TicketStruct(Tics[i].TicketID, Tics[i].TicketNumber, Tics[i].WorkDate, Tics[i].WorkTime, Tics[i].Client.ClientID, Tics[i].PaidServices.ServiceID));
                }
                serializer.Serialize(file, ticketStructs);
                file.Write(',');
                Dispatcher.Invoke((Action)(() => ChangeProgress(81.2)));
                List<ResultStruct> results = new List<ResultStruct>();
                var ress = MyDBContext.DBContext.Results.ToList();
                for (int i = 0; i < ress.Count; i++)
                {
                    results.Add(new ResultStruct(ress[i].ResultID, ress[i].ResultText, ress[i].Ticket.TicketID));
                }
                serializer.Serialize(file, results);
                file.Write(']');
                Dispatcher.Invoke((Action)(() => ChangeProgress(100)));
                MessageBox.Show("Экспорт прошел успешно!");
            }
        }
        public void ImportData()
        {
            Dispatcher.Invoke((Action)(() => ChangeProgress(0)));
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            var res = (bool)fileDialog.ShowDialog();
            if (!res) return;
            string FilePath = fileDialog.FileName;
            MyDBContext.DBContext.Database.Delete();
            MyDBContext.DBContext.Database.Create();
            using (StreamReader file = new StreamReader(FilePath))
            {
                string JSONdata = file.ReadToEnd();
                JArray JSON = JArray.Parse(JSONdata);
                dynamic AccessLevelsJson = JSON[0];
                for(int i = 0; i < AccessLevelsJson.Count; i++)
                {
                    AccessLevel al = new AccessLevel();
                    al.AccessLevelID = AccessLevelsJson[i].AccessLevelID;
                    al.Level = AccessLevelsJson[i].Level;
                    MyDBContext.DBContext.AccessLevels.Add(al);
                    MyDBContext.DBContext.SaveChanges();
                }
                Dispatcher.Invoke((Action)(() => ChangeProgress(16.3)));
                //Workers
                dynamic WorkersJSON = JSON[1];
                for (int i = 0; i < AccessLevelsJson.Count; i++)
                {
                    Worker worker = new Worker();
                    worker.WorkerID = WorkersJSON[i].WorkerID;
                    worker.Name = WorkersJSON[i].Name;
                    worker.Surname = WorkersJSON[i].Surname;
                    worker.Lastname = WorkersJSON[i].Lastname;
                    worker.Birthday = (DateTime)WorkersJSON[i].Birthday;
                    worker.Expirience = WorkersJSON[i].Expirience;
                    worker.Specialize = WorkersJSON[i].Specialize;
                    worker.Phone = WorkersJSON[i].Phone;
                    worker.Login = WorkersJSON[i].Login;
                    worker.Password = WorkersJSON[i].Password;
                    worker.AccessLevel = MyDBContext.DBContext.AccessLevels.Find(Convert.ToInt32(WorkersJSON[i].AccessLevelID));
                    MyDBContext.DBContext.Workers.Add(worker);
                    MyDBContext.DBContext.SaveChanges();
                }
                Dispatcher.Invoke((Action)(() => ChangeProgress(32.3)));
                //clients
                dynamic ClientsJSON = JSON[1].Next;
                for (int i = 0; i < ClientsJSON.Count; i++)
                {
                    Client client = new Client();
                    client.ClientID = ClientsJSON[i].ClientID;
                    client.Name = ClientsJSON[i].Name;
                    client.Surname = ClientsJSON[i].Surname;
                    client.Lastname = ClientsJSON[i].Lastname;
                    client.Birthday = (DateTime)ClientsJSON[i].Birthday;
                    client.Adress = ClientsJSON[i].Adress;
                    client.Phone = ClientsJSON[i].Phone;
                    client.Gender = ClientsJSON[i].Gender;
                    MyDBContext.DBContext.Clients.Add(client);
                    MyDBContext.DBContext.SaveChanges();
                }
                Dispatcher.Invoke((Action)(() => ChangeProgress(48.6)));
                //Services
                dynamic ServicesJSON = JSON[1].Next.Next;
                for (int i = 0; i < ServicesJSON.Count; i++)
                {
                    PaidServices Service = new PaidServices();
                    Service.ServiceID = ServicesJSON[i].ServiceID;
                    Service.Name = ServicesJSON[i].Name;
                    Service.Price = ServicesJSON[i].Price;
                    Service.Worker = MyDBContext.DBContext.Workers.Find(Convert.ToInt32(ServicesJSON[i].WorkerID));
                    MyDBContext.DBContext.PaidServices.Add(Service);
                    MyDBContext.DBContext.SaveChanges();
                }
                Dispatcher.Invoke((Action)(() => ChangeProgress(64.9)));
                //Tickets
                dynamic TicketsJSON = JSON[1].Next.Next.Next;
                for (int i = 0; i < TicketsJSON.Count; i++)
                {
                    Ticket ticket = new Ticket();
                    ticket.TicketID = TicketsJSON[i].TicketID;
                    ticket.TicketNumber = TicketsJSON[i].TicketNumber;
                    ticket.WorkDate = TicketsJSON[i].WorkDate;
                    ticket.WorkTime = TicketsJSON[i].WorkTime;
                    ticket.Client = MyDBContext.DBContext.Clients.Find(Convert.ToInt32(TicketsJSON[i].ClientID));
                    ticket.PaidServices = MyDBContext.DBContext.PaidServices.Find(Convert.ToInt32(TicketsJSON[i].PaidServicesID));
                    MyDBContext.DBContext.Tickets.Add(ticket);
                    MyDBContext.DBContext.SaveChanges();
                }
                Dispatcher.Invoke((Action)(() => ChangeProgress(81.2)));
                //Results
                dynamic ResultsJSON = JSON[1].Next.Next.Next.Next;
                for (int i = 0; i < ResultsJSON.Count; i++)
                {
                    Result result = new Result();
                    result.ResultID = ResultsJSON[i].ResultID;
                    result.ResultText = ResultsJSON[i].ResultText;
                    result.Ticket = MyDBContext.DBContext.Tickets.Find(Convert.ToInt32(ResultsJSON[i].TicketID));
                    MyDBContext.DBContext.Results.Add(result);
                    MyDBContext.DBContext.SaveChanges();
                }
                Dispatcher.Invoke((Action)(() => ChangeProgress(100)));
                MessageBox.Show("Импорт прошел успешно!");
            }
        }
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            Thread ExportThread = new Thread(() => ExportData());
            ExportThread.Start();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            Thread ImportThread = new Thread(() => ImportData());
            ImportThread.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Вы действительно хотите удалють всю базу данных?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                MyDBContext.DBContext.Database.Delete();
            }
        }
    }
}