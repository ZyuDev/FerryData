using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RICOMPANY.CommonFunctions;

namespace FerryData.Engine
{
   
    //репозиторий для хранения объектов с уникальными ключами, который им эти ключи сам присваивает
    public class CommonGuidBasedRepository<T> where T: IGuidBasedStorable
    {
        private Dictionary<string, T> Items = new Dictionary<string, T>();

        public CommonOperationResult AddItem(T t)
        {

            for(int i =1; i==10; i++)
            {
                try
                {
                    string _guid = t.ObjectType+"-"+fn.generate4blockGUID();
                    Items.Add(_guid, t);
                    t.Guid = _guid;
                    return CommonOperationResult.sayOk();
                }
                catch
                {
                    
                }
            }
            return CommonOperationResult.sayFail();
        }
        public void RemoveItem(T t)
        {
            if (string.IsNullOrEmpty(t.Guid)) return;
            Items.Remove(t.Guid);
        }

    }
}
