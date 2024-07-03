namespace RedisDemo
{
    public class DataStore
    {
        public List<Product> DataList { get; set; }

        public DataStore()
        {
            DataList = new List<Product>()
            {
                new Product
            {
                ProductId = 1,
                ProductName = "First Product",
                ProductDescription = "Desc",
                Stock = 10
            },
                new Product
            {
                ProductId = 2,
                ProductName = "Product2",
                ProductDescription = "Desc2",
                Stock = 10
            },
                new Product
            {
                ProductId = 3,
                ProductName = "Product3",
                ProductDescription = "Desc3",
                Stock = 10
            },
                new Product
            {
                ProductId = 4,
                ProductName = "Product4",
                ProductDescription = "Desc4",
                Stock = 10
            }
            };
        }
    }
}
