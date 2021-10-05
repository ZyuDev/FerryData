using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerryData.Engine
{
    public class FerrySession: IGuidBasedStorable
    {
        //это сессия, она одна на каждого залогиненного юзера

        private FerrySession(FerryApplication ferryApplication, FerryUser ferryUser)
        {
            _ferryApplication = ferryApplication;
            _ferryUser = ferryUser;
        }

        FerryApplication _ferryApplication;

        FerryUser _ferryUser { get; set; }

        public string Guid { get; set; }
        public string ObjectType { get { return "FerrySession"; } }

        public static FerrySession GetMyInstance(FerryApplication ferryApplication, FerryUser ferryUser)
        {
            try
            {
                //тут будут разные проверки на возможность создания сессии, поэтому try-catch
                return new FerrySession(ferryApplication, ferryUser);
            }
            catch
            {
                return null;
            }
            
        }



    }
}
