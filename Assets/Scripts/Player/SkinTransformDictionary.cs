using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [Serializable]
    public class SkinTransformDictionary : SerializableDictionaryBase<PlayerMoneySkinKey, Transform> { };
}