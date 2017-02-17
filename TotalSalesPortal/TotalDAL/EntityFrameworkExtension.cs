using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;


namespace TotalDAL
{
    public static class EntityFrameworkExtension
    {
        public static IQueryable<TEntity> IncludeEntity<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return query;
        }

        public static IQueryable<TEntity> IncludeEntity<TEntity>(this IDbSet<TEntity> dbSet, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            IQueryable<TEntity> query = null;
            foreach (var include in includes)
            {
                query = dbSet.IncludeEntity(include);
            }

            return query == null ? dbSet : query;
        }


        /// <summary>
        /// Check stored procedure exist
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="storedProcedureName"></param>
        /// <returns></returns>
        public static bool StoredProcedureExists(this DbContext dbContext, string storedProcedureName)
        {
            var query = dbContext.Database.SqlQuery(typeof(int), string.Format("SELECT COUNT(*) FROM [sys].[objects] WHERE [type_desc] = 'SQL_STORED_PROCEDURE' AND [name] = '{0}';", storedProcedureName), new object[] { });

            int exists = query.Cast<int>().Single();

            return (exists > 0);
        }

        /// <summary>
        /// Check trigger exist
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="triggerName"></param>
        /// <returns></returns>
        public static bool TriggerExists(this DbContext dbContext, string triggerName)
        {
            var query = dbContext.Database.SqlQuery(typeof(int), string.Format("SELECT COUNT(*) FROM [sys].[objects] WHERE [type_desc] = 'SQL_TRIGGER' AND [name] = '{0}';", triggerName), new object[] { });

            int exists = query.Cast<int>().Single();

            return (exists > 0);
        }

        /// <summary>
        /// Check view exist
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static bool ViewExists(this DbContext dbContext, string viewName)
        {
            var query = dbContext.Database.SqlQuery(typeof(int), string.Format("SELECT COUNT(*) FROM [sys].[views] WHERE [type_desc] = 'VIEW' AND [name] = '{0}';", viewName), new object[] { });

            int exists = query.Cast<int>().Single();

            return (exists > 0);
        }

        /// <summary>
        /// Check user defined function exist
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userDefinedFunctionName"></param>
        /// <returns></returns>
        public static bool UserDefinedFunctionExists(this DbContext dbContext, string userDefinedFunctionName)
        {
            var query = dbContext.Database.SqlQuery(typeof(int), string.Format("SELECT COUNT(*) FROM [sys].[objects] WHERE TYPE in ('FN', 'IF', 'TF') AND [name] = '{0}';", userDefinedFunctionName), new object[] { });

            int exists = query.Cast<int>().Single();

            return (exists > 0);
        }


        /// <summary>
        /// Create a new stored procedure
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="queryString"></param>
        public static void CreateStoredProcedure(this DbContext dbContext, string storedProcedureName, string queryString)
        {
            if (dbContext.StoredProcedureExists(storedProcedureName)) dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE " + storedProcedureName);

            dbContext.Database.ExecuteSqlCommand(@"CREATE PROC " + storedProcedureName + "\r\n" + queryString);

        }


        /// <summary>
        /// Create a new trigger
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="triggerName"></param>
        /// <param name="queryString"></param>
        public static void CreateTrigger(this DbContext dbContext, string triggerName, string queryString)
        {
            if (dbContext.TriggerExists(triggerName)) dbContext.Database.ExecuteSqlCommand(@"DROP TRIGGER " + triggerName);

            dbContext.Database.ExecuteSqlCommand(@"CREATE TRIGGER " + triggerName + "\r\n" + queryString);

        }



        /// <summary>
        /// Create a new stored procedure
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="queryString"></param>
        public static void CreateView(this DbContext dbContext, string viewName, string queryString)
        {
            if (dbContext.ViewExists(viewName)) dbContext.Database.ExecuteSqlCommand(@"DROP VIEW " + viewName);

            dbContext.Database.ExecuteSqlCommand(@"CREATE VIEW " + viewName + " WITH ENCRYPTION AS \r\n" + queryString);

        }


        /// <summary>
        /// Create a new user defined function
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userDefinedFunctionName"></param>
        /// <param name="queryString"></param>
        public static void CreateUserDefinedFunction(this DbContext dbContext, string userDefinedFunctionName, string queryString)
        {
            if (dbContext.UserDefinedFunctionExists(userDefinedFunctionName)) dbContext.Database.ExecuteSqlCommand(@"DROP FUNCTION " + userDefinedFunctionName);

            dbContext.Database.ExecuteSqlCommand(@"CREATE FUNCTION " + userDefinedFunctionName + "\r\n" + queryString);
        }


        /// <summary>
        /// Create new stored procedure to check a pecific existing, for example: Editable, Revisable, ...
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterString"></param>
        /// <param name="queryArray"></param>
        public static void CreateProcedureToCheckExisting(this DbContext dbContext, string storedProcedureName, string[] queryArray)
        {
            string queryString = "";

            queryString = " @EntityID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @FoundEntity nvarchar(100) " + "\r\n";

            if (queryArray != null)
            {
                foreach (string subQuery in queryArray)
                {
                    queryString = queryString + "       " + subQuery + "\r\n";
                    queryString = queryString + "       IF NOT @FoundEntity IS NULL " + "\r\n";
                    queryString = queryString + "           BEGIN " + "\r\n";
                    queryString = queryString + "               SELECT @FoundEntity AS FoundEntity " + "\r\n";
                    queryString = queryString + "               RETURN 1 " + "\r\n";
                    queryString = queryString + "           END " + "\r\n";
                }
            }

            queryString = queryString + "       SELECT @FoundEntity AS FoundEntity " + "\r\n";
            queryString = queryString + "       RETURN 0 " + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            dbContext.CreateStoredProcedure(storedProcedureName, queryString);
        }

        public static void CreateProcedureToCheckExisting(this DbContext dbContext, string storedProcedureName)
        {
            dbContext.CreateProcedureToCheckExisting(storedProcedureName, null);
        }



    }
}



//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE GoodsReceiptEditable");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE StockTransferEditable");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE SalesInvoiceEditable");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE PurchaseOrderEditable");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE PurchaseInvoiceEditable");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE QuotationEditable");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE VehiclesInvoiceEditable");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE PartsInvoiceEditable");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE ServicesInvoiceEditable");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE TransferOrderEditable");

//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE GoodsReceiptPostSaveValidate");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE PartsInvoicePostSaveValidate");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE PartTransferPostSaveValidate");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE PurchaseInvoicePostSaveValidate");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE QuotationPostSaveValidate");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE ServiceContractPostSaveValidate");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE ServicesInvoicePostSaveValidate");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE TransferOrderPostSaveValidate");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE VehiclesInvoicePostSaveValidate");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE VehicleTransferPostSaveValidate");

//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE QuotationApproved");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE SalesInvoiceApproved");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE ServicesInvoiceSaveRelative");

//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE ServicesInvoiceDeletable");
//dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE ServicesContractDeletable");

