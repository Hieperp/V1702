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
            this.GetPendingCustomerCredits();

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

            queryString = queryString + "       SELECT      Receipts.ReceiptID, CAST(Receipts.EntryDate AS DATE) AS EntryDate, Receipts.Reference, Locations.Code AS LocationCode, Customers.Name AS CustomerName, Receipts.GoodsIssueReferences, Receipts.Description, " + "\r\n";
            queryString = queryString + "                   Receipts.ReceiptTypeID, ReceiptTypes.Code AS ReceiptTypeCode, MonetaryAccounts.Code AS MonetaryAccountCode, AdvanceReceipts.Reference AS AdvanceReceiptReference, AdvanceReceipts.EntryDate AS AdvanceReceiptDate, SalesReturns.Reference AS SalesReturnReference, SalesReturns.EntryDate AS SalesReturnDate, CreditNotes.Reference AS CreditNoteReference, CreditNotes.EntryDate AS CreditNoteDate, " + "\r\n";
            queryString = queryString + "                   Receipts.TotalDepositAmount, Receipts.TotalReceiptAmount, Receipts.TotalCashDiscount, Receipts.TotalOtherDiscount, Receipts.TotalFluctuationAmount, CASE WHEN Receipts.ReceiptTypeID = " + GlobalReceiptTypeID.ReceiveMoney + " THEN Receipts.TotalDepositAmount - Receipts.TotalReceiptAmount - Receipts.TotalFluctuationAmount ELSE 0 END AS TotalDepositAmountPending " + "\r\n";
            queryString = queryString + "       FROM        Receipts " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON Receipts.EntryDate >= @FromDate AND Receipts.EntryDate <= @ToDate AND Receipts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.Receipt + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = Receipts.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ReceiptTypes ON Receipts.ReceiptTypeID = ReceiptTypes.ReceiptTypeID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers Customers ON Receipts.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN MonetaryAccounts ON Receipts.MonetaryAccountID = MonetaryAccounts.MonetaryAccountID" + "\r\n";
            queryString = queryString + "                   LEFT JOIN Receipts AS AdvanceReceipts ON Receipts.AdvanceReceiptID = AdvanceReceipts.ReceiptID" + "\r\n";
            queryString = queryString + "                   LEFT JOIN SalesReturns ON Receipts.SalesReturnID = SalesReturns.SalesReturnID" + "\r\n";
            queryString = queryString + "                   LEFT JOIN CreditNotes ON Receipts.CreditNoteID = CreditNotes.CreditNoteID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetReceiptIndexes", queryString);
        }

        private void GetGoodsIssueReceivables()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          TOP 1000 GoodsIssues.GoodsIssueID, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.TotalGrossAmount, ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount - GoodsIssues.TotalCashDiscount - GoodsIssues.TotalOtherDiscount, " + (int)GlobalEnums.rndAmount + ") AS AmountDue, GoodsIssues.Description, GoodsIssues.Remarks, " + "\r\n";
            queryString = queryString + "                       GoodsIssues.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, CASE WHEN Customers.MonetaryAccountID IS NULL THEN CustomerCategories.MonetaryAccountID ELSE Customers.MonetaryAccountID END AS MonetaryAccountID " + "\r\n";

            queryString = queryString + "       FROM            GoodsIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON GoodsIssues.LocationID = @LocationID AND GoodsIssues.Approved = 1 AND ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount - GoodsIssues.TotalCashDiscount - GoodsIssues.TotalOtherDiscount, " + (int)GlobalEnums.rndAmount + ") > 0 AND GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON GoodsIssues.ReceiverID = Receivers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetGoodsIssueReceivables", queryString);
        }

        private void GetCustomerReceivables()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       SELECT          Customers.CustomerID AS CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.Telephone AS CustomerTelephone, Customers.BillingAddress AS CustomerBillingAddress, EntireTerritories.EntireName AS CustomerEntireTerritoryEntireName, CASE WHEN Customers.MonetaryAccountID IS NULL THEN CustomerCategories.MonetaryAccountID ELSE Customers.MonetaryAccountID END AS MonetaryAccountID, PENDINGCustomerReceivables.AmountDue " + "\r\n";
            queryString = queryString + "       FROM           (SELECT CustomerID, ROUND(SUM(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount - GoodsIssues.TotalCashDiscount - GoodsIssues.TotalOtherDiscount), " + (int)GlobalEnums.rndAmount + ") AS AmountDue FROM GoodsIssues WHERE LocationID = @LocationID AND Approved = 1 AND ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount - GoodsIssues.TotalCashDiscount - GoodsIssues.TotalOtherDiscount, " + (int)GlobalEnums.rndAmount + ") > 0 GROUP BY CustomerID) PENDINGCustomerReceivables " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON PENDINGCustomerReceivables.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetCustomerReceivables", queryString);
        }



        private void GetPendingCustomerCredits()
        {
            string queryString = " @LocationID int, @CustomerID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + GlobalCreditTypeID.AdvanceReceipt + " AS CreditTypeID, N'Trả tiền trước' AS CreditTypeName, ReceiptID AS ReceiptCreditID, Reference, EntryDate, Description, TotalDepositAmount AS TotalCreditAmount, TotalReceiptAmount, TotalFluctuationAmount, ROUND(TotalDepositAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS TotalCreditAmountPending FROM Receipts WHERE LocationID = @LocationID AND CustomerID = @CustomerID AND ReceiptTypeID = " + GlobalReceiptTypeID.ReceiveMoney + " AND ROUND(TotalDepositAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "       UNION ALL       " + "\r\n";
            queryString = queryString + "       SELECT          " + GlobalCreditTypeID.SalesReturn + " AS CreditTypeID, N'Trả hàng' AS CreditTypeName, SalesReturnID AS ReceiptCreditID, Reference, EntryDate, Description, TotalGrossAmount AS TotalCreditAmount, TotalReceiptAmount, TotalFluctuationAmount, ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS TotalCreditAmountPending FROM SalesReturns WHERE LocationID = @LocationID AND CustomerID = @CustomerID AND Approved = 1 AND ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "       UNION ALL       " + "\r\n";
            queryString = queryString + "       SELECT          " + GlobalCreditTypeID.CreditNote + " AS CreditTypeID, N'Chiết khấu' AS CreditTypeName, CreditNoteID AS ReceiptCreditID, Reference, EntryDate, Description, TotalGrossAmount AS TotalCreditAmount, TotalReceiptAmount, TotalFluctuationAmount, ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS TotalCreditAmountPending FROM CreditNotes WHERE LocationID = @LocationID AND CustomerID = @CustomerID AND Approved = 1 AND ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetPendingCustomerCredits", queryString);
        }


        private void GetReceiptViewDetails()
        {
            string queryString;
            
            queryString = " @ReceiptID Int, @LocationID Int, @GoodsIssueID Int, @CustomerID Int, @IsReadonly bit " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@GoodsIssueID > 0) ";
            queryString = queryString + "           " + this.BuildSQL(true);
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           " + this.BuildSQL(false);

            queryString = queryString + "   END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetReceiptViewDetails", queryString);

        }

        private string BuildSQL(bool isGoodsIssueID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF (@ReceiptID <= 0) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               " + this.BuildSQLNew(isGoodsIssueID) + "\r\n";
            queryString = queryString + "               ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "           IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLEdit(isGoodsIssueID) + "\r\n";
            queryString = queryString + "                   ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "           ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isGoodsIssueID) + " AND GoodsIssues.GoodsIssueID NOT IN (SELECT GoodsIssueID FROM ReceiptDetails WHERE ReceiptID = @ReceiptID) " + "\r\n";
            queryString = queryString + "                   UNION ALL " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLEdit(isGoodsIssueID) + "\r\n";
            queryString = queryString + "                   ORDER BY GoodsIssues.EntryDate, GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isGoodsIssueID)
        {
            string queryString;

            queryString = "                     SELECT          TOP 200 GoodsIssues.GoodsIssueID, GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.CustomerID, Customers.Name AS CustomerName, GoodsIssues.ReceiverID, Receivers.Name AS ReceiverName, GoodsIssues.Description, 0 AS ReceiptDetailID, 0 AS ReceiptID, '' AS Remarks, " + "\r\n";
            queryString = queryString + "                       GoodsIssues.TotalGrossAmount, ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount - GoodsIssues.TotalCashDiscount - GoodsIssues.TotalOtherDiscount, " + (int)GlobalEnums.rndAmount + ") AS AmountDue, 0.0 AS ReceiptAmount, 0.0 AS CashDiscount, 0.0 AS OtherDiscount, 0.0 AS FluctuationAmount " + "\r\n";

            queryString = queryString + "       FROM            GoodsIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON " + (isGoodsIssueID ? "GoodsIssues.GoodsIssueID = @GoodsIssueID" : "GoodsIssues.LocationID = @LocationID AND GoodsIssues.CustomerID = @CustomerID") + " AND GoodsIssues.CustomerID = Customers.CustomerID AND GoodsIssues.Approved = 1 AND ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount - GoodsIssues.TotalCashDiscount - GoodsIssues.TotalOtherDiscount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON GoodsIssues.ReceiverID = Receivers.CustomerID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isGoodsIssueID)
        {
            string queryString;

            queryString = "                     SELECT          TOP 200 GoodsIssues.GoodsIssueID, GoodsIssues.EntryDate AS GoodsIssueEntryDate, GoodsIssues.Reference AS GoodsIssueReference, GoodsIssues.CustomerID, Customers.Name AS CustomerName, GoodsIssues.ReceiverID, Receivers.Name AS ReceiverName, GoodsIssues.Description, ReceiptDetails.ReceiptDetailID, ReceiptDetails.ReceiptID, ReceiptDetails.Remarks, " + "\r\n";
            queryString = queryString + "                       GoodsIssues.TotalGrossAmount, ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount - GoodsIssues.TotalCashDiscount - GoodsIssues.TotalOtherDiscount + ReceiptDetails.ReceiptAmount + ReceiptDetails.CashDiscount + ReceiptDetails.OtherDiscount, " + (int)GlobalEnums.rndAmount + ") AS AmountDue, ReceiptDetails.ReceiptAmount, ReceiptDetails.CashDiscount, ReceiptDetails.OtherDiscount, ReceiptDetails.FluctuationAmount " + "\r\n";

            queryString = queryString + "       FROM            ReceiptDetails " + "\r\n";
            queryString = queryString + "                       INNER JOIN GoodsIssues ON ReceiptDetails.ReceiptID = @ReceiptID AND ReceiptDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON GoodsIssues.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers Receivers ON GoodsIssues.ReceiverID = Receivers.CustomerID " + "\r\n";

            return queryString;
        }

        private void ReceiptSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       UPDATE          GoodsIssues " + "\r\n";
            queryString = queryString + "       SET             GoodsIssues.TotalReceiptAmount = ROUND(GoodsIssues.TotalReceiptAmount + ReceiptDetails.ReceiptAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), GoodsIssues.TotalCashDiscount = ROUND(GoodsIssues.TotalCashDiscount + ReceiptDetails.CashDiscount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), GoodsIssues.TotalOtherDiscount = ROUND(GoodsIssues.TotalOtherDiscount + ReceiptDetails.OtherDiscount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + ") " + "\r\n";
            queryString = queryString + "       FROM            ReceiptDetails INNER JOIN GoodsIssues ON ReceiptDetails.ReceiptID = @EntityID AND GoodsIssues.Approved = 1 AND ReceiptDetails.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = (SELECT COUNT(*) FROM ReceiptDetails WHERE ReceiptID = @EntityID) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               DECLARE         @AdvanceReceiptID int, @SalesReturnID int, @CreditNoteID int, @affectedRows int " + "\r\n";
            queryString = queryString + "               SELECT          @AdvanceReceiptID = AdvanceReceiptID, @SalesReturnID = SalesReturnID, @CreditNoteID = CreditNoteID FROM Receipts WHERE ReceiptID = @EntityID " + "\r\n";

            queryString = queryString + "               IF  (NOT @AdvanceReceiptID IS NULL OR NOT @SalesReturnID IS NULL OR NOT @CreditNoteID IS NULL) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n"; //BOTH UPDATE AND CHECK FOR VALID DATA (DATE + VALUE) WHEN UPDATE BACK TO THE: AdvanceReceipts OR SalesReturns OR CreditNotes
            queryString = queryString + "                       IF  (NOT @AdvanceReceiptID IS NULL) " + "\r\n";
            queryString = queryString + "                           BEGIN " + "\r\n";
            queryString = queryString + "                               UPDATE      AdvanceReceipts " + "\r\n";
            queryString = queryString + "                               SET         AdvanceReceipts.TotalReceiptAmount = ROUND(AdvanceReceipts.TotalReceiptAmount + Receipts.TotalReceiptAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), AdvanceReceipts.TotalCashDiscount = ROUND(AdvanceReceipts.TotalCashDiscount + Receipts.TotalCashDiscount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), AdvanceReceipts.TotalOtherDiscount = ROUND(AdvanceReceipts.TotalOtherDiscount + Receipts.TotalOtherDiscount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), AdvanceReceipts.TotalFluctuationAmount = ROUND(AdvanceReceipts.TotalFluctuationAmount + Receipts.TotalFluctuationAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + ") " + "\r\n";
            queryString = queryString + "                               FROM        Receipts INNER JOIN Receipts AS AdvanceReceipts ON Receipts.ReceiptID = @EntityID AND Receipts.AdvanceReceiptID = AdvanceReceipts.ReceiptID             AND Receipts.EntryDate >= AdvanceReceipts.EntryDate          AND                                      AdvanceReceipts.TotalDepositAmount >= ROUND(ROUND(AdvanceReceipts.TotalReceiptAmount + Receipts.TotalReceiptAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + ") + ROUND(AdvanceReceipts.TotalFluctuationAmount + Receipts.TotalFluctuationAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), " + (int)GlobalEnums.rndAmount + ")                                                            ; " + "\r\n";
            
            queryString = queryString + "                               SET         @affectedRows = @@ROWCOUNT; " + "\r\n";
            queryString = queryString + "                           END " + "\r\n";

            queryString = queryString + "                       IF  (NOT @SalesReturnID IS NULL) " + "\r\n";
            queryString = queryString + "                           BEGIN " + "\r\n";
            queryString = queryString + "                               UPDATE      SalesReturns " + "\r\n";
            queryString = queryString + "                               SET         SalesReturns.TotalReceiptAmount = ROUND(SalesReturns.TotalReceiptAmount + Receipts.TotalReceiptAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), SalesReturns.TotalCashDiscount = ROUND(SalesReturns.TotalCashDiscount + Receipts.TotalCashDiscount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), SalesReturns.TotalOtherDiscount = ROUND(SalesReturns.TotalOtherDiscount + Receipts.TotalOtherDiscount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), SalesReturns.TotalFluctuationAmount = ROUND(SalesReturns.TotalFluctuationAmount + Receipts.TotalFluctuationAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + ") " + "\r\n";
            queryString = queryString + "                               FROM        Receipts INNER JOIN SalesReturns ON Receipts.ReceiptID = @EntityID AND Receipts.SalesReturnID = SalesReturns.SalesReturnID                              AND Receipts.EntryDate >= SalesReturns.EntryDate             AND                                      SalesReturns.TotalGrossAmount >= ROUND(ROUND(SalesReturns.TotalReceiptAmount + Receipts.TotalReceiptAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + ") + ROUND(SalesReturns.TotalFluctuationAmount + Receipts.TotalFluctuationAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), " + (int)GlobalEnums.rndAmount + ")                       AND SalesReturns.Approved = 1                   ; " + "\r\n";
            
            queryString = queryString + "                               SET         @affectedRows = @@ROWCOUNT; " + "\r\n";
            queryString = queryString + "                           END " + "\r\n";

            queryString = queryString + "                       IF  (NOT @CreditNoteID IS NULL) " + "\r\n";
            queryString = queryString + "                           BEGIN " + "\r\n";
            queryString = queryString + "                               UPDATE      CreditNotes " + "\r\n";
            queryString = queryString + "                               SET         CreditNotes.TotalReceiptAmount = ROUND(CreditNotes.TotalReceiptAmount + Receipts.TotalReceiptAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), CreditNotes.TotalCashDiscount = ROUND(CreditNotes.TotalCashDiscount + Receipts.TotalCashDiscount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), CreditNotes.TotalOtherDiscount = ROUND(CreditNotes.TotalOtherDiscount + Receipts.TotalOtherDiscount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), CreditNotes.TotalFluctuationAmount = ROUND(CreditNotes.TotalFluctuationAmount + Receipts.TotalFluctuationAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + ") " + "\r\n";
            queryString = queryString + "                               FROM        Receipts INNER JOIN CreditNotes ON Receipts.ReceiptID = @EntityID AND Receipts.CreditNoteID = CreditNotes.CreditNoteID                                  AND Receipts.EntryDate >= CreditNotes.EntryDate              AND                                      CreditNotes.TotalGrossAmount >= ROUND(ROUND(CreditNotes.TotalReceiptAmount + Receipts.TotalReceiptAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + ") + ROUND(CreditNotes.TotalFluctuationAmount + Receipts.TotalFluctuationAmount * @SaveRelativeOption, " + (int)GlobalEnums.rndAmount + "), " + (int)GlobalEnums.rndAmount + ")                         AND CreditNotes.Approved = 1                    ; " + "\r\n";
            
            queryString = queryString + "                               SET         @affectedRows = @@ROWCOUNT; " + "\r\n";
            queryString = queryString + "                           END " + "\r\n";


            queryString = queryString + "                       IF  (@affectedRows <> 1) " + "\r\n"; //Note: The statement before THROW MUST BE END WITH CHARACTER ';'
            queryString = queryString + "                           THROW   61001,  N'Không thể cấn trừ vô chứng từ gốc. Vui lòng kiểm tra lại ngày lập hoặc số tiền cấn trừ.', 1; " + "\r\n";

            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Phiếu xuất kho không tồn tại, chưa duyệt hoặc đã hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("ReceiptSaveRelative", queryString);
        }

        private void ReceiptPostSaveValidate()
        {
            string[] queryArray = new string[4]; //Just check validate for detail only. When Apply to credit: It will be checked while SaveRelative. See ReceiptSaveRelative for more detail.

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày PXK: ' + CAST(GoodsIssues.EntryDate AS nvarchar) FROM ReceiptDetails INNER JOIN GoodsIssues ON ReceiptDetails.ReceiptID = @EntityID AND ReceiptDetails.GoodsIssueID = GoodsIssues.GoodsIssueID AND ReceiptDetails.EntryDate < GoodsIssues.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số tiền cấn trừ vượt quá số phải thu: (PXK: ' + Reference + '): ' + CAST(ROUND(TotalGrossAmount - TotalReceiptAmount - TotalCashDiscount - TotalOtherDiscount, " + (int)GlobalEnums.rndAmount + ") AS nvarchar) FROM GoodsIssues WHERE (ROUND(TotalGrossAmount - TotalReceiptAmount - TotalCashDiscount - TotalOtherDiscount, " + (int)GlobalEnums.rndAmount + ") < 0) ";

            queryArray[2] = "                 DECLARE   @AdvanceReceiptID Int, @SalesReturnID Int, @CreditNoteID Int            DECLARE @EntryDate Datetime " + "\r\n";
            queryArray[2] = queryArray[2] + " SELECT TOP 1 @AdvanceReceiptID = AdvanceReceiptID, @SalesReturnID = SalesReturnID, @CreditNoteID = CreditNoteID, @EntryDate = EntryDate FROM Receipts WHERE ReceiptID = @EntityID " + "\r\n";
            
            queryArray[2] = queryArray[2] + " IF (@AdvanceReceiptID > 0) " + "\r\n";
            queryArray[2] = queryArray[2] + "       SELECT TOP 1 @FoundEntity = N'Ngày thanh toán: ' + CAST(EntryDate AS nvarchar) FROM Receipts WHERE ReceiptID = @AdvanceReceiptID AND EntryDate > @EntryDate " + "\r\n";
            queryArray[2] = queryArray[2] + " IF (@SalesReturnID > 0) " + "\r\n";
            queryArray[2] = queryArray[2] + "       SELECT TOP 1 @FoundEntity = N'Ngày trả hàng: ' + CAST(EntryDate AS nvarchar) FROM SalesReturns WHERE SalesReturnID = @SalesReturnID AND EntryDate > @EntryDate " + "\r\n";
            queryArray[2] = queryArray[2] + " IF (@CreditNoteID > 0) " + "\r\n";
            queryArray[2] = queryArray[2] + "       SELECT TOP 1 @FoundEntity = N'Ngày chiết khấu: ' + CAST(EntryDate AS nvarchar) FROM CreditNotes WHERE CreditNoteID = @CreditNoteID AND EntryDate > @EntryDate " + "\r\n";

            queryArray[3] = "                 IF (@AdvanceReceiptID > 0) " + "\r\n";
            queryArray[3] = queryArray[3] + "       SELECT TOP 1 @FoundEntity = N'Số tiền cấn trừ vượt quá số tiền thanh toán: ' + CAST(ROUND(TotalDepositAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS nvarchar) FROM Receipts WHERE ReceiptID = @AdvanceReceiptID AND ROUND(TotalDepositAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") < 0 " + "\r\n";
            queryArray[3] = queryArray[3] + " IF (@SalesReturnID > 0) " + "\r\n";
            queryArray[3] = queryArray[3] + "       SELECT TOP 1 @FoundEntity = N'Số tiền cấn trừ vượt quá số tiền trả hàng: ' + CAST(ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS nvarchar) FROM SalesReturns WHERE SalesReturnID = @SalesReturnID AND ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") < 0 " + "\r\n";
            queryArray[3] = queryArray[3] + " IF (@CreditNoteID > 0) " + "\r\n";
            queryArray[3] = queryArray[3] + "       SELECT TOP 1 @FoundEntity = N'Số tiền cấn trừ vượt quá số tiền chiết khấu: ' + CAST(ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS nvarchar) FROM CreditNotes WHERE CreditNoteID = @CreditNoteID AND ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") < 0 " + "\r\n";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("ReceiptPostSaveValidate", queryArray);
        }


        private void ReceiptEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = ReceiptID FROM Receipts WHERE AdvanceReceiptID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("ReceiptEditable", queryArray);
        }

        private void ReceiptInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("Receipts", "ReceiptID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.Receipt));
            this.totalSalesPortalEntities.CreateTrigger("ReceiptInitReference", simpleInitReference.CreateQuery());
        }


    }
}
