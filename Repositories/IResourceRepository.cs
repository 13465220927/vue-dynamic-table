using System.Collections.Generic;
using System.Threading.Tasks;
using Logistyx.Bios.WebApp.Entities;

namespace Logistyx.Bios.WebApp.Repositories
{
    public interface IResourceRepository
    {
        IEnumerable<Resource> GetResource();

        Task<Resource> GetResource(string id);

        Task<bool> PutResource(string id, Resource resource);

        Task<bool> PostResource(Resource resource);

        Task<bool> DeleteResource(string id);

        bool ResourceExists(string id, string language);
    }
}
