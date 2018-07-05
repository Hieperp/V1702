using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Generals
{
    public class UserReference
    {
        private readonly TotalSalesPortalEntities totalSalesPortalEntities;

        public UserReference(TotalSalesPortalEntities totalSalesPortalEntities)
        {
            this.totalSalesPortalEntities = totalSalesPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetModuleDetailIndexes();

            this.GetUserIndexes();

            this.UserEditable();
            this.UserRegister();
            this.UserUnregister();
            this.UserToggleVoid();

            this.GetLocationOrganizationalUnits();

            this.GetUserAccessControls();
            this.SaveUserAccessControls();

            this.GetUserReportControls();
            this.SaveUserReportControls();
        }

        private void GetModuleDetailIndexes()
        {
            string queryString;

            queryString = " " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      ModuleDetails.TaskID, Modules.SerialID AS ModuleSerialID, Modules.Description AS ModuleName, ModuleDetails.Description AS TaskName, ModuleDetails.SoftDescription AS SoftTaskName " + "\r\n";
            queryString = queryString + "       FROM        Modules INNER JOIN ModuleDetails ON Modules.ModuleID = ModuleDetails.ModuleID " + "\r\n";
            queryString = queryString + "       WHERE       Modules.InActive = 0 AND ModuleDetails.Enabled = 1 " + "\r\n";
            queryString = queryString + "       ORDER BY    Modules.SerialID, ModuleDetails.SerialID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetTaskIndexes", queryString);
        }

        private void GetUserIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime, @ActiveOption int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      AspNetUsers.UserID, AspNetUsers.FirstName, AspNetUsers.LastName, AspNetUsers.UserName, OrganizationalUnits.Name AS OrganizationalUnitName, OrganizationalUnits.LocationID, Locations.Name AS LocationName, OrganizationalUnitUsers.InActive " + "\r\n";
            queryString = queryString + "       FROM        AspNetUsers " + "\r\n";
            queryString = queryString + "                   INNER JOIN OrganizationalUnitUsers ON AspNetUsers.UserID = OrganizationalUnitUsers.UserID AND (@ActiveOption = " + (int)GlobalEnums.ActiveOption.Both + " OR OrganizationalUnitUsers.InActive = @ActiveOption) " + "\r\n";
            queryString = queryString + "                   INNER JOIN OrganizationalUnits ON OrganizationalUnitUsers.OrganizationalUnitID = OrganizationalUnits.OrganizationalUnitID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON OrganizationalUnits.LocationID = Locations.LocationID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetUserIndexes", queryString);
        }


        private void GetLocationOrganizationalUnits()
        {
            string queryString;

            queryString = " @Nothing int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      OrganizationalUnits.OrganizationalUnitID, Locations.Code + '\\' + OrganizationalUnits.Code AS LocationOrganizationalUnitCode " + "\r\n";
            queryString = queryString + "       FROM        OrganizationalUnits INNER JOIN Locations ON OrganizationalUnits.LocationID = Locations.LocationID " + "\r\n";
            queryString = queryString + "       ORDER BY    LocationOrganizationalUnitCode " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetLocationOrganizationalUnits", queryString);
        }

        private void UserEditable()
        {
            string[] queryArray = new string[9];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = UserID FROM SalesOrders WHERE UserID = @EntityID OR PreparedPersonID = @EntityID OR ApproverID = @EntityID ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = UserID FROM DeliveryAdvices WHERE UserID = @EntityID OR PreparedPersonID = @EntityID OR ApproverID = @EntityID ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = UserID FROM SalesReturns WHERE UserID = @EntityID OR PreparedPersonID = @EntityID OR ApproverID = @EntityID ";
            queryArray[3] = " SELECT TOP 1 @FoundEntity = UserID FROM GoodsIssues WHERE UserID = @EntityID OR PreparedPersonID = @EntityID OR ApproverID = @EntityID ";
            queryArray[4] = " SELECT TOP 1 @FoundEntity = UserID FROM HandlingUnits WHERE UserID = @EntityID OR PreparedPersonID = @EntityID OR ApproverID = @EntityID ";
            queryArray[5] = " SELECT TOP 1 @FoundEntity = UserID FROM GoodsDeliveries WHERE UserID = @EntityID OR PreparedPersonID = @EntityID OR ApproverID = @EntityID ";
            queryArray[6] = " SELECT TOP 1 @FoundEntity = UserID FROM AccountInvoices WHERE UserID = @EntityID OR PreparedPersonID = @EntityID OR ApproverID = @EntityID ";
            queryArray[7] = " SELECT TOP 1 @FoundEntity = UserID FROM Receipts WHERE UserID = @EntityID OR PreparedPersonID = @EntityID OR ApproverID = @EntityID ";
            queryArray[8] = " SELECT TOP 1 @FoundEntity = UserID FROM CreditNotes WHERE UserID = @EntityID OR PreparedPersonID = @EntityID OR ApproverID = @EntityID ";

            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("UserEditable", queryArray);
        }

        private void UserRegister()
        {
            string queryString = " @UserID Int, @OrganizationalUnitID int, @SameOUAccessLevel int, @SameLocationAccessLevel int, @OtherOUAccessLevel int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            //LUU Y RAT QUAN TRONG - VERY IMPORTANT
            //AT NOW: OrganizationalUnitUsers KHONG CON PHU HOP NUA, TUY NHIEN, VAN PHAI DUY TRI VI KHONG CO THOI GIAN MODIFY
            //YEU CAU LUC NAY LA: LAM SAO DAM BAO Users(UserID, OrganizationalUnitID) VA OrganizationalUnitUsers(OrganizationalUnitID, UserID) PHAI MATCH 1-1
            //DO DO: KHI ADD, REMOVE, EDIT, INACTIVE, ... PHAI DAM BAO YEU CAU NAY THI MOI THU SE OK
            queryString = queryString + "       BEGIN " + "\r\n";

            //---08/JAN/2018: HIỆN TẠI KHÔNG CHO PHÉP ĐĂNG KÝ TRÙNG USER-LOCATION
            //--->TUY NHIÊN, HOÀN TOÀN CÓ THỂ CẢI TIẾN CHỔ NÀY, CHO PHÉP ĐĂNG KÝ TRÙNG CHO NEW USER-LOCATION NẾU: Users.InActive = 0 AND 
            //--->KHI ĐÓ: UserToggleVoid: CẦN PHẢI XEM XÉT LẠI --> NHẰM ĐẢM BẢO RẰNG: NẾU ĐÃ CÓ 1 InActive THÌ SẼ KHÔNG THỂ ENABLE CÙNG 1 USER-LOCATION
            //--->TỨC LÀ: CÓ THỂ CẢI TIẾN --> CHO PHÉP TRÙNG USER-LOCATION, TUY NHIÊN: PHẢI ĐẢM BẢO CHỈ CÓ 1 USER-LOCATION LÀ InActive = 0
            //--->NHU CẦU ĐĂMG KÝ TRÙNG USER-LOCATION LÀ CÓ: NÓ GIẢI QUYẾT VẤN ĐỀ CẤP LẠI OrganizationalUnits CHO USER TRONG CÙNG LOCATION (VÍ DỤ: CẦN CHIA OrganizationalUnits => DO ĐÓ: PHẢI ĐĂNG KÝ LẠI USER-LOCATION CHO MỘT OrganizationalUnits KHÁC)
            queryString = queryString + "           INSERT INTO     OrganizationalUnitUsers (OrganizationalUnitID, UserID, InActive, InActiveDate) VALUES (@OrganizationalUnitID, @UserID, 0, NULL); " + "\r\n";

            queryString = queryString + "           INSERT INTO     AccessControls (UserID, NMVNTaskID, OrganizationalUnitID, AccessLevel, ApprovalPermitted, UnApprovalPermitted, VoidablePermitted, UnVoidablePermitted, ShowDiscount, InActive) " + "\r\n";
            queryString = queryString + "           SELECT          @UserID, ModuleDetails.TaskID, OrganizationalUnits.OrganizationalUnitID, CASE WHEN OrganizationalUnits.OrganizationalUnitID = @OrganizationalUnitID THEN @SameOUAccessLevel WHEN OrganizationalUnits.LocationID = (SELECT LocationID FROM OrganizationalUnits WHERE OrganizationalUnitID = @OrganizationalUnitID) THEN @SameLocationAccessLevel ELSE @OtherOUAccessLevel END AS AccessLevel, CASE WHEN OrganizationalUnits.OrganizationalUnitID = @OrganizationalUnitID AND @SameOUAccessLevel = " + (int)GlobalEnums.AccessLevel.Editable + " THEN 1 ELSE 0 END AS ApprovalPermitted, CASE WHEN OrganizationalUnits.OrganizationalUnitID = @OrganizationalUnitID AND @SameOUAccessLevel = " + (int)GlobalEnums.AccessLevel.Editable + " THEN 1 ELSE 0 END AS UnApprovalPermitted, CASE WHEN OrganizationalUnits.OrganizationalUnitID = @OrganizationalUnitID AND @SameOUAccessLevel = " + (int)GlobalEnums.AccessLevel.Editable + " THEN 1 ELSE 0 END AS VoidablePermitted, CASE WHEN OrganizationalUnits.OrganizationalUnitID = @OrganizationalUnitID AND @SameOUAccessLevel = " + (int)GlobalEnums.AccessLevel.Editable + " THEN 1 ELSE 0 END AS UnVoidablePermitted, 0 AS ShowDiscount, 0 AS InActive " + "\r\n";
            queryString = queryString + "           FROM            ModuleDetails CROSS JOIN OrganizationalUnits" + "\r\n";
            queryString = queryString + "           WHERE           ModuleDetails.InActive = 0; " + "\r\n";

            queryString = queryString + "           INSERT INTO     ReportControls (UserID, ReportID, Enabled) " + "\r\n";
            queryString = queryString + "           SELECT          @UserID, ReportID, 0 AS Enabled " + "\r\n";
            queryString = queryString + "           FROM            Reports     ORDER BY ReportID; " + "\r\n";

            queryString = queryString + "       END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("UserRegister", queryString);
        }

        private void UserUnregister()
        {
            string queryString = " @UserID int, @UserName nvarchar(256), @OrganizationalUnitName nvarchar(256)" + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";

            queryString = queryString + "           DECLARE     @FoundEntitys TABLE (FoundEntity int NULL) " + "\r\n";
            queryString = queryString + "           INSERT INTO @FoundEntitys EXEC UserEditable @UserID " + "\r\n";

            queryString = queryString + "           IF (SELECT COUNT(*) FROM @FoundEntitys WHERE NOT FoundEntity IS NULL) <= 0 " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DELETE FROM     AccessControls WHERE UserID = @UserID " + "\r\n";
            queryString = queryString + "                   DELETE FROM     OrganizationalUnitUsers WHERE UserID = @UserID " + "\r\n";
            queryString = queryString + "                   DELETE FROM     ReportControls WHERE UserID = @UserID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "           ELSE " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'Không thể hủy đăng ký ' + @UserName + N' tại ' + @OrganizationalUnitName + N', do ' + @UserName + N' hiện đang có dữ liệu tại ' + @OrganizationalUnitName + N'\r\n\r\n\r\nVui lòng Inactive để dừng đăng ký và sử dụng ' + @UserName + N' tại ' + @OrganizationalUnitName + '.' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "       END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("UserUnregister", queryString);
        }

        private void UserToggleVoid()
        {
            string SQW = " AND UserID NOT IN (SELECT AspNetUsers.UserID FROM AspNetUsers INNER JOIN AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId INNER JOIN AspNetRoles ON AspNetUserRoles.RoleId = AspNetRoles.Id WHERE AspNetRoles.Name = N'Admin' OR AspNetRoles.Name = N'Vendor')";
            

            string queryString = " @EntityID int, @InActive bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      AccessControls              SET InActive = @InActive                            WHERE UserID = @EntityID AND InActive = ~@InActive" + SQW + "\r\n";
            queryString = queryString + "       UPDATE      OrganizationalUnitUsers     SET InActive = @InActive, InActiveDate = GetDate()  WHERE UserID = @EntityID AND InActive = ~@InActive" + SQW + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("UserToggleVoid", queryString);
        }

        private void GetUserAccessControls()
        {
            string queryString;

            queryString = " @UserID int, @NMVNTaskID int" + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      AccessControls.AccessControlID, OrganizationalUnits.LocationID, Locations.Name AS LocationName, OrganizationalUnits.Name AS OrganizationalUnitName, AccessControls.OrganizationalUnitID, AccessControls.AccessLevel, AccessControls.ApprovalPermitted, AccessControls.UnApprovalPermitted, AccessControls.VoidablePermitted, AccessControls.UnVoidablePermitted, AccessControls.ShowDiscount " + "\r\n";
            queryString = queryString + "       FROM        AccessControls INNER JOIN OrganizationalUnits ON AccessControls.OrganizationalUnitID = OrganizationalUnits.OrganizationalUnitID INNER JOIN Locations ON OrganizationalUnits.LocationID = Locations.LocationID " + "\r\n";
            queryString = queryString + "       WHERE       AccessControls.UserID = @UserID AND AccessControls.NMVNTaskID = @NMVNTaskID " + "\r\n";
            queryString = queryString + "       ORDER BY    Locations.Name, OrganizationalUnits.Name " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetUserAccessControls", queryString);
        }

        private void SaveUserAccessControls()
        {
            string queryString = " @AccessControlID int, @AccessLevel Int, @ApprovalPermitted bit, @UnApprovalPermitted bit, @VoidablePermitted bit, @UnVoidablePermitted bit, @ShowDiscount bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN " + "\r\n";

            queryString = queryString + "           UPDATE          AccessControls " + "\r\n";
            queryString = queryString + "           SET             AccessLevel = @AccessLevel, ApprovalPermitted = @ApprovalPermitted, UnApprovalPermitted = @UnApprovalPermitted, VoidablePermitted = @VoidablePermitted, UnVoidablePermitted = @UnVoidablePermitted, ShowDiscount = @ShowDiscount " + "\r\n";
            queryString = queryString + "           WHERE           AccessControlID = @AccessControlID " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> 1 " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'Unknow error: Update AccessControls. Please exit then open and try again.' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("SaveUserAccessControls", queryString);
        }





        private void GetUserReportControls()
        {
            string queryString;

            queryString = " @UserID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      ReportControls.ReportControlID, ReportControls.ReportID, Reports.ReportGroupName, Reports.ReportName, ReportControls.Enabled " + "\r\n";
            queryString = queryString + "       FROM        ReportControls INNER JOIN Reports ON ReportControls.UserID = @UserID AND ReportControls.ReportID = Reports.ReportID " + "\r\n";
            queryString = queryString + "       ORDER BY    Reports.ReportGroupName, Reports.ReportName " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetUserReportControls", queryString);
        }

        private void SaveUserReportControls()
        {
            string queryString = " @ReportControlID int, @Enabled bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN " + "\r\n";

            queryString = queryString + "           UPDATE          ReportControls " + "\r\n";
            queryString = queryString + "           SET             Enabled = @Enabled " + "\r\n";
            queryString = queryString + "           WHERE           ReportControlID = @ReportControlID " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> 1 " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'Unknow error: Update ReportControls. Please exit then open and try again.' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("SaveUserReportControls", queryString);
        }


    }
}