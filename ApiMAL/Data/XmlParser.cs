using ApiMAL.EnumData;
using System.Xml.Linq;
using System;
using System.Globalization;

namespace ApiMAL.Data
{
    /// <summary>
    /// Parser to interprete a XML File with an user Anime List info
    /// </summary>
    public class XmlParser
    {
        /// <summary>
        /// A list to save all anime of an user list
        /// </summary>
        public List<AnimeList> UserListParser { get; set; }

        public XmlParser () {
            UserListParser = new List<AnimeList>();
        }

        /// <summary>
        /// Method to Parse the XML file
        /// </summary>
        /// <returns>A list containg all the readed and important data of the XML file</returns>
        public List<AnimeList> Parse () {
            var filename = "animelist_1.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var purchaseOrderFilepath = Path.Combine(currentDirectory + "\\Data\\files\\", filename);

            XElement info = XElement.Load(purchaseOrderFilepath);
            var myInfo = from item in info.Descendants("myinfo")
                            select item.Element("user_id").Value;

            uint userId = Convert.ToUInt32(myInfo.FirstOrDefault());
            var animes = from item in info.Elements("anime")
                         select item.Descendants();
            var x = animes.ElementAt(0);
            for (int i = 0; i < animes.Count(); i++) {
                var elements = animes.ElementAt(i);
                AnimeType type;
                Enum.TryParse(elements.ElementAt(2).Value, out type);
                AnimeList anime = new AnimeList {
                    Id = Convert.ToUInt32(elements.ElementAt(0).Value),
                    Title = elements.ElementAt(1).Value,
                    UserId = userId,
                    Status = GetStatus(elements.ElementAt(12).Value),
                    Type = type,
                    NrEpisodes = Convert.ToUInt32(elements.ElementAt(3).Value),
                    NrWatched = Convert.ToUInt32(elements.ElementAt(5).Value),
                    StartDate = StringToDateTime(elements.ElementAt(6).Value),
                    EndDate = StringToDateTime(elements.ElementAt(7).Value),
                    Score = Convert.ToUInt16(elements.ElementAt(9).Value),
                    TimesWatched = Convert.ToUInt16(elements.ElementAt(14).Value)
                };
                UserListParser.Add(anime);
            }

            return UserListParser;
            
        }
        
        /// <summary>
        /// Convert a String to DateTime type
        /// </summary>
        /// <param name="date"></param>
        /// <returns>A nullable DateTime</returns>
        private DateTime? StringToDateTime (string date) {
            if (date.Substring(0, 4) == "0000") {
                return null;
            }
            date = date.Replace("-", "/");
            DateTime? myDate = DateTime.Parse(date);
            return myDate;
        }

        /// <summary>
        /// Parse the anime status classification in the XML file
        /// </summary>
        /// <param name="type"></param>
        /// <returns>An Enum value of Status</returns>
        private Status GetStatus (string type) {
            Status result;
            switch (type) {
                case "Watching":
                    result = Status.Watching;
                    break;
                case "Completed":
                    result = Status.Completed;
                    break;
                case "Plan to Watch":
                    result = Status.PlanToWatch;
                    break;
                case "Dropped":
                    result = Status.Dropped;
                    break;
                case "On-Hold":
                    result = Status.Onhold;
                    break;
                default:
                    result = Status.Completed;
                    break;
            }
            return result;
        }
    }
}
