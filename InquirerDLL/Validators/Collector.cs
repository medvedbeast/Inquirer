using FluentValidation;
using InquirerDLL.Entities;
using System;
using System.Collections.Generic;

namespace InquirerDLL.Validators
{
    public class CollectorValidator : AbstractValidator<Collector>
    {
        public CollectorValidator()
        {
            When(x => x.Name != null, () =>
            {
                RuleFor(x => x.Name)
                    .MinimumLength(1)
                    .MaximumLength(128);
            });
        }
    }
}
