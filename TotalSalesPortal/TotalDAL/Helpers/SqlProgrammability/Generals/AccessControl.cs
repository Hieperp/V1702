using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Generals
{
    public class AccessControl
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public AccessControl(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetAccessLevel();
            this.GetApprovalPermitted();
            this.GetUnApprovalPermitted();
            this.GetVoidablePermitted();
            this.GetUnVoidablePermitted();

            this.GetShowDiscount();
            this.GetShowDiscountByCustomer();
            this.UpdateLockedDate();
        }

        /// <summary>
        /// Get the permission for a specific UserID on a specific NMVNTaskID ON A SPECIFIC OrganizationalUnitID
        /// Exspecially: WHEN OrganizationalUnitID = 0: Get the top level permission for a specific UserID on a specific NMVNTaskID
        /// </summary>    
        private void GetAccessLevel()
        {
            //VIEC GetAccessLevel: CO 2 TRUONG HOP
            //    '//A. Maintenance List (DANH MUC THAM KHAO): OrganizationalUnitID = 0 => CO QUYEN HAY KHONG MA THOI, BOI VI MAU TIN TRONG DANH SACH THAM KHAO LA CUA CHUNG => DO DO KHONG CAN DEN OrganizationalUnitID
            //    '//B. Transaction Data (CAC MAU TIN GIAO DICH - CAC MAU TIN CO CHU SO HUU), KHI DO CO 2 TINH HUONG:
            //        '//B.1 TINH HUONG 1: MAU TIN DA SAVE: (TUC LA DA CO CHU SO HUU - CO OrganizationalUnitID): NGUOI DANG TRUY CAP CO QUYEN EDIT TREN MAU TIN CO CHU SO HUU HAY KHONG?
            //        '//B2. TINH HUONG 2:
            //                '//B.2: NGUOI DUNG CO QUYEN EDITABLE TREN NMVNTaskID, NHUNG KHONG BIET CO QUYEN TREN MOT DON VI CU THE OrganizationalUnitID HAY KHONG?
            //                '//B.2: THEM VAO DO, TAI THOI DIEM EDIT, CHUA XAC DINH MAU TIN THUOC VE AI, KHI DO OrganizationalUnitID = 0,
            //                '//B.2: DO DO CUNG CHI XAC DINH DUA TREN QUYEN EDITABLE CUA NMVNTaskID CUA UserID HIEN HANH MA THOI
            //                '//B.2: KHI SAVE (HOAC HAY HON LA KHI CHON CHU SO HUU - VI DU: CHON UserID TRONG QUOTATION) TA MOI XAC DINH DUOC OrganizationalUnitID
            //                '//B.2: DEN LUC NAY VIEC XAC DINH QUYEN EDITABLE LA CU THE ROI, DO DO NEU KHONG CO QUYEN EDITABLE CHO MOT DON VI CU THE => THI SE KHONG CHO SAVE

            string queryString = " @UserID Int, @NMVNTaskID Int, @OrganizationalUnitID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT      MAX(AccessLevel) AS AccessLevel FROM AccessControls " + "\r\n";
            queryString = queryString + "       WHERE       UserID = @UserID AND NMVNTaskID = @NMVNTaskID AND (@OrganizationalUnitID <= 0 OR (@OrganizationalUnitID > 0 AND OrganizationalUnitID = @OrganizationalUnitID)) " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetAccessLevel", queryString);
        }

        private void GetApprovalPermitted()
        {
            string queryString = " @UserID Int, @NMVNTaskID Int, @OrganizationalUnitID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT      CAST(MAX(CAST(ApprovalPermitted AS Int)) AS Bit) AS ApprovalPermitted FROM AccessControls " + "\r\n";
            queryString = queryString + "       WHERE       UserID = @UserID AND NMVNTaskID = @NMVNTaskID AND (@OrganizationalUnitID <= 0 OR (@OrganizationalUnitID > 0 AND OrganizationalUnitID = @OrganizationalUnitID)) " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetApprovalPermitted", queryString);
        }

        private void GetUnApprovalPermitted()
        {
            string queryString = " @UserID Int, @NMVNTaskID Int, @OrganizationalUnitID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT      CAST(MAX(CAST(UnApprovalPermitted AS Int)) AS Bit) AS UnApprovalPermitted FROM AccessControls " + "\r\n";
            queryString = queryString + "       WHERE       UserID = @UserID AND NMVNTaskID = @NMVNTaskID AND (@OrganizationalUnitID <= 0 OR (@OrganizationalUnitID > 0 AND OrganizationalUnitID = @OrganizationalUnitID)) " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetUnApprovalPermitted", queryString);
        }


        private void GetVoidablePermitted()
        {
            string queryString = " @UserID Int, @NMVNTaskID Int, @OrganizationalUnitID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT      CAST(MAX(CAST(VoidablePermitted AS Int)) AS Bit) AS VoidablePermitted FROM AccessControls " + "\r\n";
            queryString = queryString + "       WHERE       UserID = @UserID AND NMVNTaskID = @NMVNTaskID AND (@OrganizationalUnitID <= 0 OR (@OrganizationalUnitID > 0 AND OrganizationalUnitID = @OrganizationalUnitID)) " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetVoidablePermitted", queryString);
        }

        private void GetUnVoidablePermitted()
        {
            string queryString = " @UserID Int, @NMVNTaskID Int, @OrganizationalUnitID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT      CAST(MAX(CAST(UnVoidablePermitted AS Int)) AS Bit) AS UnVoidablePermitted FROM AccessControls " + "\r\n";
            queryString = queryString + "       WHERE       UserID = @UserID AND NMVNTaskID = @NMVNTaskID AND (@OrganizationalUnitID <= 0 OR (@OrganizationalUnitID > 0 AND OrganizationalUnitID = @OrganizationalUnitID)) " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetUnVoidablePermitted", queryString);
        }


        private void GetShowDiscount()
        {
            string queryString = " @UserID Int, @NMVNTaskID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT      CAST(MAX(CAST(ShowDiscount AS Int)) AS Bit) AS ShowDiscount FROM AccessControls " + "\r\n";
            queryString = queryString + "       WHERE       UserID = @UserID AND NMVNTaskID = @NMVNTaskID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetShowDiscount", queryString);
        }

        private void GetShowDiscountByCustomer()
        {
            string queryString = " @CustomerID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT      CAST(MAX(CAST(CustomerCategories.ShowDiscount AS Int)) AS Bit) AS ShowDiscount FROM Customers INNER JOIN CustomerCategories ON Customers.CustomerID = @CustomerID AND Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetShowDiscountByCustomer", queryString);
        }

        private void UpdateLockedDate()
        {
            string queryString = " @AspUserID nvarchar(128), @LocationID Int, @LockedDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      Locations " + "\r\n";
            queryString = queryString + "       SET         LockedDate = @LockedDate, AspUserID = @AspUserID, EditedDate = GetDate() " + "\r\n";
            queryString = queryString + "       WHERE       LocationID = @LocationID " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("UpdateLockedDate", queryString);
        }
    }
}
