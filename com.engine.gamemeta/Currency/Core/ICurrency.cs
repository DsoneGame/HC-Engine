using HCEngine.DI;
using HCEngine.Events;

namespace HCEngine.Currency
{
    public interface ICurrency : IIdentificator
    {
        int totalCoins { get; }

        IEventSubscribe<ICurrencyUpdated> OnCurrencyUpdated { get; }

        bool AddCurrency(int amount);

        bool RemoveCurrency(int amount);
    }
}
