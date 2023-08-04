using HCEngine.DI;

namespace HCEngine.Upgrade
{
    public interface IUpgradeable : IReadOnlyUpgradeable, IMakeUpgrade, IIdentificator, IResetable
    {

    }
}
