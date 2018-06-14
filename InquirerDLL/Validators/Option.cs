using FluentValidation;
using InquirerDLL.Entities;
using System;
using System.Collections.Generic;

namespace InquirerDLL.Validators
{
    public class OptionValidator : AbstractValidator<Option>
    {
        public OptionValidator()
        {
            When(x => x.Label != null, () =>
            {
                RuleFor(x => x.Label)
                    .MinimumLength(1)
                    .MaximumLength(128);
            });

            When(x => x.Value != null, () =>
            {
                RuleFor(x => x.Value)
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
