using System.Collections.Generic;
using System.Linq;

public static class UserService
{
    private static List<UserModel> _users = new List<UserModel>();
    private static int _nextId = 1;

    public static IReadOnlyList<UserModel> GetAll() => _users.AsReadOnly();
    public static UserModel? GetById(int id) => _users.Find(u => u.Id == id);
    public static UserModel? GetByEmail(string email) => _users.Find(u => u.Email == email);
    public static void Add(UserModel user)
    {
        user.Id = _nextId++;
        _users.Add(user);
    }
    public static bool ValidateLogin(string email, string password)
    {
        var user = GetByEmail(email);
        return user != null && user.Password == password; // In real app, hash passwords
    }
}