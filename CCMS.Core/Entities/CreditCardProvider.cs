using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMS.Core.Entities
{
    public class CreditCardProvider
    {
        private string _id;
        private int _version;
        private Audit _audit;
        private string _name;
        private decimal _interestRate;
        private ICollection<ComputationSetting> _computationSettings;

        public virtual string Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        public virtual int Version
        {
            get { return _version; }
            protected set { _version = value; }
        }

        public virtual Audit Audit
        {
            get { return _audit; }
            set { _audit = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual decimal InterestRate
        {
            get { return _interestRate; }
            set { _interestRate = value; }
        }

        public virtual IEnumerable<ComputationSetting> ComputationSettings
        {
            get { return _computationSettings; }
            set { SyncComputationSettings(value); }
        }

        #region Methods

        public virtual void AddComputationSetting(ComputationSetting item)
        {
            item.Provider = this;
            _computationSettings.Add(item);
        }

        public virtual void RemoveComputationSetting(ComputationSetting item)
        {
            item.Provider = null;
            _computationSettings.Remove(item);
        }

        private void SyncComputationSettings(IEnumerable<ComputationSetting> items)
        {
            // set relation
            foreach (var item in items)
                item.Provider = this;

            var itemsToInsert = items.Except(_computationSettings).ToList();
            var itemsToUpdate = _computationSettings.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = _computationSettings.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                item.Provider = this;
                _computationSettings.Add(item);
            }

            // update
            foreach (var item in itemsToUpdate)
            {
                var value = items.FirstOrDefault(x => x == item);
                item.Provider = this;
                item.SerializeWith(value);
            }

            // delete
            foreach (var item in itemsToRemove)
            {
                item.Provider = null;
                _computationSettings.Remove(item);
            }
        }

        //public virtual Money Compute<T>(Money amount) where T : ComputationSetting
        //{
        //    var computationSetting = _computationSettings.OfType<T>()
        //        .Where(x =>
        //            x.MinimumAmount != null &&
        //            x.MinimumAmount.Currency.Id == amount.Currency.Id
        //        )
        //        .FirstOrDefault();

        //    if (computationSetting != null)
        //        return computationSetting.Compute(amount);

        //    return new Money(amount.Currency, 0M);
        //}

        #endregion

        #region Constructors

        public CreditCardProvider() 
        {
            _computationSettings = new List<ComputationSetting>();
        }

        public CreditCardProvider(string id, string name)
            : this()
        {
            _id = id;
            _name = name;
        }

        public CreditCardProvider(string id, string name, decimal interestRate)
            : this(id, name)
        {
            _interestRate = interestRate;
        }

        public CreditCardProvider(string id, string name, decimal interestRate, IEnumerable<ComputationSetting> computationSettings)
            : this(id, name, interestRate)
        {
            SyncComputationSettings(computationSettings);
        }

        #endregion

        #region Static Members

        private static IEnumerable<ComputationSetting> DefaultSettings(decimal interestRate)
        {
            var computationSettings = new List<ComputationSetting>();

            foreach (var currency in Currency.All)
            {
                computationSettings.Add(new FinanceChargeSetting() { MinimumAmount = new Money(currency, 500), Rate = interestRate });
                computationSettings.Add(new InterestSetting() { MinimumAmount = new Money(currency, 500), Rate = interestRate });
                computationSettings.Add(new LateChargeSetting() { MinimumAmount = new Money(currency, 500), Rate = interestRate });
                computationSettings.Add(new MinimumPaymentSetting() { MinimumAmount = new Money(currency, 500), Rate = interestRate });
                computationSettings.Add(new OverlimitFeeSetting() { MinimumAmount = new Money(currency, 500), Rate = interestRate });
                computationSettings.Add(new ServiceFeeSetting() { MinimumAmount = new Money(currency, 500), Rate = interestRate });
            }

            return computationSettings;

            //return new ComputationSetting[]
            //{
            //    new FinanceChargeSetting() { MinimumAmount = new Money(Currency.PHP, 500), Rate = interestRate },
            //    new InterestSetting() { MinimumAmount = new Money(Currency.PHP, 500), Rate = interestRate },
            //    new LateChargeSetting() { MinimumAmount = new Money(Currency.PHP, 500), Rate = interestRate },
            //    new OverlimitFeeSetting() { MinimumAmount = new Money(Currency.PHP, 500), Rate = interestRate },
            //    new ServiceFeeSetting() { MinimumAmount = new Money(Currency.PHP, 500), Rate = interestRate },
            //};
        }

        public static readonly CreditCardProvider HSBC = new CreditCardProvider("HSBC", "Hong Kong Banking Corporation", 0.03M, DefaultSettings(0.03M));
        public static readonly CreditCardProvider RCBC = new CreditCardProvider("RCBC", "Rizal Commercial Banking Corporation", 0.04M, DefaultSettings(0.04M));
        public static readonly CreditCardProvider BPI = new CreditCardProvider("BPI", "Bank of Philippine Islands", 0.05M, DefaultSettings(0.05M));
        public static readonly CreditCardProvider BDO = new CreditCardProvider("BDO", "Banco De Oro", 0.03M, DefaultSettings(0.03M));
        public static readonly CreditCardProvider AMX = new CreditCardProvider("AMX", "American Express", 0.04M, DefaultSettings(0.04M));
        public static readonly IEnumerable<CreditCardProvider> All = new CreditCardProvider[] { HSBC, RCBC, BPI, BDO, AMX };

        #endregion

        #region Equality Comparer

        private Nullable<int> _hashCode;

        public override bool Equals(object obj)
        {
            var that = obj as CreditCardProvider;

            if (that == null)
                return false;

            if (string.IsNullOrWhiteSpace(that.Id) && string.IsNullOrWhiteSpace(this.Id))
                return object.ReferenceEquals(that, this);

            return (that.Id == this.Id);
        }

        public override int GetHashCode()
        {
            if (_hashCode == null)
            {
                _hashCode = (!string.IsNullOrWhiteSpace(this.Id))
                    ? this.Id.GetHashCode()
                    : base.GetHashCode();
            }

            return _hashCode.Value;
        }

        public static bool operator ==(CreditCardProvider x, CreditCardProvider y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(CreditCardProvider x, CreditCardProvider y)
        {
            return !Equals(x, y);
        }

        #endregion
    }
}
