using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Logistyx.Bios.WebApp.Models;
using Logistyx.Bios.WebApp.Entities;
using Logistyx.Bios.WebApp.Repositories;
using AutoMapper;

namespace Logistyx.Bios.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly IMapper _mapper;

        public ResourcesController(IResourceRepository resourceRepository,
                IMapper mapper)
        {
            _resourceRepository = resourceRepository;
            _mapper = mapper;
        }

        // GET: api/Resources
        [HttpGet]
        public IEnumerable<Resource> GetResource()
        {
            return _resourceRepository.GetResource();
        }

        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResource([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var resource = await _resourceRepository.GetResource(id);

            if (resource == null)
            {
                return NotFound();
            }

            return Ok(resource);
        }

        // PUT: api/Resources/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResource([FromRoute] string id, [FromBody] ResourceForManipulationDto resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var resourceToUpdate = _mapper.Map<Resource>(resource);
                resourceToUpdate.Key = id;

                if (!await _resourceRepository.PutResource(id, resourceToUpdate))
                {
                    if (await _resourceRepository.PostResource(resourceToUpdate))
                    {
                        return CreatedAtAction("GetResource", new { id = resourceToUpdate.Key }, resource);
                    }
                    else
                    {
                        return new StatusCodeResult(StatusCodes.Status409Conflict);
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();
        }

        // POST: api/Resources
        [HttpPost]
        public async Task<IActionResult> PostResource([FromBody] ResourceForCreationDto resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var resourceToCreate = _mapper.Map<Resource>(resource);

                if (_resourceRepository.ResourceExists(resource.Key, resource.LanguageCode))
                {
                    
                    if (await _resourceRepository.PutResource(resource.Key, resourceToCreate))
                    {
                        return Ok();
                    }
                    else
                    {
                        return new StatusCodeResult(StatusCodes.Status409Conflict);
                    }

                }
                else if (!await _resourceRepository.PostResource(resourceToCreate))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetResource", new { id = resource.Key }, resource);
        }

        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (await _resourceRepository.DeleteResource(id))
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
