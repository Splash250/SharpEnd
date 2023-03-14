namespace SharpEnd.Miscellaneous
{
    internal class MultiDictionary<TKey, TValue>
    {
        private Dictionary<TKey, List<TValue>> _dictionary = new Dictionary<TKey, List<TValue>>();

        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }
        public ICollection<TKey> Keys
        {
            get
            {
                return _dictionary.Keys;
            }
        }
        public ICollection<List<TValue>> Values
        {
            get
            {
                return _dictionary.Values;
            }
        }
        public MultiDictionary()
        {
            _dictionary = new Dictionary<TKey, List<TValue>>();
        }
        public void Add(TKey key, TValue value)
        {
            if (!_dictionary.ContainsKey(key))
                _dictionary[key] = new List<TValue>();

            _dictionary[key].Add(value);
        }

        public bool Has(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool HasValue(TValue value)
        {
            foreach (var list in _dictionary.Values)
            {
                if (list.Contains(value))
                    return true;
            }

            return false;
        }

        public List<TValue> this[TKey key]
        {
            get
            {
                if (_dictionary.TryGetValue(key, out List<TValue> values))
                    return values;

                return new List<TValue>();
            }
            set
            {
                _dictionary[key] = value;
            }
        }

        public bool Remove(TKey key, TValue value)
        {
            if (_dictionary.TryGetValue(key, out List<TValue> values))
            {
                bool removed = values.Remove(value);

                if (values.Count == 0)
                    _dictionary.Remove(key);

                return removed;
            }

            return false;
        }
        public bool RemoveKey(TKey key)
        {
            return _dictionary.Remove(key);
        }
    }
}
