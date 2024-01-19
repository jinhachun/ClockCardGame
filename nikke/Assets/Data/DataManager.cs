using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public TextAsset data;
    static private AllData datas;
    static public string RuleName(int no,bool Kor) => Kor?datas.Rule[no].RuleName_KOR:datas.Rule[no].RuleName_ENG;
    static public string RuleInfo(int no, bool Kor) => Kor ? datas.Rule[no].RuleInfo_KOR : datas.Rule[no].RuleInfo_ENG;
    static public string CardName(int no, bool Kor,bool Token) => Kor ? (Token? datas.TokenCard[no].CardName_KOR : datas.Card[no].CardName_KOR) : (Token ? datas.TokenCard[no].CardName_ENG : datas.Card[no].CardName_ENG);
    static public string CardInfo(int no, bool Kor,bool Token) => Kor ? (Token? datas.TokenCard[no].CardInfo_KOR : datas.Card[no].CardInfo_KOR) : (Token? datas.TokenCard[no].CardInfo_ENG : datas.Card[no].CardInfo_ENG);
    void Awake()
    {
        datas = JsonUtility.FromJson<AllData>(data.text);
    }

    void Update()
    {
        
    }
}
[System.Serializable]
public class AllData
{
    public RuleData[] Rule;
    public CardData[] Card;
    public CardData[] TokenCard;

}

[System.Serializable]
public class RuleData
{
    public int RuleNum;
    public string RuleName_KOR;
    public string RuleInfo_KOR;
    public string RuleName_ENG;
    public string RuleInfo_ENG;
}
[System.Serializable]
public class CardData
{
    public int Number;
    public string CardName_KOR;
    public string CardInfo_KOR;
    public string CardName_ENG;
    public string CardInfo_ENG;
}
