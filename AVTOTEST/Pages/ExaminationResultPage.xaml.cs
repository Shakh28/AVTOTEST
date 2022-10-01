using AVTOTEST.Models;
using System.Windows;
using System.Windows.Controls;



namespace AVTOTEST.Pages
{
    public partial class ExaminationResultPage : Page
    {
        public ExaminationResultPage(Ticket ticket)
        {
            InitializeComponent();

            CorrectAnswerCount.Text = ticket.CorrectAnswersCount.ToString();
            QeustionCount.Text = ticket.QuestionsCount.ToString();
        }

        private void MenuButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainF.Navigate(new MenuPage());
        }
    }
}
