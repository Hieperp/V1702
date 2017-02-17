using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;


using TotalModel.Validations;

namespace TotalModel.Models
{
    public partial class TotalSalesPortalEntities
    {
        public override int SaveChanges()
        {
            IEnumerable<DbEntityValidationResult> errors = new List<DbEntityValidationResult>();
            if (this.Configuration.ValidateOnSaveEnabled)
            {
                errors = this.GetValidationErrors();
            }
            if (!errors.Any())
            {
                try
                {
                    //base.ChangeTracker.DetectChanges();
                    return base.SaveChanges();
                }
                catch (OptimisticConcurrencyException concurrencyException)
                {
                    throw new DatabaseConcurrencyException("Someone else has edited the entity in the same time of you. Please refresh and save again.", concurrencyException);
                }
                catch (DBConcurrencyException concurrencyException)
                {
                    throw new DatabaseConcurrencyException("Someone else has edited the entity in the same time of you. Please refresh and save again.", concurrencyException);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new DatabaseConcurrencyException("Someone else has edited the entity in the same time of you. Please refresh and save again.", e);
                }
                catch (DbEntityValidationException ex)
                {
                    throw new DatabaseValidationErrors(ex.EntityValidationErrors);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            else
            {
                throw new DatabaseValidationErrors(errors);
            }

        }

    }
}
