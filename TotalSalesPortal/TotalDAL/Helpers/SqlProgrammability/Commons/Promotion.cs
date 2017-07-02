using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;


namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Promotion
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public Promotion(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPromotionByCustomers();
            this.AddPromotionCustomers();
            this.RemovePromotionCustomers();
        }


        private void GetPromotionByCustomers()
        {
            string queryString;

            queryString = " @CustomerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@CustomerID IS NULL) ";
            queryString = queryString + "           BEGIN ";
            queryString = queryString + "               SELECT * FROM Promotions WHERE Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate " + "\r\n";
            queryString = queryString + "           END ";
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           BEGIN ";

            queryString = queryString + "               DECLARE @CustomerCategoryID int " + "\r\n";
            queryString = queryString + "               SELECT  @CustomerCategoryID = CustomerCategoryID FROM Customers WHERE CustomerID = @CustomerID "; //GET @CustomerCategoryID OF @CustomerID

            queryString = queryString + "               SELECT * FROM Promotions WHERE PromotionID IN " + "\r\n";
            queryString = queryString + "                  (SELECT Promotions.PromotionID FROM Promotions WHERE Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND Promotions.ApplyToAllCustomers = 1 " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomers ON Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND PromotionCustomers.CustomerID = @CustomerID AND Promotions.PromotionID = PromotionCustomers.PromotionID " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   SELECT Promotions.PromotionID FROM Promotions INNER JOIN PromotionCustomerCategoryies ON Promotions.InActive = 0 AND GetDate() >= Promotions.StartDate AND GetDate() <= Promotions.EndDate AND Promotions.PromotionID = PromotionCustomerCategoryies.PromotionID INNER JOIN AncestorCustomerCategories ON PromotionCustomerCategoryies.CustomerCategoryID = AncestorCustomerCategories.AncestorID AND AncestorCustomerCategories.CustomerCategoryID = @CustomerCategoryID " + "\r\n";
            queryString = queryString + "                  )" + "\r\n";
            queryString = queryString + "               ORDER BY Code, Name ";
            queryString = queryString + "           END ";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPromotionByCustomers", queryString);
        }

        private void AddPromotionCustomers()
        {
            string queryString;

            queryString = " @PromotionID int, @CustomerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       INSERT INTO     PromotionCustomers (PromotionID, CustomerID, InActive) " + "\r\n";
            queryString = queryString + "       VALUES          (@PromotionID, @CustomerID, 0) " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("AddPromotionCustomers", queryString);
        }

        private void RemovePromotionCustomers()
        {
            string queryString;

            queryString = " @PromotionID int, @CustomerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";
            queryString = queryString + "       DELETE FROM     PromotionCustomers " + "\r\n";
            queryString = queryString + "       WHERE           PromotionID = @PromotionID AND CustomerID = @CustomerID " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("RemovePromotionCustomers", queryString);

        }

    }
}
