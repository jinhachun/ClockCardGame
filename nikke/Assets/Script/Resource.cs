using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Resource : MonoBehaviour
{
    private static Resource instance;
    public static Resource Instance => instance;
    [SerializeField] Card _cardPrefab;
    [SerializeField] BonusRule _rulePrefab;
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
    [ContextMenu("레벨설정")]
    public void setLEVEL()
    {
        int tmpLv = LEVEL;
        int loopCnt = 1000;
        while (tmpLv > 0 && loopCnt>0)
        {
            int randomIndex = Random.Range(0, CardDatabase.Instance.ruleDatabase.Count);
            RuleStruct bonusRule = CardDatabase.Instance.ruleDatabase[randomIndex];
            if (tmpLv < bonusRule._level) continue;
            if (Rule_no(bonusRule._Number))
            {
                if (bonusRule._only) continue;
                Rules[DataManager.RuleName(bonusRule._Number, Kor)] ++;
            }
            else
            {
                Rules.Add(DataManager.RuleName(bonusRule._Number, Kor), 1);
            }
            tmpLv -= bonusRule._level;
            loopCnt--;
        }
        
        
    }
    public bool Rule_no(int n) => Rules.ContainsKey(DataManager.RuleName(n, Kor));
    public void StartGame()
    {
        time = 0f;
        Kor = true;
        metSquad = new List<EnemySquadStruct>();
        Rules = new Dictionary<string, int>();
        setLEVEL();
        VillageLevel = new Dictionary<string, int>();
        VillageLevel.Add("Farm", 0);
        VillageLevel.Add("House", 0);
        VillageLevel.Add("Church", 0);
        VillageLevel.Add("Bath", 0);


        SupportPrice = new Dictionary<string, int>();
        SupportPrice.Add("Evolve", 0);
        SupportPrice.Add("Add", 0);
        SupportPrice.Add("Delete", 0);
        SupportPrice.Add("Heal", 0);
        float Rule_no2_1 = Rule_no(2) ? -0.3f : 0;
        float Rule_no2_2 = Rule_no(2) ? 0.1f : 0;

        combiRate.Add(1f + Rule_no2_1);
        combiRate.Add(1.2f + Rule_no2_1);
        combiRate.Add(1.5f + Rule_no2_1);
        combiRate.Add(2f + Rule_no2_1);
        combiRate.Add(2.5f + Rule_no2_2);
        combiRate.Add(3f + Rule_no2_2);
        combiRate.Add(3.5f + Rule_no2_2);

        if (Rule_no(14)) combiRate = Enumerable.Reverse(combiRate).ToList();

        float Rule_no3 = Rule_no(3) ? 10f : 0f;
        float Rule_no9 = (Rule_no(9) ? -20 : 0);
        bool Rule_no4 = Rule_no(4);
        Hp = (int)((100f - Rule_no3) + Rule_no9); 
        tmpMhp = Hp; Area = 1; Stage = 1;
        money = 100; jewel = 3;
        shopcard = CardDatabase.Instance.card("동글이");
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
        if (Rule_no4) Deck.Add(CardDatabase.Instance.card_token(DataManager.CardName(2,Kor,true)));
    }
    public List<CardStruct> Deck;
    public int Hp;
    public bool Kor;
    public int tmpMhp;
    public int mHp => tmpMhp * (100 + 10 * VillageLevel["Bath"]) / 100;
    public int Area; public int Stage;
    public int money;
    public int LEVEL;
    public Dictionary<string,int> Rules;
    public Dictionary<string,int> VillageLevel;
    public Dictionary<string, int> SupportPrice;
    public int jewel;
    public List<float> combiRate;
    public List<EnemySquadStruct> metSquad;
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
    public void Deck_Remove(int num,bool token)
    {
        foreach (CardStruct tmp in Deck)
            if (tmp.NUM.Equals(num))
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
    public void Deck_Add(int num)
    {
        CardStruct tmp = CardDatabase.Instance.cardDatabase[num];
        Deck_Add(tmp);

    }
    public void Deck_Add(string name)
    {
        CardStruct tmp = CardDatabase.Instance.card(name);
        Deck_Add(tmp);

    }
    public void Deck_Add(int num, bool token)
    {
        CardStruct tmp = CardDatabase.Instance.cardDatabase_token[num];
        Deck_Add(tmp);

    }
    public void Deck_Add(string name, bool token)
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
