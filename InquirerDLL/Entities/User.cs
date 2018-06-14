using System;
using System.Collections.Generic;

namespace InquirerDLL.Entities
{
    public class User
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Sex { get; set; }
        public string Image { get; set; }
        public int? GroupId { get; set; }
        public int? EducationTypeId { get; set; }
        public int? EducationProgressId { get; set; }
        public int? LanguageId { get; set; }
        public int? OccupationId { get; set; }
        public int? LocationId { get; set; }

        public IEnumerable<Report> ReportCreators { get; set; }
        public IEnumerable<Report> ReportUsers { get; set; }
        public IEnumerable<Survey> Surveys { get; set; }
        public IEnumerable<Respondent> Respondents { get; set; }
    }
}
