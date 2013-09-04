using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CCMS.UI.Infrastructure
{
    public class ChangeTrackingObservableCollection<T> : ObservableCollection<T>, UI.Infrastructure.IChangeTracking
        where T : UI.Infrastructure.IChangeTracking
    {
        private IList<T> _deletedItems = new List<T>();
        private IList<T> _insertedItems = new List<T>();

        public ChangeTrackingObservableCollection() { }

        public ChangeTrackingObservableCollection(IList<T> list)
            : base(list) { this.AcceptChanges(); }

        public ChangeTrackingObservableCollection(IEnumerable<T> collection)
            : base(collection) { this.AcceptChanges(); }

        protected override void ClearItems()
        {
            base.ClearItems();
            _deletedItems = new List<T>();
        }

        protected override void InsertItem(int index, T item)
        {
            _insertedItems.Add(item);
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            if (!base.Contains(item))
                return;

            if (_insertedItems.Contains(item))
                _insertedItems.Remove(item);
            else
                _deletedItems.Add(item);

            base.RemoveItem(index);
        }

        #region Methods

        public virtual IEnumerable<T> GetAllItems()
        {
            return this.Union(_deletedItems);
        }

        public virtual IEnumerable<T> GetInsertedItems()
        {
            return _insertedItems;
        }

        public virtual IEnumerable<T> GetEditedItems()
        {
            return this.Where(x => x.IsDirty && !_insertedItems.Contains(x));
        }

        public virtual IEnumerable<T> GetDeletedItems()
        {
            return _deletedItems;
        }

        #endregion

        #region IChangeTracking Members

        public bool IsDirty
        {
            get
            {
                return this.Any(x => x.IsDirty) ||
                    _deletedItems.Count > 0 ||
                    _insertedItems.Count > 0;
            }
        }

        public bool HasAppliedChanges
        {
            get { return this.All(x => x.HasAppliedChanges); }
        }

        public virtual void AcceptChanges()
        {
            foreach (var item in this)
                item.AcceptChanges();
        }

        #endregion


    }
}
