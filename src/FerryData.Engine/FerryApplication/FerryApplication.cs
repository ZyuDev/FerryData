using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine
{
    public class FerryApplication: IFerryApplication
    {
        //это класс самого приложения, самый глобальнык класс в FerryApp
        //здесь хранятся глобальные настройки, с которыми запускается приложение, а также сессии
        //это синглтон

        public FerryApplicationSettings Settings { get; set; } = FerryApplicationSettings.GetMyInstance();

        CommonGuidBasedRepository<FerrySession> FerrySessions = new CommonGuidBasedRepository<FerrySession>();

        public bool MonoUserLoggedIn { get; set; } = false;
        private FerryApplication()
        {

        }

        private static FerryApplication _myInstance =null;

        public static FerryApplication GetMyInstance()
        {
            if (_myInstance == null) _myInstance = new FerryApplication();
            return _myInstance;
        }

        public CommonOperationResult CreateNewSession(FerryUser UserData)
        {
            var rez=FerrySession.GetMyInstance(this, UserData);

            if (rez==null)
            {
                return CommonOperationResult.sayFail();
            }
            else
            {
                FerrySessions.AddItem(rez);
                return CommonOperationResult.sayOk();
            }
        }


    }
}
