using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Accounts
{
    public class Receipt
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public Receipt(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetReceiptIndexes();

            this.GetGoodsIssueReceivables();
            this.GetCustomerReceivables();

            this.GetReceiptViewDetails();
            this.ReceiptSaveRelative();
            this.ReceiptPostSaveValidate();

            this.ReceiptEditable();

            this.ReceiptInitReference();
        }

        private void GetReceiptIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Receipts.ReceiptID, CAST(Receipts.EntryDate AS DATE) AS EntryDate, Receipts.Reference, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.BillingAddress AS CustomerDescription, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.EntryDate AS GoodsIssueEntryDate, Receipts.TotalDepositAmount, Receipts.TotalDepositAmount - Receipts.TotalReceiptAmount AS TotalUnApplyAmount, Receipts.Description " + "\r\n";
            queryString = queryString + "       FROM        Receipts INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON Receipts.EntryDate >= @FromDate AND Receipts.EntryDate <= @ToDate AND Receipts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.Receipt + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = Receipts.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers Customers ON Receipts.CustomerID = Customers.CustomerID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   GoodsIssues ON Receipts.GoodsIssueID = GoodsIssues.GoodsIssueID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetReceiptIndexes", queryString);
        }

        private void GetGoodsIssueReceivables()
        {
            string queryString = " @LocationID int, @ReceiptID int, @GoodsIssueReference nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          GoodsIssues.GoodsIssueID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.Description, GoodsIssues.Remarks, " + "\r\n";
            queryString = queryString + "                       GoodsIssues.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM            GoodsIssues INNER JOIN Customers ON (@GoodsIssueReference = '' OR GoodsIssues.Reference LIKE '%' + @GoodsIssueReference + '%') AND GoodsIssues.LocationID = @LocationID AND GoodsIssues.CustomerID = Customers.CustomerID INNER JOIN EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";

            queryString = queryString + "       WHERE           GoodsIssues.GoodsIssueID IN (SELECT GoodsIssueID FROM ReceiptDetails WHERE ReceiptID = @ReceiptID) OR ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount, 0) > 0 " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetGoodsIssueReceivables", queryString);
        }

        private void GetCustomerReceivables()
        {
            string queryString = " @LocationID int, @ReceiptID int, @CustomerName nvarchar(100) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Customers.CustomerID AS CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM            Customers INNER JOIN EntireTerritories ON (@CustomerName = '' OR Customers.Name LIKE '%' + @CustomerName + '%') AND Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";

            queryString = queryString + "       WHERE           CustomerID IN   " + "\r\n";

            queryString = queryString + "                      (SELECT CustomerID FROM GoodsIssues WHERE LocationID = @LocationID AND ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount, 0) > 0 " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       SELECT CustomerID FROM Receipts WHERE ReceiptID = @ReceiptID) " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetCustomerReceivables", queryString);
        }



        private void GetReceiptViewDetails()
        {
            string queryString; string queryEdit; string queryNew;

            queryNew = "                SELECT          GoodsIssues.GoodsIssueID, GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.Reference AS GoodsIssueReference, 0 AS ReceiptDetailID, 0 AS ReceiptID, " + "\r\n";
            queryNew = queryNew + "                     GoodsIssues.CustomerID, Customers.Name AS CustomerName, ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount, 0) AS AmountDue, " + "\r\n";
            queryNew = queryNew + "                     0.0 AS ReceiptAmount, GoodsIssues.TotalGrossAmount, GoodsIssues.Remarks " + "\r\n";

            queryNew = queryNew + "     FROM            GoodsIssues INNER JOIN " + "\r\n";
            queryNew = queryNew + "                     Customers ON GoodsIssues.CustomerID = Customers.CustomerID AND ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount, 0) > 0 " + "\r\n";
            queryNew = queryNew + "     WHERE           (@GoodsIssueID = 0 OR GoodsIssues.GoodsIssueID = @GoodsIssueID) AND (@CustomerID = 0 OR GoodsIssues.CustomerID = @CustomerID) " + "\r\n";


            queryEdit = "               SELECT          GoodsIssues.GoodsIssueID, GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.Reference AS GoodsIssueReference, ReceiptDetails.ReceiptDetailID, ReceiptDetails.ReceiptID, " + "\r\n";
            queryEdit = queryEdit + "                   GoodsIssues.CustomerID, Customers.Name AS CustomerName, ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount + ReceiptDetails.ReceiptAmount, 0) AS AmountDue, " + "\r\n";
            queryEdit = queryEdit + "                   ReceiptDetails.ReceiptAmount, GoodsIssues.TotalGrossAmount, ReceiptDetails.Remarks " + "\r\n";

            queryEdit = queryEdit + "   FROM            GoodsIssues INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   ReceiptDetails ON ReceiptDetails.ReceiptID = @ReceiptID AND GoodsIssues.GoodsIssueID = ReceiptDetails.GoodsIssueID INNER JOIN " + "\r\n";
            queryEdit = queryEdit + "                   Customers ON ReceiptDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryEdit = queryEdit + "   WHERE           (@GoodsIssueID = 0 OR GoodsIssues.GoodsIssueID = @GoodsIssueID) AND (@CustomerID = 0 OR GoodsIssues.CustomerID = @CustomerID) " + "\r\n";



            queryString = " @ReceiptID Int, @GoodsIssueID Int, @CustomerID Int, @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + " IF (@ReceiptID <= 0) " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";
            queryString = queryString + "           " + queryNew + "\r\n";
            queryString = queryString + "           ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "       END " + "\r\n";
            queryString = queryString + " ELSE " + "\r\n";

            queryString = queryString + "       IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               " + queryEdit + "\r\n";
            queryString = queryString + "               ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               " + queryNew + " AND GoodsIssues.GoodsIssueID NOT IN (SELECT GoodsIssueID FROM ReceiptDetails WHERE ReceiptID = @ReceiptID) " + "\r\n";
            queryString = queryString + "               UNION ALL " + "\r\n";
            queryString = queryString + "               " + queryEdit + "\r\n";
            queryString = queryString + "               ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetReceiptViewDetails", queryString);

        }


        private void ReceiptSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE          GoodsIssues " + "\r\n";
            queryString = queryString + "       SET             GoodsIssues.TotalReceiptAmount = ROUND(GoodsIssues.TotalReceiptAmount + (ReceiptDetails.ReceiptAmount + ReceiptDetails.CashDiscount + ReceiptDetails.FluctuationAmount) * @SaveRelativeOption, 0) " + "\r\n";
            queryString = queryString + "       FROM            ReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                       GoodsIssues ON ReceiptDetails.ReceiptID = @EntityID AND ReceiptDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";
            
            this.totalSalesPortalEntities.CreateStoredProcedure("ReceiptSaveRelative", queryString);

        }

        private void ReceiptPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = 'D.A Date: ' + CAST(GoodsIssues.EntryDate AS nvarchar) FROM ReceiptDetails INNER JOIN GoodsIssues ON ReceiptDetails.ReceiptID = @EntityID AND ReceiptDetails.GoodsIssueID = GoodsIssues.GoodsIssueID AND ReceiptDetails.EntryDate < GoodsIssues.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = 'Over amount due: ' + CAST(ROUND(TotalGrossAmount - TotalReceiptAmount, 0) AS nvarchar) FROM GoodsIssues WHERE (ROUND(TotalGrossAmount - TotalReceiptAmount, 0) < 0) ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("ReceiptPostSaveValidate", queryArray);
        }


        private void ReceiptEditable()
        {
            string[] queryArray = new string[0];

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("ReceiptEditable", queryArray);
        }

        private void ReceiptInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("Receipts", "ReceiptID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.Receipt));
            this.totalSalesPortalEntities.CreateTrigger("ReceiptInitReference", simpleInitReference.CreateQuery());
        }


    }
}
