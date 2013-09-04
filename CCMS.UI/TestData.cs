using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CCMS.Core.Common;
using CCMS.Core.Entities;
using CCMS.Data;
using CCMS.UI.Bootstrappers.IoC;
using CCMS.UI.Features.SplashScreens;
using CCMS.UI.Features.Users;
using CCMS.UI.Infrastructure;
using NHibernate;
using NHibernate.Linq;
using ReactiveUI;

namespace CCMS.UI
{
    public static class TestData
    {
        private static readonly IMessageBus _messageBus = IoC.Container.Resolve<IMessageBus>();
        private static readonly ISessionProvider _sessionProvider = IoC.Container.Resolve<ISessionProvider>();

        private static int _currentClassificationIndex;
        private static int _currentCurrencyIndex;

        #region Routine Helpers

        private static TransactionClassification GetToggledTransactionClassification(IList<TransactionClassification> items)
        {
            var count = items.Count();
            if (count == 0)
                return null;

            if (_currentClassificationIndex == count - 1)
                _currentClassificationIndex = 0;
            else
                _currentClassificationIndex++;

            return items[_currentClassificationIndex];
        }

        private static Currency GetToggledCurrency(IList<Currency> items)
        {
            var count = items.Count();
            if (count == 0)
                return null;

            if (_currentCurrencyIndex == count - 1)
                _currentCurrencyIndex = 0;
            else
                _currentCurrencyIndex++;

            return items[_currentCurrencyIndex];
        }

        private static Money GenerateRandomAmount(Currency currency)
        {
            //sleep so we get a different seed
            Thread.Sleep(20);

            var random = (new Random().NextDouble() * 1.0f - 0f);
            var ccnumber = Math.Floor(random * 10);
            var value = (decimal)ccnumber + 42.23m * 8m;
            return new Money(currency, value);
        }

        private static string GenerateRandomAccountNumber()
        {
            return RandomCreditCardNumberGenerator.GenerateMasterCardNumber();
        }

        #endregion

        public static IEnumerable<CreditCardProvider> GenerateProviders()
        {
            var session = _sessionProvider.GetSharedSession();
            return session.Query<CreditCardProvider>().ToList();
        }

        private static IEnumerable<CreditCard> GenerateCreditCards(User user, IEnumerable<CreditCardProvider> providers)
        {
            var session = _sessionProvider.GetSharedSession();
            var creditCards = new List<CreditCard>();
            var currencies = session.Query<Currency>().ToList();

            foreach (var provider in providers)
            {
                if (user.CreditCards.Any(x => x.Provider == provider))
                    continue;

                var currency = GetToggledCurrency(currencies);
                var creditCard = new CreditCard(user, provider, currency);
                creditCard.AccountName = "Johnny Alibuyog";
                creditCard.AccountNumber = GenerateRandomAccountNumber();
                creditCard.CutOff = 10;
                creditCard.IssueDate = DateTime.Today.AddYears(-3);
                creditCard.ExpiryDate = DateTime.Today.AddYears(3);
                creditCard.User = user;
                creditCard.Provider = provider;
                creditCard.TransactionCurrency = currency;
                creditCard.CreditLimit = GenerateRandomAmount(currency).Times(10000M);
                creditCard.CashAdvanceLimit = GenerateRandomAmount(currency).Times(5000M);
                creditCard.OutstandingBalance = new Money(currency);
                creditCard.AvailableCredit = creditCard.CreditLimit;
                creditCards.Add(creditCard);

                _messageBus.SendMessage(new SplashScreenMessage("Creating Credit Cards: " + creditCard.AccountName));

                Thread.Sleep(100);
            }

            user.AddCreditCards(creditCards);
            return creditCards;
        }

        private static void CreateBillings(CreditCard creditCard)
        {
            if (creditCard.Billings.Count() > 0)
                return;

            var session = _sessionProvider.GetSharedSession();
            var transactionClassifications = session.Query<TransactionClassification>().ToList();
            var staff = GenerateStaff(creditCard.User);

            var start = DateTime.Today.AddMonths(-4);
            var end = DateTime.Today;

            for (var monthCounter = start; monthCounter <= end; monthCounter = monthCounter.AddMonths(1))
            {
                var billing = creditCard.Billings
                    .Where(x =>
                        monthCounter >= x.StartDate &&
                        monthCounter <= x.EndDate
                    )
                    .FirstOrDefault();

                if (billing == null)
                {
                    billing = new Billing();
                    creditCard.AddBilling(billing);
                    billing.SetDuration(monthCounter);
                }

                for (var dayCounter = billing.StartDate; dayCounter <= billing.EndDate; dayCounter = dayCounter.AddDays(1))
                {
                    var transactionAmount = dayCounter.Year * dayCounter.Month;
                    if (creditCard.ResolveMoney(transactionAmount) > creditCard.AvailableCredit)
                        continue;

                    billing.AddBillingItem(new PurchaseBillingItem(new Purchase()
                    {
                        Date = dayCounter,
                        CreditCard = creditCard,
                        Details = string.Format(
                            "Purchase {0} on {1}",
                            creditCard.Provider.Id,
                            dayCounter.ToString("yyyy-MM-dd")
                        ),
                        TransactionClassification = GetToggledTransactionClassification(transactionClassifications),
                        Staff = staff,
                        Amount = new Money(creditCard.TransactionCurrency, transactionAmount),
                    }));
                }

                Thread.Sleep(10);

                _messageBus.SendMessage(new SplashScreenMessage("Creating Billing for: " + billing.StartDate.ToString("yyyy-MM-dd")));
            }
        }

        public static User GenerateUser(bool? persistOnCall = false)
        {
            var session = _sessionProvider.GetSharedSession();
            var user = session.Query<User>().FirstOrDefault();
            if (user == null)
            {
                user = new User()
                {
                    Username = "johnny.alibuyog",
                    Password = "slow_dance",
                    Person = new Person(
                        firstName: "Johnny",
                        middleName: "Asprec",
                        lastName: "Alibuyog",
                        birthDate: new DateTime(1982, 03, 28)
                    )
                };
                session.Save(user);

                if (persistOnCall == true)
                    session.Flush();
            }

            return user;
        }

        public static Staff GenerateStaff(User user)
        {
            var session = _sessionProvider.GetSharedSession();
            var staff = session.Query<Staff>().FirstOrDefault(x => x.User == user);
            if (staff == null)
            {
                staff = new Staff()
                {
                    User = user,
                    Person = new Person(
                        firstName: "Euphrates",
                        middleName: "Agnes",
                        lastName: "Efa"
                    )
                };
                session.Save(staff);
            }
            return staff;
        }

        public static void GenerateData()
        {
            using (var session = _sessionProvider.GetSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(5000);

                var providers = GenerateProviders();
                var user = GenerateUser(); ;
                GenerateCreditCards(user, providers);

                foreach (var creditCard in user.CreditCards)
                    CreateBillings(creditCard);

                session.Save(user);

                App.Data.CurrentUser = new UserViewModel()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Fullname = user.Person.Fullname,
                };

                _messageBus.SendMessage(new SplashScreenMessage("Saving data..."));

                transaction.Commit();
                _sessionProvider.ReleaseSharedSession();
            }
        }
    }
}
