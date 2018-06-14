using FluentValidation;
using InquirerDLL.Entities;
using InquirerDLL.Enumerations;
using System;
using System.Collections.Generic;

namespace InquirerDLL.Validators
{
    public class SurveyValidator : AbstractValidator<Survey>
    {
        public SurveyValidator()
        {
            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title)
                    .MinimumLength(1)
                    .MaximumLength(256);
            });

            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description)
                    .MinimumLength(1)
                    .MaximumLength(4096);
            });

            When(x => x.StartDate != null, () =>
            {
                RuleFor(x => x.StartDate)
                    .NotNull()
                    .LessThanOrEqualTo(x => x.EndDate);

                RuleFor(x => x.EndDate)
                    .NotNull()
                    .GreaterThanOrEqualTo(x => x.StartDate);
            });
        }
    }
}
