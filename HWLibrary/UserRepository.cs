using System.Collections.Generic;
using System.Linq;
using HWLibrary.Domain;

namespace HWLibrary.Infrastucture
{
    public class UserRepository
    {
        public readonly List<User> Users=new List<User>();

        public bool LoadUser(string name)
        {
            if (Users.FirstOrDefault(i => i.Name == name) != null) return false;
            Users.Add(new User(name));
            return true;
        }

        public void UserExit(string name)
        {
            var user = Users.Find(i => i.Name == name);
            Users.Remove(user);
        }
    }
}
