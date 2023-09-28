using System.Threading.Tasks;

namespace Common
{
    public interface IKeyvaultClient
    {
        Task FetchConnectionStringsFromKeyvault();
    }
}