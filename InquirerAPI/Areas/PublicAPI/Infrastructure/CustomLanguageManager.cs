using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InquirerAPI.PublicAPI.Infrastructure
{
    public class CustomLanguageManager : FluentValidation.Resources.LanguageManager
    {
        public CustomLanguageManager()
        {
            AddTranslation("uk", "EmailValidator", "'{PropertyName}' не є email-адресою.");
            AddTranslation("uk", "GreaterThanOrEqualValidator", "'{PropertyName}' має бути більшим, або дорівнювати '{ComparisonValue}'.");
            AddTranslation("uk", "GreaterThanValidator", "'{PropertyName}' має бути більшим за '{ComparisonValue}'.");
            AddTranslation("uk", "LengthValidator", "'{PropertyName}' має бути довжиною від {MinLength} до {MaxLength} символів. Ви ввели {TotalLength} символів.");
            AddTranslation("uk", "MinimumLengthValidator", "Довжина '{PropertyName}' має бути не меншою ніж {MinLength} символів. Ви ввели {TotalLength} символів.");
            AddTranslation("uk", "MaximumLengthValidator", "Довжина '{PropertyName}' має бути {MaxLength} символів, або менше. Ви ввели {TotalLength} символів.");
            AddTranslation("uk", "LessThanOrEqualValidator", "'{PropertyName}' має бути меншим, або дорівнювати '{ComparisonValue}'.");
            AddTranslation("uk", "LessThanValidator", "'{PropertyName}' має бути меншим за '{ComparisonValue}'.");
            AddTranslation("uk", "NotEmptyValidator", "'{PropertyName}' не може бути порожнім.");
            AddTranslation("uk", "NotEqualValidator", "'{PropertyName}' не може дорівнювати '{ComparisonValue}'.");
            AddTranslation("uk", "NotNullValidator", "'{PropertyName}' не може бути порожнім.");
            AddTranslation("uk", "PredicateValidator", "Вказана умова не є задовільною для '{PropertyName}'.");
            AddTranslation("uk", "AsyncPredicateValidator", "Вказана умова не є задовільною для '{PropertyName}'.");
            AddTranslation("uk", "RegularExpressionValidator", "'{PropertyName}' має неправильний формат.");
            AddTranslation("uk", "EqualValidator", "'{PropertyName}' має дорівнювати '{ComparisonValue}'.");
            AddTranslation("uk", "ExactLengthValidator", "'{PropertyName}' має бути довжиною {MaxLength} символів. Ви ввели {TotalLength} символів.");
            AddTranslation("uk", "InclusiveBetweenValidator", "'{PropertyName}' має бути між {From} та {To} (включно). Ви ввели {Value}.");
            AddTranslation("uk", "ExclusiveBetweenValidator", "'{PropertyName}' має бути між {From} та {To}. Ви ввели {Value}.");
            AddTranslation("uk", "CreditCardValidator", "'{PropertyName}' не є номером кредитної картки.");
            AddTranslation("uk", "ScalePrecisionValidator", "'{PropertyName}' не може мати більше за {expectedPrecision} цифр всього, з {expectedScale} десятковими знаками. {digits} цифр та {actualScale} десяткових знаків знайдено.");
            AddTranslation("uk", "EmptyValidator", "'{PropertyName}' має бути порожнім.");
            AddTranslation("uk", "NullValidator", "'{PropertyName}' має бути порожнім.");
            AddTranslation("uk", "EnumValidator", "'{PropertyName}' має діапазон значень, який не включає '{PropertyValue}'.");
        }
    }

}
