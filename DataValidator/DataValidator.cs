using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;

public class DataValidator : ValidationRule
{
    public bool isNumeric { get; set; }


    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        double num;

        if (!double.TryParse(value.ToString(), out num))
        {
            return new ValidationResult(false, $"Blablablabla");
        }

        return new ValidationResult(true, null);

    }
}