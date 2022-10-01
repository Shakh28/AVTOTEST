using AVTOTEST.Pages;
using AVTOTEST.Repository;
using System.Windows;


namespace AVTOTEST
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        public QuestionsRepository QuestionsRepository;
        public TicketsRepository TicketsRepository;
        public int CorrectCount;

        public MainWindow()
        {
            InitializeComponent();

            Instance = this;
            QuestionsRepository = new QuestionsRepository();
            TicketsRepository = new TicketsRepository();
            var menuPage = new MenuPage();
            MainF.Navigate(menuPage);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TicketsRepository.WriteToJson();
        }
    }
}
