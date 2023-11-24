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
    [SerializeField] List<CardStruct> cardDatabase_token;
    [SerializeField] List<SpeciesSprite> cardinfoSprites_species;
    [SerializeField] List<TypeSprite> cardinfoSprites_type;
    [SerializeField] List<Sprite> btnSprites;
    
    public int RandomCardIndex => Random.Range(0, cardDatabase.Count);
    public CardStruct card(string a) => cardDatabase.Where(x => x._name.Equals(a)).FirstOrDefault();
    public CardStruct card_token(string a) => cardDatabase_token.Where(x => x._name.Equals(a)).FirstOrDefault();

    public CardStruct card(int a) => cardDatabase[a];
    public CardStruct RandomCard()
    {
        int a = RandomCardIndex;
        return card(a);
    }
    public List<CardStruct> cardByTierList(int a) => cardDatabase.Where(x => x._tier <= a).ToList();
    public CardStruct cardByTier(int a) => cardByTierList(a)[Random.Range(0,cardByTierList(a).Count)];
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
            case "Á»ºñ":
                return (() => {
                    BattleManager.Instance.Hp += 2;
                });
            case "¿ÕÁ»ºñ":
                return (() => {
                    BattleManager.Instance.Hp += 4;
                });
            case "¿Õ¿ÕÁ»ºñ":
                return (() => {
                    BattleManager.Instance.Hp += 4;
                });
            case "¸Ô±úºñ":
                return (() => {
                    BattleManager.Instance.takeDamage(1);
                });
            case "º¸µÎ¾Þµ¹ÀÌ":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        tmp.StatChange("Attack", tmp.Stat.attack + 1);
                    }
                });
            case "¿¹´ÏÃ¼µ¹ÀÌ":
                return (() => {
                    b.StatChange("Attack", b.Stat.attack+1);
                });
            case "¼úÅºµ¹ÀÌ":
                return (() => {
                    b.StatChange("Attack", b.Stat.attack + 2);
                });
            case "»ì¶óµòµ¹ÀÌ":
                return (() => {
                    b.StatChange("Attack", b.Stat.attack * 2);
                });
            case "¸®ÀÚµå":
                return (() => {
                    BattleManager.Instance.takeDamage(2);
                });
            case "Æ¼¶ó³ë":
                return (() => {
                    BattleManager.Instance.takeDamage(3);
                    if (BattleManager.Instance.Hp <= BattleManager.Instance.Mhp * 50 / 100) 
                        foreach (Enemy tmp in BattleManager.Instance.Enemies)
                         BattleManager.Instance.enemyDamage(5,false, tmp);
                });
            case "ºí·¢Æ¼¶ó³ë":
                return (() => {
                    BattleManager.Instance.takeDamage(5);
                    if (BattleManager.Instance.Hp <= BattleManager.Instance.Mhp * 50 / 100)
                        foreach (Enemy tmp in BattleManager.Instance.Enemies)
                        BattleManager.Instance.enemyDamage(10, false, tmp);
                });

            case "¹ö¼¸±úºñ":
                return (() => {
                    BattleManager.Instance.rerolladd(1,true);
                });
            case "Äõµå¹ö¼¸±úºñ":
                return (() => {
                    BattleManager.Instance.rerolladd(1,true);
                });
            case "¸Ôº¸±«¼ö":
                return (() => {
                    var target = BattleManager.Instance.targetEnemy.Count == 0 ? BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)] : BattleManager.Instance.targetEnemy[0];
                    BattleManager.Instance.enemyDamage(BattleManager.Instance.Def, false, target);
                });

            case "È²±Ý¸ó":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        if(tmp.name.Equals("È²±Ý¸ó"))
                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                    }
                    foreach (Card tmp in BattleManager.Instance.Grave)
                    {
                        if (tmp.name.Equals("È²±Ý¸ó"))
                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                    }
                });
            case "µ·º­¶ôµ¿±Û":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        if (tmp.name.Equals("È²±Ý¸ó"))

                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                            tmp.StatChange("Defence", tmp.Stat.defence + 1);
                        }
                    
                    foreach (Card tmp in BattleManager.Instance.Grave)
                    {
                        if (tmp.name.Equals("È²±Ý¸ó"))
                        {
                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                            tmp.StatChange("Defence", tmp.Stat.defence + 1);
                        }

                    }
                });
            case "¸¶ÀÌµ¿½º¿Õ":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("È²±Ý¸ó"));
                    queue.Enqueue(CardDatabase.instance.card("È²±Ý¸ó"));
                    BattleManager.Instance.AddCard(queue,true);
                });
        }
        return (() => { });
    }
    public CardAction BeforeCardActionFunc(Card b)
    {
        string a = b.name;
        switch (a)
        {
            case "±øÅë¸ó":
                return (() => {
                    b.StatChange("Attack", Random.Range(b.Stat.attack - 2, b.Stat.attack + 3));
                });
            case "±øÅëº¿":
                return (() => {
                    b.StatChange("Attack", Random.Range(b.Stat.attack - 2, b.Stat.attack + 3));
                });
            case "ÆÝÄ¡º¿":
                return (() => {
                    b.StatChange("Attack", Random.Range(b.Stat.attack - 3, b.Stat.attack + 4));
                });
            
            case "»ç½Å":
                return (() => {
                    if(BattleManager.Instance.tmpRate<1.2)
                        BattleManager.Instance.tmpRate = 1.2;
                });
            case "¸¶°è¿Õ":
                return (() => {
                    if (BattleManager.Instance.tmpRate < 1.5)
                        BattleManager.Instance.tmpRate = 1.5;
                });
            case "¹ö¼¸¼øÀÌ":
                return (() => {
                    BattleManager.Instance.rerolladd(1,false);
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
    [Multiline(3)]
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