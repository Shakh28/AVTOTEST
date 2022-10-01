using Newtonsoft.Json.Bson;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AVTOTEST.Pages
{
    public partial class TicketsPage : Page
    {
        public TicketsPage()
        {
            InitializeComponent();
            GenerateTicketButtons();
        }

        private void GenerateTicketButtons()
        {
            var questionsRepository = MainWindow.Instance.QuestionsRepository;
            int ticketsCount = questionsRepository.GetTicketsCount();
            var ticketRepository = MainWindow.Instance.TicketsRepository;
            for (int i = 0; i < ticketsCount; i++)
            {

                var button = new Button();
                if (ticketRepository.UserTickets.Any(ticket => ticket.Index == i))
                {
                    var ticket = ticketRepository.UserTickets.First(ticket => ticket.Index == i);
                    button.Content = ticket.TicketCompleted
                        ? "Ticket" + (i + 1) + "\t✅"
                        : "Ticket" + (i + 1) + $"\t{ticket.CorrectAnswersCount}/{ticket.QuestionsCount}";
                }
                else
                {
                    button.Content = "Ticket " + (i + 1);
                }
                button.Width = 300;
                button.Height = 40;
                button.FontSize = 20;
                button.Click += TicketButtonClick;
                button.Tag = i;

                TicketButtonsPanel.Children.Add(button);
            }
        }

        private void TicketButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var ticketIndex = (int)button.Tag;

            var examPage = new ExaminationPage(ticketIndex);
            MainWindow.Instance.MainF.Navigate(examPage);
        }

        private void MenuButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainF.Navigate(new MenuPage());
        }
    }
}
