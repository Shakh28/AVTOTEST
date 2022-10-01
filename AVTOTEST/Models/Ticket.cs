using System.Collections.Generic;
using System.Linq;

namespace AVTOTEST.Models
{
    public class Ticket
    {
        public int Index;
        public int CorrectAnswersCount;
        public int QuestionsCount;

        public List<QuestionEntity> Questions;
        public List<TicketData> SelectedQuestionIndexs;

        public bool IsQuestionCompleted(int questionIndex)
        {
            return SelectedQuestionIndexs.Any(td => td.QuestionIndex == questionIndex);
        }

        public bool IsChoiceCompleted(int questionIndex, int choiceIndex)
        {
            return SelectedQuestionIndexs.
                Any(td => td.QuestionIndex == questionIndex
                          && td.SelectedChoiceIndex == choiceIndex);
        }

        public bool TicketCompleted
        {
            get
            {
                return CorrectAnswersCount == QuestionsCount;
            }
        }

        public Ticket(int index, List<QuestionEntity> questions)
        {
            Index = index;
            Questions = questions;
            SelectedQuestionIndexs = new List<TicketData>();
            QuestionsCount = questions.Count;
        }

        public Ticket(int index, int correctAnswersCount, int questionsCount)
        {
            Index = index;
            CorrectAnswersCount = correctAnswersCount;
            QuestionsCount = questionsCount;
        }
        public Ticket()
        {

        }
    }

    public class TicketData
    {
        public int QuestionIndex;
        public int SelectedChoiceIndex;

        public TicketData(int questionIndex, int selectedChoiceIndex)
        {
            QuestionIndex = questionIndex;
            SelectedChoiceIndex = selectedChoiceIndex;
        }
    }
}                                                                      
                                                                       