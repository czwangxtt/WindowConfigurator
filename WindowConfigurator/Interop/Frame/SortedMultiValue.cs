using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WindowConfigurator.Interop.FrameUtil
{
    public class SortedMultiValue<TKey, TValue> : IEnumerable<TValue>
    {
        private SortedDictionary<TKey, List<TValue>> _data;


        public SortedMultiValue()
        {
            _data = new SortedDictionary<TKey, List<TValue>>();
        }


        public void Clear()
        {
            _data.Clear();
        }


        public void Add(TKey key, TValue value)
        {
            if (!_data.TryGetValue(key, out List<TValue> items))
            {
                items = new List<TValue>();
                _data.Add(key, items);
            }
            items.Add(value);
        }


        public void Remove(TKey key, TValue value)
        {
            _data[key].Remove(value);
        }


        public int IndexOf(TValue value)
        {
            int index = 0;
            var comparer = EqualityComparer<TValue>.Default; // or pass in as a parameter
            foreach (List<TValue> items in _data.Values)
            {
                foreach (TValue item in items){
                    if (comparer.Equals(value, item)) return index;
                    index++;
                }
            }
            return -1;
        }


        public IEnumerable<TValue> Get(TKey key)
        {
            if (_data.TryGetValue(key, out List<TValue> items))
            {
                return items;
            }
            throw new KeyNotFoundException();
        }


        public IEnumerator<TValue> GetEnumerator()
        {
            return CreateEnumerable().GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return CreateEnumerable().GetEnumerator();
        }


        IEnumerable<TValue> CreateEnumerable()
        {
            foreach (IEnumerable<TValue> values in _data.Values)
            {
                foreach (TValue value in values)
                {
                    yield return value;
                }
            }
        }
    }
}
