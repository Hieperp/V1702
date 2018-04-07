namespace TotalBase.Enums
{
    public static class GlobalEnums
    {
        public static bool ERPConnected = true;

        public static int CalculatingTypeID = 1; //JUST CHANGE FROM [0] TO [1] AT 12.00PM ON 24.MAR.2O18 => TO SHOW THE ORIGINAL GROSS PRICE, GROSS AMOUNT ON PXK. 
        //THEO SỰ XEM XÉT NGÀY 24.MAR.2O18 THÌ THAY ĐỔI NÀY SẼ THAY ĐỔI 4 GIÁ TRỊ: ListedVATAmount, VATAmount, ListedGrossAmount, GrossAmount. 
        //TUY NHIÊN, NÓ HOÀN TOÀN KHÔNG THAY ĐỔI GIÁ TRỊ CỦA TABLE MASTER, VÌ: VATbyRow = false: TỨC LÀ: TotalListedVATAmount, TotalVATAmount, TotalListedGrossAmount, TotalGrossAmount ĐƯỢC TÍNH BỞI definedExemplar.prototype._updateTotalVATAmountToModelProperty: CÁCH TÍNH VAT 1 DÒNG
        //TÚM LẠI: CalculatingTypeID = [0] HAY [1] KHI VATbyRow = false: THÌ GIÁ TRỊ TỔNG CỦA TotalListedVATAmount, TotalVATAmount, TotalListedGrossAmount, TotalGrossAmount LÀ KHÔNG ĐỔI [DO: VATbyRow = false: CÁCH TÍNH VAT 1 DÒNG]

        public static bool VATbyRow = false;
        public static decimal VATPercent = 10;

        public static int rndQuantity = 0;
        public static int rndAmount = 0;
        public static int rndDiscountPercent = 1;

        public static int rndWeight = 2;

        public enum SubmitTypeOption
        {
            Save = 0, //Save and return (keep) current view
            Popup = 1, //Save popup windows
            Create = 3, //Save and the create new
            Closed = 9 //Save and close (return index view)
        };

        public enum NmvnTaskID
        {
            UnKnown = 0,



            Customer = 8001,
            Commodity = 8002,
            Promotion = 8003,
            Employee = 8005,
            CommodityPrice = 8006,

            PurchaseOrder = 8021,
            PurchaseInvoice = 8022,



            Quotation = 8031,
            SalesOrder = 8032,
            DeliveryAdvice = 8035,
            GoodsIssue = 8037,

            SalesInvoice = 8051,

            VehiclesInvoice = 8052,
            PartsInvoice = 8053,
            ServicesInvoice = 8055,


            ServiceContract = 8056,

            AccountInvoice = 8057,
            Receipt = 8059,


            SalesReturn = 8038,
            CreditNote = 8060,


            GoodsReceipt = 8077,
            InventoryAdjustment = 8078,
            VehicleAdjustment = 8078008,
            PartAdjustment = 8078009,

            HandlingUnit = 9010,
            GoodsDelivery = 9012,

            TransferOrder = 8071,
            VehicleTransferOrder = 8071008,
            PartTransferOrder = 8071009,

            StockTransfer = 8073,
            VehicleTransfer = 8075,
            PartTransfer = 8076

        };

        public enum GoodsReceiptTypeID
        {
            AllGoodsReceipt = 999,
            PurchaseInvoice = 1,
            GoodsReturn = 2,
            StockTransfer = 3,
            InventoryAdjustment = 4
        };

        public enum SalesInvoiceTypeID
        {
            AllInvoice = 1,
            VehiclesInvoice = 10,
            PartsInvoice = 20,
            ServicesInvoice = 30
        };

        public enum StockTransferTypeID
        {
            VehicleTransfer = 10,
            PartTransfer = 20
        };

        public enum ServiceContractTypeID
        {
            Warranty = 1,
            Repair = 2,
            Maintenance = 3
        };

        public enum InventoryAdjustmentTypeID
        {
            AllAdjustment = 1,
            VehicleAdjustment = 10,
            PartAdjustment = 20
        };


        public enum CommodityTypeID
        {
            All = 999,
            Vehicles = 1,
            Parts = 2,
            Consumables = 3,
            Services = 6,
            CreditNote = 8,
            Unknown = 99
        };

        public enum WarehouseTaskID
        {
            SalesOrder = 8032,
            DeliveryAdvice = 8035,
            SalesReturn = 8038
        };

        public enum WarehouseClassID
        {
            L1 = 1,//Loại 1
            L2 = 2,//Loại 2
            L3 = 3,//Loại 3
            L4 = 4,//Thiếu
            L5 = 5,//Dư
            LD = 9 //Lên Đời
        }

        public enum WarehouseGroupID
        {
            CO = 1,
            HD = 2,
            MT = 3,
            TT = 4,
            XK = 5,
            ST = 6,
            BC = 7
        }


        public enum ReceiptTypeID
        {
            ReceiveMoney = 1,
            ApplyCredit = 2
        };


        public enum UpdateWarehouseBalanceOption
        {
            Add = 1,
            Minus = -1
        };


        public enum AccessLevel
        {
            Deny = 0,
            Readable = 1,
            Editable = 2
        };
    }


    public static class GlobalReceiptTypeID
    {
        public static int ApplyCredit { get { return (int)GlobalEnums.ReceiptTypeID.ApplyCredit; } }
        public static int ReceiveMoney { get { return (int)GlobalEnums.ReceiptTypeID.ReceiveMoney; } }
    }

    public static class GlobalCreditTypeID
    {
        public static int AdvanceReceipt { get { return (int)GlobalEnums.NmvnTaskID.Receipt; } }
        public static int SalesReturn { get { return (int)GlobalEnums.NmvnTaskID.SalesReturn; } }
        public static int CreditNote { get { return (int)GlobalEnums.NmvnTaskID.CreditNote; } }
    }

}
