using medikeeper_api.Data;
using medikeeper_api.Data.Entities;
using medikeeper_api.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace medikeeper_api.Controllers
{
    [RoutePrefix("api/item")]
    public class ItemController : ApiController
    {
        //DataAccessObject
        private ItemDAO Dao;

        public ItemController()
        {
            Dao = new ItemDAO();
        }
        [Route()]
        public async Task<IHttpActionResult> Get(bool onlyMax = false)
        {
            IEnumerable<Item> items;
            IEnumerable<ItemModel> modelItems;
            if (onlyMax)
            {
                items = await Dao.GetItemsWithMaxPrices(); 
                modelItems = items.Select(x => ItemMapper.EntityToModel(x));
                return Ok(modelItems.Select(x => new { x.Name, x.Cost }));
            }
            else
            {
                items = await Dao.GetItems();
                modelItems = items.Select(x => ItemMapper.EntityToModel(x));
                return Ok(modelItems);
            }
        }

        [Route("{id:int}", Name = "GetItemById")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var item = await Dao.GetItemById(id);
            if (item is null)
                return NotFound();
            return Ok(ItemMapper.EntityToModel(item));
        }

        [Route("{name}", Name = "GetItemByName")]
        public async Task<IHttpActionResult> GetByName(string name, bool onlyMax = false)
        {
            if (onlyMax)
            {
                var item = await Dao.GetItemWithMaxPriceByName(name);
                if (item is null)
                    return NotFound();
                return Ok(ItemMapper.EntityToModel(item));
            }
            else
            {
                var items = await Dao.GetItemsByName(name);
                if (items is null || items.Count() == 0)
                    return NotFound();
                return Ok(items.Select(x => ItemMapper.EntityToModel(x)));
            }
        }

        [Route()]
        public async Task<IHttpActionResult> Post(ItemModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var itemsWithExtId = await Dao.GetItemsByExternalId(model.ExternalId);
            if (itemsWithExtId.Count() != 0)
                return BadRequest("Item with that ExternalID already exists");

            var item = ItemMapper.ModelToEntity(model);
            var newId = await Dao.CreateItem(item);
            if (newId == 0)
                return InternalServerError();

            var newItem = await Dao.GetItemById(newId);
            return CreatedAtRoute("GetItemById", new { id = newId }, ItemMapper.EntityToModel(newItem));
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> Put(int id, ItemModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id == 0)
                return BadRequest("Id needed");

            var itemToUpdate = await Dao.GetItemById(id);
            if (itemToUpdate is null)
                return NotFound();

            if (model.ExternalId != itemToUpdate.ExternalId)
            {
                var itemsWithExtId = await Dao.GetItemsByExternalId(model.ExternalId);
                if (itemsWithExtId.Count() != 0)
                    return BadRequest("Item with that ExternalID already exists");
            }
            var item = ItemMapper.ModelToEntity(model);
            var success = await Dao.UpdateItem(id, item);
            if (!success)
                return InternalServerError();

            var result = await Dao.GetItemById(id);
            return Ok(result);
        }

        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest("Id needed");

            var success = await Dao.DeleteItem(id);
            if (!success)
                return NotFound();
            return Ok();
        }
    }
}