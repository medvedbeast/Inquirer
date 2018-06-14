using FluentValidation;
using InquirerDLL.Entities;
using System;
using System.Collections.Generic;

namespace InquirerDLL.Validators
{
    public class AnswerValidator : AbstractValidator<Answer>
    {
        public AnswerValidator()
        {
            When(x => x.Content != null, () =>
            {
                RuleFor(x => x.Content)
                    .MinimumLength(1)
                    .MaximumLength(1024);
            });
        }
    }
}
