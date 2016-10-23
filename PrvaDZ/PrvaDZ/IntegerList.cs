using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrvaDZ
{
    public class IntegerList : IIntegerList
    {
        private int[] _internalStorage;
        private int _index = -1;
        public int[] InternalStorage
        {
            get
            {
                return _internalStorage;
            }
        }

        public IntegerList()
        {
            _internalStorage = new int[4];
        }

        public IntegerList(int InitialSize)
        {
            if (InitialSize < 0)
            {
                Console.WriteLine("Veličina liste mora biti pozitivan broj");
            }
            else
            {
                _internalStorage = new int[InitialSize];
            }
        }

        public void Add(int item)
        {
            if (_internalStorage.Length-1 == _index)
            {
                Array.Resize(ref _internalStorage, 2 * _internalStorage.Length);
            }
            _internalStorage[++_index] = item;

        }

        public bool Remove(int item)
        {
            for (int i = 0; i <= _index; i++)
            {
                if (_internalStorage[i] == item)
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

        public int GetElement(int index)
        {
            if (index <= _index)
            {
                return _internalStorage[index];
            }
            throw new IndexOutOfRangeException("Izvan spremnika");
        }

        public int IndexOf(int item)
        {
            for (int i = 0; i < _index; i++)
            {
                if (_internalStorage[i] == item)
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
            _internalStorage = new int[0];
            _index = -1;

        }
        public bool Contains(int item)
        {
            for (int i = 0; i < _index; i++)
            {
                if (_internalStorage[i] == item)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
