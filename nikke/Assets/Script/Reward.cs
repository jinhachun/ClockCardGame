using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RewardUnit
{
    public string _name;
    public REWARD _reward;
    public int _value;
    public Sprite sprite;

    RewardUnit(string name,REWARD reward,int value)
    {
        this._name = name;
        this._reward = reward;
        this._value = value;
    }
}
[Serializable]
public enum REWARD
{
    CREDIT,NORMALMOLD,HIGHQMOLD,NORMALCARD,RARECARD
}
public class Reward : MonoBehaviour
{
    
    [SerializeField] RewardUnitSprite _rewardPrefab;
    List<RewardUnit> rewardUnits;

    public void Set(List<RewardUnit> units)
    {
        rewardUnits = units;
        for (int i = 0; i < units.Count; i++)
        {
            var rewardUnit = Instantiate(_rewardPrefab, new Vector2(0f, 2.33f - 1.5f * i), Quaternion.identity);
            rewardUnit.Set(units[i]);
        }
    }
}
