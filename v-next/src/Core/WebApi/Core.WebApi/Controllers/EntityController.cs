namespace BeerAppreciation.Core.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Extensions;
    using global::Core.Shared.Data.Services;
    using global::Core.Shared.Domain;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class EntityController<T, TKey>: ControllerBase where T : BaseEntity<TKey>
    {
        private readonly IEntityService<T, TKey> entityService;
        private readonly ILogger<EntityController<T, TKey>> logger;

        public EntityController(IEntityService<T, TKey> entityService, ILogger<EntityController<T,TKey>> logger)
        {
            this.entityService = entityService;
            this.logger = logger;
        }

        // GET api/v1/[controller]/?pageSize=3&pageIndex=10]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultModel<>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var pagedList = await this.entityService.GetPagedList(pageIndex, pageSize);

            return this.Ok(pagedList.ToPagedResultModel());
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(TKey id)
        {
            if (id.Equals(default(TKey)))
            {
                return this.BadRequest();
            }

            var entity = await this.entityService.GetById(id);

            if (entity != null)
            {
                return this.Ok(entity);
            }

            return this.NotFound();
        }

        //PUT api/v1/[controller]/{id}
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public virtual async Task<IActionResult> Update(TKey id, [FromBody]T entity)
        {
            var existingEntity = await this.entityService.GetById(id);

            if (existingEntity == null)
            {
                return this.NotFound(new { Message = $"Entity with id {id} not found." });
            }

            await this.entityService.Update(entity);

            this.OnEntityUpdated(entity);

            return this.CreatedAtAction(nameof(GetById), new { id = entity.Id }, null);
        }

        //POST api/v1/[controller]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public virtual async Task<IActionResult> Post([FromBody]T entity)
        {
            await this.entityService.Insert(entity);

            this.OnEntityCreated(entity);

            return this.CreatedAtAction(nameof(GetById), new { id = entity.Id }, null);
        }

        //DELETE api/v1/[controller]/{id}
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        protected virtual async Task<IActionResult> Delete(TKey id)
        {
            var entity = await this.entityService.GetById(id);

            if (entity == null)
            {
                return this.NotFound();
            }

            await this.entityService.Delete(id);

            this.OnEntityDeleted(entity);

            return this.NoContent();
        }

        protected virtual void OnEntityCreated(T entity)
        {
            this.logger.LogInformation($"{entity.GetType().Name} was created");
        }

        protected virtual void OnEntityUpdated(T entity)
        {
            this.logger.LogInformation($"{entity.GetType().Name} was updated");
        }

        protected virtual void OnEntityDeleted(T entity)
        {
            this.logger.LogInformation($"{entity.GetType().Name} was deleted");
        }
    }
}
