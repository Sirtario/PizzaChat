using System;
using System.IO;
using System.Security.Cryptography;
using PIZZA.Hub.Interface;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PIZZA.Hub
{
    class HubServerAuthentication
    {
        private string _authenticationpassword;
        private MD5 _md5;
       

        public HubServerAuthentication()
        {
            LoadPassword();
            _md5 = new MD5CryptoServiceProvider();
        }

        public void LoadPassword()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + @"\auth.cfg"))
            {
                _authenticationpassword = File.ReadAllText(Directory.GetCurrentDirectory() + @"\auth.cfg");
                HubTerminal.Cout(ConsoleColor.Green, "[Authentication] Server Authentication Password loaded...");
               
            }
            else
            {
                _authenticationpassword = String.Empty;
                HubTerminal.Cout(ConsoleColor.Red, "[Authentication] Password could not be loaded...");
               
            }
        }

        public bool IsPasswordSet()
        {
            if (_authenticationpassword != String.Empty)
                return true;
            else return false;
        }

        public int SetPassword(string[] input)
        {
            byte[] stringInput = Encoding.UTF8.GetBytes(input[0]);
            byte[] hash = _md5.ComputeHash(stringInput);

            _authenticationpassword = Encoding.UTF8.GetString(hash);

            File.WriteAllText(Directory.GetCurrentDirectory() + @"\auth.cfg", _authenticationpassword);

            return 0;
        }

        public int ShowPassword(string[] input)
        {
            HubTerminal.Cout(ConsoleColor.White, _authenticationpassword);

            return 0;
        }

        public bool IsPasswordValid(string md5)
        {
            if (md5 == _authenticationpassword)
                return true;
            else return false;
        }
    }
}
