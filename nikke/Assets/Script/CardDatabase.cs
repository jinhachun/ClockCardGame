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
    [SerializeField] public List<CardStruct> cardDatabase;
    [SerializeField] public List<CardStruct> cardDatabase_token;
    [SerializeField] public List<RuleStruct> ruleDatabase;
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
    public List<CardStruct> cardByRareTierList(int a) => cardDatabase.Where(x => x.isRare && x._tier == a).ToList();
    public List<CardStruct> cardByUltimate => cardDatabase.Where(x => x.isUltimate).ToList();
    public CardStruct cardByTier(int a) => cardByTierList(a)[Random.Range(0, cardByTierList(a).Count)];
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
        var result = list.GroupBy(x => x).Where(g => g.Key != SPECIES.NONE && g.Count() > 1).ToDictionary(x => x.Key, x => x.Count());
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
        if (combi == 0) return "No Pair";
        else if (combi == 1) return "One Pair";
        else if (combi == 2) return "Two Pair";
        else if (combi == 3) return "Triple";
        else if (combi == 4) return "Full House";
        else if (combi == 5) return "Four Card";
        else return "Five Card";
    }
    public float SpeciesCombiRate(int combi)
    {
        if (combi == 0) return Resource.Instance.combiRate_Species[0];
        else if (combi == 1) return Resource.Instance.combiRate_Species[1];
        else if (combi == 2) return Resource.Instance.combiRate_Species[2];
        else if (combi == 3) return Resource.Instance.combiRate_Species[3];
        else if (combi == 4) return Resource.Instance.combiRate_Species[4];
        else if (combi == 5) return Resource.Instance.combiRate_Species[5];
        else return Resource.Instance.combiRate_Species[6];

    }
    public float SpeciesCombiRate(List<SPECIES> list)
    {
        int combi = SpeciesCombination(list);
        return SpeciesCombiRate(combi);
    }
    public int TypeCombination(List<TYPE> list)
    {
        var result = list.GroupBy(x => x).Where(g => g.Key != TYPE.NONE && g.Count() > 1).ToDictionary(x => x.Key, x => x.Count());
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
    public bool Rule_no5_0()
    {
        int cnt_card5tier = Resource.Instance.Deck.Where(x => x._tier >= 5).Count();
        return cnt_card5tier >= 5;
    }
    public string TypeCombinationText(List<TYPE> list)
    {
        int combi = TypeCombination(list);
        if (combi == 0) return "No Pair";
        else if (combi == 1) return "One Pair";
        else if (combi == 2) return "Two Pair";
        else if (combi == 3) return "Triple";
        else if (combi == 4) return "Full House";
        else if (combi == 5) return "Four Card";
        else return "Five Card";
    }
    public float TypeCombiRate(int combi)
    {
        float rateAdd = 0f;
        if (Resource.Instance.Rule_no(5))
        {
            bool cnt_card5tier = Rule_no5_0();
            if (cnt_card5tier) rateAdd += 0.3f;
            else return 1f;
        }
        if (combi == 0) return Resource.Instance.combiRate_Type[0];
        else if (combi == 1) return Resource.Instance.combiRate_Type[1] + rateAdd;
        else if (combi == 2) return Resource.Instance.combiRate_Type[2] + rateAdd;
        else if (combi == 3) return Resource.Instance.combiRate_Type[3] + rateAdd;
        else if (combi == 4) return Resource.Instance.combiRate_Type[4] + rateAdd;
        else if (combi == 5) return Resource.Instance.combiRate_Type[5] + rateAdd;
        else return Resource.Instance.combiRate_Type[6] + rateAdd;

    }
    public float TypeCombiRate(List<TYPE> list)
    {
        int combi = TypeCombination(list);
        return TypeCombiRate(combi);
    }

    public bool CardActionFunc(Card b)
    {
        string a = b.name;
        switch (a)
        {
            case "좀비":
                {
                    BattleManager.Instance.takeHeal(2);
                    return true;
                }
            case "왕좀비":
            case "왕왕좀비":
            case "초대형좀비":
                {
                    BattleManager.Instance.takeHeal(4);
                    return true;
                }
            case "보두앵돌이":
                {
                    foreach (Card tmp in BattleManager.Instance.Grave)
                    {
                        tmp.StatChange("Attack", tmp.Stat.attack + 2);
                    }
                    foreach (Card tmp in BattleManager.Instance.Deck)
                    {
                        tmp.StatChange("Attack", tmp.Stat.attack + 2);
                    }
                    return true;
                }
            case "예니체돌이":
                {
                    b.StatChange("Attack", b.Stat.attack + 1);
                    return true;
                }
            case "술탄돌이":
                {
                    b.StatChange("Attack", b.Stat.attack + 2);
                    return true;
                }
            case "살라딘돌이":
                {
                    b.StatChange("Attack", b.Stat.attack * 2);
                    return true;
                }
            case "리자드":
                {
                    BattleManager.Instance.takeDamage(2);
                    return true;
                }
            case "티라노":
                {
                    BattleManager.Instance.takeDamage(2);
                    BattleManager.Instance.enemyWideDamage(5);
                    return true;
                }
            case "블랙티라노":
                {
                    BattleManager.Instance.takeDamage(2);
                    BattleManager.Instance.enemyWideDamage(25);
                    return true;
                }

            case "버섯깨비":
            case "쿼드버섯깨비":
                {
                    BattleManager.Instance.AddRerollChance();
                    return true;
                }
            case "먹보괴수":
                {
                    if (BattleManager.Instance.Enemies.Count <= 0) return false ;
                    var target = BattleManager.Instance.targetEnemy.Count == 0 ? BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)] : BattleManager.Instance.targetEnemy[0];
                    BattleManager.Instance.enemyDamage(BattleManager.Instance.Def, false, target);
                    return true;
                }

            case "황금몬":
                {
                    foreach (Card tmp in BattleManager.Instance.AllCards)
                    {
                        if (tmp.name.Equals("황금몬"))
                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                    }
                    return true;
                }
            case "돈벼락동글":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("황금몬"));
                    BattleManager.Instance.AddCard(queue, false);
                    return true;
                }
            case "마이동스왕":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("황금몬"));
                    queue.Enqueue(CardDatabase.instance.card("황금몬"));
                    BattleManager.Instance.AddCard(queue, true);
                    return true;
                }
            case "삼두라":
            case "케르베로삼":

                {
                    if (CardDatabase.Instance.SpeciesCombination(BattleManager.Instance.SpeciesCombi) == 3)
                    {
                        foreach (Card card in BattleManager.Instance.AllCards)
                        {
                            card.StatChange("Attack", card.Stat.attack + 1);
                            card.StatChange("Defence", card.Stat.defence + 1);
                        }
                    }
                    return true;
                }
            case "킹삼도라":
                {
                    if (CardDatabase.Instance.SpeciesCombination(BattleManager.Instance.SpeciesCombi) == 3)
                    {
                        foreach (Card card in BattleManager.Instance.AllCards)
                        {
                            card.StatChange("Attack", card.Stat.attack + 3);
                            card.StatChange("Defence", card.Stat.defence + 3);
                        }
                    }
                    return true;
                }
            case "골목대장동글":
                {
                    foreach (Card tmp in BattleManager.Instance.AllCards)
                    {
                        if (tmp.name.Equals("동글이"))
                        {

                            tmp.StatChange("Attack", tmp.Stat.attack + 1);
                            tmp.StatChange("Defence", tmp.Stat.defence + 1);
                        }
                    }
                    return true;
                }
            case "민병대동글":
                {
                    foreach (Card tmp in BattleManager.Instance.AllCards)
                    {
                        if (tmp.name.Equals("동글이"))
                        {

                            tmp.StatChange("Attack", tmp.Stat.attack + 2);
                            tmp.StatChange("Defence", tmp.Stat.defence + 2);
                        }
                    }
                    return true;
                }
            case "동글의사당":
                {
                    foreach (Card tmp in BattleManager.Instance.AllCards)
                    {
                        if (tmp.name.Equals("동글이"))
                        {

                            tmp.StatChange("Attack", tmp.Stat.attack + 5);
                            tmp.StatChange("Defence", tmp.Stat.defence + 5);
                        }
                    }
                    return true;
                }
            case "동챙이":
            case "동구리":
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Card tmpHandCard = BattleManager.Instance.Hand[Random.Range(0, BattleManager.Instance.Hand.Count)];
                        tmpHandCard.StatChange("Attack", tmpHandCard.Stat.attack + 1);
                        tmpHandCard.StatChange("Defence", tmpHandCard.Stat.defence + 1);
                    }
                    return true;
                }
            case "수륙양용동글":
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Card tmpHandCard = BattleManager.Instance.Hand[Random.Range(0, BattleManager.Instance.Hand.Count)];
                        tmpHandCard.StatChange("Attack", b.Stat.attack + tmpHandCard.Stat.attack);
                        tmpHandCard.StatChange("Defence", b.Stat.defence + tmpHandCard.Stat.defence);
                    }
                    return true;
                }
            case "동글네시":
                {
                    foreach (Card tmpHandCard in BattleManager.Instance.Hand)
                    {
                        tmpHandCard.StatChange("Attack", tmpHandCard.Stat.attack + 1);
                        tmpHandCard.StatChange("Defence", tmpHandCard.Stat.defence + 1);
                    }
                    return true;
                }
            case "샴동글이":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("샴동글이"));
                    queue.Enqueue(CardDatabase.instance.card("샴동글이"));
                    BattleManager.Instance.AddCard(queue, false);
                    return true;
                }

            case "삼동글이":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("삼동글이"));
                    queue.Enqueue(CardDatabase.instance.card("삼동글이"));
                    BattleManager.Instance.AddCard(queue, false);
                    return true;
                }
            case "동글키메라":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card("동글키메라"));
                    queue.Enqueue(CardDatabase.instance.card("동글키메라"));
                    queue.Enqueue(CardDatabase.instance.card("동글키메라"));
                    BattleManager.Instance.AddCard(queue, false);
                    return true;
                }
            case "봄글이":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card_token("폭탄"));
                    BattleManager.Instance.AddCard(queue, true);
                    return true;
                }
            case "폭탄":
                {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(30, false, target);
                    return true;
                }
            case "동글너마이트":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card_token("다이너마이트"));
                    BattleManager.Instance.AddCard(queue, true);
                    return true;
                }
            case "다이너마이트":
                {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(60, false, target);
                    return true;
                }
            case "핵동글":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.instance.card_token("핵폭탄"));
                    BattleManager.Instance.AddCard(queue, true);
                    return true;
                }
            case "핵폭탄":
                {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(120, false, target);
                    return true;
                }
            case "유령":
            case "폴터가이스트":
                {
                    var cardList = BattleManager.Instance.Grave.Where(x => x.Str._species == SPECIES.UNDEAD).ToList();
                    var card = cardList.Count == 0 ? null : cardList[Random.Range(0, cardList.Count)];
                    BattleManager.Instance.moveCardGraveToDeck(card);
                    return true;
                }
            case "지니":
                {
                    var cardList = BattleManager.Instance.Grave.Where(x => x.Str._species == SPECIES.UNDEAD).ToList();
                    int loopCnt = 0;
                    while (cardList.Count > 0 && ++loopCnt <= 1000)
                    {
                        cardList = BattleManager.Instance.Grave.Where(x => x.Str._species == SPECIES.UNDEAD).ToList();
                        var card = cardList.Count == 0 ? null : cardList[Random.Range(0, cardList.Count)];
                        BattleManager.Instance.moveCardGraveToDeck(card);
                    }

                    return true;
                }
            case "슬롯머신":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(RandomCard());
                    BattleManager.Instance.AddCard(queue, true);
                    return true;
                }
            case "슬롯머신+":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    var tier = Random.Range(3, 6);
                    queue.Enqueue(cardByTier(tier));
                    BattleManager.Instance.AddCard(queue, true);
                    return true;
                }
            case "슬롯머신++":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    var tier = 5;
                    queue.Enqueue(cardByTier(tier));
                    BattleManager.Instance.AddCard(queue, true);
                    return true;
                }
            case "천사동글이":
                {
                    foreach (Card card in BattleManager.Instance.AllCards)
                    {
                        card.StatChange("Attack", card.Stat.attack + 2);
                        card.StatChange("Defence", card.Stat.defence + 2);
                    }
                    return true;
                }
            case "불가사리":
                {
                    var deleteTarget = BattleManager.Instance.Deck.Count>0?BattleManager.Instance.Deck[0]:null;
                    if (deleteTarget != null)
                    {
                        BattleManager.Instance.Deck.RemoveAt(0);
                        b.StatChange("Attack", b.Stat.attack + deleteTarget.Stat.attack);
                        b.StatChange("Defence", b.Stat.defence + deleteTarget.Stat.defence);
                        deleteTarget.deleteCard(1f);
                    }

                    deleteTarget = BattleManager.Instance.Grave.Count > 0 ? BattleManager.Instance.Grave[0] : null;
                    if (deleteTarget != null)
                    {
                        BattleManager.Instance.Grave.RemoveAt(0);
                    b.StatChange("Attack", b.Stat.attack + deleteTarget.Stat.attack);
                    b.StatChange("Defence", b.Stat.defence + deleteTarget.Stat.defence);
                    deleteTarget.deleteCard(1f);
                    }

                return true;
                }
            case "슈퍼컴퓨터":
                {
                    BattleManager.Instance.AddRerollChance(5);
                    return true;
                }
            case "동글라미드":
                {
                    b.StatChange("Defence", b.Stat.defence + 4);
                    return true;
                }
            case "엘동글라도":
                {
                    b.StatChange("Defence", b.Stat.defence + 6);
                    if(b.Stat.attack<20 && b.Stat.defence>=20)
                        b.StatChange("Attack", 20);

                    return true;
                }
            case "마왕동글이":
                {
                    foreach (Card card in BattleManager.Instance.AllCards)
                    {
                        if (card.Type == TYPE.DARK)
                        {
                            card.StatChange("Attack", card.Stat.attack + 3);
                            card.StatChange("Defence", card.Stat.defence + 3);
                        }
                    }
                    return true;
                }
            case "네크로동글":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    var tmpGrave = new List<Card>();
                    for (int i = 0; i < BattleManager.Instance.Grave.Count; i++)
                    {
                        tmpGrave.Add(BattleManager.Instance.Grave[i]);
                    }
                    foreach(Card card in tmpGrave)
                    {
                        if (card.tier <= 2)
                        {
                            var deleteTarget = card;
                            BattleManager.Instance.Grave.Remove(card);
                            var addTarget = cardDatabase.Where(x => x._species == SPECIES.UNDEAD).ToList().OrderBy(x => Random.value).FirstOrDefault();
                            queue.Enqueue(addTarget);
                            deleteTarget.deleteCard(1f);
                        }
                    }
                    if (queue.Count > 0)
                        BattleManager.Instance.AddCard(queue, false);
                    else return false;


                    return true;
                }
            case "저주인형":
                {
                    foreach (Enemy target in BattleManager.Instance.Enemies)
                    {
                        target.setAttackDebuff(0);
                        target.SetPatternText();
                    }
                    return true;
                }
            case "피노키오":
                {

                    foreach (Enemy target in BattleManager.Instance.Enemies)
                    {
                        target.setAttackDebuff(0);
                        target.lossShield(target._shield);
                        target.SetPatternText();
                    }
                    return true;
                }
            case "동글팩토리":
                {
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    for (int i = 0; i < BattleManager.Instance.Deck.Count; i++)
                    {
                        if (BattleManager.Instance.Deck[i].Str._Token)
                        {
                            var deleteTarget = BattleManager.Instance.Deck[i];
                            BattleManager.Instance.Deck.RemoveAt(i);
                            var addTarget = cardDatabase.Where(x => x._species == SPECIES.MECH && x._tier==5).ToList().OrderBy(x => Random.value).FirstOrDefault();
                            queue.Enqueue(addTarget);
                            deleteTarget.deleteCard(1f);
                            BattleManager.Instance.AddCard(queue, true);
                            return true;
                        }
                    }
                    return false;
                }


            /////////////////////////////////////////////////
            case "저주":
                {
                    var tmpList = BattleManager.Instance.Hand.Where(x => !(x.Stat.attack == 0 && x.Stat.defence == 0)).ToList();
                    if (tmpList.Count == 0) return true;
                    var tmpCard = tmpList[Random.Range(0, tmpList.Count)];
                    tmpCard.StatChange("Attack", tmpCard.Stat.attack - 3);
                    tmpCard.StatChange("Defence", tmpCard.Stat.defence - 3);
                    return true;
                }


        }
        return false;
    }
    public bool BeforeCardActionFunc(Card b)
    {
        string a = b.name;
        switch (a)
        {
            case "깡통몬":
                {
                    b.StatChange("Attack", Random.Range(b.Stat.attack-2, b.Stat.attack + 3));
                    return true;
                }
            case "깡통봇":
                {
                    b.StatChange("Attack", Random.Range(b.Stat.attack, b.Stat.attack + 3));
                    return true;
                }
            case "펀치봇":
                {
                    b.StatChange("Attack", Random.Range(b.Stat.attack+1, b.Stat.attack + 4));
                    return true;
                }
            case "건당글이":
                {
                    b.StatChange("Attack", Random.Range(b.Stat.attack+1, b.Stat.attack + 6));
                    b.StatChange("Defence", Random.Range(b.Stat.defence+1, b.Stat.defence + 6));
                    return true;
                }

            case "사신":
                {
                    if (BattleManager.Instance.tmpRate < 1.2)
                        BattleManager.Instance.tmpRate = 1.2;
                    return true;
                }
            case "마계왕":
                {
                    if (BattleManager.Instance.tmpRate < 1.5)
                        BattleManager.Instance.tmpRate = 1.5;
                    return true;
                }
            case "버섯순이":
                {
                    BattleManager.Instance.AddRerollChance();
                    return true;
                }
            case "리빙아머":
                {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    return true;
                }
            case "듀라한":
                {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    return true;
                }
            case "백작동글이":
                {
                    var target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    target = BattleManager.Instance.Enemies.Count == 0 ? null : BattleManager.Instance.Enemies[Random.Range(0, BattleManager.Instance.Enemies.Count)];
                    BattleManager.Instance.enemyDamage(b.Stat.attack, false, target);
                    return true;
                }
            case "동글키메라":
                {
                    b.StatChange("Attack", BattleManager.Instance.Grave.Count);
                    return true;
                }
            case "랩터라이더":
                {
                    foreach(Card card in BattleManager.Instance.Hand) {
                        if (card.isFixed)
                        {
                            card.StatChange("Attack", card.Stat.attack+1);
                            card.StatChange("Defence", card.Stat.defence + 1);
                        }
                    }
                    return true;
                }
            case "치킨라이더":
                {
                    foreach (Card card in BattleManager.Instance.Hand)
                    {
                        if (card.isFixed)
                        {
                            card.StatChange("Attack", card.Stat.attack + 3);
                            card.StatChange("Defence", card.Stat.defence + 2);
                        }
                    }
                    return true;
                }

        }
        return false;
    }
    public bool BeforeCardActionFunc_Always(Card b)
    {
        string a = b.name;
        switch (a)
        {
            case "프랑켄동글":
                {
                    b.StatChange("Attack", b.Stat.attack+2);
                    b.StatChange("Defence", b.Stat.defence + 2);
                    return true;
                }


        }
        return false;
    }
}
[Serializable]
public struct CardStruct
{
    public int NUM;
    public bool _Token;
    public Sprite _img;
    public SPECIES _species;
    public string _name;
    public int _tier;
    public STAT _stat;
    public TYPE _type;
    public bool isExhaust;
    public bool isEthereal;
    public bool isFixed;
    public List<String> evol;
    public bool isRare;
    public bool isUltimate;
}
[Serializable]
public struct RuleStruct
{
    public Sprite _img;
    public int _Number;
    public int _level;
    public bool _only;
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
    NONE, FIRE, WATER, GRASS, LIGHT, DARK
}

public delegate void CardAction();