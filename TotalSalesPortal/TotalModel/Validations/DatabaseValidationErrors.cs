﻿using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace TotalModel.Validations
{
    public class DatabaseValidationErrors : ValidationErrors
    {

        public DatabaseValidationErrors(IEnumerable<DbEntityValidationResult> errors)
            : base()
        {
            foreach (var err in errors.SelectMany(dbEntityValidationResult => dbEntityValidationResult.ValidationErrors))
            {
                Errors.Add(new PropertyError(err.PropertyName, err.ErrorMessage));
            }
        }
    }
}
