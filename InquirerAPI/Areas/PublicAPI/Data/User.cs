using System;
using System.Collections.Generic;

namespace InquirerAPI.PublicAPI.Data
{
    public class User
    {
        public User()
        {
            Surveys = new HashSet<Survey>();
            Respondents = new HashSet<Respondent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Sex { get; set; }
        public string Image { get; set; }
        public int GroupId { get; set; }
        public int? EducationTypeId { get; set; }
        public int? EducationProgressId { get; set; }
        public int? LanguageId { get; set; }
        public int? OccupationId { get; set; }
        public int? LocationId { get; set; }

        public UserEducationProgress EducationProgress { get; set; }
        public UserEducationType EducationType { get; set; }
        public UserGroup Group { get; set; }
        public UserLanguage Language { get; set; }
        public UserLocation Location { get; set; }
        public UserOccupation Occupation { get; set; }

        public ICollection<Survey> Surveys { get; set; }
        public ICollection<Respondent> Respondents { get; set; }
    }
}
