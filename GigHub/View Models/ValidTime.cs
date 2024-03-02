using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GigHub.View_Models
{
    public class ValidTime : ValidationAttribute
    {
        /// <summary>
        /// </summary>
        /// <param name="value">Represents the value of the Date property in the GigFormViewModel</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            // Parse the value argument to a DateTime object
            DateTime dateTime;
            var isValid = DateTime.TryParseExact(Convert.ToString(value),
                "HH:mm",
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out dateTime //if the conversion is successful, the dateTime object will hold the value
            );

            //Check if the time is valid
            return (isValid);

        }
    }
}