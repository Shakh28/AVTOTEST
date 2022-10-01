using AVTOTEST.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AVTOTEST.Repository
{
    public class TicketsRepository
    {
        public List<Ticket> UserTickets = new List<Ticket>();

        private const string Folder = "UserData";
        private const string FileName = "usertickets.json";

        public TicketsRepository()
        {
            ReadJsonData();
        }

        public void WriteToJson()
        {
            List<Ticket> ticketsData = UserTickets
                .Select(t => new Ticket(t.Index, t.CorrectAnswersCount, t.QuestionsCount))
                .ToList();

            var jsonData = JsonConvert.SerializeObject(ticketsData);

            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }

            File.WriteAllText(Path.Combine(Folder, FileName), jsonData);
        }

        public void ReadJsonData()
        {
            if (!File.Exists(Path.Combine(Folder, FileName))) return;

            var jsonData = File.ReadAllText(Path.Combine(Folder, FileName));
            UserTickets = JsonConvert.DeserializeObject<List<Ticket>>(jsonData);
        }
    }
}
