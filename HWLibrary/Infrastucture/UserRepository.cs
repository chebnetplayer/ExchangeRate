using System.Collections.Generic;
using System.Linq;
using HWLibrary.Domain;

namespace HWLibrary.Infrastucture
{
    public class UserRepository
    {
        private readonly List<User> _users=new List<User>();

        public bool LoadUser(string name)
        {
            if (_users.FirstOrDefault(i => i.Name == name) != null) return false;
            _users.Add(new User(name));
            return true;
        }

        public void UserExit(string name)
        {
            var user = _users.Find(i => i.Name == name);
            _users.Remove(user);
        }

    }
}
