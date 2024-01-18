using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
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
    [SerializeField] List<RuleStruct> ruleDatabase;
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
    public List<CardStruct> cardByTierList(int a) => cardDatabase.Where(x => x._tier == a).ToList();
    public List<CardStruct> cardByRareTierList(int a) => cardDatabase.Where(x =>x.isRare && x._tier == a).ToList();
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
        try
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (result.ContainsKey(list[i])) BattleManager.Instance.Hand[i].combiSpeices = true;
            }
        }
        catch (Exception e) { }
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
        for (int i = 0; i < list.Count; i++)
        {
            if (result.ContainsKey(list[i])) BattleManager.Instance.Hand[i].combiType = true;
        }
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
                    BattleManager.Instance.takeHeal(2);
                });
            case "¿ÕÁ»ºñ":
            case "¿Õ¿ÕÁ»ºñ":
            case "ÃÊ´ëÇüÁ»ºñ":
                return (() => {
                    BattleManager.Instance.takeHeal(4);
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
                    {
                        BattleManager.Instance.enemyWideDamage(15);
                    }
                });
            case "ºí·¢Æ¼¶ó³ë":
                return (() => {
                    BattleManager.Instance.takeDamage(5);
                    if (BattleManager.Instance.Hp <= BattleManager.Instance.Mhp * 50 / 100)
                    {
                        BattleManager.Instance.enemyWideDamage(40);
                    }
                });

            case "¹ö¼¸±úºñ":
            case "Äõµå¹ö¼¸±úºñ":
                return (() => {
                    BattleManager.Instance.rerolladd(1);
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
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("È²±Ý¸ó"));
                    BattleManager.Instance.AddCard(queue, false);
                });
            case "¸¶ÀÌµ¿½º¿Õ":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("È²±Ý¸ó"));
                    queue.Enqueue(CardDatabase.instance.card("È²±Ý¸ó"));
                    BattleManager.Instance.AddCard(queue,true);
                });
            case "»ïµÎ¶ó":
            case "ÄÉ¸£º£·Î»ï":
                return (() =>
                {
                    if (CardDatabase.Instance.SpeciesCombination(BattleManager.Instance.SpeciesCombi) == 3)
                        b.StatChange("Attack", b.Stat.attack * 2);
                });
            case "Å·»ïµµ¶ó":
                return (() =>
                {
                    if (CardDatabase.Instance.SpeciesCombination(BattleManager.Instance.SpeciesCombi) == 3)
                        b.StatChange("Attack", b.Stat.attack * 3);
                });
            case "°ñ¸ñ´ëÀåµ¿±Û":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        if (tmp.name.Equals("µ¿±ÛÀÌ"))
                        {

                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                            tmp.StatChange("Defence", tmp.Stat.defence + 1);
                        }
                    }

                    foreach (Card tmp in BattleManager.Instance.Grave)
                    {
                        if (tmp.name.Equals("µ¿±ÛÀÌ"))
                        {
                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                            tmp.StatChange("Defence", tmp.Stat.defence + 1);
                        }

                    }
                });
            case "¹Îº´´ëµ¿±Û":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        if (tmp.name.Equals("µ¿±ÛÀÌ"))
                        {

                            tmp.StatChange("Attack", tmp.Stat.attack + 2);
                            tmp.StatChange("Defence", tmp.Stat.defence + 2);
                        }
                    }

                    foreach (Card tmp in BattleManager.Instance.Grave)
                    {
                        if (tmp.name.Equals("µ¿±ÛÀÌ"))
                        {
                            tmp.StatChange("Attack", tmp.Stat.attack + 2);
                            tmp.StatChange("Defence", tmp.Stat.defence + 2);
                        }

                    }
                });
            case "µ¿±ÛÀÇ»ç´ç":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        if (tmp.name.Equals("µ¿±ÛÀÌ"))
                        {

                            tmp.StatChange("Attack", tmp.Stat.attack + 5);
                            tmp.StatChange("Defence", tmp.Stat.defence + 5);
                        }
                    }

                    foreach (Card tmp in BattleManager.Instance.Grave)
                    {
                        if (tmp.name.Equals("µ¿±ÛÀÌ"))
                        {
                            tmp.StatChange("Attack", tmp.Stat.attack + 5);
                            tmp.StatChange("Defence", tmp.Stat.defence + 5);
                        }

                    }
                });
            case "µ¿Ã¬ÀÌ":
            case "µ¿±¸¸®":
                return (() => {
                    for (int i = 0; i < 2; i++)
                    {
                        var a = BattleManager.Instance.Hand[Random.Range(0, BattleManager.Instance.Hand.Count)];
                        a.StatChange("Attack", a.Stat.attack + 1);
                        a.StatChange("Defence", a.Stat.defence + 1);
                    }
                });
            case "¼ö·ú¾ç¿ëµ¿±Û":
                return (() => {
                    for (int i = 0; i < 2; i++)
                    {
                        var a = BattleManager.Instance.Hand[Random.Range(0, BattleManager.Instance.Hand.Count)];
                        a.StatChange("Attack", b.Stat.attack + a.Stat.attack);
                        a.StatChange("Defence", b.Stat.defence + a.Stat.defence);
                    }
                });
            case "µ¿±Û³×½Ã":
                return (() => {
                    foreach (Card a in BattleManager.Instance.Hand)
                    {
                        a.StatChange("Attack", a.Stat.attack + 1);
                        a.StatChange("Defence", a.Stat.defence + 1);
                    }
                });
            case "¼¤µ¿±ÛÀÌ":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("¼¤µ¿±ÛÀÌ"));
                    BattleManager.Instance.AddCard(queue, false);
                });

            case "»ïµ¿±ÛÀÌ":
            case "µ¿±ÛÅ°¸Þ¶ó":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("»ïµ¿±ÛÀÌ"));
                    queue.Enqueue(CardDatabase.instance.card("»ïµ¿±ÛÀÌ"));
                    BattleManager.Instance.AddCard(queue, false);
                });
            case "º½±ÛÀÌ":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card_token("ÆøÅº"));
                    BattleManager.Instance.AddCard(queue, true);
                });
            case "ÆøÅº":
                return (() => {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(30, false, target);
                });
            case "µ¿±Û³Ê¸¶ÀÌÆ®":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card_token("´ÙÀÌ³Ê¸¶ÀÌÆ®"));
                    BattleManager.Instance.AddCard(queue, true);
                });
            case "´ÙÀÌ³Ê¸¶ÀÌÆ®":
                return (() => {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(60, false, target);
                });
            case "ÇÙµ¿±Û":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card_token("ÇÙÆøÅº"));
                    BattleManager.Instance.AddCard(queue, true);
                });
            case "ÇÙÆøÅº":
                return (() => {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(120, false, target);
                });
            case "À¯·É":
            case "ÆúÅÍ°¡ÀÌ½ºÆ®":
                return (() => {
                    var cardList = BattleManager.Instance.Grave.Where(x => x.Str._species == SPECIES.UNDEAD).ToList();
                    var card = cardList.Count==0?null:cardList[Random.Range(0, cardList.Count)];
                    BattleManager.Instance.moveCardGraveToDeck(card);
                });
            case "Áö´Ï":
                return (() => {
                    var cardList = BattleManager.Instance.Grave.Where(x => x.Str._species == SPECIES.UNDEAD).ToList();
                    int loopCnt = 0;
                    while (cardList.Count > 0 && ++loopCnt <=1000)
                    {
                        cardList = BattleManager.Instance.Grave.Where(x => x.Str._species == SPECIES.UNDEAD).ToList();
                        var card = cardList.Count == 0 ? null : cardList[Random.Range(0, cardList.Count)];
                        BattleManager.Instance.moveCardGraveToDeck(card);
                    }

                });
            case "½½·Ô¸Ó½Å":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(RandomCard());
                    BattleManager.Instance.AddCard(queue, true);
                });
            case "½½·Ô¸Ó½Å+":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    var tier = Random.Range(3, 6);
                    queue.Enqueue(cardByTier(tier));
                    BattleManager.Instance.AddCard(queue, true);
                });
            case "½½·Ô¸Ó½Å++":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    var tier = 5;
                    queue.Enqueue(cardByTier(tier));
                    BattleManager.Instance.AddCard(queue, true);
                });

                /////////////////////////////////////////////////
            case "ÀúÁÖ":
                return (() => {
                    var tmpList = BattleManager.Instance.Hand.Where(x => !(x.Stat.attack == 0 && x.Stat.defence == 0)).ToList();
                    if (tmpList.Count == 0) return;
                    var tmpCard = tmpList[Random.Range(0, tmpList.Count)];
                    tmpCard.StatChange("Attack", tmpCard.Stat.attack -4);
                    tmpCard.StatChange("Defence", tmpCard.Stat.defence -4);
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
            case "±øÅëº¿":
                return (() => {
                    b.StatChange("Attack", Random.Range(b.Stat.attack - 2, b.Stat.attack + 3));
                });
            case "ÆÝÄ¡º¿":
                return (() => {
                    b.StatChange("Attack", Random.Range(b.Stat.attack - 3, b.Stat.attack + 4));
                });
            case "°Ç´ç±ÛÀÌ":
                return (() => {
                    b.StatChange("Attack", Random.Range(b.Stat.attack, b.Stat.attack + 6));
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
                    BattleManager.Instance.rerolladd(1);
                });
            case "¸®ºù¾Æ¸Ó":
                return (() => {
                    var target = BattleManager.Instance.Enemies.Count==0?null:BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                });
            case "µà¶óÇÑ":
                return (() => {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                });
            case "¹éÀÛµ¿±ÛÀÌ":
                return (() => {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                });
            case "µ¿±ÛÅ°¸Þ¶ó":
                return (() => {
                    b.StatChange("Attack", BattleManager.Instance.Grave.Count);
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
    public bool isRare;
}
[Serializable]
public struct RuleStruct
{
    public Sprite _img;
    public string _name;
    [Multiline(3)]
    public string _text;
    public int _level;
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