public static class SessionService
{
    public static UserModel? CurrentUser { get; set; }
    public static bool IsLoggedIn => CurrentUser != null;
    public static void Login(UserModel user)
    {
        CurrentUser = user;
    }
    public static void Logout()
    {
        CurrentUser = null;
    }
}