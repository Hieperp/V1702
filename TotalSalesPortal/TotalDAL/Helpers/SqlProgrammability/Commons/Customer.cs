using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Customer
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public Customer(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetCustomerIndexes();

            this.CustomerEditable(); //CÓ THỂ KHÔNG CẦN Editable/ NHƯNG CẦN PHẢI CÓ Deletable
            this.CustomerSaveRelative();
        }


        private void GetCustomerIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      CustomerCategories.CustomerCategoryID, CustomerCategories.Name AS Category, Customers.CustomerID, Customers.Code, Customers.Name, Customers.OfficialName, Customers.VATCode, Customers.BillingAddress, Customers.ShippingAddress, PriceCategories.Code AS PriceCategoryCode, PaymentTerms.Name AS PaymentTermName, Employees.Name AS SalespersonName, Territories.Name AS TerritoryName " + "\r\n";
            queryString = queryString + "       FROM        CustomerCategories " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON CustomerCategories.CustomerCategoryID = Customers.CustomerCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PriceCategories ON Customers.PriceCategoryID = PriceCategories.PriceCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PaymentTerms ON Customers.PaymentTermID = PaymentTerms.PaymentTermID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees ON Customers.SalespersonID = Employees.EmployeeID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Territories ON Customers.TerritoryID = Territories.TerritoryID " + "\r\n";
            queryString = queryString + "       ORDER BY    Customers.Code DESC " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetCustomerIndexes", queryString);
        }


        private void CustomerSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               INSERT INTO CustomerWarehouses (CustomerID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
            queryString = queryString + "               SELECT      CustomerID, 46 AS WarehouseID, " + (int)GlobalEnums.NmvnTaskID.SalesOrder + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Customers WHERE CustomerID = @EntityID " + "\r\n";

            queryString = queryString + "               INSERT INTO CustomerWarehouses (CustomerID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n"; //ALL CustomerCategoryID NOT IN (4, 5, 7, 9, 10, 11, 12) DEFINED BY Warehouses.WarehouseCategoryID. HERE WE SET Warehouses.WarehouseCategoryID AND CustomerCategories.WarehouseCategoryID PARALLEL
            queryString = queryString + "               SELECT      Customers.CustomerID, Warehouses.WarehouseID, " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Customers INNER JOIN Warehouses ON Customers.CustomerID = @EntityID AND Customers.CustomerCategoryID NOT IN (4, 5, 7, 9, 10, 11, 12) AND Customers.CustomerCategoryID = Warehouses.WarehouseCategoryID " + "\r\n";

            queryString = queryString + "               INSERT INTO CustomerWarehouses (CustomerID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n"; //MAP ALL CustomerCategoryID IN (4, 5, 7, 9, 10, 11, 12) TO WarehouseID 82: STPX
            queryString = queryString + "               SELECT      CustomerID, 82 AS WarehouseID, " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Customers WHERE CustomerID = @EntityID AND CustomerCategoryID IN (4, 5, 7, 9, 10, 11, 12) " + "\r\n";

            queryString = queryString + "           END " + "\r\n";
            
            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           DELETE      CustomerWarehouses WHERE CustomerID = @EntityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("CustomerSaveRelative", queryString);
        }


        private void CustomerEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = CustomerID FROM Customers WHERE CustomerID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = CustomerID FROM GoodsIssueDetails WHERE CustomerID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("CustomerEditable", queryArray);
        }

    }
}
