using System.Threading.Tasks;

namespace FerryData.Server.Data
{
    public interface IDbInitializer
    {
        void InitializeDb();
    }
}
