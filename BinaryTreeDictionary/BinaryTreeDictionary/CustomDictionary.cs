using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTreeDictionary
{
   public class CustomDictionary<Tkey, TVal> : IDictionary<Tkey, TVal> where Tkey : IComparable
    {
        # region Helper
        private class TreeItem
        {
            public KeyValuePair<Tkey, TVal> _pair;
            public TreeItem _parent;
            public TreeItem _left;
            public TreeItem _right;

            public TreeItem(Tkey key, TVal value, TreeItem parent = null, TreeItem left = null, TreeItem right = null)
            {
                _pair = new KeyValuePair<Tkey, TVal>(key, value);
                _parent = parent;
                _left = left;
                _right = right;
            }
            public TreeItem(KeyValuePair<Tkey, TVal> pair, TreeItem parent = null, TreeItem left = null, TreeItem right = null)
            {
                _pair = pair;
                _parent = parent;
                _left = left;
                _right = right;
            }
        }
        #endregion

        # region Fields
        private TreeItem _root = null;
        private int _counter = 0;
        private bool _allowDuplicateKeys;
        #endregion

        # region Ctor
        public CustomDictionary(bool allowDuplicateKeys = false)
        {
            _allowDuplicateKeys = allowDuplicateKeys;
        }
        # endregion

        #region Properties
        public ICollection<Tkey> Keys
        {
            get
            {
                List<Tkey> keyList = new List<Tkey>();
                foreach (var item in this)
                {
                    keyList.Add(item.Key);
                }
                return keyList;
            }
        }

        public ICollection<TVal> Values
        {
            get
            {
                List<TVal> valList = new List<TVal>();
                foreach (var item in this)
                {
                   valList.Add(item.Value);
                }
                return valList;
            }
        }

        public int Count
        {
            get
            {
                return _counter;
            }
            set
            {
                _counter = value;
            }
        }

        public bool IsReadOnly { get => false; }

        public TVal this[Tkey key]
        {
            get
            {
                bool point = true;
                TVal val = default(TVal);
                foreach (var item in this)
                {
                    if (item.Key.CompareTo(key)==0)
                    {
                        val = item.Value;
                        point = false;
                        break;
                    }
                }
                if (point)
                {
                    throw new Exception();
                }
                return val;
            }
            set
            {
                Stack<TreeItem> stack = new Stack<TreeItem>();
                TreeItem tempItem = _root;

                while (tempItem != null || stack.Count != 0)
                {
                    if (stack.Count != 0)
                    {
                        tempItem = stack.Pop();

                        if (tempItem._pair.Key.CompareTo (key)==0)
                        {
                            tempItem._pair = new KeyValuePair<Tkey, TVal>(key, value);
                            break;
                        }
                        if (tempItem._right != null)
                        {
                            tempItem = tempItem._right;
                        }
                        else
                        {
                            tempItem = null;
                        }
                    }
                    while (tempItem != null)
                    {
                        stack.Push(tempItem);
                        tempItem = tempItem._left;
                    }
                }
            }
        }
        #endregion

        # region Methods

        public bool ContainsKey(Tkey key)
        {
            foreach (var item in this)
            {
                if (item.Key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        public void Add(Tkey key, TVal value)
        {
            Add(new KeyValuePair<Tkey, TVal>(key, value));
        }

        public void Add(KeyValuePair<Tkey, TVal> item)
        {
            if (_root == null)
            {
                _root = new TreeItem(item);
                ++_counter;
            }
            else
            {
                Add(item, _root);
            }
        }

        void Add(KeyValuePair<Tkey,TVal> pair, TreeItem parent)
        {
            if (!_allowDuplicateKeys && pair.Key.CompareTo(parent._pair.Key)==0)
            {
                parent._pair = pair;
            }
            else if (parent._pair.Key.CompareTo(parent._pair.Key) > 0)
            {
                if (parent._left == null)
                {
                    parent._left = new TreeItem(pair);
                    _counter++;
                }
                else
                {
                    Add(pair, parent._left);
                }
            }
            else
            {
                if (parent._right==null)
                {
                    parent._right = new TreeItem(pair);
                    _counter++;
                }
                else
                {
                    Add(pair, parent._right);
                }
            }
        }

        public bool TryGetValue(Tkey key, out TVal value)
        {
            foreach (var item in this)
            {
                if (item.Key.CompareTo(key)==0)
                {
                    value = item.Value;
                    return true;
                }
            }
            value = default(TVal);
            return false;
        }

        public void Clear()
        {
            _counter = 0;
            _root = null;
        }

        public bool Contains(KeyValuePair<Tkey, TVal> item)
        {
            foreach (var pair in this)
            {
                if (item.Value.Equals(pair.Value))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(KeyValuePair<Tkey, TVal>[] array, int arrayIndex)
        {
            foreach (var item in array)
            {
                array[arrayIndex++] = item;
            }
        }

        public bool Remove(KeyValuePair<Tkey, TVal> item)
        {
            if (_allowDuplicateKeys == false)
            {
                Remove(item.Key);
            }

            TreeItem _currItem;
            TreeItem _baseItem;

            _currItem = FindWithParentByItem(item, out _baseItem);

            if (_currItem == null)
            {
                return false;
            }
            Count--;

            if (_currItem._right == null)
            {
                RemoveRightNull(_currItem, _baseItem);
            }

            else if (_currItem._right._left == null)
            {
                RemoveRightLeftNull(_currItem, _baseItem);
            }
            else
            {
                RemoveRightLeftAre(_currItem, _baseItem);
            }
            return true;
        }

        public bool Remove(Tkey key)
        {
            TreeItem _currItem;
            TreeItem _baseItem;

            _currItem = FindWithParentByKey(key, out _baseItem);

            if (_currItem == null)
            {
                return false;
            }
            Count--;

            if (_currItem._right == null)
            {
                RemoveRightNull(_currItem, _baseItem);
            }

            else if (_currItem._right._left == null)
            {
                RemoveRightLeftNull(_currItem, _baseItem);
            }
            else
            {
                RemoveRightLeftAre(_currItem, _baseItem);
            }
            return true;
        }


        #endregion

        # region Utilities

        private TreeItem FindWithParentByItem(KeyValuePair<Tkey,TVal> item, out TreeItem baseItem)
        {
            TreeItem _curr = _root;
            baseItem = null;

            while (_curr != null)
            {
                int rez = _curr._pair.Key.CompareTo(item.Key);

                if (rez>0)
                {
                    baseItem = _curr;
                    _curr = _curr._left;
                }
                else if (rez<0)
                {
                    baseItem = _curr;
                    _curr = _curr._right;
                }
                else
                {
                    if (_curr._pair.Value.Equals(item.Value))
                    {
                        break;
                    }
                    else
                    {
                        baseItem = _curr;
                        _curr = _curr._right;
                    }
                }
            }
            return _curr;
        }

        private TreeItem FindWithParentByKey(Tkey key, out TreeItem baseItem)
        {
            TreeItem _curr = _root;
            baseItem = null;

            while (_curr != null)
            {
                int rez = _curr._pair.Key.CompareTo(key);

                if (rez > 0)
                {
                    baseItem = _curr;
                    _curr = _curr._left;
                }
                else if (rez < 0)
                {
                    baseItem = _curr;
                    _curr = _curr._right;
                }
                else
                {
                    break;
                }
            }
            return _curr;
        }

        private void RemoveRightNull(TreeItem currItem, TreeItem baseItem)
        {
            if (baseItem == null)
            {
                _root = currItem._left;
            }
            else
            {
                if (baseItem._pair.Key.CompareTo(currItem._pair.Key)>0)
                {
                    baseItem._left = currItem._left;
                }
                else
                {
                    baseItem._right = currItem._left;
                }
            }
        }

        private void RemoveRightLeftNull(TreeItem currItem, TreeItem baseItem)
        {
            currItem._right._left = currItem._left;

            if (baseItem == null)
            {
                _root = currItem._right;
            }
            else
            {
                if (baseItem._pair.Key.CompareTo(currItem._pair.Key)>0)
                {
                    baseItem._left = currItem._right;
                }
                else
                {
                    baseItem._right = currItem._right;
                }
            }
        }

        private void RemoveRightLeftAre(TreeItem currItem, TreeItem baseItem)
        {
            TreeItem _left = currItem._right._left;
            TreeItem _leftBase = currItem._right;

            while (_left._left != null)
            {
                _leftBase = _left;
                _left = _left._left;
            }

            _leftBase._left = _left._right;

            _left._left = currItem._left;
            _left._right = currItem._right;

            if (baseItem == null)
            {
                //_root == _left;
            }
            else
            {
                if (baseItem._pair.Key.CompareTo(currItem._pair.Key)>0)
                {
                    baseItem._left = _left;
                }
                else
                {
                    baseItem._right = _left;
                }
            }
        }

        # endregion

        #region IEnumerable

        public IEnumerator<KeyValuePair<Tkey, TVal>> GetEnumerator()
        {
            Stack<TreeItem> stack = new Stack<TreeItem>();
            TreeItem tempItem = _root;

            while (tempItem != null || stack.Count != 0)
            {
                if (stack.Count != 0)
                {
                    tempItem = stack.Pop();
                    yield return tempItem._pair;

                    if (tempItem._right != null)
                    {
                        tempItem = tempItem._right;
                    }
                    else
                    {
                        tempItem = null;
                    }
                }
                while (tempItem != null)
                {
                    stack.Push(tempItem);
                    tempItem = tempItem._left;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }
}
