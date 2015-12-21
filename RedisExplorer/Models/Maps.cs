using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace RedisExplorer.Models
{
    public static class Maps
    {
        public static Dictionary<RedisType, Type> RedisTypeKeyMap = new Dictionary<RedisType, Type> 
                                                 {
                                                     { RedisType.String, typeof(RedisKeyString) },
                                                     { RedisType.Set, typeof(RedisKeySet) },
                                                     { RedisType.List, typeof(RedisKeyList) },
                                                     { RedisType.Hash, typeof(RedisKeyHash) },
                                                     { RedisType.SortedSet, typeof(RedisKeySortedSet) }
                                                 }; 
    }
}
