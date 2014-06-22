using System;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class AutoKeyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Fields

        private readonly IDictionary<TKey, TValue> dictionary;

        private readonly Func<TValue, TKey> keyGenerator;

        #endregion

        #region Constructors and Destructors

        public AutoKeyDictionary(Func<TValue, TKey> keyGenerator)
        {
            this.keyGenerator = keyGenerator;
            this.dictionary = new Dictionary<TKey, TValue>();
        }

        public AutoKeyDictionary(Func<TValue, TKey> keyGenerator, IEqualityComparer<TKey> equalityComparer)
        {
            this.keyGenerator = keyGenerator;
            this.dictionary = new Dictionary<TKey, TValue>(equalityComparer);
        }

        #endregion

        #region Public Properties

        public int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.dictionary.IsReadOnly;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return this.dictionary.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return this.dictionary.Values;
            }
        }

        #endregion

        #region Public Indexers

        public TValue this[TKey key]
        {
            get
            {
                return this.dictionary[key];
            }

            set
            {
                this.dictionary[key] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public TKey Add(TValue value)
        {
            var key = this.keyGenerator(value);
            this.dictionary.Add(key, value);
            return key;
        }

        public void Add(TKey key, TValue value)
        {
            this.dictionary.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.dictionary.Add(item);
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.dictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.dictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return this.dictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.dictionary.Remove(item);
        }

        public bool TryGetValue(TKey key, out TValue result)
        {
            return this.dictionary.TryGetValue(key, out result);
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        #endregion
    }
}
