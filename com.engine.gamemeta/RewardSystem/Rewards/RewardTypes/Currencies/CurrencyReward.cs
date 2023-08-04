using HCEngine.Currency;
using HCEngine.DI;
using System.Diagnostics.Contracts;
using System.Linq;

namespace HCEngine.RewardSystem
{
    public class CurrencyReward : Rewardable, IAwake
    {
        private CurrencyType _currencyType;
        private ICurrency _currency;

        public CurrencyReward(RewardableType type, CurrencyType currencyType) : base(type)
        {
            _currencyType = currencyType;
        }

        public void Awake()
        {
            InitializeCurrency();
        }

        public void InitializeCurrency()
        {
            _currency = DIContainer.WhereId<ICurrency>((int)_currencyType).LastOrDefault();
            Contract.Assert(_currency != null, $"The currency of type {_currencyType} is not found!");
        }

        public override void Claim(int count)
        {
            _currency.AddCurrency(count);
        }
    }
}