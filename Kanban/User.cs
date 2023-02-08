using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_Class
{
    class User
    {
        List<string> position = new List<string>
        {
            "Työntekijä",
            "Tiimivastaava"
        };


        private string _Username;
        private string _Password;

        public User(string username, string password)
        {
            _Username = username;
            _Password = password;
        }

        public string Get_Username()
        {
            return _Username;
        }
        public string Get_Password()
        {
            return _Password;
        }
        public void Set_Password(string password)
        {
            _Password = password;
        }
    }
}
