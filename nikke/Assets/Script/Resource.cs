using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private static Resource instance;
    public static Resource Instance => instance;
    public void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        
    }
    public void Start()
    {
        VillageLevel = new Dictionary<string, int>();
        VillageLevel.Add("Farm", 0);
        VillageLevel.Add("House", 0);
        VillageLevel.Add("Church", 0);
        VillageLevel.Add("Bath", 0);
        Hp = 100; tmpMhp = 100; Area = 1; Stage = 1;
        money = 100; jewel = 10;
        Deck = new List<CardStruct>();
        for (int i = 0; i < 20; i++)
        {
            var tmp = (CardDatabase.Instance.RandomCard());
            Deck.Add(tmp);
        }
    }
    public List<CardStruct> Deck;
    public int Hp;
    public int tmpMhp;
    public int mHp => tmpMhp * (100 + 5 * VillageLevel["Bath"]) / 100;
    public int Area; public int Stage;
    public int money;
    public Dictionary<string,int> VillageLevel;
    public int jewel;
    public List<float> combiRate;

    public void StageUp()
    {
        Stage++;
        if (Stage > 5)
        {
            Area++;
            Stage = 1;
        }
    }
    public void setHp(int a)
    {
        Hp = a;
    }
    public void Event_Heal(int a)
    {
        Hp += a;
        if (Hp >= mHp) Hp = mHp;
    }
    public void Event_Damage(int a)
    {
        Hp -= a;
        if (Hp <= 0) Hp = 1;
    }
}
