using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisTool
{
    public class RedisHelper : IDisposable
    {
        RedisClient _redisClient;

        public RedisHelper(string address, int port)
        {
            _redisClient = new RedisClient(address, port);
        }

        ~RedisHelper()
        {
            if (_redisClient != null)
            {
                _redisClient.Dispose();
            }
        }

        public void Dispose()
        {
            _redisClient.Dispose();
        }

        #region Key

        /// <summary>
        /// 设置key的值，若T为复杂类，存储序列化后的JSON字符串
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">Key值</param>
        /// <param name="value">Value值</param>
        public void Set<T>(string key, T value)
        {
            _redisClient.Set(key, value);
        }

        /// <summary>
        /// 获取Key对应的值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">Key值</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return _redisClient.Get<T>(key);
        }

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key">key名</param>
        /// <returns>删除是否成功</returns>
        public bool Remove(string key)
        {
            return _redisClient.Remove(key);
        }

        /// <summary>
        /// 输出序列化后key的值(base64)
        /// </summary>
        /// <param name="key">key名</param>
        /// <returns>序列化后的Byte数组</returns>
        public byte[] Dump(string key)
        {
            return _redisClient.Dump(key);
        }

        /// <summary>
        /// 还原DUMP序列化后的byte[]存入新key,若新key名已存在，则报错
        /// </summary>
        /// <param name="key">新key名</param>
        /// <param name="ms">新key过期时间，0为不过期，单位ms</param>
        /// <param name="bytes">序列化后的Byte数组</param>
        public void Restore(string key, int ms, byte[] bytes)
        {
            byte[] newBytes = _redisClient.Restore(key, ms, bytes);
        }

        /// <summary>
        /// 判断是否存在key
        /// </summary>
        /// <param name="key">key名</param>
        /// <returns>是否存在</returns>
        public bool Exists(string key)
        {
            return _redisClient.Exists(key) > 0;
        }

        /// <summary>
        /// 设置key超时时间，返回是否成功
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="seconds">超时秒数</param>
        /// <returns>设置超时是否成功，若key不存在，返回False</returns>
        public bool Expire(string key, int seconds)
        {
            return _redisClient.Expire(key, seconds);
        }

        /// <summary>
        /// 设置key超时时间，返回是否成功
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="unixTime">UNIX 时间戳</param>
        /// <returns>设置超时是否成功，若key不存在，返回False</returns>
        public bool ExpireAt(string key, long unixTime)
        {
            return _redisClient.ExpireAt(key, unixTime);
        }

        /// <summary>
        /// 设置key超时时间，返回是否成功
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="milliseconds">超时毫秒数</param>
        /// <returns>设置超时是否成功，若key不存在，返回False</returns>
        public bool PExpire(string key, int milliseconds)
        {
            return _redisClient.PExpire(key, milliseconds);
        }

        /// <summary>
        /// 设置key超时时间，返回是否成功
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="unixTimeMs">UNIX时间戳，已毫秒计</param>
        /// <returns>设置超时是否成功，若key不存在，返回False</returns>
        public bool PExpireAt(string key, long unixTimeMs)
        {
            return _redisClient.PExpireAt(key, unixTimeMs);
        }

        /// <summary>
        /// 获取指定模式的key
        /// </summary>
        /// <param name="pattern">模式字符串,*作模糊匹配</param>
        /// <returns>所有匹配模式的key</returns>
        public IEnumerable<string> GetKeysByPattern(string pattern)
        {
            return _redisClient.GetKeysByPattern(pattern);
        }

        /// <summary>
        /// 将key移入指定DB，取值范围为0-15，超出范围报错
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="db">db编号（0-15）</param>
        /// <returns>若指定key不存在，或者转移失败，则返回False，否则返回True</returns>
        public bool Move(string key, int db)
        {
            return _redisClient.Move(key, db);
        }

        /// <summary>
        /// 移除key的过期时间，持久化key
        /// </summary>
        /// <param name="key">key名</param>
        /// <returns>若指定key不存在，或者持久化失败则返回False，否则返回True</returns>
        public bool Persist(string key)
        {
            return _redisClient.Persist(key);
        }

        /// <summary>
        /// 返回key的剩余的过期时间，单位ms，(-2 key不存在，-1 key永不过期 >=0 剩余过期毫秒数)
        /// </summary>
        /// <param name="key"></param>
        /// <returns>剩余过期时间的毫秒数</returns>
        public long PTtl(string key)
        {
            return _redisClient.PTtl(key);
        }

        /// <summary>
        /// 返回key的剩余的过期时间，单位s，(-2 key不存在，-1 key永不过期 >=0 剩余过期秒数)
        /// </summary>
        /// <param name="key"></param>
        /// <returns>剩余过期时间的秒数</returns>
        public long Ttl(string key)
        {
            return _redisClient.Ttl(key);
        }

        /// <summary>
        /// 随机获取一个key名
        /// </summary>
        /// <returns></returns>
        public string RandomKey()
        {
            return _redisClient.RandomKey();
        }

        /// <summary>
        /// 重命名key的名字，若新key已存在，则覆盖
        /// </summary>
        /// <param name="oldKeyName">原key名</param>
        /// <param name="newKeyName">新key名</param>
        public void Rename(string oldKeyName, string newKeyName)
        {
            _redisClient.Rename(oldKeyName, newKeyName);
        }

        /// <summary>
        /// 重命名key的名字，若新key已存在，则取消重命名
        /// </summary>
        /// <param name="oldKeyName">原key名</param>
        /// <param name="newKeyName">新key名</param>
        /// <returns>新key已存在，返回False</returns>
        public bool RenameNx(string oldKeyName, string newKeyName)
        {
            return _redisClient.RenameNx(oldKeyName, newKeyName);
        }

        /// <summary>
        /// 返回指定key的类型：none (key不存在) string (字符串) list (列表) set (集合) zset (有序集) hash (哈希表)
        /// </summary>
        /// <param name="key">key名</param>
        /// <returns>key类型</returns>
        public string Type(string key)
        {
            return _redisClient.Type(key);
        }

        #endregion

        #region String

        /// <summary>
        /// 获取key的值
        /// </summary>
        /// <param name="key">key名</param>
        /// <returns>key的值</returns>
        public string Get(string key)
        {
            byte[] bytes = _redisClient.Get(key);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 截取获取key的值中指定位置区域的字符串
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="fromIndex">起始位置</param>
        /// <param name="toIndex">结束位置</param>
        /// <returns>截取后的字符串</returns>
        public string GetRange(string key, int fromIndex, int toIndex)
        {
            return Encoding.UTF8.GetString(_redisClient.GetRange(key, fromIndex, toIndex));
        }

        /// <summary>
        /// 获取key的旧值并赋新值，若key不存在，则返回Null，并赋值
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="value">key的新值</param>
        /// <returns>key的旧值</returns>
        public string GetSet(string key, string value)
        {
            byte[] bytes = _redisClient.GetSet(key, Encoding.UTF8.GetBytes(value));
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 获取key的值，在指定偏移量处的bit值，超过长度返回0
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="offset">偏移量</param>
        /// <returns></returns>
        public long GetBit(string key, int offset)
        {
            return _redisClient.GetBit(key, offset);
        }

        /// <summary>
        /// 获取多个key的值
        /// </summary>
        /// <param name="keys">key列表</param>
        /// <returns></returns>
        public string[] MGet(params string[] keys)
        {
            return _redisClient.MGet(keys).ToStringArray();
        }

        /// <summary>
        /// 设置key的值在指定偏移量处的bit（0，1），并返回原始的bit值
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="offset">偏移量</param>
        /// <param name="value">bit值</param>
        /// <returns>原始的bit值</returns>
        public long SetBit(string key, int offset, int value)
        {
            return _redisClient.SetBit(key, offset, value);
        }

        /// <summary>
        /// 设置key的值及其过期时间
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="expireSeconds">过期秒数</param>
        /// <param name="value">key新值</param>
        public void SetEx(string key, int expireSeconds, string value)
        {
            _redisClient.SetEx(key, expireSeconds, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// key不存在的情况下，设置key的值，返回设置是否成功
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="value">key值</param>
        /// <returns>设置结果</returns>
        public bool SetNx(string key, string value)
        {
            return _redisClient.SetNX(key, Encoding.UTF8.GetBytes(value)) > 0;
        }

        /// <summary>
        /// 用指定的字符串覆盖给定key所储存的字符串值，偏移量为offset的值，返回覆盖后，字符串的长度
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="offset">偏移量</param>
        /// <param name="value">覆盖字符串</param>
        /// <returns>覆盖后字符串的长度</returns>
        public long SetRange(string key, int offset, string value)
        {
            return _redisClient.SetRange(key, offset, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 返回key所储存的字符串值的长度
        /// </summary>
        /// <param name="key">key名</param>
        /// <returns>key值长度</returns>
        public long StrLen(string key)
        {
            return _redisClient.StrLen(key);
        }

        /// <summary>
        /// 设置一个或多个key-value对
        /// </summary>
        /// <param name="dict">key-value字典</param>
        public void MSet(Dictionary<string, string> dict)
        {
            _redisClient.MSet(dict.Keys.ToArray(), dict.Values.To2DByteArray());
        }

        /// <summary>
        /// 设置一个或多个key-value对，若有key已经存在，则返回false
        /// </summary>
        /// <param name="dict">key-value字典</param>
        /// <returns>设置结果</returns>
        public bool MSetNx(Dictionary<string, string> dict)
        {
            return _redisClient.MSetNx(dict.Keys.ToArray(), dict.Values.To2DByteArray());
        }

        /// <summary>
        /// 设置key的值及其过期时间（毫秒）
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="expireMs">过期毫秒数</param>
        /// <param name="value">key新值</param>
        public void PSetEx(string key, int expireMs, string value)
        {
            _redisClient.PSetEx(key, expireMs, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 将key中储存的数字值增一，返回运算结果，若key值不为整数，则报错，若key不存在，则新建key，运算基值为0
        /// </summary>
        /// <param name="key">key名</param>
        /// <returns>运算结果</returns>
        public long Incr(string key)
        {
            return _redisClient.Incr(key);
        }

        /// <summary>
        /// 将key所储存的值加上给定的增量值，返回运算结果，若key值不为整数，则报错，若key不存在，则新建key，运算基值为0
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="increment">增量</param>
        /// <returns>运算结果</returns>
        public long IncrBy(string key, int increment)
        {
            return _redisClient.IncrBy(key, increment);
        }

        /// <summary>
        /// 将key所储存的值加上给定的增量值，返回运算结果，若key值不为数值，则报错，若key不存在，则新建key，运算基值为0
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="increment">增量</param>
        /// <returns>运算结果</returns>
        public double IncrByFloat(string key, double increment)
        {
            return _redisClient.IncrByFloat(key, increment);
        }

        /// <summary>
        /// 将key中储存的数字值减一，返回运算结果，若key值不为整数，则报错，若key不存在，则新建key，运算基值为0
        /// </summary>
        /// <param name="key">key名</param>
        /// <returns>运算结果</returns>
        public long Decr(string key)
        {
            return _redisClient.Decr(key);
        }

        /// <summary>
        /// 将key所储存的值减去给定的增量值，返回运算结果，若key值不为整数，则报错，若key不存在，则新建key，运算基值为0
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="increment">减量</param>
        /// <returns>运算结果</returns>
        public long DecrBy(string key, int increment)
        {
            return _redisClient.DecrBy(key, increment);
        }

        /// <summary>
        /// 将value追加到key原来的值的末尾，若key不存在，则新建key，值为value
        /// </summary>
        /// <param name="key">key名</param>
        /// <param name="value">追加值</param>
        /// <returns>结果长度</returns>
        public long Append(string key, string value)
        {
            return _redisClient.Append(key, Encoding.UTF8.GetBytes(value));
        }

        #endregion

        #region Hash

        /// <summary>
        /// 删除hash中的多个key
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="keys">key数组</param>
        public void HDel(string hashId, string[] keys)
        {
            _redisClient.HDel(hashId, keys.To2DByteArray());
        }

        /// <summary>
        /// hash中是否存在指定key，如果hashId或者key不存在，则返回false
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key名</param>
        /// <returns>是否存在指定key</returns>
        public bool HExists(string hashId, string key)
        {
            return _redisClient.HExists(hashId, Encoding.UTF8.GetBytes(key)) > 0;
        }

        /// <summary>
        /// 返回给定key的值，若hashId或key不存在，则返回null
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key名</param>
        /// <returns>key值</returns>
        public string HGet(string hashId, string key)
        {
            byte[] bytes = _redisClient.HGet(hashId, Encoding.UTF8.GetBytes(key));
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 返回哈希表的key-value字典
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <returns>hashId字典</returns>
        public Dictionary<string, string> HGetAll(string hashId)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            byte[][] bytes = _redisClient.HGetAll(hashId);
            if (bytes == null)
            {
                return null;
            }

            for (int i = 0; i < bytes.Length; i = i + 2)
            {
                dict.Add(Encoding.UTF8.GetString(bytes[i]), Encoding.UTF8.GetString(bytes[i + 1]));
            }

            return dict;
        }

        /// <summary>
        /// 为哈希表key中的指定字段的整数值加上增量increment，返回运算结果，若该字段不是整数，则报错
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key名</param>
        /// <param name="increment">增量</param>
        /// <returns>运算结果</returns>
        public long HIncrBy(string hashId, string key, int increment)
        {
            return _redisClient.HIncrby(hashId, Encoding.UTF8.GetBytes(key), increment);
        }

        /// <summary>
        /// 为哈希表key中的指定字段的浮点数值加上增量increment，返回运算结果，若该字段不是数值，则报错
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key名</param>
        /// <param name="increment">增量</param>
        /// <returns>运算结果</returns>
        public double HIncrByFloat(string hashId, string key, double increment)
        {
            return _redisClient.HIncrbyFloat(hashId, Encoding.UTF8.GetBytes(key), increment);
        }

        /// <summary>
        /// 获取所有哈希表中的字段，若该哈希表不存在，则返回数组长度为0
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <returns>哈希表字段数组</returns>
        public string[] HKeys(string hashId)
        {
            byte[][] bytes = _redisClient.HKeys(hashId);
            string[] result = new string[bytes.Length];
            if (bytes == null)
            {
                return null;
            }

            for (int i = 0; i < bytes.Length; i++)
            {
                result[i] = Encoding.UTF8.GetString(bytes[i]);
            }

            return result;
        }

        /// <summary>
        /// 获取哈希表中字段的数量，若哈希表不存在，则返回0
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public long HLen(string hashId)
        {
            return _redisClient.HLen(hashId);
        }

        /// <summary>
        /// 获取所有给定字段的值，若哈希表不存在，返回null，若key不存在，返回null
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="keys"></param>
        /// <returns>哈希表给定字段的值</returns>
        public string[] HMGet(string hashId, params string[] keys)
        {
            byte[][] keyBytes = keys.To2DByteArray();
            byte[][] valueBytes = _redisClient.HMGet(hashId, keyBytes);
            return valueBytes.ToStringArray();
        }

        /// <summary>
        /// 多个field-value设置到哈希表中，dict的Value不能为null，否则报错
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="dict">key-value字典</param>
        public void HMSet(string hashId, Dictionary<string, string> dict)
        {
            _redisClient.HMSet(hashId, dict.Keys.To2DByteArray(), dict.Values.To2DByteArray());
        }

        /// <summary>
        /// 将哈希表 hashId 中的字段 key 的值设为 value 。
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key名</param>
        /// <param name="value">key值</param>
        public void HSet(string hashId, string key, string value)
        {
            _redisClient.HSet(hashId, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 设置哈希表字段的值，只有在key不存在时设置成功，返回true
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <param name="key">key名</param>
        /// <param name="value">key值</param>
        /// <returns>设置是否成功</returns>
        public bool HSetNX(string hashId, string key, string value)
        {
            return _redisClient.HSetNX(hashId, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(value)) > 0;
        }

        /// <summary>
        /// 获取哈希表中所有值，若没有hashId对应的哈希表，则返回长度为0的数组
        /// </summary>
        /// <param name="hashId">hashId</param>
        /// <returns>哈希表所有值</returns>
        public string[] HVals(string hashId)
        {
            byte[][] bytes = _redisClient.HVals(hashId);
            if (bytes == null)
            {
                return null;
            }

            return bytes.ToStringArray();
        }

        #endregion

        #region List

        /// <summary>
        /// 移出并获取列表的第一个元素，如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止，若超时则返回null
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <param name="timeOutSecs">超时时间</param>
        /// <returns>列表第一个元素</returns>
        public string BLPop(string listId, int timeOutSecs)
        {
            byte[] bytes = _redisClient.BLPopValue(listId, timeOutSecs);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 移出并获取列表的最后一个元素，如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止，若超时则返回null
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <param name="timeOutSecs">超时时间</param>
        /// <returns>列表第一个元素</returns>
        public string BRPop(string listId, int timeOutSecs)
        {
            byte[] bytes = _redisClient.BRPopValue(listId, timeOutSecs);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 从列表中弹出一个值，将弹出的元素插入到另外一个列表中并返回它， 如果列表没有元素会阻塞列表直到等待超时或发现可弹出元素为止，若超时则返回null
        /// </summary>
        /// <param name="fromListId">取出列表Id</param>
        /// <param name="toListId">插入列表Id</param>
        /// <param name="timeOutSecs">超时时间</param>
        /// <returns>弹出值</returns>
        public string BRPopLPush(string fromListId, string toListId, int timeOutSecs)
        {
            byte[] bytes = _redisClient.BRPopLPush(fromListId, toListId, timeOutSecs);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 通过索引获取列表中的元素，若List不存在或者Index超出List长度，则返回null
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <param name="index">索引</param>
        /// <returns>指定索引处的值</returns>
        public string LIndex(string listId, int index)
        {
            byte[] bytes = _redisClient.LIndex(listId, index);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 获取列表长度，若列表不存在，则返回0
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <returns>列表长度</returns>
        public long LLen(string listId)
        {
            return _redisClient.LLen(listId);
        }

        /// <summary>
        /// 移出并获取列表的第一个元素，若列表不存在， 或者长度为空，则返回null
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <returns>列表第一个元素</returns>
        public string LPop(string listId)
        {
            byte[] bytes = _redisClient.LPop(listId);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 将一个或多个值插入到列表头部，若插入值中存在null，返回false
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <param name="values">插入值</param>
        /// <returns>插入是否成功</returns>
        public bool LPush(string listId, params string[] values)
        {
            byte[][] bytes = values.To2DByteArray();
            return _redisClient.LPush(listId, bytes) > 0;
        }

        /// <summary>
        /// 将一个或多个值插入到列表头部，若插入值中存在null，返回false
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <param name="values">插入值</param>
        /// <returns>插入是否成功</returns>
        public bool LPushX(string listId, params string[] values)
        {
            byte[][] bytes = values.To2DByteArray();
            for (int i = 0; i < bytes.Length; i++)
            {
                if (_redisClient.LPushX(listId, bytes[i]) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取列表指定范围内的元素，若列表不存在，或指定范围内不存在元素，则返回空数组
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <param name="fromIndex">开始Index</param>
        /// <param name="toIndex">结束Index</param>
        /// <returns>指定范围内元素</returns>
        public string[] LRange(string listId, int fromIndex, int toIndex)
        {
            byte[][] bytes = _redisClient.LRange(listId, fromIndex, toIndex);
            return bytes.ToStringArray();
        }

        /// <summary>
        /// 移除列表中值为value的元素，数量为Count的绝对值
        /// count 大于 0：从表头开始向表尾搜索
        /// count 小于 0：从表尾开始向表头搜索
        /// count 等于 0：移除表中所有与 VALUE 相等的值
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <param name="count">数量</param>
        /// <param name="value"></param>
        /// <returns>移除数量</returns>
        public long LRem(string listId, int count, string value)
        {
            return _redisClient.LRem(listId, count, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 通过索引设置列表元素的值,索引不能超过列表长度，否则报错
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <param name="index">索引值</param>
        /// <param name="value">新值</param>
        public void LSet(string listId, int index, string value)
        {
            _redisClient.LSet(listId, index, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 对一个列表进行修剪(trim)，让列表只保留指定区间内的元素，不在指定区间之内的元素都将被删除。
        /// 若keepStartingFrom大于keepEndingTo，则移除所有元素
        /// Starting,Ending为负表示从列表尾部开始的Index
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <param name="keepStartingFrom">开始Index</param>
        /// <param name="keepEndingTo">结束Index</param>
        public void LTrim(string listId, int keepStartingFrom, int keepEndingTo)
        {
            _redisClient.LTrim(listId, keepStartingFrom, keepEndingTo);
        }

        /// <summary>
        /// 移出并获取列表最后一个元素，若列表不存在， 或者长度为空，则返回null
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <returns>列表最后一个元素</returns>
        public string RPop(string listId)
        {
            byte[] bytes = _redisClient.RPop(listId);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 从列表中弹出一个值，将弹出的元素插入到另外一个列表中并返回它， 若没有值则返回null
        /// </summary>
        /// <param name="fromListId">取出列表Id</param>
        /// <param name="toListId">插入列表Id</param>
        /// <returns>弹出值</returns>
        public string RPopLPush(string fromListId, string toListId)
        {
            byte[] bytes = _redisClient.RPopLPush(fromListId, toListId);
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 在列表中添加一个或多个值
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <param name="values">插入值</param>
        public void RPush(string listId, params string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                _redisClient.RPush(listId, Encoding.UTF8.GetBytes(values[i]));
            }

        }

        /// <summary>
        /// 将一个或多个值插入到列表尾部，若插入值中存在null，返回false
        /// </summary>
        /// <param name="listId">列表Id</param>
        /// <param name="values">插入值</param>
        /// <returns>插入是否成功</returns>
        public bool RPushX(string listId, params string[] values)
        {
            byte[][] bytes = values.To2DByteArray();
            for (int i = 0; i < bytes.Length; i++)
            {
                if (_redisClient.RPushX(listId, bytes[i]) == 0)
                {
                    return false;
                }
            }

            return true;
        }


        #endregion

        #region Set

        /// <summary>
        /// 向集合添加一个或多个成员,若value中包含Null，则取消添加
        /// </summary>
        /// <param name="setId">集合Id</param>
        /// <param name="values">添加值</param>
        /// <returns>新插入值数量</returns>
        public long SAdd(string setId, params string[] values)
        {
            byte[][] bytes = values.To2DByteArray();
            return _redisClient.SAdd(setId, bytes);
        }

        /// <summary>
        /// 获取集合的成员数
        /// </summary>
        /// <param name="setId">集合Id</param>
        /// <returns></returns>
        public long SCard(string setId)
        {
            return _redisClient.SCard(setId);
        }

        /// <summary>
        /// 返回给定所有集合的差集（fromSetId集合除去withSetId集合）
        /// </summary>
        /// <param name="fromSetId">fromSetId</param>
        /// <param name="withSetId">withSetId</param>
        /// <returns>fromSetId集合除去withSetId集合</returns>
        public string[] SDiff(string fromSetId, string withSetId)
        {
            byte[][] bytes = _redisClient.SDiff(fromSetId, withSetId);
            return bytes.ToStringArray();
        }

        /// <summary>
        /// 将fromSetId，withSetId两个结合的差集存储在intoSetId中
        /// </summary>
        /// <param name="intoSetId">存储集合Id</param>
        /// <param name="fromSetId">fromSetId</param>
        /// <param name="withSetId">withSetId</param>
        public void SDiffStore(string intoSetId, string fromSetId, string withSetId)
        {
            _redisClient.SDiffStore(intoSetId, fromSetId, withSetId);
        }

        /// <summary>
        /// 返回给定所有集合的交集
        /// </summary>
        /// <param name="setIds">集合Id数组</param>
        /// <returns></returns>
        public string[] SInter(params string[] setIds)
        {
            return _redisClient.SInter(setIds).ToStringArray();
        }

        /// <summary>
        /// 返回给定所有集合的交集并存储在 intoSetId 中
        /// </summary>
        /// <param name="intoSetId">存储集合Id</param>
        /// <param name="setIds">集合Id数组</param>
        public void SInterStore(string intoSetId, params string[] setIds)
        {
            _redisClient.SInterStore(intoSetId, setIds);
        }

        /// <summary>
        /// 判断 value 元素是否是 setId 的成员
        /// </summary>
        /// <param name="setId">集合Id</param>
        /// <param name="value">值</param>
        /// <returns>是否setId成员</returns>
        public bool SIsMember(string setId, string value)
        {
            return _redisClient.SIsMember(setId, Encoding.UTF8.GetBytes(value)) > 0;
        }

        /// <summary>
        /// 返回集合中的所有成员，若集合不存在，则返回数组长度为0
        /// </summary>
        /// <param name="setId">集合Id</param>
        /// <returns>集合成员数组</returns>
        public string[] SMembers(string setId)
        {
            return _redisClient.SMembers(setId).ToStringArray();
        }

        /// <summary>
        /// 将 member 元素从 fromSetId 集合移动到 toSetId 集合
        /// </summary>
        /// <param name="fromSetId"></param>
        /// <param name="toSetId"></param>
        /// <param name="value">移动元素</param>
        public void SMove(string fromSetId, string toSetId, string value)
        {
            _redisClient.SMove(fromSetId, toSetId, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        /// 移除并返回集合中的一个或多个随机元素
        /// </summary>
        /// <param name="setId">集合Id</param>
        /// <param name="count">移除数量</param>
        /// <returns>移除并返回的元素</returns>
        public string[] SPop(string setId, int count)
        {
            return _redisClient.SPop(setId, count).ToStringArray();
        }

        /// <summary>
        /// 返回集合中一个或多个随机数
        /// </summary>
        /// <param name="setId">集合Id</param>
        /// <param name="count">返回数量</param>
        /// <returns></returns>
        public string[] SRandMember(string setId, int count)
        {
            return _redisClient.SRandMember(setId, count).ToStringArray();
        }

        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        /// <param name="setId">集合Id</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SRem(string setId, params string[] value)
        {
            return _redisClient.SRem(setId, value.To2DByteArray()) > 0;
        }

        /// <summary>
        /// 返回所有给定集合的并集
        /// </summary>
        /// <param name="setIds">集合Id数组</param>
        /// <returns>集合并集</returns>
        public string[] SUnion(params string[] setIds)
        {
            return _redisClient.SUnion(setIds).ToStringArray();
        }

        /// <summary>
        /// 所有给定集合的并集存储在 intoSetId 集合中
        /// </summary>
        /// <param name="intoSetId">存储集合Id</param>
        /// <param name="setIds">集合Id数组</param>
        public void SUnionStore(string intoSetId, params string[] setIds)
        {
            _redisClient.SUnionStore(intoSetId, setIds);
        }

        /// <summary>
        /// 迭代集合键中的元素
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="cursor"></param>
        /// <param name="count"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public ScanResult SScan(string setId, ulong cursor, int count = 10, string match = null)
        {
            return _redisClient.SScan(setId, cursor, count, match);
        }

        #endregion

        #region SortedSet

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数，返回新增数量
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="pairs">新分数</param>
        /// <returns>新增数量</returns>
        public long ZAdd(string setId, List<KeyValuePair<string, double>> pairs)
        {
            return _redisClient.ZAdd(setId, pairs.Select(p => new KeyValuePair<byte[], double>(Encoding.UTF8.GetBytes(p.Key), p.Value)).ToList());
        }

        /// <summary>
        /// 获取有序集合的成员数
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <returns>成员数量</returns>
        public long ZCard(string setId)
        {
            return _redisClient.ZCard(setId);
        }

        /// <summary>
        /// 计算在有序集合中指定区间分数的成员数
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="min">区间下限</param>
        /// <param name="max">区间上限</param>
        /// <returns></returns>
        public long ZCount(string setId, double min, double max)
        {
            return _redisClient.ZCount(setId, min, max);
        }

        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment，若有序集合不存在，或者member不存在，则新建，分数为increment
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="increBy">增量</param>
        /// <param name="member">有序集合成员</param>
        public void ZIncrBy(string setId, double increBy, string member)
        {
            _redisClient.ZIncrBy(setId, increBy, Encoding.UTF8.GetBytes(member));
        }

        /// <summary>
        /// 计算给定的一个或多个有序集的交集并将结果集存储在新的有序集合 key 中,返回交集元素数量
        /// </summary>
        /// <param name="intoSetId">存储有序集合Id</param>
        /// <param name="setIds">所有有序集合Id数组</param>
        /// <returns>交集元素数量</returns>
        public long ZInterStore(string intoSetId, params string[] setIds)
        {
            return _redisClient.ZInterStore(intoSetId, setIds);
        }

        /// <summary>
        /// 在有序集合中计算指定字典区间内成员数量
        /// 实例：
        /// zlexcount set [a [f（包含边界）
        /// zlexcount set (a (f（不包含边界）
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="min">最小成员</param>
        /// <param name="max">最大成员</param>
        /// <returns></returns>
        public long ZLexCount(string setId, string min, string max)
        {
            return _redisClient.ZLexCount(setId, min, max);
        }

        /// <summary>
        /// 通过索引区间返回有序集合成指定区间内的成员
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="start">起始索引值</param>
        /// <param name="stop">结束索引值</param>
        /// <returns>指定索引区间内的成员</returns>
        public string[] ZRange(string setId, int start, int stop)
        {
            return _redisClient.ZRange(setId, start, stop).ToStringArray();
        }

        /// <summary>
        /// 通过字典区间返回有序集合的成员
        /// 实例：
        /// ZRANGEBYLEX set [a [f（包含边界）
        /// ZRANGEBYLEX set (a (f（不包含边界）
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="min">最小成员</param>
        /// <param name="max">最大成员</param>
        /// <param name="skip">跳过数量</param>
        /// <param name="take">获取数量</param>
        /// <returns>指定字典区间内的成员</returns>
        public string[] ZRangeByLex(string setId, string min, string max, int? skip = default(int?), int? take = default(int?))
        {
            return _redisClient.ZRangeByLex(setId, min, max, skip, take).ToStringArray();
        }

        /// <summary>
        /// 通过分数区间返回有序集合的成员，包含边界
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="skip">跳过数量</param>
        /// <param name="take">获取数量</param>
        /// <returns>指定字典区间内的成员</returns>
        public string[] ZRangeByScore(string setId, double min, double max, int? skip, int? take)
        {
            return _redisClient.ZRangeByScore(setId, min, max, skip, take).ToStringArray();
        }

        /// <summary>
        /// 返回有序集合中指定成员的索引，若member不存在，返回-1
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="member">成员</param>
        /// <returns>成员序列</returns>
        public long ZRank(string setId, string member)
        {
            return _redisClient.ZRank(setId, Encoding.UTF8.GetBytes(member));
        }

        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="member">删除成员</param>
        /// <returns>删除是否成功</returns>
        public bool ZRem(string setId, string member)
        {
            return _redisClient.ZRem(setId, Encoding.UTF8.GetBytes(member)) > 0;
        }

        /// <summary>
        /// 移除有序集合中给定的字典区间的所有成员
        /// 实例：
        /// ZREMRANGEBYLEX set [a [f（包含边界）
        /// ZREMRANGEBYLEX set (a (f（不包含边界）
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="min">最小成员</param>
        /// <param name="max">最大成员</param>
        /// <returns>移除数量</returns>
        public long ZRemRangeByLex(string setId, string min, string max)
        {
            return _redisClient.ZRemRangeByLex(setId, min, max);
        }

        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="min">最小rank</param>
        /// <param name="max">最大rank</param>
        /// <returns>移除数量</returns>
        public long ZRemRangeByRank(string setId, int min, int max)
        {
            return _redisClient.ZRemRangeByRank(setId, min, max);
        }

        /// <summary>
        /// 移除有序集合中给定的分数区间的所有成员
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="minScore">最小分数</param>
        /// <param name="maxScore">最大分数</param>
        /// <returns>移除数量</returns>
        public long ZRemRangeByScore(string setId, double minScore, double maxScore)
        {
            return _redisClient.ZRemRangeByScore(setId, minScore, maxScore);
        }

        /// <summary>
        /// 返回有序集中指定区间内的成员，通过索引，分数从高到底
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="min">最小索引</param>
        /// <param name="max">最大索引</param>
        public string[] ZRevRange(string setId, int min, int max)
        {
            return _redisClient.ZRevRange(setId, min, max).ToStringArray();
        }

        /// <summary>
        /// 返回有序集中指定分数区间内的成员，分数从高到低排序
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="min">最小分数</param>
        /// <param name="max">最大分数</param>
        /// <param name="skip">跳过数</param>
        /// <param name="take">返回数</param>
        /// <returns></returns>
        public string[] ZRevRangeByScore(string setId, double min, double max, int? skip, int? take)
        {
            return _redisClient.ZRevRangeByScore(setId, min, max, skip, take).ToStringArray();
        }

        /// <summary>
        /// 返回有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="member">成员</param>
        /// <returns>成员排序（降序）</returns>
        public long ZRevRank(string setId, string member)
        {
            return _redisClient.ZRevRank(setId, Encoding.UTF8.GetBytes(member));
        }

        /// <summary>
        /// 返回有序集中，成员的分数值，若member不存在，则返回double.NaN
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="member">成员</param>
        /// <returns>成员分数</returns>
        public double ZScore(string setId, string member)
        {
            return _redisClient.ZScore(setId, Encoding.UTF8.GetBytes(member));
        }

        /// <summary>
        /// 计算给定的一个或多个有序集的并集，并存储在新的 key 中
        /// </summary>
        /// <param name="intoSetId">存储有序集合Id</param>
        /// <param name="setIds">有序集合Id数组</param>
        /// <returns>新集合成员个数</returns>
        public long ZUnionStore(string intoSetId, params string[] setIds)
        {
            return _redisClient.ZUnionStore(intoSetId, setIds);
        }

        /// <summary>
        /// 迭代有序集合中的元素（包括元素成员和元素分值）
        /// </summary>
        /// <param name="setId">有序集合Id</param>
        /// <param name="cursor"></param>
        /// <param name="count"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public ScanResult ZScan(string setId, ulong cursor, int count = 10, string match = null)
        {
            return _redisClient.ZScan(setId, cursor, count, match);
        }

        #endregion


    }
}
