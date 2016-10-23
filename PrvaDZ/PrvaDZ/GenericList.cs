using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrvaDZ
{
    public class GenericList<X> : IGenericList<X>
    {
        private X[] _internalStorage;
        private int _index = -1;
        public X[] InternalStorage
        {
            get
            {
                return _internalStorage;
            }
        }

        public GenericList()
        {
            _internalStorage = new X[4];
        }

        public GenericList(int InitialSize)
        {
            if (InitialSize < 0)
            {
                Console.WriteLine("Veličina liste mora biti pozitivan broj");
            }
            else
            {
                _internalStorage = new X[InitialSize];
            }
        }

        public void Add( X item)
        {
            if (_internalStorage.Length - 1 == _index)
            {
                Array.Resize(ref _internalStorage, 2 * _internalStorage.Length);
            }
            _internalStorage[++_index] = item;

        }

        public bool Remove( X item)
        {
            for (int i = 0; i <= _index; i++)
            {
                if (_internalStorage[i].Equals(item))
                {
                    return RemoveAt(i);
                }
            }
            return false;
        }

        public bool RemoveAt(int index)
        {
            if (index > _index)
            {
                return false;
            }
            for (int i = index; i < _index; i++)
            {
                _internalStorage[i] = _internalStorage[i + 1];
            }
            _index--;
            return true;

        }

        public X GetElement(int index)
        {
            if (index <= _index)
            {
                return _internalStorage[index];
            }
            throw new IndexOutOfRangeException("Izvan spremnika");
        }

        public int IndexOf(X item)
        {
            for (int i = 0; i < _index; i++)
            {
                if (_internalStorage[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public int Count
        {
            get
            {
                return _index + 1;
            }
        }
        public void Clear()
        {
            _internalStorage = new X [0];
            _index = -1;

        }
        public bool Contains( X item)
        {
            for (int i = 0; i < _index; i++)
            {
                if (_internalStorage[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<X> GetEnumerator()
        {
            return new GenericListEnumerator<X>(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
