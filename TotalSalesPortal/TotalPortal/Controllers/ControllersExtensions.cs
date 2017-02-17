using System;
using System.Web.Mvc;

using TotalModel.Validations;


namespace TotalPortal.Controllers
{
    public static class ControllersExtensions
    {
        /// <summary>
        /// Add to the model state a list of error that came from properties. If the property name
        /// is empty, this one will be without property (general)
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <param name="propertyErrors">The property errors.</param>
        public static void AddValidationErrors(this ModelStateDictionary modelState, Exception exception)
        {
            if (exception is IValidationErrors)
            {
                foreach (var databaseValidationError in (exception as ValidationErrors).Errors)
                {
                    modelState.AddModelError(databaseValidationError.PropertyName ?? string.Empty, databaseValidationError.PropertyExceptionMessage);
                }
            }
            else
            {
                modelState.AddModelError(string.Empty, exception.Message + (exception.Message != exception.GetBaseException().Message ? "\r\n" + exception.GetBaseException().Message : ""));
            }

        }

    }
}