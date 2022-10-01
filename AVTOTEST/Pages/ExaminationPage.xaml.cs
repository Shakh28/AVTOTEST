using AVTOTEST.Models;
using AVTOTEST.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

namespace AVTOTEST.Pages
{
    public partial class ExaminationPage : Page
    {
        private int currentQuestionIndex = 0;
        private Ticket currentTicket;

        public ExaminationPage(int currentTicketIndex = -1)
        {
            InitializeComponent();

            if (currentTicketIndex <= -1)
            {
                var random = new Random();

                currentTicketIndex = random.Next(0, MainWindow.Instance.QuestionsRepository.GetTicketsCount());
            }

            Title.Content = $"Ticket{currentTicketIndex + 1}";
            CreateTicket(currentTicketIndex);

            GenerateTicketQuestionIndexButtons();

            ShowQuestion();
        }
        public void StartQuestionIndexButtonAnimation(int animTime, Button btn)
        {
            DoubleAnimation animOpacity = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                BeginTime = TimeSpan.FromMilliseconds(animTime)
            };

            btn.BeginAnimation(Button.OpacityProperty, animOpacity);
        }

        public void StartQuestionChoiceButtonAnimation(int animTime, Button btn)
        {
            DoubleAnimation animOpacity = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                BeginTime = TimeSpan.FromMilliseconds(animTime)
            };

            ThicknessAnimation animLeft = new ThicknessAnimation();
            animLeft.From = new Thickness(30, 20, 3, 10);
            animLeft.To = new Thickness(0, 0, 0, 0);
            animLeft.Duration = new Duration(TimeSpan.FromMilliseconds(300));

            btn.BeginAnimation(Button.OpacityProperty, animOpacity);
            btn.BeginAnimation(Button.MarginProperty, animLeft);
        }


        private void CreateTicket(int ticketIndex)
        {
            var ticketQuestionsCount = MainWindow.Instance.QuestionsRepository.TicketQuestionsCount;
            var from = ticketIndex * ticketQuestionsCount;
            var ticketQuestions = MainWindow.Instance.QuestionsRepository.GetQuestionsRange(from, ticketQuestionsCount);
            currentTicket = new Ticket(ticketIndex, ticketQuestions);
        }

        private void GenerateTicketQuestionIndexButtons()
        {
            int animTime = 0;
            for (int i = 0; i < currentTicket.QuestionsCount; i++)
            {

                var button = new Button();
                if (i == 0)
                {
                    button.Style = FindResource("CurrentQuestionIndexButtonStyle") as Style;
                }
                else
                {
                    button.Style = FindResource("DefaultQuestionIndexButtonStyle") as Style;
                }
                button.Content = i + 1;
                button.Click += TicketQuestionIndexButtonClick;
                button.Tag = i;
                TicketQuestionIndexButtonPanel.Children.Add(button);

                animTime += 100;
                StartQuestionIndexButtonAnimation(animTime, button);
            }
        }

        private void TicketQuestionIndexButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.Style = FindResource("CurrentQuestionIndexButtonStyle") as Style;

            var oldButton = TicketQuestionIndexButtonPanel.Children[currentQuestionIndex] as Button;
            oldButton.Style = FindResource("DefaultQuestionIndexButtonStyle") as Style;
            currentQuestionIndex = (int)button.Tag;

            ShowQuestion();
        }

        private void MenuButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.MainF.Navigate(new MenuPage());
        }

        private void ShowQuestion()
        {
            var question = currentTicket.Questions[currentQuestionIndex];

            QuesitonText.Text = $"{currentQuestionIndex + 1}. {question.Question}";

            LoadQuestionImage(question.Media);

            GenerateChoiceButtons(question.Choices);
        }

        private void LoadQuestionImage(Media questionMedia)
        {
            string imagePath;

            if (questionMedia.Exist)
            {
                imagePath = Path.Combine(Environment.CurrentDirectory, "Images", $"{questionMedia.Name}.png");
            }
            else
            {
                imagePath = Path.Combine(Environment.CurrentDirectory, "Images", "D:\\visual studio 2022\\bootcamp\\AVTOTEST\\AVTOTEST\\Noimage.png");
            }
            QuestionImage.Source = new BitmapImage(new Uri(imagePath));
        }

        private void GenerateChoiceButtons(List<Choice> choices)
        {

            ChoicePanel.Children.Clear();
            int animTime = 0;
            for (int i = 0; i < choices.Count; i++)
            {
                var choice = choices[i];
                var button = new Button();

                if (currentTicket.IsChoiceCompleted(currentQuestionIndex, i))
                {
                    if (choice.Answer)
                    {
                        button.Background = new SolidColorBrush(Colors.LightGreen);
                    }
                    else
                        button.Background = new SolidColorBrush(Colors.Red);
                }
                button.Width = 300;
                button.MinHeight = 30;
                button.FontSize = 14;
                button.Click += ChoiceButtonClick;

                choice.Index = i;
                button.Tag = choice;

                var textBlock = new TextBlock();
                textBlock.Text = choice.Text;
                textBlock.TextWrapping = TextWrapping.Wrap;
                button.Content = textBlock;

                ChoicePanel.Children.Add(button);

                animTime += 100;
                StartQuestionChoiceButtonAnimation(animTime, button);
            }
        }

        private void ChoiceButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentTicket.IsQuestionCompleted(currentQuestionIndex)) return;
            var button = sender as Button;
            var choice = (Choice)button.Tag;

            if (choice.Answer)
            {
                button.Background = new SolidColorBrush(Colors.LightGreen);
                (TicketQuestionIndexButtonPanel.Children[currentQuestionIndex] as Button)!.Background = new SolidColorBrush(Colors.LightGreen);
                currentTicket.CorrectAnswersCount++;
                MainWindow.Instance.CorrectCount++;
            }
            else
            {
                button.Background = new SolidColorBrush(Colors.Red);
                (TicketQuestionIndexButtonPanel.Children[currentQuestionIndex] as Button)!.Background = new SolidColorBrush(Colors.Red);
            }
            currentTicket.SelectedQuestionIndexs.Add(new TicketData(currentQuestionIndex, choice.Index));

            if (currentTicket.SelectedQuestionIndexs.Count == currentTicket.QuestionsCount)
            {
                var ticketsRepository = MainWindow.Instance.TicketsRepository;

                var isCompletedTicket = ticketsRepository.UserTickets.Any(ut => ut.Index == currentTicket.Index);
                if (isCompletedTicket)
                {
                    var oldTicket = ticketsRepository.UserTickets
                        .First(ut => ut.Index == currentTicket.Index);

                    ticketsRepository.UserTickets.Remove(oldTicket);
                }

                ticketsRepository.UserTickets.Add(currentTicket);

                MainWindow.Instance.MainF.Navigate(new ExaminationResultPage(currentTicket));
            }
        }
    }
}
