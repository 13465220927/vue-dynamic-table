using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logistyx.Bios.WebApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logistyx.Bios.WebApp.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly TX_BIOSContext _context;

        public ResourceRepository(TX_BIOSContext context)
        {
            _context = context;
        }

        public IEnumerable<Resource> GetResource()
        {
            return _context.Resource;
        }

        public Task<Resource> GetResource(string id)
        {
            return _context.Resource.FindAsync(id);
        }
        
        public async Task<bool> PutResource(string id, Resource resource)
        {
            _context.Entry(resource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(id, resource.LanguageCode))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }
        
        public async Task<bool> PostResource(Resource resource)
        {
            _context.Resource.Add(resource);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ResourceExists(resource.Key, resource.LanguageCode))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> DeleteResource(string id)
        {
            var resources = _context.Resource.Where(c => c.Key == id);
            
            if (!resources.ToList().Any())
            {
                return false;
            }

            _context.Resource.RemoveRange(resources);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public bool ResourceExists(string id, string language)
        {
            return _context.Resource.Any(e => e.Key == id && e.LanguageCode == language);
        }
    }
}
