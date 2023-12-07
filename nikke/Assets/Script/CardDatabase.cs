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
    public List<CardStruct> cardByTierList(int a) => cardDatabase.Where(x => x._tier == a).ToList();
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
            case "좀비":
                return (() => {
                    BattleManager.Instance.takeHeal(2);
                });
            case "왕좀비":
            case "왕왕좀비":
            case "초대형좀비":
                return (() => {
                    BattleManager.Instance.takeHeal(4);
                });
            case "보두앵돌이":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        tmp.StatChange("Attack", tmp.Stat.attack + 1);
                    }
                });
            case "예니체돌이":
                return (() => {
                    b.StatChange("Attack", b.Stat.attack+1);
                });
            case "술탄돌이":
                return (() => {
                    b.StatChange("Attack", b.Stat.attack + 2);
                });
            case "살라딘돌이":
                return (() => {
                    b.StatChange("Attack", b.Stat.attack * 2);
                });
            case "리자드":
                return (() => {
                    BattleManager.Instance.takeDamage(2);
                });
            case "티라노":
                return (() => {
                    BattleManager.Instance.takeDamage(3);
                    if (BattleManager.Instance.Hp <= BattleManager.Instance.Mhp * 50 / 100)
                    {
                        var tmpEnemyList = new List<Enemy>();
                        foreach (Enemy tmp in BattleManager.Instance.Enemies)
                            tmpEnemyList.Add(tmp);
                        foreach(Enemy tmp in tmpEnemyList)
                            BattleManager.Instance.enemyDamage(15, false, tmp);
                    }
                });
            case "블랙티라노":
                return (() => {
                    BattleManager.Instance.takeDamage(5);
                    if (BattleManager.Instance.Hp <= BattleManager.Instance.Mhp * 50 / 100)
                    {
                        var tmpEnemyList = new List<Enemy>();
                        foreach (Enemy tmp in BattleManager.Instance.Enemies)
                            tmpEnemyList.Add(tmp);
                        foreach (Enemy tmp in tmpEnemyList)
                            BattleManager.Instance.enemyDamage(40, false, tmp);
                    }
                });

            case "버섯깨비":
            case "쿼드버섯깨비":
                return (() => {
                    BattleManager.Instance.rerolladd(1,true);
                });
            case "먹보괴수":
                return (() => {
                    var target = BattleManager.Instance.targetEnemy.Count == 0 ? BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)] : BattleManager.Instance.targetEnemy[0];
                    BattleManager.Instance.enemyDamage(BattleManager.Instance.Def, false, target);
                });

            case "황금몬":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        if(tmp.name.Equals("황금몬"))
                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                    }
                    foreach (Card tmp in BattleManager.Instance.Grave)
                    {
                        if (tmp.name.Equals("황금몬"))
                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                    }
                });
            case "돈벼락동글":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("황금몬"));
                    BattleManager.Instance.AddCard(queue, false);
                });
            case "마이동스왕":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("황금몬"));
                    queue.Enqueue(CardDatabase.instance.card("황금몬"));
                    BattleManager.Instance.AddCard(queue,true);
                });
            case "삼두라":
            case "케르베로삼":
                return (() =>
                {
                    if (CardDatabase.Instance.SpeciesCombination(BattleManager.Instance.SpeciesCombi) == 3)
                        b.StatChange("Attack", b.Stat.attack * 2);
                });
            case "킹삼도라":
                return (() =>
                {
                    if (CardDatabase.Instance.SpeciesCombination(BattleManager.Instance.SpeciesCombi) == 3)
                        b.StatChange("Attack", b.Stat.attack * 3);
                });
            case "골목대장동글":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        if (tmp.name.Equals("동글이"))
                        {

                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                            tmp.StatChange("Defence", tmp.Stat.defence + 1);
                        }
                    }

                    foreach (Card tmp in BattleManager.Instance.Grave)
                    {
                        if (tmp.name.Equals("동글이"))
                        {
                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                            tmp.StatChange("Defence", tmp.Stat.defence + 1);
                        }

                    }
                });
            case "민병대동글":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        if (tmp.name.Equals("동글이"))
                        {

                            tmp.StatChange("Attack", tmp.Stat.attack + 2);
                            tmp.StatChange("Defence", tmp.Stat.defence + 2);
                        }
                    }

                    foreach (Card tmp in BattleManager.Instance.Grave)
                    {
                        if (tmp.name.Equals("동글이"))
                        {
                            tmp.StatChange("Attack", tmp.Stat.attack + 2);
                            tmp.StatChange("Defence", tmp.Stat.defence + 2);
                        }

                    }
                });
            case "동글의사당":
                return (() => {
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        if (tmp.name.Equals("동글이"))
                        {

                            tmp.StatChange("Attack", tmp.Stat.attack + 5);
                            tmp.StatChange("Defence", tmp.Stat.defence + 5);
                        }
                    }

                    foreach (Card tmp in BattleManager.Instance.Grave)
                    {
                        if (tmp.name.Equals("동글이"))
                        {
                            tmp.StatChange("Attack", tmp.Stat.attack + 5);
                            tmp.StatChange("Defence", tmp.Stat.defence + 5);
                        }

                    }
                });
            case "동챙이":
            case "동구리":
                return (() => {
                    for (int i = 0; i < 2; i++)
                    {
                        var a = BattleManager.Instance.Hand[Random.Range(0, BattleManager.Instance.Hand.Count)];
                        a.StatChange("Attack", a.Stat.attack + 1);
                        a.StatChange("Defence", a.Stat.defence + 1);
                    }
                });
            case "수륙양용동글":
                return (() => {
                    for (int i = 0; i < 2; i++)
                    {
                        var a = BattleManager.Instance.Hand[Random.Range(0, BattleManager.Instance.Hand.Count)];
                        a.StatChange("Attack", b.Stat.attack + a.Stat.attack);
                        a.StatChange("Defence", b.Stat.defence + a.Stat.defence);
                    }
                });
            case "동글네시":
                return (() => {
                    foreach (Card a in BattleManager.Instance.Hand)
                    {
                        a.StatChange("Attack", a.Stat.attack + 1);
                        a.StatChange("Defence", a.Stat.attack + 1);
                    }
                });
            case "샴동글이":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("샴동글이"));
                    BattleManager.Instance.AddCard(queue, false);
                });

            case "삼동글이":
            case "동글키메라":
                return (() => {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("삼동글이"));
                    queue.Enqueue(CardDatabase.instance.card("삼동글이"));
                    BattleManager.Instance.AddCard(queue, false);
                });

        }
        return (() => { });
    }
    public CardAction BeforeCardActionFunc(Card b)
    {
        string a = b.name;
        switch (a)
        {
            case "깡통몬":
            case "깡통봇":
                return (() => {
                    b.StatChange("Attack", Random.Range(b.Stat.attack - 2, b.Stat.attack + 3));
                });
            case "펀치봇":
                return (() => {
                    b.StatChange("Attack", Random.Range(b.Stat.attack - 3, b.Stat.attack + 4));
                });
            case "건당글이":
                return (() => {
                    b.StatChange("Attack", Random.Range(b.Stat.attack, b.Stat.attack + 6));
                });

            case "사신":
                return (() => {
                    if(BattleManager.Instance.tmpRate<1.2)
                        BattleManager.Instance.tmpRate = 1.2;
                });
            case "마계왕":
                return (() => {
                    if (BattleManager.Instance.tmpRate < 1.5)
                        BattleManager.Instance.tmpRate = 1.5;
                });
            case "버섯순이":
                return (() => {
                    BattleManager.Instance.rerolladd(1,false);
                });
            case "리빙아머":
                return (() => {
                    var target = BattleManager.Instance.Enemies.Count==0?null:BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                });
            case "듀라한":
                return (() => {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                });
            case "백작동글이":
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
            case "동글키메라":
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