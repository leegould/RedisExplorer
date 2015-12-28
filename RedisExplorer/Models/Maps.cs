using System;
using System.Collections.Generic;
using RedisExplorer.Controls;
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

        public static Dictionary<RedisType, Type> RedisTypeViewModelMap = new Dictionary<RedisType, Type>
                                                  {
                                                    { RedisType.String, typeof(KeyStringViewModel) },
                                                    { RedisType.Set, typeof(KeySetViewModel) },
                                                    { RedisType.List, typeof(KeyListViewModel) },
                                                    { RedisType.Hash, typeof(KeyHashViewModel) },
                                                    { RedisType.SortedSet, typeof(KeySortedSetViewModel) }
                                                  };

        public static Dictionary<RedisType, Type> RedisTypeValueTypeMap = new Dictionary<RedisType, Type>
                                                  {
                                                    { RedisType.String, typeof(string) },
                                                    { RedisType.Set, typeof(List<string>) },
                                                    { RedisType.List, typeof(List<string>) },
                                                    { RedisType.Hash, typeof(Dictionary<string, string>) },
                                                    { RedisType.SortedSet, typeof(List<SortedSetEntry>) }
                                                  };
    }
}
