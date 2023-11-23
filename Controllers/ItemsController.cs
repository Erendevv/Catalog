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
        public async Task<IEnumerable<ItemDto>> GetItemsAsync(){
            var items = (await repository.GetItemsAsync()).Select(item=> item.AsDto());
            return items;
        
        }

        //GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id){
           var item = await repository.GetItemAsync(id);
           if(item == null){
               return NotFound();
           
           }
           return item.AsDto();
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto createItemDto)
        {
            Item item = new(){
                Id= Guid.NewGuid(),
                Name = createItemDto.Name,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await repository.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new {id = item.Id}, item.AsDto());
        }

        // PUT /items/1
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto updateItemDto){
            var existingItem = await repository.GetItemAsync(id);
            if(existingItem is null){
                return NotFound();
            }
            Item updatedItem = existingItem with {
                Name = updateItemDto.Name,
                Price = updateItemDto.Price
            };

            await repository.UpdateItemAsync(updatedItem);
            return NoContent();

        }

        // DELETE /items/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id){ 
         var existingItem = await repository.GetItemAsync(id);
           if(existingItem is null){
                return NotFound();
            }
            await repository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}