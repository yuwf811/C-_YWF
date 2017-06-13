using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YOrm.Hash;
using YOrm.Provider;

namespace YOrm
{
    public class YTable<T> : IQueryable<T>
    {
        private Expression _expression;
        private IDbQueryProvider _provider;
        private List<T> _queryItems;
        private List<T> _deletedItems;
        private List<T> _addedItems;
        private HashSet<string> _originalHashSet;

        public YTable()
        {
            _expression = Expression.Constant(this);
            _provider = new YQueryProvider<T>(this);
            _queryItems = new List<T>();
            _deletedItems = new List<T>();
            _addedItems = new List<T>();
            _originalHashSet = new HashSet<string>();
        }

        public YTable(Expression expression, IDbQueryProvider provider)
        {
            _expression = expression;
            _provider = provider;
            _queryItems = new List<T>();
            _deletedItems = new List<T>();
            _addedItems = new List<T>();
            _originalHashSet = new HashSet<string>();
        }

        public List<T> QueryItems { get { return _queryItems; } }
        public List<T> AddedItems { get { return _addedItems; } }
        public List<T> DeletedItems { get { return _deletedItems; } }
        public HashSet<string> OriginalHashSet { get { return _originalHashSet; } }

        public void SetExpression(Expression expression)
        {
            this._expression = expression;
        }

        public void SetConnection(string connStr)
        {
            _provider.SetConnection(connStr);
        }

        public void ClearItems()
        {
            _queryItems.Clear();
            _addedItems.Clear();
            _deletedItems.Clear();
        }

        public void InitData(T t)
        {
            _queryItems.Add(t);
            _originalHashSet.Add(new MD5Computer().ComputeHash<T>(t));
        }

        #region IQueryable
        public IEnumerator<T> GetEnumerator()
        {
            var result = _provider.Execute<IEnumerable<T>>(_expression);
            if (result == null)
                yield break;
            foreach (var item in result)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get { return _expression; }
        }

        public IQueryProvider Provider
        {
            get { return _provider; }
        }
        #endregion

        #region Operation

        public void Add(T item)
        {
            _addedItems.Add(item);
        }

        public void Delete(T item)
        {
            _addedItems.Remove(item);
            _queryItems.Remove(item);
            _deletedItems.Add(item);
        }

        #endregion
    }
}
