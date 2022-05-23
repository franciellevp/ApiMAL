using ApiMAL.EnumData;
using System.ComponentModel.DataAnnotations;

namespace ApiMAL
{
    /// <summary>
    /// The Class that specifies all information saved from an user anime list
    /// </summary>
    public class AnimeList : IValidatableObject
    {
        /// <summary>
        /// Constructor of the class with all the attributes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="nrEpisodes"></param>
        /// <param name="nrWatched"></param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="score"></param>
        /// <param name="timesWatched"></param>
        public AnimeList (uint id, uint userId, string title, AnimeType type, uint nrEpisodes, uint nrWatched, Status status, DateTime? startDate, DateTime? endDate, ushort score, ushort timesWatched) {
            Id = id;
            UserId = userId;
            Title = title;
            Type = type;
            NrEpisodes = nrEpisodes;
            NrWatched = nrWatched;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
            Score = score;
            TimesWatched = timesWatched;
        }

        public AnimeList () {
        }

        public uint Id { get; set; }

        [Required]
        public uint UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [EnumDataType(typeof(AnimeType))]
        public AnimeType Type { get; set; }

        public uint NrEpisodes { get; set; }

        public uint NrWatched { get; set; }

        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ushort Score { get; set; }

        public ushort TimesWatched { get; set; }

        /// <summary>
        /// Method to Validate the value of some attributes of the class
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns>A message alerting the user of the errors/rules to insert and update the class values</returns>
        public IEnumerable<ValidationResult> Validate (ValidationContext validationContext) {
            if (StartDate.HasValue && StartDate.Value.Year > DateTime.Now.Year)
                yield return new ValidationResult("Start Date is invalid!");
            if (StartDate.HasValue && EndDate.HasValue && EndDate < StartDate)
                yield return new ValidationResult("End Date is invalid! Cant be lower than the `Start Date`");
            if (Id < 0)
                yield return new ValidationResult("Id must be greater than 0");
            if (UserId <= 0)
                yield return new ValidationResult("User Id must be greater than 0");
        }
    }
}
