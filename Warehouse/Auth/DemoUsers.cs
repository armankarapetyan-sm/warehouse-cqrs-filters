namespace Warehouse.Auth;

public static class DemoUsers
{
    public static readonly DemoUser Clerk = new DemoUser
    {
        Id = "1",
        UserName = "clerk",
        Password = "clerk",
        Role = "Clerk"
    };

    public static readonly DemoUser Admin = new DemoUser
    {
        Id = "2",
        UserName = "admin",
        Password = "admin",
        Role = "Admin"
    };

    public static DemoUser? Find(string userName, string password)
    {
        if (userName == Clerk.UserName && password == Clerk.Password)
        {
            return Clerk;
        }

        if (userName == Admin.UserName && password == Admin.Password)
        {
            return Admin;
        }

        return null;
    }
}
