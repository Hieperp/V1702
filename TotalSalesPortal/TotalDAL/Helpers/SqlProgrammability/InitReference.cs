namespace TotalDAL.Helpers.SqlProgrammability
{
    public class SimpleInitReference
    {
        protected readonly string tableName;
        protected readonly string identityName;
        protected readonly string referenceName;
        protected readonly int referenceLength;
        protected readonly string prefixLetter;

        public SimpleInitReference(string tableName, string identityName, string referenceName, int referenceLength, string prefixLetter)
        {
            this.tableName = tableName;
            this.identityName = identityName;
            this.referenceName = referenceName;
            this.referenceLength = referenceLength;
            this.prefixLetter = prefixLetter;
        }

        public string CreateQuery()
        {
            string queryString = " ON " + this.tableName + " AFTER INSERT " + "\r\n";
            //queryString = queryString + " ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + this.QueryDeclare() + "\r\n";
            queryString = queryString + this.QueryPrefix() + "\r\n";

            queryString = queryString + "   DECLARE     @columnNameMax int " + "\r\n";

            queryString = queryString + "   SET         @columnNameMax = (SELECT MAX(CAST( SUBSTRING(" + this.referenceName + ", LEN(@PrefixLetter) + 1, CASE PATINDEX('%.%', " + this.referenceName + ") WHEN 0 THEN LEN(" + this.referenceName + ") - LEN(@PrefixLetter) ELSE PATINDEX('%.%', " + this.referenceName + ") - 1 - LEN(@PrefixLetter) END) AS Int)) AS columnNameMax FROM " + this.tableName + " " + this.QueryWhere() + ") ";

            queryString = queryString + "   IF          @columnNameMax IS NULL SET @columnNameMax = 1 ELSE SET @columnNameMax = @columnNameMax + 1 ";


            queryString = queryString + "   UPDATE      " + this.tableName + "\r\n";
            queryString = queryString + "   SET         " + this.referenceName + " = @PrefixLetter + RIGHT(CAST(100000000 + @columnNameMax as varchar), " + this.referenceLength + " - LEN(@PrefixLetter)) " + "\r\n";
            queryString = queryString + "   WHERE       " + this.identityName + " = @EntityID " + "\r\n";

            return queryString;
        }

        protected virtual string QueryDeclare()
        {
            string queryString = "          DECLARE     @LocationID int         SET @LocationID = (SELECT LocationID FROM Inserted) " + "\r\n";
            queryString = queryString + "   DECLARE     @EntityID int           SET @EntityID = (SELECT " + this.identityName + " FROM Inserted) " + "\r\n";
            queryString = queryString + "   DECLARE     @EntryDate DateTime     SET @EntryDate = (SELECT EntryDate FROM Inserted) " + "\r\n";

            return queryString;
        }

        protected virtual string QueryPrefix()
        {
            return "          DECLARE     @PrefixLetter varchar(10)   SET @PrefixLetter = '" + this.prefixLetter + "'\r\n";
        }

        protected virtual string QueryWhere()
        {
            return " WHERE LocationID = @LocationID AND YEAR(EntryDate) = YEAR(@EntryDate) AND MONTH(EntryDate) = MONTH(@EntryDate) " + "\r\n";
        }

    }

    public class SalesInvoiceInitReference : SimpleInitReference
    {
        public SalesInvoiceInitReference(string tableName, string identityName, string referenceName, int referenceLength, string prefixLetter): base(tableName, identityName, referenceName, referenceLength, prefixLetter)
        {}

        protected override string QueryDeclare()
        {
            string queryString = base.QueryDeclare() + "\r\n";
            queryString = queryString + "   DECLARE     @SalesInvoiceTypeID int           SET @SalesInvoiceTypeID = (SELECT SalesInvoiceTypeID FROM Inserted) " + "\r\n";

            return queryString;
        }

        protected override string QueryPrefix()
        {
            return "          DECLARE     @PrefixLetter varchar(10)   SET @PrefixLetter = " + this.prefixLetter + "\r\n";
        }

        protected override string QueryWhere()
        {
            string queryString = base.QueryWhere() + "\r\n";
            queryString = queryString + "   AND     SalesInvoiceTypeID = @SalesInvoiceTypeID " + "\r\n";
            return queryString;
        }
    }


    public class TransferOrderInitReference : SimpleInitReference
    {
        public TransferOrderInitReference(string tableName, string identityName, string referenceName, int referenceLength, string prefixLetter)
            : base(tableName, identityName, referenceName, referenceLength, prefixLetter)
        { }

        protected override string QueryDeclare()
        {
            string queryString = base.QueryDeclare() + "\r\n";
            queryString = queryString + "   DECLARE     @StockTransferTypeID int           SET @StockTransferTypeID = (SELECT StockTransferTypeID FROM Inserted) " + "\r\n";

            return queryString;
        }

        protected override string QueryPrefix()
        {
            return "          DECLARE     @PrefixLetter varchar(10)   SET @PrefixLetter = " + this.prefixLetter + "\r\n";
        }

        protected override string QueryWhere()
        {
            string queryString = base.QueryWhere() + "\r\n";
            queryString = queryString + "   AND     StockTransferTypeID = @StockTransferTypeID " + "\r\n";
            return queryString;
        }
    }

    public class StockTransferInitReference : SimpleInitReference
    {
        public StockTransferInitReference(string tableName, string identityName, string referenceName, int referenceLength, string prefixLetter)
            : base(tableName, identityName, referenceName, referenceLength, prefixLetter)
        { }

        protected override string QueryDeclare()
        {
            string queryString = base.QueryDeclare() + "\r\n";
            queryString = queryString + "   DECLARE     @StockTransferTypeID int           SET @StockTransferTypeID = (SELECT StockTransferTypeID FROM Inserted) " + "\r\n";

            return queryString;
        }

        protected override string QueryPrefix()
        {
            return "          DECLARE     @PrefixLetter varchar(10)   SET @PrefixLetter = " + this.prefixLetter + "\r\n";
        }

        protected override string QueryWhere()
        {
            string queryString = base.QueryWhere() + "\r\n";
            queryString = queryString + "   AND     StockTransferTypeID = @StockTransferTypeID " + "\r\n";
            return queryString;
        }
    }


    public class InventoryAdjustmentInitReference : SimpleInitReference
    {
        public InventoryAdjustmentInitReference(string tableName, string identityName, string referenceName, int referenceLength, string prefixLetter)
            : base(tableName, identityName, referenceName, referenceLength, prefixLetter)
        { }

        protected override string QueryDeclare()
        {
            string queryString = base.QueryDeclare() + "\r\n";
            queryString = queryString + "   DECLARE     @InventoryAdjustmentTypeID int           SET @InventoryAdjustmentTypeID = (SELECT InventoryAdjustmentTypeID FROM Inserted) " + "\r\n";

            return queryString;
        }

        protected override string QueryPrefix()
        {
            return "          DECLARE     @PrefixLetter varchar(10)   SET @PrefixLetter = " + this.prefixLetter + "\r\n";
        }

        protected override string QueryWhere()
        {
            string queryString = base.QueryWhere() + "\r\n";
            queryString = queryString + "   AND     InventoryAdjustmentTypeID = @InventoryAdjustmentTypeID " + "\r\n";
            return queryString;
        }
    }


}
