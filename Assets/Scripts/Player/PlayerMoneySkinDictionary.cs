using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [Serializable]
    public class PlayerMoneySkinDictionary : SerializableDictionaryBase<PlayerMoneySkinKey, PlayerMoneySkin> { }

    [Serializable]
    public struct PlayerMoneySkin
    {
        public Sprite sprite;
        public Color textColor;
        public string textText;
    }

    public enum PlayerMoneySkinKey
    {
        Hobo, Poor, Decent, Rich, Millionere
    }
}