using System.Collections.Generic;
using System.Windows.Media;

namespace AVTOTEST.Models
{
    public class QuestionEntity
    {
        public int Id;
        public string Question;
        public string Description;
        public List<Choice> Choices;
        public Media Media;
    }

    public class Choice
    {
        public int Index;
        public string Text;
        public bool Answer;
    }

    public class Media
    {
        public bool Exist;
        public string Name;
    }
}
