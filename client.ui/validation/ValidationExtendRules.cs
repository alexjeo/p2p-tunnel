using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace client.ui.validation
{
    public class TextBoxValidationExtendRules
    {
        public static ValidationResult Required(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new ValidationResult("必填");
            }
            return ValidationResult.Success;
        }
    }
}
