using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Random = UnityEngine.Random;

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
        Rules = new DictionaryOfStringAndInteger();
        setLEVEL();
        VillageLevel = new DictionaryOfStringAndInteger();
        VillageLevel.Add("Farm", 0);
        VillageLevel.Add("House", 0);
        VillageLevel.Add("Church", 0);
        VillageLevel.Add("Bath", 0);


        SupportPrice = new DictionaryOfStringAndInteger();
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
        for(int i = 0; i < 4; i++)
        {
            var tmp = (CardDatabase.Instance.cardByTier(1));
            Deck.Add(tmp);

        }
        for (int i = 0; i < 12; i++)
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
    public DictionaryOfStringAndInteger Rules;
    public DictionaryOfStringAndInteger VillageLevel;
    public DictionaryOfStringAndInteger SupportPrice;
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

    public void Save()
    {
        ResourceSaveData data = new ResourceSaveData
            (Deck, Hp, Kor, tmpMhp, Area, Stage, money, LEVEL, Rules, VillageLevel, SupportPrice, jewel, combiRate, metSquad, shopcard, time);


        // ToJson을 사용하면 JSON형태로 포멧팅된 문자열이 생성된다  
        string jsonData = JsonUtility.ToJson(data);
        // 데이터를 저장할 경로 지정
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        // 파일 생성 및 저장
        File.WriteAllText(path, jsonData);
    }

    public ResourceSaveData Load()//로드    
    {
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        if (!File.Exists(path)) return null;
        ResourceSaveData data =null;

        // 파일의 텍스트를 string으로 저장
        string jsonData = File.ReadAllText(path);
        // 이 Json데이터를 역직렬화하여 playerData에 넣어줌
        data = JsonUtility.FromJson<ResourceSaveData>(jsonData);
        return data;    
    }
    public void LoadData()
    {
        var data = Load();
        if (data == null) return;
        Deck = new List<CardStruct>();
        for(int i=0;i<data.Deck.Count;i++)
        {
            CardStruct card;
            if(data.Deck_token[i])
                card = CardDatabase.Instance.card_token(data.Deck[i]);
            else card = CardDatabase.Instance.card(data.Deck[i]);

            Deck.Add(card);
        }
        this.Hp = data.Hp;
        this.Kor = data.Kor;
        this.tmpMhp = data.tmpMhp;
        this.Area = data.Area;
        this.Stage = data.Stage;
        this.money = data.money;
        this.LEVEL = data.LEVEL;
        this.Rules = data.Rules;
        this.VillageLevel = data.VillageLevel;
        this.SupportPrice = data.SupportPrice;
        this.jewel = data.jewel;
        this.combiRate = data.combiRate;
        this.metSquad = data.metSquad;
        this.shopcard = CardDatabase.Instance.card(data.shopcard);
        this.time = data.time;
        File.Delete(Path.Combine(Application.persistentDataPath, "playerData.json"));
    }

}
[Serializable]
public class ResourceSaveData
{
    public List<string> Deck;
    public List<bool> Deck_token;
    public int Hp;
    public bool Kor;
    public int tmpMhp;
    public int Area;
    public int Stage;
    public int money;
    public int LEVEL;
    public DictionaryOfStringAndInteger Rules;
    public DictionaryOfStringAndInteger VillageLevel;
    public DictionaryOfStringAndInteger SupportPrice;
    public int jewel;
    public List<float> combiRate;
    public List<EnemySquadStruct> metSquad;
    public string shopcard;
    public float time;

    public ResourceSaveData(List<CardStruct> deck, int hp, bool kor, int tmpMhp, int area, int stage, int money, int lEVEL, DictionaryOfStringAndInteger rules, DictionaryOfStringAndInteger villageLevel, DictionaryOfStringAndInteger supportPrice, int jewel, List<float> combiRate, List<EnemySquadStruct> metSquad, CardStruct shopcard, float time)
    {
        Deck = new List<string>();
        Deck_token = new List<bool>();
        foreach(CardStruct card in deck)
        {
            Deck.Add(card._name);
            Deck_token.Add(card._Token);
        }
        Hp = hp;
        Kor = kor;
        this.tmpMhp = tmpMhp;
        Area = area;
        Stage = stage;
        this.money = money;
        LEVEL = lEVEL;
        Rules = rules;
        VillageLevel = villageLevel;
        SupportPrice = supportPrice;
        this.jewel = jewel;
        this.combiRate = combiRate;
        this.metSquad = metSquad;
        this.shopcard = shopcard._name;
        this.time = time;
    }
}


[System.Serializable]
public class DictionaryOfStringAndInteger : AT.SerializableDictionary.SerializableDictionary<string, int> { }
