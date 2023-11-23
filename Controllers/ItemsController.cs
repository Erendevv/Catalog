using System.Linq;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers{

    //GET /items

    [ApiController]
    [Route("[controller]")]
    public class ItemsController:ControllerBase
    {
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository){
            this.repository = repository;
        }

        // GET /items
        [HttpGet]
        public IEnumerable<ItemDto> GetItems(){
            var items = repository.GetItems().Select(item=> item.AsDto());

            return items;
        
        }

        //GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id){
           var item = repository.GetItem(id);
           if(item == null){
               return NotFound();
           
           }
           return item.AsDto();
        }

        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto createItemDto)
        {
            Item item = new(){
                Id= Guid.NewGuid(),
                Name = createItemDto.Name,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            repository.CreateItem(item);
            return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDto());
        }

        // PUT /items/1
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto updateItemDto){
            var existingItem = repository.GetItem(id);
            if(existingItem is null){
                return NotFound();
            }
            Item updatedItem = existingItem with {
                Name = updateItemDto.Name,
                Price = updateItemDto.Price
            };

            repository.UpdateItem(updatedItem);
            return NoContent();

        }

        // DELETE /items/1
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id){ 
         var existingItem = repository.GetItem(id);
           if(existingItem is null){
                return NotFound();
            }
            repository.DeleteItem(id);
            return NoContent();
        }
    }
}