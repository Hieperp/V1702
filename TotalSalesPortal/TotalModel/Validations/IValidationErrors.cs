using System.Collections.Generic;

namespace TotalModel.Validations
{
    public interface IValidationErrors
    {
        List<IBaseError> Errors { get; set; }
    }
}
