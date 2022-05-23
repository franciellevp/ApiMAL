using ApiMAL.EnumData;
using System.ComponentModel.DataAnnotations;

namespace ApiMAL
{
    public class AnimeList : IValidatableObject
    {
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
