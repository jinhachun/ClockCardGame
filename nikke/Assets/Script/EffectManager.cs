using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    static EffectManager instance;
    [SerializeField]
    public List<Transform> effectList;

    private void Start()
    {
        instance = this;
    }
    public enum EffectName { Fracture,Attack,Summon,Rage,Heal,Shield,Debuff,Fire,Water,Grass,Light,Dark };

    static public Transform effectOn(EffectName name, Enemy enemy)
    {
        var Effect = Instantiate(instance.effectList[(int)name].transform, new Vector2(enemy._img.transform.position.x, (enemy._img.transform.position.y - 2f)), Quaternion.identity);
        return Effect;
    }
    static public Transform effectOn(EffectName name, Card card)
    {
        var Effect = Instantiate(instance.effectList[(int)name].transform, new Vector2(card.transform.position.x, (card.transform.position.y - 2f)), Quaternion.identity);
        return Effect;
    }
    static public Transform effectOn(EffectName name, Vector2 v)
    {
        var Effect = Instantiate(instance.effectList[(int)name].transform, v, Quaternion.identity);
        return Effect;
    }
}
