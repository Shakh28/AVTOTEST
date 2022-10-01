using AVTOTEST.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace AVTOTEST.Repository
{
    public class QuestionsRepository
    {
        public List<QuestionEntity> Questions { get; set; }

        public int TicketQuestionsCount = 5;

        public QuestionsRepository()
        {
            LoadQuestionsFromJsonFile();
        }

        public void LoadQuestionsFromJsonFile()
        {
            var jsonStringData = File.ReadAllText("JsonData/uzlotin.json");

            Questions = JsonConvert.DeserializeObject<List<QuestionEntity>>(jsonStringData);
        }

        public int GetTicketsCount()
        {
            return Questions.Count / TicketQuestionsCount;
        }

        public List<QuestionEntity> GetQuestionsRange(int from, int count)
        {
            return Questions.Skip(from).Take(count).ToList();
        }
    }
   }
