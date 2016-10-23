using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrvaDZ
{
    public class GenericListEnumerator <X>: IEnumerator<X>
    {
        private int _index = -1;
        private X[] _list;
        private X _current;

        public X Current
        {
            get
            {
                return _current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public GenericListEnumerator(GenericList<X> lo)
        {
               this._list = lo.InternalStorage; 
        }
       

        public void Dispose()
        { 
        }

        public void Reset()
        {
            _index = -1;
        }

        public bool MoveNext()
        {
            if (++_index < _list.Length)
            {
                _current = _list[_index];
                return true;
            }    
            return false;
        }


    }
}
