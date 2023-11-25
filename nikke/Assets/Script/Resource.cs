using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Resource : MonoBehaviour
{
    private static Resource instance;
    public static Resource Instance => instance;
    [SerializeField] Card _cardPrefab;
    public int startDeckTier;
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
        money = 100; jewel = 5;
        shopcard = CardDatabase.Instance.card("µø±€¿Ã");
        Deck = new List<CardStruct>();
        for(int i = 0; i < 5; i++)
        {
            var tmp = (CardDatabase.Instance.cardByTier(1));
            Deck.Add(tmp);

        }
        for (int i = 0; i < 15; i++)
        {
            var tmp = (CardDatabase.Instance.cardByTier(startDeckTier));
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
    public CardStruct shopcard;

    public void StageUp()
    {
        Stage++;
        if (Stage > 6)
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
    public void Deck_Remove(string name)
    {
        foreach(CardStruct tmp in Deck)
            if (tmp._name.Equals(name))
            {
                Deck.Remove(tmp);
                var card = Instantiate(_cardPrefab, new Vector2(0, 0), Quaternion.identity);
                card.transform.localScale = new Vector2(2f, 2f);
                card.Set(tmp);
                card.setLayer(0, 500);
                card.TouchableChange(false);
                card.transform.DOScale(0, 1f).OnComplete(() => { Destroy(card.gameObject); });
                return;
            }
    }
    public void Deck_Add(string name)
    {
        CardStruct tmp = CardDatabase.Instance.card(name);
        Deck.Add(tmp);

        var card = Instantiate(_cardPrefab, new Vector2(0, 0), Quaternion.identity);
        card.transform.localScale = new Vector2(0f, 0f);
        card.Set(tmp);
        card.setLayer(0,500);
        card.TouchableChange(false);
        card.transform.DOScale(2, 1f).OnComplete(() => { Destroy(card.gameObject); });

    }

    [Space(10f)]
    public int cardNum;
    [ContextMenu("addCard")]
    public void cont_addCard()
    {
        Deck.Add(CardDatabase.Instance.card(cardNum));
    }
    [ContextMenu("stageUp")]
    public void cont_stageUp()
    {
        StageUp();
    }
}
