using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoneySkinDictionary : List<PlayerMoneySkin> { }

[Serializable]
public struct PlayerMoneySkin
{
    public float minMoney;
    public Sprite sprite;
    public Color textColor;
    public string textText;
}

public enum PlayerMoneySkinKey
{
    Poor, Decent, Rich
}
