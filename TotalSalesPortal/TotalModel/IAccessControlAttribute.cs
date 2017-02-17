namespace TotalModel
{
    public interface IAccessControlAttribute
    {
        int UserID { get; set; }
        int PreparedPersonID { get; set; }
        int OrganizationalUnitID { get; set; }
        int LocationID { get; set; }
    }
}
