using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Accounts
{
    public class CreditNote
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public CreditNote(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetCreditNoteIndexes();

            this.CreditNoteSaveRelative();
            this.CreditNotePostSaveValidate();

            this.CreditNoteApproved();
            this.CreditNoteEditable();

            this.CreditNoteToggleApproved();

            this.CreditNoteInitReference();
        }


        private void GetCreditNoteIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      CreditNotes.CreditNoteID, CAST(CreditNotes.EntryDate AS DATE) AS EntryDate, CreditNotes.Reference, Promotions.Code AS PromotionCode, CreditNotes.PromotionVouchers, CreditNotes.MemoCode, CreditNotes.MemoDate, Locations.Code AS LocationCode, Customers.Name AS CustomerName, CreditNotes.Description, CreditNotes.TotalQuantity, CreditNotes.TotalGrossAmount, CreditNotes.TotalGrossAmount - CreditNotes.TotalReceiptAmount - CreditNotes.TotalFluctuationAmount AS TotalGrossAmountPending, CreditNotes.Approved " + "\r\n";
            queryString = queryString + "       FROM        CreditNotes " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON CreditNotes.EntryDate >= @FromDate AND CreditNotes.EntryDate <= @ToDate AND CreditNotes.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.CreditNote + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = CreditNotes.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON CreditNotes.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN Promotions ON CreditNotes.PromotionID = Promotions.PromotionID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetCreditNoteIndexes", queryString);
        }

        #region X


        private void CreditNoteSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("CreditNoteSaveRelative", queryString);
        }

        private void CreditNotePostSaveValidate()
        {
            string[] queryArray = new string[0];

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("CreditNotePostSaveValidate", queryArray);
        }




        private void CreditNoteApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = CreditNoteID FROM CreditNotes WHERE CreditNoteID = @EntityID AND Approved = 1";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("CreditNoteApproved", queryArray);
        }


        private void CreditNoteEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = CreditNoteID FROM Receipts WHERE CreditNoteID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("CreditNoteEditable", queryArray);
        }


        private void CreditNoteToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      CreditNotes  SET Approved = @Approved, ApprovedDate = GetDate() WHERE CreditNoteID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          CreditNoteDetails  SET Approved = @Approved WHERE CreditNoteID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("CreditNoteToggleApproved", queryString);
        }

        private void CreditNoteInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("CreditNotes", "CreditNoteID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.CreditNote));
            this.totalSalesPortalEntities.CreateTrigger("CreditNoteInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}
