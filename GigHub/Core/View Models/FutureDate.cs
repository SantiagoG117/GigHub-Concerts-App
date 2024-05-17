using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GigHub.Core.View_Models
{
    public class FutureDate : ValidationAttribute
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
                "d MMM yyyy",
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out dateTime //if the conversion is successful, the dateTime object will hold the value
                );

            //Check if the date time is in the future
            return (isValid && dateTime > DateTime.Now);

        }
    }
}