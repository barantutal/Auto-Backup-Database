
using System.Threading.Tasks;

namespace AutoBackup.Infrastructure.Provider
{
    public interface IProvider
    {
        Task UploadAsync(string file_path, string file_name);
        Task DeleteAsync(string fileNameForStorage);
    }

}