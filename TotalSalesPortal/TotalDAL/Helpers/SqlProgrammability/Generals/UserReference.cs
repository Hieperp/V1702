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
            //this.GetUserIndexes();

            //this.GetActiveUsers();

            //this.UserEditable();
            //this.UserRegister();
            //this.UserUnregister();
            //this.UserToggleVoid();

            //this.GetUserAccessControls();
            //this.SaveUserAccessControls();

            this.GetUserTrees();
        }


        private void GetUserIndexes()
        {
            string queryString;

            queryString = " @UserID Int, @FromDate DateTime, @ToDate DateTime, @ActiveOption int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Users.UserID, Users.FirstName, Users.LastName, Users.UserName, Users.SecurityIdentifier, Users.IsDatabaseAdmin, OrganizationalUnits.Name AS OrganizationalUnitName, OrganizationalUnits.LocationID, Locations.Name AS LocationName, OrganizationalUnitUsers.InActive " + "\r\n";
            queryString = queryString + "       FROM        Users " + "\r\n";
            queryString = queryString + "                   INNER JOIN OrganizationalUnitUsers ON Users.UserID = OrganizationalUnitUsers.UserID AND (@ActiveOption = " + (int)GlobalEnums.ActiveOption.Both + " OR Users.InActive = @ActiveOption) " + "\r\n";
            queryString = queryString + "                   INNER JOIN OrganizationalUnits ON OrganizationalUnitUsers.OrganizationalUnitID = OrganizationalUnits.OrganizationalUnitID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON OrganizationalUnits.LocationID = Locations.LocationID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetUserIndexes", queryString);
        }

        private void GetActiveUsers()
        {
            string queryString;

            queryString = " @SecurityIdentifier nvarchar(256) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Users.UserID, Users.FirstName, Users.LastName, Users.UserName, Users.SecurityIdentifier, Users.IsDatabaseAdmin, Users.OrganizationalUnitID, OrganizationalUnits.Name AS OrganizationalUnitName, OrganizationalUnits.LocationID, Locations.Name AS LocationName " + "\r\n";
            queryString = queryString + "       FROM        Users " + "\r\n";
            queryString = queryString + "                   INNER JOIN OrganizationalUnits ON Users.SecurityIdentifier = @SecurityIdentifier AND Users.InActive = 0 AND Users.OrganizationalUnitID = OrganizationalUnits.OrganizationalUnitID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON OrganizationalUnits.LocationID = Locations.LocationID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetActiveUsers", queryString);
        }

        private void UserEditable()
        {
            string[] queryArray = new string[8];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = UserID FROM BinLocations WHERE UserID = @EntityID ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = UserID FROM SalesOrders WHERE UserID = @EntityID ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = UserID FROM DeliveryAdvices WHERE UserID = @EntityID ";
            queryArray[3] = " SELECT TOP 1 @FoundEntity = UserID FROM TransferOrders WHERE UserID = @EntityID ";
            queryArray[4] = " SELECT TOP 1 @FoundEntity = UserID FROM GoodsIssues WHERE UserID = @EntityID ";
            queryArray[5] = " SELECT TOP 1 @FoundEntity = UserID FROM Pickups WHERE UserID = @EntityID ";
            queryArray[6] = " SELECT TOP 1 @FoundEntity = UserID FROM GoodsReceipts WHERE UserID = @EntityID ";
            queryArray[7] = " SELECT TOP 1 @FoundEntity = UserID FROM WarehouseAdjustments WHERE UserID = @EntityID ";


            this.totalSalesPortalEntities.CreateProcedureToCheckExisting("UserEditable", queryArray);
        }

        private void UserRegister()
        {
            string queryString = " @LocationID int, @OrganizationalUnitID int, @FirstName nvarchar(60), @LastName nvarchar(60), @UserName nvarchar(256), @SecurityIdentifier nvarchar(256), @SameOUAccessLevel int, @SameLocationAccessLevel int, @OtherOUAccessLevel int " + "\r\n";
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
            queryString = queryString + "           IF (SELECT COUNT(Users.UserID) FROM Users INNER JOIN OrganizationalUnits ON Users.OrganizationalUnitID = OrganizationalUnits.OrganizationalUnitID WHERE OrganizationalUnits.LocationID = @LocationID AND Users.SecurityIdentifier = @SecurityIdentifier) <= 0 " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE         @UserID Int" + "\r\n";
            queryString = queryString + "                   INSERT INTO     Users (OrganizationalUnitID, FirstName, LastName, UserName, SecurityIdentifier, IsDatabaseAdmin, InActive) VALUES (@OrganizationalUnitID, @FirstName, @LastName, @UserName, @SecurityIdentifier, 0, 0); " + "\r\n";
            queryString = queryString + "                   SELECT          @UserID = SCOPE_IDENTITY(); " + "\r\n";
            queryString = queryString + "                   INSERT INTO     OrganizationalUnitUsers (OrganizationalUnitID, UserID, InActive, InActiveDate) VALUES (@OrganizationalUnitID, @UserID, 0, NULL); " + "\r\n";

            queryString = queryString + "                   INSERT INTO     AccessControls (UserID, NMVNTaskID, OrganizationalUnitID, AccessLevel, ApprovalPermitted, UnApprovalPermitted, VoidablePermitted, UnVoidablePermitted, ShowDiscount, InActive) " + "\r\n";
            queryString = queryString + "                   SELECT          @UserID, ModuleDetails.ModuleDetailID, OrganizationalUnits.OrganizationalUnitID, CASE WHEN OrganizationalUnits.OrganizationalUnitID = @OrganizationalUnitID THEN @SameOUAccessLevel WHEN OrganizationalUnits.LocationID = @LocationID THEN @SameLocationAccessLevel ELSE @OtherOUAccessLevel END AS AccessLevel, CASE WHEN OrganizationalUnits.OrganizationalUnitID = @OrganizationalUnitID AND @SameOUAccessLevel = " + (int)GlobalEnums.AccessLevel.Editable + " THEN 1 ELSE 0 END AS ApprovalPermitted, CASE WHEN OrganizationalUnits.OrganizationalUnitID = @OrganizationalUnitID AND @SameOUAccessLevel = " + (int)GlobalEnums.AccessLevel.Editable + " THEN 1 ELSE 0 END AS UnApprovalPermitted, CASE WHEN OrganizationalUnits.OrganizationalUnitID = @OrganizationalUnitID AND @SameOUAccessLevel = " + (int)GlobalEnums.AccessLevel.Editable + " THEN 1 ELSE 0 END AS VoidablePermitted, CASE WHEN OrganizationalUnits.OrganizationalUnitID = @OrganizationalUnitID AND @SameOUAccessLevel = " + (int)GlobalEnums.AccessLevel.Editable + " THEN 1 ELSE 0 END AS UnVoidablePermitted, 0 AS ShowDiscount, 0 AS InActive " + "\r\n";
            queryString = queryString + "                   FROM            ModuleDetails CROSS JOIN OrganizationalUnits" + "\r\n";
            queryString = queryString + "                   WHERE           ModuleDetails.InActive = 0; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

            queryString = queryString + "           ELSE " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'Đăng ký user trùng location.' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";

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
            queryString = queryString + "                   DELETE FROM     Users WHERE UserID = @UserID " + "\r\n";
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
            string queryString = " @EntityID int, @InActive bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      Users                       SET InActive = @InActive                            WHERE UserID = @EntityID AND InActive = ~@InActive" + "\r\n";
            queryString = queryString + "       UPDATE      AccessControls              SET InActive = @InActive                            WHERE UserID = @EntityID AND InActive = ~@InActive" + "\r\n";
            queryString = queryString + "       UPDATE      OrganizationalUnitUsers     SET InActive = @InActive, InActiveDate = GetDate()  WHERE UserID = @EntityID AND InActive = ~@InActive" + "\r\n";

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
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'Unknow error: SaveUserAccessControls. Please exit then open and try again.' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("SaveUserAccessControls", queryString);
        }


        //private void GetUserTrees()
        //{
        //    string queryString;

        //    queryString = " @ActiveOption int " + "\r\n";
        //    queryString = queryString + " WITH ENCRYPTION " + "\r\n";
        //    queryString = queryString + " AS " + "\r\n";
        //    queryString = queryString + "    BEGIN " + "\r\n";

        //    queryString = queryString + "       SELECT      " + GlobalEnums.RootNode + " + LocationID AS NodeID, 0 AS ParentNodeID, LocationID AS PrimaryID, NULL AS AncestorID, Code, Name, 'LocationID' AS ParameterName, CAST(0 AS bit) AS Selected " + "\r\n";
        //    queryString = queryString + "       FROM        Locations " + "\r\n";

        //    queryString = queryString + "       UNION ALL " + "\r\n";
        //    queryString = queryString + "       SELECT      " + GlobalEnums.AncestorNode + " + OrganizationalUnitID AS NodeID, " + GlobalEnums.RootNode + " + LocationID AS ParentNodeID, OrganizationalUnitID AS PrimaryID, LocationID AS AncestorID, Code, Name, 'OrganizationalUnitID' AS ParameterName, CAST(0 AS bit) AS Selected " + "\r\n";
        //    queryString = queryString + "       FROM        OrganizationalUnits " + "\r\n";
        //    queryString = queryString + "       UNION ALL " + "\r\n";
        //    queryString = queryString + "       SELECT      UserID AS NodeID, " + GlobalEnums.AncestorNode + " + OrganizationalUnitID AS ParentNodeID, UserID AS PrimaryID, OrganizationalUnitID AS AncestorID, SecurityIdentifier AS Code, UserName AS Name, 'UserID' AS ParameterName, InActive AS Selected " + "\r\n";
        //    queryString = queryString + "       FROM        Users " + "\r\n";
        //    queryString = queryString + "       WHERE       (@ActiveOption = " + (int)GlobalEnums.ActiveOption.Both + " OR Users.InActive = @ActiveOption) " + "\r\n";

        //    queryString = queryString + "    END " + "\r\n";

        //    this.totalSalesPortalEntities.CreateStoredProcedure("GetUserTrees", queryString);
        //}

        private void GetUserTrees()
        {
            string queryString;

            queryString = " @Id int, @ActiveOption int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF @Id IS NULL " + "\r\n";

            queryString = queryString + "           SELECT      " + GlobalEnums.RootNode + " + LocationID AS id, LocationID AS PrimaryID, Name, CAST(1 AS bit) AS hasChildren " + "\r\n";
            queryString = queryString + "           FROM        Locations " + "\r\n";
            queryString = queryString + "           WHERE       LocationID IN (SELECT LocationID FROM OrganizationalUnits) " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               IF @Id >= " + GlobalEnums.RootNode + " AND @Id <= " + GlobalEnums.AncestorNode + "\r\n";

            queryString = queryString + "                   SELECT      " + GlobalEnums.AncestorNode + " + OrganizationalUnitID AS id, OrganizationalUnitID AS PrimaryID, Code AS Name, CAST(1 AS bit) AS hasChildren " + "\r\n";
            queryString = queryString + "                   FROM        OrganizationalUnits " + "\r\n";
            queryString = queryString + "                   WHERE       OrganizationalUnitID IN (SELECT OrganizationalUnitUsers.OrganizationalUnitID FROM AspNetUsers INNER JOIN OrganizationalUnitUsers ON AspNetUsers.UserID = OrganizationalUnitUsers.UserID AND (@ActiveOption = " + (int)GlobalEnums.ActiveOption.Both + " OR OrganizationalUnitUsers.InActive = @ActiveOption)) AND " + GlobalEnums.RootNode + " + LocationID = @Id " + "\r\n";

            queryString = queryString + "                   " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n";

            queryString = queryString + "                   SELECT      AspNetUsers.UserID AS id, AspNetUsers.UserID AS PrimaryID, AspNetUsers.UserName AS Name, CAST(0 AS bit) AS hasChildren " + "\r\n";
            queryString = queryString + "                   FROM        AspNetUsers INNER JOIN OrganizationalUnitUsers ON AspNetUsers.UserID = OrganizationalUnitUsers.UserID " + "\r\n";
            queryString = queryString + "                   WHERE       " + GlobalEnums.AncestorNode + " + OrganizationalUnitUsers.OrganizationalUnitID = @Id AND (@ActiveOption = " + (int)GlobalEnums.ActiveOption.Both + " OR OrganizationalUnitUsers.InActive = @ActiveOption) " + "\r\n";
            
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSalesPortalEntities.CreateStoredProcedure("GetUserTrees", queryString);
        }

    }
}