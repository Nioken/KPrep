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
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using WFAEntity.API;
using WpfApp1.Classes;

namespace WpfApp1.Forms.DoctorForms
{
    /// <summary>
    /// Логика взаимодействия для ResultForm.xaml
    /// </summary>
    public partial class ResultForm : Window
    {
        Ticket SelectedTicket;
        public ResultForm(Ticket SelectedTicket)
        {
            InitializeComponent();
            this.SelectedTicket = SelectedTicket;
            InfoLabel.Content = SelectedTicket.PaidServices.Name + ": " + SelectedTicket.Client.Surname + " " + SelectedTicket.Client.Name + " " + SelectedTicket.Client.Lastname;
        }
        public static string GetText(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            string text = textRange.Text;
            return text;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(GetText(ResultBox)))
            {
                Result res = new Result();
                res.ResultID = MyDBContext.DBContext.Results.Count();
                res.ResultID++;

                res.ResultText = GetText(ResultBox);
                res.Ticket = SelectedTicket;
                MyDBContext.DBContext.Results.Add(res);
                MyDBContext.DBContext.SaveChanges();
                this.Close();
            }
        }
    }
}
