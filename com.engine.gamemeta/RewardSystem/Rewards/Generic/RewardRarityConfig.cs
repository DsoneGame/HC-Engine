using HCEngine.Random;
using UnityEngine;

namespace HCEngine.RewardSystem.Generic
{
    [System.Serializable]
    public struct RewardRarityConfig : IRandomRarity
    {
        public float Rarity;

        public Vector2Int CountRange;

        public RewardableType RewardableType;

        public float RarityValue => Rarity;
    }
}