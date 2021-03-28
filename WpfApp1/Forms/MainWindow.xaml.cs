﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Classes;
using WpfApp1.Forms.RegistratorForms;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WFAEntity.API.MyDBContext MyDBContext = new WFAEntity.API.MyDBContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MyDBContext.Database.Exists() == false)
                {
                    MyDBContext.Database.Create();
                    List<AccessLevel> ListLevels = new List<AccessLevel>(); 
                    AccessLevel Level = new AccessLevel();
                    Level.AccessLevelID = 1;
                    Level.Level = 1;
                    MyDBContext.AccessLevels.Add(Level);
                    ListLevels.Add(Level);
                    Level = new AccessLevel();
                    Level.AccessLevelID = 2;
                    Level.Level = 2;
                    MyDBContext.AccessLevels.Add(Level);
                    ListLevels.Add(Level);
                    Level = new AccessLevel();
                    Level.AccessLevelID = 3;
                    Level.Level = 3;
                    MyDBContext.AccessLevels.Add(Level);
                    ListLevels.Add(Level);
                    ////
                    Worker WorkerObj = new Worker();
                    WorkerObj.WorkerID = 1;
                    WorkerObj.Name = "Сергей";
                    WorkerObj.Surname = "Крутой";
                    WorkerObj.Lastname = "Крутейший";
                    WorkerObj.Expirience = 5;
                    WorkerObj.Birthday = DateTime.Parse("03.03.2003");
                    WorkerObj.Specialize = "Администратор";
                    WorkerObj.Phone = "+375291337228";
                    WorkerObj.Login = "Admin";
                    WorkerObj.Password = "Admin";
                    //WorkerObj.AccessILevelD = 1;
                    WorkerObj.AccessLevel = ListLevels[0];
                    MyDBContext.Workers.Add(WorkerObj);
                    MyDBContext.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var tmp = (
                from tmpWorker in MyDBContext.Workers.ToList<Worker>()
                where tmpWorker.Login.CompareTo(LoginBox.Text) == 0 && tmpWorker.Password.CompareTo(PasswordBox.Text) == 0
                select tmpWorker
                      ).ToList();
            if(tmp.Count > 0)
            {
                if (tmp[0].AccessLevel.AccessLevelID == 1){
                    AdminForm adminForm = new AdminForm(tmp[0].Name, tmp[0].Surname, tmp[0].Lastname, tmp[0].Specialize);
                    adminForm.Show();
                    this.Close();
                }
                if (tmp[0].AccessLevel.AccessLevelID == 3)
                {
                    RegistratorWindow RegForm = new RegistratorWindow(tmp[0].Name, tmp[0].Surname, tmp[0].Lastname, tmp[0].Specialize);
                    RegForm.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
    }
}