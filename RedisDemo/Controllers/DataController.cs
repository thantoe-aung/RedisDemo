using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RedisDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            const string cacheKey = "myRedisHashKey";

            //Creates a new redis cache instance 
            var cacheInstance = new RedisDataStore("127.0.0.1:6379");

            var allData = cacheInstance.GetCollection<Product>(cacheKey);
            if (allData != null && allData.Any())
            {
                return Ok(allData);
            }
            else
            {
                var dataList =new DataStore().DataList;
                cacheInstance.SetCollection(cacheKey, new List<(string, Product)>
                {
                    ($"Product{1}",new Product(){  ProductId = 1,
                            ProductName = "First Product",
                            ProductDescription = "Desc",
                            Stock = 10 }),
                     ($"Product{2}",new Product(){  ProductId = 2,
                            ProductName = "Second Product",
                            ProductDescription = "Desc",
                            Stock = 10 }),
                },
                TimeSpan.FromMinutes(2));
                return Ok(dataList);
            }

        }
    }
}
