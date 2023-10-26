using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class CardDatabase : MonoBehaviour
{
    private static CardDatabase instance;
    public static CardDatabase Instance => instance;
    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    [SerializeField] List<CardStruct> cardDatabase;
    [SerializeField] List<SpeciesSprite> cardinfoSprites_species;
    [SerializeField] List<TypeSprite> cardinfoSprites_type;
    [SerializeField] List<Sprite> btnSprites;
    
    public int RandomCardIndex => Random.Range(0, cardDatabase.Count);
    public CardStruct card(string a) => cardDatabase.Where(x => x._name.Equals(a)).FirstOrDefault();

    public CardStruct card(int a) => cardDatabase[a];
    public CardStruct RandomCard()
    {
        int a = RandomCardIndex;
        return card(a);
    }
    public Sprite btn(int i) => btnSprites[i];
    public Sprite speciesSprite(SPECIES c)
    {
        return cardinfoSprites_species.Find(g => g.species == c).sprite;
        
    }
    public Sprite typeSprite(TYPE r)
    {
        return cardinfoSprites_type.Find(g => g.species == r).sprite;
    }
    public int SpeciesCombination(List<SPECIES> list)
    {
        var result = list.GroupBy(x => x).Where(g => g.Key!=SPECIES.NONE && g.Count() > 1).ToDictionary(x => x.Key, x => x.Count());
        int combi = 0;
        foreach (var num in result)
        {
            if (combi < 1 && num.Value == 2) combi = 1;
            else if (combi == 1 && num.Value == 2) combi = 2;
            else if (combi == 3 && num.Value == 2 || combi == 1 && num.Value == 3) combi = 4;
            else if (combi < 3 && num.Value == 3) combi = 3;
            else if (combi < 5 && num.Value == 4) combi = 5;
            else if (num.Value == 5) combi = 6;
        }
        return combi;
    }
    public string SpeciesCombinationText(List<SPECIES> list)
    {
        int combi = SpeciesCombination(list);
        if (combi == 0) return " X"+Resource.Instance.combiRate[0];
        else if (combi == 1) return "OnePair X"+Resource.Instance.combiRate[1];
        else if (combi == 2) return "TwoPair X"+Resource.Instance.combiRate[2];
        else if (combi == 3) return "Triple X" + Resource.Instance.combiRate[3];
        else if (combi == 4) return "FullHouse X" + Resource.Instance.combiRate[4];
        else if (combi == 5) return "FourCard X" + Resource.Instance.combiRate[5];
        else return "FiveCard X" + Resource.Instance.combiRate[6];
    }
    public float SpeciesCombiRate(List<SPECIES> list)
    {
        int combi = SpeciesCombination(list);
        if (combi == 0) return Resource.Instance.combiRate[0];
        else if (combi == 1) return Resource.Instance.combiRate[1];
        else if (combi == 2) return Resource.Instance.combiRate[2];
        else if (combi == 3) return Resource.Instance.combiRate[3];
        else if (combi == 4) return Resource.Instance.combiRate[4];
        else if (combi == 5) return Resource.Instance.combiRate[5];
        else return Resource.Instance.combiRate[6];

    }
    public int TypeCombination(List<TYPE> list)
    {
        var result = list.GroupBy(x => x).Where(g =>g.Key!=TYPE.NONE && g.Count() > 1).ToDictionary(x => x.Key, x => x.Count());
        int combi = 0;
        foreach (var num in result)
        {
            if (combi < 1 && num.Value == 2) combi = 1;
            else if (combi == 1 && num.Value == 2) combi = 2;
            else if (combi == 3 && num.Value == 2 || combi == 1 && num.Value == 3) combi = 4;
            else if (combi < 3 && num.Value == 3) combi = 3;
            else if (combi < 5 && num.Value == 4) combi = 5;
            else if (num.Value == 5) combi = 6;
        }
        return combi;
    }
    public string TypeCombinationText(List<TYPE> list)
    {
        int combi = TypeCombination(list);
        if (combi == 0) return " X1";
        else if (combi == 1) return "OnePair X1.2";
        else if (combi == 2) return "TwoPair X1.4";
        else if (combi == 3) return "Triple X1.6";
        else if (combi == 4) return "FullHouse X1.8";
        else if (combi == 5) return "FourCard X2";
        else return "FiveCard X3";
    }
    public float TypeCombiRate(List<TYPE> list)
    {
        int combi = TypeCombination(list);
        if (combi == 0) return 1f;
        else if (combi == 1) return 1.2f;
        else if (combi == 2) return 1.4f;
        else if (combi == 3) return 1.6f;
        else if (combi == 4) return 1.8f;
        else if (combi == 5) return 2;
        else return 3;

    }
    
    public CardAction CardActionFunc(Card b)
    {
        string a = b.name;
        switch (a)
        {
            case "����":
                return (() => {
                    BattleManager.Instance.Hp += 2;
                });
            case "������":
                return (() => {
                    BattleManager.Instance.Hp += 4;
                });
            case "�տ�����":
                return (() => {
                    BattleManager.Instance.Hp += 4;
                });
            case "�Ա���":
                return (() => {
                    BattleManager.Instance.Hp -= 1;
                });
            case "���ξ޵���":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        tmp.StatChange("Attack", tmp.Stat.attack + 1);
                    }
                });
            case "����ü����":
                return (() => {
                    b.StatChange("Attack", b.Stat.attack+1);
                });
            case "��ź����":
                return (() => {
                    b.StatChange("Attack", b.Stat.attack + 2);
                });
            case "������":
                return (() => {
                    b.StatChange("Attack", b.Stat.attack * 2);
                });
        }
        return (() => { });
    }
    public CardAction BeforeCardActionFunc(Card b)
    {
        string a = b.name;
        switch (a)
        {
            case "�����":
                return (() => {
                    b.StatChange("Attack", Random.Range(b.Stat.attack - 2, b.Stat.attack + 3));
                });
            case "���":
                return (() => {
                    if(BattleManager.Instance.tmpRate<1.2)
                        BattleManager.Instance.tmpRate = 1.2;
                });
            case "�����":
                return (() => {
                    if (BattleManager.Instance.tmpRate < 1.5)
                        BattleManager.Instance.tmpRate = 1.5;
                });
        }
        return (() => { });
    }
}
[Serializable]
public struct CardStruct
{
    public Sprite _img;
    public SPECIES _species;
    public string _name;
    public string _text;
    public int _tier;
    public STAT _stat;
    public TYPE _type;
    public bool isExhaust;
    public bool isEthereal;
    public bool isFixed;
    public List<String> evol;
}
[Serializable]
public class STAT
{
    public int attack;
    public int defence;
}
[Serializable]
public struct SpeciesSprite
{
    public SPECIES species;
    public Sprite sprite;
}
[Serializable]
public struct TypeSprite
{
    public TYPE species;
    public Sprite sprite;
}
public enum SPECIES
{
    NONE, HUMAN, UNDEAD, MONSTER, MECH
}
public enum TYPE
{
    NONE,FIRE,WATER,GRASS,LIGHT,DARK
}

public delegate void CardAction();