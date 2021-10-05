using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine
{
    public class FerryApplicationSettings: IFerryApplicationSettings
    {
        //это класс, который отвечает за настройки приложения


        public int TestInt { get; set; } = 1;

        private FerryApplicationSettings()
        {

        }

        private static FerryApplicationSettings _myInstance =null;

        public static FerryApplicationSettings GetMyInstance()
        {
            if (_myInstance == null) _myInstance = new FerryApplicationSettings();
            return _myInstance;
        }

        //запуск в режиме моно-юзреа, когда за авторизацией ходит к себе же
        public bool MonoUser { get; } = false; 

        public string AuthServerAddress { get { return "http://localhost:18155"; } }
        public string LoginCallbackUrl { get { return "https://localhost:44326/authentication/login-callback"; } }
        public string LogoutCallbackUrl { get { return "https://localhost:44326/authentication/logout-callback"; } }

    }
}
