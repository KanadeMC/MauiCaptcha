using ZALUPA.Database;
using ZALUPA.Database.Classes;

namespace ZALUPA.Database.Engine;

public class Actions
{
    public User? GetUser(string login, string password)
    {
        using (ApplicationContext context = new ApplicationContext())
        {
            User? user = context.Users.FirstOrDefault(u=> u.Login==login && u.Password==password);
            return user;
        }
    }
}