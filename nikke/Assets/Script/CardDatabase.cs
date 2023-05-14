using System.Collections;
using System.Collections.Generic;
using System;
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
        switch (c){
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
    
}
[Serializable]
public struct CardStruct{
    public Sprite _img;
    public COMPANY _company;
    public string _name;
    public string _text;
    public int _number;
    public int _value;
    public CARDTYPE _type;
    public RANK _rank;
}
public enum COMPANY
{
    ELISION,MISSILIS,TETRA,PILGRIM
}
public enum CARDTYPE
{
    ATT,DEF,UTIL
}
public enum RANK
{
    R,SR,SSR
}
