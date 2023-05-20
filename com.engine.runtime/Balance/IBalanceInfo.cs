using System;

namespace Engine.Balance
{

    public interface IBalanceInfo : ICloneable, IAsset
    {
        string Name { get; }
        float GetValue(int level);
    }
}
