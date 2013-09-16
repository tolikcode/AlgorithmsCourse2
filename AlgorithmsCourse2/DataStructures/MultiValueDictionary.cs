using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmsCourse2.DataStructures
{
    class MultiValueDictionary<TKey, TValue>
    {
        private Dictionary<TKey, List<TValue>> data = new Dictionary<TKey, List<TValue>>();

        public void Add(TKey key, TValue value)
        {
            if(data.ContainsKey(key))
            {
                data[key].Add(value);
            }
            else
            {
                data.Add(key, new List<TValue> {value});
            }
        }

        public bool ContainsKey(TKey key)
        {
            return data.ContainsKey(key);
        }

        public List<TValue> this[TKey key]
        {
            get { return data[key]; }
            set { data[key] = value; }

        }

        public void Remove(TKey key)
        {
            data.Remove(key);
        }

        public void Remove(TKey key, TValue value)
        {
            if(data.ContainsKey(key))
            {
                if(data[key].Contains(value))
                {
                    if (data[key].Count > 1)
                    {
                        data[key].Remove(value);
                    }
                    else
                    {
                        data.Remove(key);
                    }
                }
            }
        }

    }
}
