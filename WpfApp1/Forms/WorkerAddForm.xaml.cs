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
    /// Логика взаимодействия для WorkerAddForm.xaml
    /// </summary>
    public partial class WorkerAddForm : Window
    {
        AdminForm AF;
        bool IsEdit = false;
        Worker EditWorker;
        WFAEntity.API.MyDBContext MyDBContext = new WFAEntity.API.MyDBContext();
        public WorkerAddForm(AdminForm AF)
        {
            this.AF = AF;
            InitializeComponent();
        }
        public WorkerAddForm(AdminForm AF, Worker EditWorker)
        {
            IsEdit = true;
            this.AF = AF;
            this.EditWorker = EditWorker;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!IsEdit)
            {
                if (!String.IsNullOrWhiteSpace(NameBox.Text) && !String.IsNullOrWhiteSpace(SurnameBox.Text) && !String.IsNullOrWhiteSpace(LastNameBox.Text)
                    && WorkerDatePicker.SelectedDate != null && !String.IsNullOrWhiteSpace(ExpBox.Text) && !String.IsNullOrWhiteSpace(SpecializeBox.Text)
                    && !String.IsNullOrWhiteSpace(PhoneBox.Text) && !String.IsNullOrWhiteSpace(LoginBox.Text) && !String.IsNullOrWhiteSpace(PasswordBox.Text))
                {
                    Worker WorkerObj = new Worker();
                    int Temp = MyDBContext.AccessLevels.Count();
                    WorkerObj.WorkerID = Temp++;
                    WorkerObj.Name = NameBox.Text;
                    WorkerObj.Surname = SurnameBox.Text;
                    WorkerObj.Lastname = LastNameBox.Text;
                    WorkerObj.Expirience = Convert.ToInt32(ExpBox.Text);
                    WorkerObj.Birthday = (DateTime)WorkerDatePicker.SelectedDate;
                    WorkerObj.Specialize = SpecializeBox.Text;
                    WorkerObj.Phone = PhoneBox.Text;
                    WorkerObj.Login = LoginBox.Text;
                    WorkerObj.Password = PasswordBox.Text;
                    //WorkerObj.AccessILevelD = 1;
                    var tmp = (
                        from tmpAccesLevels in MyDBContext.AccessLevels.ToList<AccessLevel>()
                        select tmpAccesLevels
                        ).ToList();
                    if (AccessCombo.SelectedIndex == 1)
                    {
                        WorkerObj.AccessLevel = tmp[1];
                    }
                    if (AccessCombo.SelectedIndex == 0)
                    {
                        WorkerObj.AccessLevel = tmp[0];
                    }
                    if (AccessCombo.SelectedIndex == 2)
                    {
                        WorkerObj.AccessLevel = tmp[2];
                    }
                    MyDBContext.Workers.Add(WorkerObj);
                    NameBox.Text = String.Empty;
                    SurnameBox.Text = String.Empty;
                    LastNameBox.Text = String.Empty;
                    SpecializeBox.Text = String.Empty;
                    ExpBox.Text = String.Empty;
                    PhoneBox.Text = String.Empty;
                    LoginBox.Text = String.Empty;
                    PasswordBox.Text = String.Empty;
                    MyDBContext.SaveChanges();
                    AF.UpdateGrid();
                }
                else
                {
                    MessageBox.Show("Заполните все данные!");
                }
            }
            else
            {
                EditWorker.Name = NameBox.Text;
                EditWorker.Surname = SurnameBox.Text;
                EditWorker.Lastname = LastNameBox.Text;
                EditWorker.Expirience = Convert.ToInt32(ExpBox.Text);
                EditWorker.Specialize = SpecializeBox.Text;
                EditWorker.Phone = PhoneBox.Text;
                EditWorker.Login = LoginBox.Text;
                EditWorker.Password = PasswordBox.Text;
                EditWorker.Birthday = (DateTime)WorkerDatePicker.SelectedDate;
                var tmp = (
                    from tmpAccesLevels in MyDBContext.AccessLevels.ToList<AccessLevel>()
                    select tmpAccesLevels
                    ).ToList();
                if (AccessCombo.SelectedIndex == 1)
                {
                    EditWorker.AccessLevel = tmp[1];
                }
                if (AccessCombo.SelectedIndex == 0)
                {
                    EditWorker.AccessLevel = tmp[0];
                }
                if (AccessCombo.SelectedIndex == 2)
                {
                    EditWorker.AccessLevel = tmp[2];
                }
                MyDBContext.Workers.AddOrUpdate(EditWorker);
                MyDBContext.SaveChanges();
                AF.UpdateGrid();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsEdit)
            {
                AddButton.Content = "Сохранить";
                NameBox.Text = EditWorker.Name;
                SurnameBox.Text = EditWorker.Surname;
                LastNameBox.Text = EditWorker.Lastname;
                ExpBox.Text = EditWorker.Expirience.ToString();
                SpecializeBox.Text = EditWorker.Specialize;
                PhoneBox.Text = EditWorker.Phone;
                LoginBox.Text = EditWorker.Login;
                PasswordBox.Text = EditWorker.Password;
                WorkerDatePicker.SelectedDate = EditWorker.Birthday;
                if (EditWorker.AccessLevel.AccessLevelID == 1)
                {
                    AccessCombo.SelectedIndex = 0;
                }
                if (EditWorker.AccessLevel.AccessLevelID == 2)
                {
                    AccessCombo.SelectedIndex = 1;
                }
                if (EditWorker.AccessLevel.AccessLevelID == 3)
                {
                    AccessCombo.SelectedIndex = 2;
                }
            }
        }
    }
}
