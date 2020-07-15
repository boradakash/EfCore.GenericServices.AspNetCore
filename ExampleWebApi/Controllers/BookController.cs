using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCore;
using CommonWebParts.Dtos;
using ExampleDatabase;
using GenericServices;
using GenericServices.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<WebApiMessageAndResult<List<Book>>>> GetManyAsync([FromServices]ICrudServicesAsync service)
        {
            return service.Response(await service.ReadManyNoTracked<Book>().ToListAsync());
        }
        [HttpGet("{id}", Name = "GetSingleBook")]
        public async Task<ActionResult<WebApiMessageAndResult<TodoItemHybrid>>> GetSingleAsync(int id, [FromServices]ICrudServicesAsync service)
        {
            return service.Response(await service.ReadSingleAsync<TodoItemHybrid>(id));
        }

        [HttpGet]
        [Route("reviews")]
        public async Task<ActionResult<WebApiMessageAndResult<List<Review>>>> GetReviewsManyAsync([FromServices]ICrudServicesAsync service)
        {
            return service.Response(await service.ReadManyNoTracked<Review>().ToListAsync());
        }

        [ProducesResponseType(typeof(CreateBookDto), 201)] 
        [HttpPost]
        public async Task<ActionResult<CreateBookDto>> Post(CreateBookDto item, [FromServices]ICrudServicesAsync service)
        {
            var result = await service.CreateAndSaveAsync(item, nameof(Book.CreateBook));
            return service.Response(this, "GetSingleBook", new { id = result.BookId }, item);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<WebApiMessageOnly>> Delete(int id, [FromServices]ICrudServicesAsync service)
        {
            await service.DeleteAndSaveAsync<TodoItemHybrid>(id);
            return service.Response();
        }

        [HttpPost]
        [Route("review")]
        public async Task<ActionResult<WebApiMessageOnly>> AddReview(AddReviewDto item, [FromServices]ICrudServicesAsync service)
        {
            await service.UpdateAndSaveAsync(item, nameof(Book.AddReview));
            return service.Response();
        }


        [HttpPost]
        [Route("review/delete")]
        public async Task<ActionResult<WebApiMessageOnly>> DeleteReview(RemoveReviewDto item, [FromServices]ICrudServicesAsync service)
        {
            await service.UpdateAndSaveAsync(item, nameof(Book.RemoveReview));
            return service.Response();
        }
    }
}
