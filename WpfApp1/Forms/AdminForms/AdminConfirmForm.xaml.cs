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

namespace WpfApp1.Forms.AdminForms
{
    /// <summary>
    /// Логика взаимодействия для AdminConfirmForm.xaml
    /// </summary>
    public partial class AdminConfirmForm : Window
    {
        public AdminConfirmForm()
        {
            InitializeComponent();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(PasswordBox.Password))
                ShowPassText.Text = PasswordBox.Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var tmp = (
    from tmpWorker in MyDBContext.DBContext.Workers.ToList<Worker>()
    where tmpWorker.Password.CompareTo(PasswordBox.Password) == 0
    select tmpWorker
          ).ToList();
            if (tmp.Count > 0)
            {
                if (tmp[0].AccessLevel.AccessLevelID == 1)
                {
                    BDConfigForm BCF = new BDConfigForm();
                    this.Close();
                    BCF.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Вы не являетесь администратором!");
                }
            }
            else
            {
                MessageBox.Show("Такого пароля не существует!");
            }
        }
    }
}
