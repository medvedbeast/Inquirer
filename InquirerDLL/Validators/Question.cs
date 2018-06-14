using FluentValidation;
using InquirerDLL.Entities;
using System;
using System.Collections.Generic;

namespace InquirerDLL.Validators
{
    public class QuestionValidator : AbstractValidator<Question>
    {
        public QuestionValidator()
        {
            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title)
                    .MinimumLength(1)
                    .MaximumLength(256);
            });

            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image)
                    .MinimumLength(1)
                    .MaximumLength(CONSTANTS.MAX_IMAGE_LENGTH);
            });
        }
    }
}
