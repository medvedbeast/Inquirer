using InquirerDLL.Entities;
using System.Collections.Generic;

namespace InquirerDLL.Output
{
    public class Enumerations
    {
        public IEnumerable<UserEducationType> EducationTypes { get; set; }
        public IEnumerable<UserEducationProgress> EducationProgresses { get; set; }
        public IEnumerable<UserSex> Sexes { get; set; }
        public IEnumerable<UserLanguage> Languages { get; set; }
        public IEnumerable<UserLocation> Locations { get; set; }
        public IEnumerable<UserOccupation> Occupations { get; set; }
        public IEnumerable<QuestionType> QuestionTypes { get; set; }
        public IEnumerable<UserGroup> UserGroups { get; set; }
    }
}
