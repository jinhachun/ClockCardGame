using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private static Resource instance;
    public static Resource Instance => instance;
    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        
    }
    public void Start()
    {
        Hp = 100; mHp = 100; Area = 1; Stage = 1;
        money = 0;
        Deck = new List<CardStruct>();
        for (int i = 0; i < 20; i++)
        {
            var tmp = (CardDatabase.Instance.RandomCard());
            Deck.Add(tmp);
        }
    }
    public List<CardStruct> Deck;
    public int Hp, mHp;
    public int Area; public int Stage;
    public int money;
    
    
    
}
