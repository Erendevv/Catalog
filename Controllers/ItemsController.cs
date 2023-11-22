using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers{

    //GET /items

    [ApiController]
    [Route("[controller]")]
    public class ItemsController:ControllerBase
    {
        private readonly InMemItemsRepository repository;

        public ItemsController(){
            repository = new InMemItemsRepository();
        }

        // GET /items
        [HttpGet]
        public IEnumerable<Item> GetItems(){
            return repository.GetItems();
        
        }

        //GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id){
           var item = repository.GetItem(id);
           if(item == null){
               return NotFound();
           
           }
           return Ok(item);
        }
    }
}