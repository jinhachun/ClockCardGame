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
    }
    [SerializeField] List<CardStruct> cardDatabase;
    [SerializeField] List<Sprite> cardinfoSprites;
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
        switch (c)
        {
            case SPECIES.HUMAN:
                return cardinfoSprites[0];
            case SPECIES.UNDEAD:
                return cardinfoSprites[1];
            case SPECIES.MONSTER:
                return cardinfoSprites[2];
            case SPECIES.MECH:
                return cardinfoSprites[3];
        }
        return null;
    }
    public Sprite typeSprite(TYPE r)
    {
        switch (r)
        {
            case TYPE.DARK:
                return cardinfoSprites[4];
            case TYPE.LIGHT:
                return cardinfoSprites[5];
        }
        return null;
    }
    public int SpeciesCombination(List<SPECIES> list)
    {
        var result = list.GroupBy(x => x).Where(g => g.Count() > 1).ToDictionary(x => x.Key, x => x.Count());
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
        if (combi == 0) return "Straight X10";
        else if (combi == 1) return "OnePair X1";
        else if (combi == 2) return "TwoPair X2";
        else if (combi == 3) return "Triple X3";
        else if (combi == 4) return "FullHouse X4";
        else if (combi == 5) return "FourCard X5";
        else return "FiveCard X10";
    }
    public int SpeciesCombiRate(List<SPECIES> list)
    {
        int combi = SpeciesCombination(list);
        if (combi == 0) return 10;
        else if (combi == 1) return 1;
        else if (combi == 2) return 2;
        else if (combi == 3) return 3;
        else if (combi == 4) return 4;
        else if (combi == 5) return 5;
        else return 10;

    }
    public int TypeCombination(List<TYPE> list)
    {
        var result = list.GroupBy(x => x).Where(g => g.Count() > 1).ToDictionary(x => x.Key, x => x.Count());
        int combi = 1;
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
        if (combi == 0) return "Straight X10";
        else if (combi == 1) return "OnePair X1";
        else if (combi == 2) return "TwoPair X2";
        else if (combi == 3) return "Triple X3";
        else if (combi == 4) return "FullHouse X4";
        else if (combi == 5) return "FourCard X5";
        else return "FiveCard X10";
    }
    public int TypeCombiRate(List<TYPE> list)
    {
        int combi = TypeCombination(list);
        if (combi == 0) return 10;
        else if (combi == 1) return 1;
        else if (combi == 2) return 2;
        else if (combi == 3) return 3;
        else if (combi == 4) return 4;
        else if (combi == 5) return 5;
        else return 10;

    }
    
    public CardAction CardActionFunc(string a)
    {
        switch (a)
        {
            case "가자꾸나 아우우":
                return (() => { 
                });
        }
        return (() => { });
    }
    public CardAction BeforeCardActionFunc(string a)
    {
        switch (a)
        {
            case "화아력!":
                return (() => {
                    BattleManager.Instance.tmpRate *= 1.2;
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
}
[Serializable]
public class STAT
{
    public int attack;
    public int defence;
}

public enum SPECIES
{
    HUMAN, UNDEAD, MONSTER, MECH
}
public enum TYPE
{
    NONE,FIRE,WATER,GRASS,LIGHT,DARK
}

public delegate void CardAction();