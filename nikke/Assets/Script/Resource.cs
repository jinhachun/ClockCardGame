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
    public void reset()
    {
        Destroy(this.gameObject);

    }
    public void Start()
    {
        time = 0f;
        VillageLevel = new Dictionary<string, int>();
        VillageLevel.Add("Farm", PlayerPrefs.GetInt("Farm",0));
        VillageLevel.Add("House", PlayerPrefs.GetInt("House", 0));
        VillageLevel.Add("Church", PlayerPrefs.GetInt("Church", 0));
        VillageLevel.Add("Bath", PlayerPrefs.GetInt("Bath", 0));


        SupportPrice = new Dictionary<string, int>();
        SupportPrice.Add("Evolve", 0);
        SupportPrice.Add("Add", 0);
        SupportPrice.Add("Delete", 0);
        SupportPrice.Add("Heal", 0);
        Hp = 100; tmpMhp = 100; Area = 1; Stage = 1;
        money = 100; jewel = PlayerPrefs.GetInt("jewel", 3);
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
    public List<BonusRule> Rules;
    public int Hp;
    public int tmpMhp;
    public int mHp => tmpMhp * (100 + 5 * VillageLevel["Bath"]) / 100;
    public int Area; public int Stage;
    public int money;
    public Dictionary<string,int> VillageLevel;
    public Dictionary<string, int> SupportPrice;
    public int jewel;
    public List<float> combiRate;
    public CardStruct shopcard;
    public float time;
    public bool haveCard(string a)
    {
        bool chk = false;
        foreach (CardStruct cs in Deck)
        {
            if (cs._name.Equals(a))
            {
                chk = true;
                return chk;
            }
        }
        return chk;
    }
    public void StageUp()
    {
        Stage++;
        if (Stage > 6)
        {
            Area++;
            Stage = 1;
        }
        Debug.Log(Area + "-" + Stage);   
    }
    public void setHp(int a)
    {
        Hp = a;
    }
    public void Event_Heal(int a)
    {
        DamagePopup.Create(new Vector2(-2,-3), "+"+a, Color.green);
        Hp += a;
        if (Hp >= mHp) Hp = mHp;
    }
    public void Event_Damage(int a)
    {
        DamagePopup.Create(new Vector2(-2, -3), "" + a, Color.white);
        Hp -= a;
        if (Hp <= 0) Hp = 1;
    }
    public void Event_MoneyEarn(int a)
    {
        money += a;
        RewardPopup.Create(a, false);
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
                card.transform.DOScale(0, 1f).SetEase(Ease.InCubic).OnComplete(() => { Destroy(card.gameObject); });
                return;
            }
    }
    public void Deck_Add(CardStruct cardStruct)
    {
        Deck.Add(cardStruct);

        var card = Instantiate(_cardPrefab, new Vector2(0, 0), Quaternion.identity);
        card.transform.localScale = new Vector2(0f, 0f);
        card.Set(cardStruct);
        card.setLayer(0, 500);
        card.TouchableChange(false);
        card.transform.DOScale(2, 1f).SetEase(Ease.OutCubic).OnComplete(() => { Destroy(card.gameObject); });

    }
    public void Deck_Add(string name)
    {
        CardStruct tmp = CardDatabase.Instance.card(name);
        Deck_Add(tmp);

    }
    public void Deck_Add(string name,bool token)
    {
        CardStruct tmp = CardDatabase.Instance.card_token(name);
        Deck_Add(tmp);

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
    [ContextMenu("areaUp")]
    public void cont_areaUp()
    {
        Area++;
    }
}
