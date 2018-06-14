using FluentValidation;
using InquirerDLL.Entities;
using InquirerDLL.Enumerations;
using System;
using System.Collections.Generic;

namespace InquirerDLL.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            When(x => x.Name != null, () =>
            {
                RuleFor(x => x.Name)
                    .MinimumLength(1)
                    .MaximumLength(128)
                    .Matches(REGULAR_EXPRESSIONS.ALPHA);
            });

            When(x => x.Email != null, () =>
            {
                RuleFor(x => x.Email)
                    .MinimumLength(1)
                    .MaximumLength(128)
                    .EmailAddress();
            });

            When(x => x.Password != null, () =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(1)
                    .MaximumLength(256)
                    .Matches(REGULAR_EXPRESSIONS.ALPHA);
            });

            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image)
                    .MinimumLength(1)
                    .MaximumLength(CONSTANTS.MAX_IMAGE_LENGTH);
            });

            When(x => x.GroupId != null, () =>
            {
                RuleFor(x => x.GroupId)
                    .InclusiveBetween(1, CONSTANTS.MAX_USER_GROUP);
            });

            When(x => x.EducationTypeId != null, () =>
            {
                RuleFor(x => x.EducationTypeId)
                    .InclusiveBetween(1, CONSTANTS.MAX_EDUCATION_TYPE);
            });

            When(x => x.EducationProgressId != null, () =>
            {
                RuleFor(x => x.EducationProgressId)
                    .InclusiveBetween(1, CONSTANTS.MAX_EDUCATION_PROGRESS);
            });

            When(x => x.LanguageId != null, () =>
            {
                RuleFor(x => x.LanguageId)
                    .InclusiveBetween(1, CONSTANTS.MAX_USER_LANGUAGE);
            });

            When(x => x.OccupationId != null, () =>
            {
                RuleFor(x => x.OccupationId)
                    .InclusiveBetween(1, CONSTANTS.MAX_USER_OCCUPATION);
            });

            When(x => x.LocationId != null, () =>
            {
                RuleFor(x => x.LocationId)
                    .InclusiveBetween(1, CONSTANTS.MAX_USER_LOCATION);
            });
        }
    }
}
