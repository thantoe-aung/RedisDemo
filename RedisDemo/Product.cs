using MessagePack;

namespace RedisDemo
{
    [MessagePackObject(keyAsPropertyName:false)] // To avoid key in member
    public class Product
    {
        [Key(0)]
        public int ProductId { get; set; }

        [Key(1)]
        public string ProductName { get; set; } = string.Empty;

        [Key(2)]
        public string ProductDescription { get; set; } = string.Empty;

        [IgnoreMember]
        public int Stock { get; set; }
    }
}
