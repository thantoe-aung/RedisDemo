using MessagePack;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisDemo
{
    public class RedisDataStore
    {
        protected IDatabase RedisDatabase { get; }
        protected ConnectionMultiplexer Connection { get; }

        public RedisDataStore(string redisServer, string password = "", int redisDatabaseId = 0)
        {
            var config = new ConfigurationOptions
            {
                EndPoints = { redisServer },
                Password = password ?? "",
                DefaultDatabase = redisDatabaseId,
                AllowAdmin = true,
                SyncTimeout = 20000,
            };
            Connection = ConnectionMultiplexer.Connect(config);
            RedisDatabase = Connection.GetDatabase(redisDatabaseId);
        }

        public bool SetCollection<T>(string key, IEnumerable<(string field, T value)> cacheObject, TimeSpan expiryTime)
        {
            var serializedData = MessagePackSerializer.Serialize("");

            //RedisDatabase.HashSet(
            //    key,
            //    cacheObject
            //        .Select(itm => new HashEntry(itm.field, JsonConvert.SerializeObject(itm.value)))
            //        .ToArray());

            RedisDatabase.HashSet(key,cacheObject.Select(x=> new HashEntry(x.field, MessagePackSerializer.Serialize(x.value))).ToArray());
            return RedisDatabase.KeyExpire(key, expiryTime);
        }

        public IEnumerable<T> GetCollection<T>(string key)
        {
            var result = RedisDatabase.HashGetAll(key);
            if (result.Length > 0)
            {
                //var items = result.Where(e => e.Value.HasValue)
                //  .Select(e => JsonConvert.DeserializeObject<T>(e.Value))
                //  .ToList();

                var items = result.Where(x=> x.Value.HasValue).Select(y => MessagePackSerializer.Deserialize<T>(y.Value)).ToList();

                return items;
            }
            return Array.Empty<T>(); ;
        }

        public IEnumerable<T> GetItemsFromCollection<T>(string key, params string[] fields)
        {
            var hashResult = RedisDatabase.HashGet(
                key,
                fields.Select(key => (RedisValue)key).ToArray());

            var items = hashResult.Where(e => e.HasValue)
                .Select(e => JsonConvert.DeserializeObject<T>(e))
                .ToList();

            return items;
        }
    }
}
