using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AVTOTEST.Pages
{
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
            var completedQuestionsCount = MainWindow.Instance.TicketsRepository.UserTickets.Sum(t => t.CorrectAnswersCount);
            var totalQuestionCount = MainWindow.Instance.QuestionsRepository.Questions.Count;
            QuestionStatus.Text = $"{completedQuestionsCount}/{totalQuestionCount}";

            var completedTicketsCount = MainWindow.Instance.TicketsRepository.UserTickets.Count(t => t.TicketCompleted);
            var totalTicketQuestionCount = MainWindow.Instance.QuestionsRepository.GetTicketsCount();
            TicketStatus.Text = $"{completedTicketsCount}/{totalTicketQuestionCount}";
        }

        public void TicketButtonClick(object obj, RoutedEventArgs eventArgs)
        {
            MainWindow.Instance.MainF.Navigate(new TicketsPage());
        }

        private void ExaminationButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainF.Navigate(new ExaminationPage());
        }
    }
}
