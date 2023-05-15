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
        Debug.Log(a);
        return card(a);
    }
    public Sprite btn(int i) => btnSprites[i];
    public Sprite companySprite(COMPANY c)
    {
        switch (c)
        {
            case COMPANY.ELISION:
                return cardinfoSprites[0];
            case COMPANY.MISSILIS:
                return cardinfoSprites[1];
            case COMPANY.TETRA:
                return cardinfoSprites[2];
            case COMPANY.PILGRIM:
                return cardinfoSprites[3];
        }
        return null;
    }
    public Sprite rankSprite(RANK r)
    {
        switch (r)
        {
            case RANK.R:
                return cardinfoSprites[4];
            case RANK.SR:
                return cardinfoSprites[5];
            case RANK.SSR:
                return cardinfoSprites[6];
        }
        return null;
    }
    public int NumCombination(List<int> list)
    {
        var result = list.GroupBy(x => x).Where(g => g.Count() > 1).ToDictionary(x => x.Key, x => x.Count());
        int combi=0;
        foreach(var num in result)
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
    public string NumCombinationText(List<int> list)
    {
        int combi = NumCombination(list);
        if (combi == 0) return "Straight X10";
        else if (combi == 1) return "OnePair X1";
        else if (combi == 2) return "TwoPair X2";
        else if (combi == 3) return "Triple X3";
        else if (combi == 4) return "FullHouse X4";
        else if (combi == 5) return "FourCard X5";
        else return "FiveCard X10";
    }
    public int ComCombination(List<COMPANY> list)
    {
        var result = list.GroupBy(x => x).Where(g => g.Count() > 1).ToDictionary(x => x.Key, x => x.Count());
        int combi = 0;
        foreach (var num in result)if (num.Value == 5) combi = 5;
        return combi;
    }
    public string ComCombinationText(List<COMPANY> list) => ComCombination(list) == 0 ? "" : "FLUSH X2";
}
[Serializable]
public struct CardStruct
{
    public Sprite _img;
    public COMPANY _company;
    public string _name;
    public string _text;
    public int _number;
    public int _value;
    public CARDTYPE _type;
    public RANK _rank;
    public bool isExhaust;
    public bool isEthereal;
    public bool isFixed;
}
public enum COMPANY
{
    ELISION, MISSILIS, TETRA, PILGRIM
}
public enum CARDTYPE
{
    ATT, DEF, UTIL
}
public enum RANK
{
    R, SR, SSR
}
