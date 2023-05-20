using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine.Data
{
    [Serializable]
    public class FieldArray<T> : IEnumerable<T>
    {
        private static bool IsCompatibleObject(object value)
        {
            return ((value is T) || (value == null && default(T) == null));
        }

        [Serializable]
        private struct Element : IComparable<Element>, IEquatable<Element>
        {
            public int Id;
            public FieldKey<T> Field;

            public Element(int key, FieldKey<T> value)
            {
                Id = key;
                Field = value ?? throw new ArgumentNullException("The FieldKey can't be null!...");
            }

            public int CompareTo(Element other)
            {
                return Id.CompareTo(other.Id);
            }

            public bool Equals(Element other)
            {
                return Id == other.Id;
            }

            public override int GetHashCode()
            {
                int hashCode = 1363396886;
                hashCode = hashCode * -1521134295 + Id.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<FieldKey<T>>.Default.GetHashCode(Field);
                return hashCode;
            }
        }

        [SerializeField, HideInInspector] private bool _autoSave;
        [SerializeField] private string _key;
        [SerializeField] private string _fileName;
        [SerializeField] private List<Element> _elements;

        public string Key => _key;
        public string FileName => _fileName;
        public int Count => _elements.Count;
        public int LastId
        {
            get
            {
                if (Count == 0) return -1;

                return _elements[_elements.Count - 1].Id;
            }
        }



        public FieldArray(string key, string fileName = null, int capacity = 0, bool autoSave = true)
        {
            _key = key;
            _autoSave = autoSave;
            _fileName = fileName;

            _elements = new List<Element>(capacity);

            for (int i = 0; i < capacity; i++)
            {
                _elements.Add(new Element(i, new FieldKey<T>(key + i, fileName, default(T), autoSave)));
            }
        }

        private int FindIndex(int id)
        {
            int startI = 0;
            int endI = _elements.Count;
            while (endI > startI)
            {
                int windowSize = endI - startI;
                int middleI = startI + (windowSize / 2);
                if (_elements[middleI].Id == id)
                {
                    return middleI;
                }
                else
                if (_elements[middleI].Id < id)
                {
                    startI = middleI + 1;
                }
                else
                {
                    endI = middleI;
                }
            }

            return -1;
        }

        private void InsertSortedElement(int index, T insertedValue = default(T))
        {
            int startI = 0;
            int endI = _elements.Count;

            while (endI > startI)
            {
                int windowSize = endI - startI;
                int middleI = startI + (windowSize / 2);
                if (_elements[middleI].Id == index)
                {
                    _elements[middleI].Field.value = insertedValue;
                    return;
                }
                else if (_elements[middleI].Id < index)
                {
                    startI = middleI + 1;
                }
                else
                {
                    endI = middleI;
                }
            }

            Element element = new Element(index, new FieldKey<T>((_key + index), _fileName, insertedValue, _autoSave));
            _elements.Insert(startI, element);
            if (_autoSave)
                Debug.Log("Save");
                element.Field.Save();
        }

        private FieldKey<T> FindFieldKey(int index)
        {
            int startI = 0;
            int endI = _elements.Count;
            while (endI > startI)
            {
                int windowSize = endI - startI;
                int middleI = startI + (windowSize / 2);
                if (_elements[middleI].Id == index)
                {
                    return _elements[middleI].Field;
                }
                else
                if (_elements[middleI].Id < index)
                {
                    startI = middleI + 1;
                }
                else
                {
                    endI = middleI;
                }
            }

            FieldKey<T> result = new FieldKey<T>((_key + index), _fileName, default(T), _autoSave);
            _elements.Insert(startI, new Element(index, result));
            return result;
        }

        public T this[int index]
        {
            get
            {
                return FindFieldKey(index).value;
            }
            set
            {
                InsertSortedElement(index, value);
            }
        }

        public void SaveAll()
        {
            for (int i = 0; i < _elements.Count; i++)
            {
                _elements[i].Field.Save();
            }
        }

        public bool HasKey(int id)
        {
            return 0 < FindIndex(id);
        }

        public FieldKey<T> GetFieldKey(int index)
        {
            return FindFieldKey(index);
        }

        public void Clear()
        {
            foreach (var item in _elements)
            {
                item.Field.Delete();
            }
            _elements.Clear();
        }

        public T Find(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException("The match has a null value!...");

            int result = _elements.FindIndex((element) => match.Invoke(element.Field.value));
            if (result < 0) return default(T);

            return _elements[result].Field.value;
        }
        
        public T FindLast(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException("The match has a null value!...");

            int result = _elements.FindLastIndex((element) => match.Invoke(element.Field.value));
            if (result < 0) return default(T);

            return _elements[result].Field.value;
        }
        
        public List<T> FindAll(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException("The match has a null value!...");

            List<Element> elements = _elements.FindAll((element) =>
            {
                return match.Invoke(element.Field.value);
            });
            List<T> values = new List<T>(elements.Count);

            foreach (Element element in elements)
            {
                values.Add(element.Field.value);
            }

            return values;
        }

        public void ForEach(Action<T> func)
        {
            if (func == null) throw new ArgumentNullException("The func has a null value!...");

            _elements.ForEach((value) => {
                T t = value.Field.value;
                if (t != null) func.Invoke(t);
            });
        }

        public int IndexOf(T item)
        {
            if (IsCompatibleObject(item))
            {
                T[] array = ToArray();
                int result = Array.IndexOf(array, item, 0);

                if (result < 0) return result;
                return _elements[result].Id;
            }

            return -1;
        }

        public bool Contains(T item)
        {
            int size = _elements.Count;
            if ((System.Object)item == null)
            {
                for (int i = 0; i < size; i++)
                    if ((System.Object)_elements[i].Field.value == null)
                        return true;
                return false;
            }
            else
            {
                EqualityComparer<T> c = EqualityComparer<T>.Default;
                for (int i = 0; i < size; i++)
                {
                    if (c.Equals(_elements[i].Field.value, item)) return true;
                }
                return false;
            }
        }

        public Dictionary<int,T> ToDictionary()
        {
            Dictionary<int, T> array = new Dictionary<int, T>();

            for (int i = 0; i < _elements.Count; i++)
            {
                array.Add(_elements[i].Id, _elements[i].Field.value);
            }

            return array;
        }

        public List<T> ToList()
        {
            List<T> list = new List<T>(_elements.Count);

            for (int i = 0; i < _elements.Count; i++)
            {
                list.Add(_elements[i].Field.value);
            }

            return list;
        }

        public T[] ToArray()
        {
            T[] array = new T[_elements.Count];

            for (int i = 0; i < _elements.Count; i++)
            {
                array[i] = _elements[i].Field.value;
            }

            return array;
        }

        public IEnumerator<T> GetEnumerator()
        {
            T[] result = ToArray();
            foreach (T value in result)
            {
                yield return value;
            }
        }

        public void Add(T value)
        {
            InsertSortedElement(LastId + 1, value);
        }
        
        public void AddRange(ICollection<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("The collection has a null value!...");

            foreach (var value in collection)
                Add(value);
        }

        public bool Remove(T item)
        {
            if (IsCompatibleObject(item))
            {
                T[] array = ToArray();
                int index = Array.IndexOf(array, item, 0);

                if (index < 0) return false;
                RemoveAtIndex(index);
                return true;
            }
            return false;
        }

        public int RemoveAll(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException("The match has a null value!...");

            return _elements.RemoveAll((element) => match.Invoke(element.Field.value));
        }

        public bool RemoveAt(int id)
        {
            int index = FindIndex(id);

            if (index < 0) return false;

            RemoveAtIndex(index);
            return true;
        }

        private void RemoveAtIndex(int index)
        {
            _elements[index].Field.Delete();
            _elements.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            T[] result = ToArray();
            foreach (T value in result)
            {
                yield return value;
            }
        }
    }
}