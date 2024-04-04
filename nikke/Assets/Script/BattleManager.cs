using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;
    public static BattleManager Instance => instance;
    public void Awake()
    {
        instance = this;
    }



    /// ------------------------------------------


    float DeckPosX = 11f, DeckPoxY = -5, HandPosX = -6.5f, HandPosY = -2.75f, HandPosBlank = 3f, GravePosX = 11f, GravePosY = 3f;
    float ButtonPosX = 8.5f, ButtonPosY = -1f, ButtonPosBlank = 1.25f;
    float EnemyPosX = -7f, EnemyPosY = 3f, EnemyPosLength = 15f, EnemyPosBlank;


    /// ------------------------------------------
    
   
    [SerializeField] Card _cardPrefab;
    [SerializeField] RerollButton _rerollButtonPrefab;
    [SerializeField] RerollButton _chkButtonPrefab;
    [SerializeField] RerollButton _attackButtonPrefab;
    [SerializeField] TMP_Text _textPrefab;
    [SerializeField] SpriteRenderer _HPbarPrefab;
    [SerializeField] AttDefCal _AttDefCalPrefab;
    [SerializeField] Enemy _EnemyPrefab;
    [SerializeField] EnemyHpBar _EnemyHPBarPrefab;
    [SerializeField] ScrollViewCardBunch _scrollViewCardPrefab;
    [SerializeField] DeckCount _deckCountPrefab;
    [SerializeField] public GameObject _canvas;
    [SerializeField] CombiText _SpeciesText;
    [SerializeField] CombiText _TypeText;
    [SerializeField] CombiText _FinalRateText;

    [SerializeField] public Transform _damagePopupPrefab;
    [SerializeField] public Transform _rewardPopupPrefab;


    /// ------------------------------------------
    
    
    Vector2 DeckPos(int i) => new Vector2(DeckPosX, DeckPoxY + 0.15f * (Deck.Count - i));
    public List<Vector2> HandPos;
    Vector2 GravePos => new Vector2(GravePosX, GravePosY);
    Vector2 ButtonPos(float i) => new Vector2(ButtonPosX, ButtonPosY - ButtonPosBlank * i);
    List<Vector2> EnemyPos;
    List<CardStruct> BaseDeck;
    public List<Card> Deck;
    public List<Card> Hand;
    public List<Card> Grave;
    public List<Card> AllCards => Deck.Concat(Hand).Concat(Grave).ToList();
    List<Card> Cards;
    public List<Enemy> targetEnemy => Enemies.Where(x => x.isTarget).ToList();
    public int howManyCardsinDeck(string N)
    {
        int chk = 0;
        foreach (Card card in Deck)
        {
            if (card.name.Equals(N)) chk++;
        }
        return chk;
    }
    public int howManyCardsinGrave(string N)
    {
        int chk = 0;
        foreach (Card card in Grave)
        {
            if (card.name.Equals(N)) chk++;
        }
        return chk;
    }

    public int area;
    public EnemyType enemyType;
    public int reward;
    public bool waitDelay;

    public int Hp, Mhp, Shield;
    public int Att;
    public int Def;
    public double Rate;
    public int RerollChance;
    public int Turn;
    TMP_Text RerollText;
    DeckCount DeckCntTxt;
    DeckCount GraveCntTxt;
    ScrollViewCardBunch scrollViewCard;

    public static BattleState GameState;
    List<EnemyStruct> final_enemylist;
    public List<Enemy> Enemies;
    public List<Enemy> EnemiesClone()
    {
        var tmpList = new List<Enemy>();
        foreach (Enemy tmp in Enemies)
            tmpList.Add(tmp);
        return tmpList;
    }


    /// ------------------------------------------
   
    
    public void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        var randomized = Resource.Instance.Deck.OrderBy(item => Random.Range(0, 999)).ToList();
        BaseDeck = randomized;
        waitDelay = false;

        Hp = Resource.Instance.Hp; Mhp = Resource.Instance.mHp; Shield = 0;
        area = Resource.Instance.Area; enemyType = Resource.Instance.Stage != 6 ? (Resource.Instance.Stage >= 4 ? EnemyType.Normal : EnemyType.Mini) : EnemyType.Giga;
        RerollChance = 0;
        reward = (int)((enemyType == EnemyType.Mini ? 30 : (enemyType == EnemyType.Normal ? 40 : 60)) * Random.Range(4, 8) * (area*2)/(1.8f));
        addcardQueue_Deck = new Queue<CardStruct>();
        addcardQueue_Grave = new Queue<CardStruct>();
        pureAttack = false;
        Change_State(BattleState.Set);
    }
    public void FixedUpdate()
    {
        _FinalRateText.Set("", (float)Rate);
    }


    /// -------------------PHASE-----------------------

    private void Change_State(BattleState battleState)
    {
        GameState = battleState;
        Debug.Log(battleState);
        switch (battleState)
        {
            case BattleState.Set:
                PHASE_Set();
                break;
            case BattleState.TurnStart:
                PHASE_TurnStart();
                EnemyStatus_TurnStart();
                Check_DeckCount_LessThanFive();
                break;
            case BattleState.Draw:
                PHASE_Draw();
                break;
            case BattleState.WaitingReroll:
                PHASE_WaitingReroll();
                break;
            case BattleState.Reroll:
                PHASE_Reroll();
                break;
            case BattleState.Attack:
                PHASE_Attack();
                break;
            case BattleState.EnemyAttack:
                PHASE_EnemyAttack();
                break;
            case BattleState.EndTurn:
                PHASE_EndTurn();
                break;
            case BattleState.Reward:
                PHASE_Reward();
                break;
            case BattleState.Lose:
                PHASE_LOSE();
                break;
        }
    }

    public void PHASE_Set()
    {
        Att = 0; Def = 0; Rate = 1; Turn = 0;
        tmpAtt = 0; tmpDef = 0; tmpRate = 1; tmpReroll = 0;
        Enemies = new List<Enemy>();
        Deck = new List<Card>();
        Hand = new List<Card>();
        Grave = new List<Card>();
        HandPos = new List<Vector2>();
        for (int i = 0; i < 5; i++)
            HandPos.Add(new Vector2(HandPosX + i * HandPosBlank, HandPosY));
        EnemyPos = new List<Vector2>();

        var HpBar = Instantiate(_HPbarPrefab, new Vector2(HandPos[0].x - 1.5f, HandPos[0].y - 3.5f), Quaternion.identity);
        var AttDef = Instantiate(_AttDefCalPrefab, ButtonPos(3), Quaternion.identity);

        DeckCntTxt = Instantiate(_deckCountPrefab, new Vector2(DeckPos(0).x + 0.85f, DeckPos(0).y - 1.2f), Quaternion.identity);
        DeckCntTxt.Set(ref Deck);

        GraveCntTxt = Instantiate(_deckCountPrefab, new Vector2(GravePosX + 0.85f, GravePosY - 1.2f), Quaternion.identity);
        GraveCntTxt.Set(ref Grave);

        RerollText = Instantiate(_textPrefab, ButtonPos(1.6f), Quaternion.identity);
        RerollText.text = "Chance : " + RerollChance.ToString();
        RerollText.fontSize = 3f;

        var cnt = 0;
        Sequence sq = DOTween.Sequence().SetAutoKill(false);
        foreach (var tmpCard in BaseDeck)
        {
            var tmp = Instantiate(_cardPrefab, new Vector2(0, 0), Quaternion.identity);
            int Layer = 50 + (BaseDeck.Count - cnt) * 3;
            tmp.TouchableChange(false);
            tmp.setLayer(0, Layer);
            tmp.Set(tmpCard);
            Deck.Add(tmp);
            Card_Move(sq, tmp, DeckPos(0), 0.5f / (float)BaseDeck.Count, true, false);

            cnt++;
        }
        final_enemylist = EnemyDatabase.Instance.final_enemylist(area, enemyType);
        EnemyPosBlank = EnemyPosLength / (final_enemylist.Count + 1);
        cnt = 0;
        foreach (var enemy in final_enemylist)
        {
            cnt++;
            var enemyposTmp = new Vector2(EnemyPosX + EnemyPosBlank * cnt, EnemyPosY + (enemy._enemyType == EnemyType.Mini ? 0 : (enemy._enemyType == EnemyType.Normal ? 0.5f : (enemy._enemyType == EnemyType.Giga ? 1f : 2f))));
            var enemyTmp = Instantiate(_EnemyPrefab, enemyposTmp, Quaternion.identity);

            enemyTmp.Set(enemy);

            var enemyHpBarTmp = Instantiate(_EnemyHPBarPrefab, enemyposTmp, Quaternion.identity);
            enemyHpBarTmp.Set(enemyTmp);
            EnemyPos.Add(enemyposTmp);
            Enemies.Add(enemyTmp);
        }
        sq.OnComplete(() =>
        {
            ShuffleDeck(sq);
        });
        sq.OnComplete(() =>
        {
            sq.Kill();
            Change_State(BattleState.TurnStart);
        });
    }
    public void PHASE_TurnStart()
    {

        Sequence sq = DOTween.Sequence();
        Turn++;
        if (!Resource.Instance.Rule_no(11)) Shield = 0;
        if (Resource.Instance.Rule_no(11) && Turn == 1) Shield += 100;
        Add_RerollChance();
        foreach (var enemy in EnemiesClone())
        {
            enemy.SetPatternText();
        }
        ShuffleDeck(sq);
    }
    public void PHASE_WaitingReroll()
    {
        ChkButtons = new List<RerollButton>();
        SpeciesCombi = new List<SPECIES>();
        TypeCombi = new List<TYPE>();
        CombiText = new List<GameObject>();
        CombiText.Add(_SpeciesText.gameObject);
        CombiText.Add(_TypeText.gameObject);
        List<RerollButton> Btns = new List<RerollButton>();
        var attackbtn = Instantiate(_attackButtonPrefab, ButtonPos(1), Quaternion.identity);
        attackbtn.ActionSet(() =>
        {
            foreach (var btn in Btns) Destroy(btn.gameObject);
            Change_State(BattleState.Attack);
        });
        Btns.Add(attackbtn);
        var rerollbtn = Instantiate(_rerollButtonPrefab, ButtonPos(0), Quaternion.identity);
        rerollbtn.ActionSet(() =>
        {
            if (RerollChance <= 0) return;
            int chk = 0;
            for (int i = 0; i < Hand.Count; i++) if (PHASE_WaitingReroll_isBtnChk(ChkButtons[i])) chk++;
            if (chk == 0) return;
            RerollChance -= 1;
            RerollText.text = "Chance : " + RerollChance.ToString();
            foreach (var btn in Btns) Destroy(btn.gameObject);
            Change_State(BattleState.Reroll);
        });
        Btns.Add(rerollbtn);
        rerollbtn.rerollEnable(false);
        int chkN = 0;
        for (int i = 0; i < Hand.Count; i++)
        {
            var chkBtn = Instantiate(_chkButtonPrefab, new Vector2(HandPos[i].x, HandPos[i].y - 2.25f), Quaternion.identity);
            chkBtn.index = i;
            chkBtn.ActionSet(() =>
            {

                chkBtn.SpriteChange(PHASE_WaitingReroll_isBtnChk(chkBtn) ? CardDatabase.Instance.btn(1) : CardDatabase.Instance.btn(0));
                if (PHASE_WaitingReroll_isBtnChk(chkBtn))
                {
                    Card_Move(DOTween.Sequence(), Hand[chkBtn.index], new Vector2(HandPos[chkBtn.index].x, HandPos[chkBtn.index].y + 1f), 0.1f, true);
                    chkN++;
                }
                else
                {
                    Card_Move(DOTween.Sequence(), Hand[chkBtn.index], HandPos[chkBtn.index], 0.1f, true);
                    chkN--;
                }
                if (chkN > 0)
                    rerollbtn.rerollEnable(true);
                else
                    rerollbtn.rerollEnable(false);
            });
            if (Hand[i].isFixed) { chkBtn.SpriteChange(CardDatabase.Instance.btn(2)); chkBtn.enabled = false; }
            ChkButtons.Add(chkBtn);
            Btns.Add(chkBtn);
            SpeciesCombi.Add(Hand[i].Species);
            TypeCombi.Add(Hand[i].Type);
        }
        var SpeciesCombiRate = CardDatabase.Instance.SpeciesCombiRate(SpeciesCombi);
        _SpeciesText.Set(CardDatabase.Instance.SpeciesCombinationText(SpeciesCombi), SpeciesCombiRate);
        PHASE_WaitingReroll_TextSet(SpeciesCombiRate, _SpeciesText._text_rate);

        float TypeCombiRate = CardDatabase.Instance.TypeCombiRate(TypeCombi);
        _TypeText.Set(CardDatabase.Instance.TypeCombinationText(TypeCombi), TypeCombiRate);
        PHASE_WaitingReroll_TextSet(TypeCombiRate, _TypeText._text_rate);

        foreach (Card card in Hand)
            card.glow(true);
        Caculate_AttackAndDefence_inHand(tmpAtt, tmpDef, tmpRate);
        Check_Battle_isOver();
    }
    public void PHASE_Draw()
    {
        Att = tmpAtt; Def = tmpDef; Rate = tmpRate;
        Sequence sq = DOTween.Sequence();
        Debug.Log("기다리는중! : " + waitDelay);
        if (waitDelay)
        {
            sq.AppendInterval(1f)
            .AppendCallback(() =>
            {
                Change_State(BattleState.Draw);
            });
            return;
        }
        PHASE_Draw_CardAction(sq);
        sq.AppendCallback(() =>
        {
            foreach (var card in Hand)
            {
                card.setLayer(0, 50);
            }
            Change_State(BattleState.WaitingReroll);
            sq.Kill();
        });

    }
    public void PHASE_Reroll()
    {
        Sequence sq = DOTween.Sequence();
        List<Card> rerollCard = new List<Card>();
        for (int i = 0; i < Hand.Count; i++) if (PHASE_WaitingReroll_isBtnChk(ChkButtons[i])) rerollCard.Add(Hand[i]);
        foreach (var card in rerollCard)
        {
            Hand.Remove(card);
            if (card.isEthereal) sq.Append(card.transform.DOScale(0, 0.5f).OnComplete(() => { Destroy(card.gameObject); }));
            else
            {
                Grave.Add(card);
                card.glow(false);
                card.setLayer(0, 50 + Grave.Count * 3);
                card.TouchableChange(false);
                float ranTmpx = Random.Range(-30, 30) / 100f + GravePosX;
                float ranTmpy = Random.Range(-30, 30) / 100f + GravePosY;
                Card_Move(sq, card, new Vector2(ranTmpx, ranTmpy), 0.2f, true);
                sq.Join(card.transform.DORotate(new Vector3(0, 0, Random.Range(-60, 60)), 0.2f));
            }
        }
        sq.AppendCallback(() =>
        {
            foreach (Enemy enemy in Enemies)
                EnemyStatus_Reroll(enemy);
            Change_State(BattleState.Draw);
            sq.Kill();
        });
    }
    public void PHASE_Attack()
    {
        Sequence sq = DOTween.Sequence();
        PHASE_Attack_CardAction(sq);

        Caculate_AttackAndDefence_inHand(tmpAtt, tmpDef, tmpRate);
        Rule_no(9);
        PHASE_Attack_AttackByHands(sq);
        Calculate_HighestDamage(Att);
        sq.AppendCallback(() =>
        {


            tmpAtt = 0; tmpDef = 0; tmpRate = 1;
            if (!Check_Battle_isOver())
            {
                if (Resource.Instance.Rule_no(11)) { Change_State(BattleState.EndTurn); return; }
                Change_State(BattleState.EnemyAttack);
            }
        });
    }
    public void PHASE_EnemyAttack()
    {
        Sequence sq = DOTween.Sequence();
        List<int> DamageList = new List<int>();

        var tmpEnemyList = new List<Enemy>();
        foreach (Enemy tmp in BattleManager.Instance.Enemies)
            tmpEnemyList.Add(tmp);
        foreach (var Enemy in tmpEnemyList)
        {
            if (Enemy.Pattern._enemyPattern == EnemyPattern.ATT)
            {
                Damage_toPlayer(sq, Enemy);
                sq.AppendInterval(0.8f);
            }
            else if (Enemy.Pattern._enemyPattern == EnemyPattern.BUFF)
            {
                sq.AppendCallback(() =>
                {
                    Enemy.setAttackBuff();
                    Enemy.transform.DOMoveY(Enemy._img.transform.position.y + 0.35f, 0.15f).SetLoops(4, LoopType.Yoyo);
                });
                sq.AppendInterval(0.8f);
            }
            else if (Enemy.Pattern._enemyPattern == EnemyPattern.CARDINSRT)
            {
                sq.AppendCallback(() =>
                {
                    Enemy.transform.DOMoveY(Enemy._img.transform.position.y + 0.35f, 0.15f).SetLoops(4, LoopType.Yoyo);
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    for (int i = 0; i < Enemy.Pattern._Value; i++)
                        queue.Enqueue(CardDatabase.Instance.card_token(Enemy.Pattern._CardName));
                    Card_Add_toDeckOrGrave(queue, Enemy.Pattern._Bool);
                });
                sq.AppendInterval(0.8f);
            }
            else if (Enemy.Pattern._enemyPattern == EnemyPattern.SUMMON)
            {
                sq.AppendCallback(() =>
                {
                    Summon_Enemy(EnemyDatabase.Instance.enemy(Enemy.Pattern._CardName));
                });
                sq.AppendInterval(0.8f);
            }
            else if (Enemy.Pattern._enemyPattern == EnemyPattern.SHIELD)
            {
                sq.AppendCallback(() =>
                {
                    Enemy.gainShield(Enemy.Pattern._Value);
                });
                sq.AppendInterval(0.8f);
            }
            else if (Enemy.Pattern._enemyPattern == EnemyPattern.HEAL)
            {
                sq.AppendCallback(() =>
                {
                    Heal_Enemy(Enemy.Pattern._Value, Enemy);
                });
                sq.AppendInterval(0.8f);
            }
        }



        sq.AppendCallback(() =>
        {
            Check_Enemy_isDead();
            if (Resource.Instance.Rule_no(11))
            {
                foreach (var Enemy in Enemies)
                {
                    Enemy.TurnEnd();
                }
                Change_State(BattleState.Draw);
                return;
            }
            Change_State(BattleState.EndTurn);
        });
    }
    public void PHASE_EndTurn()
    {

        Sequence sq = DOTween.Sequence();
        EnemyStatus_TurnEnd(sq);
        sq.AppendCallback(() => { 
            if (Turn == 10 && Rule_no(15))
            {
                EffectManager.effectOn(EffectManager.EffectName.Fire, new Vector2(BattleManager.Instance.HandPos[2].x, BattleManager.Instance.HandPos[0].y));
                Damage_toPlayer(Mhp / 2);
            }
            if (!Resource.Instance.Rule_no(11))
            {
                foreach (var Enemy in Enemies) Enemy.TurnEnd();
            }
            if (Hp <= 0) Change_State(BattleState.Lose);
            else if (Enemies.Count == 0) Change_State(BattleState.Reward);
            else
                Change_State(BattleState.TurnStart);
        });
    }
    public void PHASE_Reward()
    {
        AudioManager.instance.PlaySfx(6);
        int finalReward = reward * (Resource.Instance.VillageLevel["Farm"] * Village.Value_Farm + 100) / 100;
        RewardPopup.Create(finalReward).OnComplete(() =>
        {
            Calculate_Reward(finalReward);
        });
    }
    public void PHASE_LOSE()
    {
        Sequence sq = DOTween.Sequence();
        sq.AppendInterval(2.5f);
        sq.AppendCallback(() =>
        {
            Resource.Instance.reset();
            SceneManager.LoadScene("StartScene");
        });
    }

    /// ------------------BEFORE_WAITING_REROLL------------------------

    public void ShuffleDeck(Sequence sq)
    {
        var randomized = Deck.OrderBy(item => Random.value).ToList();
        Deck = randomized;
        foreach (Card tmp in Deck)
            tmp.flip(false);
        DeckCntTxt.Set(ref Deck);
        GraveCntTxt.Set(ref Grave);
        Deck_Move(sq);
    }
    public void PHASE_Draw_CardAction(Sequence sq)
    {
        List<Card> cardsDrawnThisTurn = new List<Card>();
        while (Hand.Count < 5)
        {
            cardsDrawnThisTurn.Add(Card_Draw(sq));
            if (Hand.Count + Deck.Count + Grave.Count < 5) break;
        }
        Deck_Move(sq);
        bool cardActionFuncAnimationChk = false;
        sq.AppendCallback(() =>
        {
            foreach (var card in cardsDrawnThisTurn)
            {
                if (CardDatabase.Instance.BeforeCardActionFunc(card))
                {
                    cardActionFuncAnimationChk = true;
                    AudioManager.instance.PlaySfx(4);
                    card.transform.DOScale(card.transform.localScale * 1.35f, 0.3f).SetLoops(2, LoopType.Yoyo);

                }
            }
        });
        if (cardActionFuncAnimationChk) sq.AppendInterval(0.6f);
        cardActionFuncAnimationChk = false;
        sq.AppendCallback(() =>
        {
            foreach (var card in Hand)
            {
                if (CardDatabase.Instance.BeforeCardActionFunc_Always(card))
                {
                    cardActionFuncAnimationChk = true;
                    AudioManager.instance.PlaySfx(4);
                    card.transform.DOScale(card.transform.localScale * 1.35f, 0.3f).SetLoops(2, LoopType.Yoyo);

                }
            }
        });
        if (cardActionFuncAnimationChk) sq.AppendInterval(0.6f);

    }

    public int tmpAtt;
    public int tmpDef;
    public double tmpRate;
    public int tmpReroll;

    public void Check_DeckCount_LessThanFive()
    {
        Sequence sq = DOTween.Sequence();
        var queue = new Queue<CardStruct>();
        float loopCnt = 0;
        while (Deck.Count + Grave.Count + loopCnt < 5)
        {
            queue.Enqueue(CardDatabase.Instance.card_token("눅눅한동글이"));
            loopCnt++;
        }
        Card_Add_toDeckOrGrave(queue, true);
        sq.AppendInterval(0.4f * loopCnt);
        sq.AppendCallback(() =>
        {
            if (Rule_no(11)) { Change_State(BattleState.EnemyAttack); return; }
            Change_State(BattleState.Draw);
        });
    }

    /// --------------------WAITING_REROLL----------------------
    
    private List<RerollButton> ChkButtons;
    public List<SPECIES> SpeciesCombi;
    public List<TYPE> TypeCombi;
    private List<GameObject> CombiText;

    public void PHASE_WaitingReroll_TextSet(float combi, TMP_Text text)
    {
        if (combi < 1)
        {
            text.fontSize = 45;
            text.color = Color.gray;
            return;
        }
        if (combi < 1.5f)
        {
            text.fontSize = 45;
            return; 
        }
        if (combi < 2f)
        {
            text.fontSize = 50;
            return; 
        }
        if (combi < 2.5f)
        {
            text.fontSize = 50;
            text.color = new Color(0, 100, 0);
            return; 
        }
        if (combi < 3f)
        {
            text.fontSize = 60;
            text.color = Color.blue;
            return;
        }
        if (combi < 3.5f)
        {
            text.fontSize = 60;
            text.color = Color.blue;
            return;
        }
        text.fontSize = 70;
        text.color = Color.red;
        return;
    }
    public bool PHASE_WaitingReroll_isBtnChk(RerollButton btn) => btn.btnSprite.Equals(CardDatabase.Instance.btn(0));

    public void Add_RerollChance()
    {
        RerollChance++;
        RerollText.text = "Chance : " + RerollChance.ToString();
    }
    public void Add_RerollChance(int n)
    {
        RerollChance+=n;
        RerollText.text = "Chance : " + RerollChance.ToString();
    }

    public void Caculate_AttackAndDefence_inHand(int a, int b, double r)
    {
        Att = 0 + a; Def = 0 + b; Rate = 1;
        float typeRate = CardDatabase.Instance.TypeCombiRate(TypeCombi);
        float speciesRate = CardDatabase.Instance.SpeciesCombiRate(SpeciesCombi);
        float tmpRate = typeRate * speciesRate * (float)r;
        Rate *= (double)Mathf.Round((tmpRate * 100f)) / 100f;
        foreach (var i in Hand) { 
            Att += (int)(Rate * i.Stat.attack * (Resource.Instance.Rule_no(20) ? (Turn / 2 == 0 ? 0.5f : 1.5f) : 1)); 
            Def += i.Stat.defence; 
        }
        this.Def = (int)(Rate * Mathf.Ceil((float)Def * (Rule_no(20) ? (Turn / 2 == 0 ? 1.5f : 0.5f) : 1)));
    }


    /// ------------------DAMAGE------------------------

    public void PHASE_Attack_CardAction(Sequence sq)
    {
        bool cardActionFuncAnimationChk = false;
        bool Rule_no19_cardActionFuncAnimationChk = false;
        foreach (var card in Hand)
        {
            card.TouchableChange(false);
            if (CardDatabase.Instance.CardActionFunc(card))
            {
                cardActionFuncAnimationChk = true;
                AudioManager.instance.PlaySfx(4);
                card.transform.DOScale(card.transform.localScale * 1.35f, 0.3f).SetLoops(2, LoopType.Yoyo);

            }
            if (Resource.Instance.Rule_no(19) && card.Species == SPECIES.HUMAN && CardDatabase.Instance.CardActionFunc(card) && Rule_no(19))
            {
                Rule_no19_cardActionFuncAnimationChk = true;
                AudioManager.instance.PlaySfx(4);
                card.transform.DOScale(card.transform.localScale * 1.35f, 0.3f).SetLoops(2, LoopType.Yoyo);

            }
        }
        if (cardActionFuncAnimationChk) sq.AppendInterval(0.6f);
        if (Rule_no19_cardActionFuncAnimationChk) sq.AppendInterval(0.6f);

    }
    public void PHASE_Attack_AttackByHands(Sequence sq)
    {
        foreach (var card in Hand)
        {
            sq.Append(card.transform.DOMoveY(card.transform.position.y + 1f, 0.2f));
            bool critical = (5 + Resource.Instance.VillageLevel["Church"] * Village.Value_Church) >= Random.Range(0, 101);
            if (critical && Resource.Instance.Rule_no(6))
                critical = Random.Range(0, 2) >= 1 ? Rule_no(6) : false;



            sq.AppendCallback(() =>
            {
                if (Resource.Instance.Rule_no(9)) Heal_Player(1);
                if (card.Species == SPECIES.MONSTER && Rule_no(18)) Heal_Player(1);
                if (Enemies.Count > 0)
                {

                    pureAttack = true;
                    var target = targetEnemy.Count == 0 ? Enemies[Random.Range(0, Enemies.Count)] : targetEnemy[0];
                    var damage = (int)(card.Stat.attack * Rate * (Resource.Instance.Rule_no(20) ? (Turn / 2 == 0 ? 0.5f : 1.5f) : 1));
                    AudioManager.instance.PlaySfx(0);
                    Damage_toEnemy(damage, critical, target);
                    if (damage > 0)
                        switch (card.Str._type)
                        {
                            case (TYPE.FIRE):
                                { EffectManager.effectOn(EffectManager.EffectName.Fire, target); return; }
                            case (TYPE.WATER):
                                { EffectManager.effectOn(EffectManager.EffectName.Water, target); return; }
                            case (TYPE.GRASS):
                                { EffectManager.effectOn(EffectManager.EffectName.Grass, target); return; }
                            case (TYPE.LIGHT):
                                { EffectManager.effectOn(EffectManager.EffectName.Light, target); return; }
                            case (TYPE.DARK):
                                { EffectManager.effectOn(EffectManager.EffectName.Dark, target); return; }
                        }
                }
            });

            if (card.isExhaust)
            {
                sq.AppendCallback(() => { card.deleteCard(); });
                sq.AppendInterval(0.2f);
            }
            else
            {
                card.glow(false);
                Grave.Add(card);
                card.TouchableChange(false);
                Card_Move(sq, card, GravePos, 0.2f, true);
                card.setLayer(0, 50 + Grave.Count * 3);
            }

        }
        Hand.Clear();
        Shield = Def;


        sq.AppendInterval(0.5f);

    }
    public void Calculate_HighestDamage(int att)
    {
        Resource.Instance.HighestDam = System.Math.Max(att, Resource.Instance.HighestDam);
    }

    public bool pureAttack;

    public bool Check_Battle_isOver()
    {
        foreach (var enem in Enemies)
        {
            if (enem._hp <= 0) Enemies.Remove(enem);
        }
        var chk = Enemies.Count == 0;
        if (chk)
            Change_State(BattleState.EndTurn);
        return chk;

    }
    public void Damage_toEnemy(int dam, bool critical, Enemy target)
    {
        if (target == null) return;
        var AttLeft = !critical ? dam : dam * (Rule_no(6) ? 3:2);
        int loopcnt = 0;

        while (Enemies.Count > 0 && AttLeft > 0)
        {
            var afterShieldAttLeft = AttLeft - target._shield;
            target.lossShield(AttLeft);
            if (afterShieldAttLeft < 0)
            {
                DamagePopup.Create(target.transform.position, "Block", Color.gray);
                return;
            }
            AttLeft = afterShieldAttLeft;
            var tmpAttLeft = AttLeft - target._hp;
            var AttEffect = EffectManager.effectOn(EffectManager.EffectName.Attack, new Vector2(target._img.transform.position.x * Random.Range(85, 115) / 100f, (target._img.transform.position.y - 1.5f) * Random.Range(85, 115) / 100f));
            AttEffect.transform.localScale *= (target.Str._enemyType == EnemyType.Mini ? 0.6f : 1f) * (0.25f + ((float)AttLeft / (float)target.Str._hp));

            target._hp -= AttLeft;
            bool lethal = (target._hp <= 0);
            if (lethal)
            {

                AudioManager.instance.PlaySfx(5);
                DamagePopup.Create(target.transform.position, AttLeft + target._hp, critical, lethal);
                var deadEnemy = target;
                target._hp = 0;
                Enemies.Remove(deadEnemy);
                var Particle = EffectManager.effectOn(EffectManager.EffectName.Fracture, deadEnemy);
                Particle.transform.localScale *= (deadEnemy.Str._enemyType == EnemyType.Mini ? 0.6f : 1f);
                deadEnemy._img.transform.DOScale(0, 0.3f).OnComplete(() =>
                {
                    EnemyStatus_WhileDying(deadEnemy);
                    Destroy(deadEnemy._hpBar.gameObject);
                    Destroy(deadEnemy.gameObject);
                });
                target =Enemies.Count>0? Enemies[Random.Range(0, Enemies.Count)]:null;
            }
            else
            {
                EnemyStatus_WhileAttack(target);
                DamagePopup.Create(target.transform.position, AttLeft, critical, lethal);
            }
            pureAttack = false;
            AttLeft = tmpAttLeft;

            loopcnt++;
            if (loopcnt > 1000)
            {
                Debug.Log("????");
                break;
            }
        }
    }
    public void Damage_toEnemy_Wide(int dam)
    {
        var tmpList = new List<Enemy>();
        foreach (Enemy tmp in Enemies)
            tmpList.Add(tmp);
        foreach (Enemy tmp in tmpList)
        {
            Debug.Log("공격대상 :" + tmp._name);
            BattleManager.Instance.Damage_toEnemy(dam, false, tmp);
        }
    }
    public int patternCnt;
    public void Check_Enemy_isDead()
    {
        if (Hp <= 0)
        {
            Hp = 0;
            Change_State(BattleState.Lose);
        }

    }
    public void Damage_toPlayer(int dam)
    {
        var ShieldDamageCompare = Shield > dam;
        int FinalDamage = ShieldDamageCompare ? 0 : dam - Shield;
        Shield = ShieldDamageCompare ? Shield - dam : 0;
        if (FinalDamage > 0)
        {
            AudioManager.instance.PlaySfx(0);
            DamagePopup.Create(new Vector2(HandPos[2].x, HandPos[0].y - 3.5f), FinalDamage + "", Color.white);
            Hp -= FinalDamage;
        }
        else
        {
            AudioManager.instance.PlaySfx(3);
            DamagePopup.Create(new Vector2(HandPos[2].x, HandPos[0].y - 3.5f), "BLOCK", Color.gray);
        }
        if (Hp <= 0) Check_Enemy_isDead();
    }
    public void Damage_toPlayer(Sequence sq, Enemy Enemy)
    {
        sq.AppendCallback(() =>
        {
            var dam = Enemy.damage;

            int FinalDamage = Shield > dam ? 0 : dam - Shield;
            Shield = Shield > dam ? Shield - dam : 0;
            if (FinalDamage > 0)
            {

                AudioManager.instance.PlaySfx(1);
                DamagePopup.Create(new Vector2(HandPos[2].x, HandPos[0].y - 3.5f), FinalDamage + "", Color.white);
                Enemy._img.transform.DOScale(4f, 0.15f).SetLoops(2, LoopType.Yoyo);
                Hp -= FinalDamage;
            }
            else
            {
                AudioManager.instance.PlaySfx(3);
                DamagePopup.Create(new Vector2(HandPos[2].x, HandPos[0].y - 3.5f), "BLOCK", Color.gray);
                Enemy._img.transform.DOScale(2f, 0.2f).SetLoops(2, LoopType.Yoyo);
            }

        });
    }
    public void Heal_Player(int value)
    {
        AudioManager.instance.PlaySfx(10);
        DamagePopup.Create(new Vector2(HandPos[2].x, HandPos[0].y - 3.5f), "+" + value, Color.green);
        Hp += value;
        if (Mhp < Hp) Hp = Mhp;
    }
    public void Calculate_Reward(int finalReward)
    {
        Resource.Instance.money += finalReward;
        if(enemyType==EnemyType.Giga)
            Resource.Instance.jewel++;
        Resource.Instance.StageUp();
        Resource.Instance.setHp(Hp);
        DOTween.KillAll();
        SceneManager.LoadScene("MainScene");
        SceneManager.LoadScene("EventScene", LoadSceneMode.Additive);
    }


    /// ----------------CARD--------------------------


    Queue<CardStruct> addcardQueue_Deck;
    Queue<CardStruct> addcardQueue_Grave;
    public void Card_Add_toDeckOrGrave(Queue<CardStruct> list, bool deck)
    {
        if (list.Count == 0) return;
        waitDelay = true;
        Sequence cardAddSequence = DOTween.Sequence();
        while (list.Count > 0)
        {
            if (deck)
                addcardQueue_Deck.Enqueue(list.Dequeue());
            else
                addcardQueue_Grave.Enqueue(list.Dequeue());
        }
        while (addcardQueue_Deck.Count > 0 || addcardQueue_Grave.Count > 0)
        {
            CardStruct str = (deck ? addcardQueue_Deck : addcardQueue_Grave).Dequeue();
            var card = Instantiate(_cardPrefab, new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)), Quaternion.Euler(0, 0, Random.Range(-90, 90)));
            card.flip(true);
            card.TouchableChange(false);
            card.Set(str);
            cardAddSequence.AppendInterval(0.3f);
            (deck ? Deck : Grave).Add(card);
            int Layer = 50 + (deck ? Deck.Count : Grave.Count) * 3;
            card.setLayer(0, Layer);
            Card_Move(cardAddSequence, card, (deck ? DeckPos(0) : GravePos), 0.5f, true);
            if (deck)
                cardAddSequence.AppendCallback(() => { card.flip(false); });

            cardAddSequence.AppendInterval(0.15f);
        }
        cardAddSequence.AppendCallback(() => { waitDelay = false; });

    }
    public Card Card_Draw(Sequence sq)
    {
        Card tmpCard;
        if (Deck.Count <= 0)
        {
            foreach (var card in Grave) { Card_Move(sq, card, DeckPos(0), 0.5f, false, false); }
            Deck = Grave;
            ShuffleDeck(sq);
            Grave.Clear();
        }
        tmpCard = Deck[0];
        Hand.Add(tmpCard);
        Deck.Remove(tmpCard);
        sq.AppendCallback(() =>
        {
            AudioManager.instance.PlaySfx(2);
            tmpCard.flip(true);
        });
        for (int i = 0; i < Hand.Count; i++)
        {
            Card_Move(sq, Hand[i], HandPos[i], 0.2f, false, true);
            Hand[i].TouchableChange(true);
        }
        return tmpCard;
    }
    public void Card_Move(Sequence seq, Card card, Vector2 v, float duration, bool one)
    {
        if (one)
            seq.Append(card.transform.DOMove(v, duration).SetEase(Ease.OutCubic));
        else
            seq.Join(card.transform.DOMove(v, duration).SetEase(Ease.OutCubic));
        if (v.Equals(GravePos))
        {
            seq.Join(card.transform.DORotate(new Vector3(0, 0, Random.Range(-60, 60)), 0.2f));
        }
        else
        {
            if (card.transform.rotation.z != 0)
                card.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void Card_Move(Sequence seq, Card card, Vector2 v, float duration, bool one, bool cardFlip)
    {
        if (one)
        {
            seq.Append(card.transform.DOMove(v, duration).SetEase(Ease.OutCubic));
            if (card.transform.rotation.z != 0)
                seq.Join(card.transform.DORotate(new Vector3(0, 0, 0), 0.2f));
            seq.AppendCallback(() => card.flip(cardFlip));
            seq.AppendInterval(0.05f);
        }
        else
            seq.Join(card.transform.DOMove(v, duration).SetEase(Ease.OutCubic).OnPlay(() => { card.flip(cardFlip); }));
    }
    public void Deck_Move(Sequence sq)
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].setLayer(Deck.Count - i, 50);
            if (Deck[i].transform.rotation.z != 0)
                Deck[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            Card_Move(sq, Deck[i], DeckPos(i), 0.02f, true);
        }
    }
    public void Deck_chkCnt(List<Card> list)
    {
        foreach (var card in Hand) { card.TouchableChange(_scrollViewCardPrefab.gameObject.activeInHierarchy); }
        if (_scrollViewCardPrefab.gameObject.activeInHierarchy)
        {
            _scrollViewCardPrefab.delete();
            return;
        }
        _scrollViewCardPrefab.gameObject.SetActive(true);
        _scrollViewCardPrefab.Set(list);
    }
    public void Card_Move_GraveToDeck(Card card)
    {
        if (card == null) return;
        Sequence cardMoveSequence = DOTween.Sequence();
        card.TouchableChange(false);
        Grave.Remove(card);
        Deck.Add(card);
        int Layer = 50 + (Deck.Count) * 3;
        card.setLayer(0, Layer);
        BattleManager.Instance.Card_Move(cardMoveSequence, card, DeckPos(0), 0.5f, false);
        cardMoveSequence.AppendCallback(() => { card.flip(false); });
    }


    /// ---------------ENEMY---------------------

    public void EnemyStatus_TurnStart()
    {
        foreach (Enemy enemy in EnemiesClone())
        {
            if (!enemy._statusName.Equals("없음"))
            {
                StatusDatabase.Instance.Action_TurnStart(enemy._statusName, enemy);
            }
        }
    }
    public void EnemyStatus_WhileAttack(Enemy enemy)
    {
        if (!enemy._statusName.Equals("없음"))
        {
            StatusDatabase.Instance.Action_WhileAttacked(enemy._statusName, enemy);
        }

    }
    public void EnemyStatus_TurnEnd(Sequence sq)
    {
        foreach (Enemy enemy in EnemiesClone())
        {
            if (!enemy._statusName.Equals("없음"))
            {
                StatusDatabase.Instance.Action_TurnEnd(enemy._statusName, enemy);
                sq.AppendInterval(0.4f);
            }
            if ((enemyType == EnemyType.Giga || enemyType == EnemyType.Final) &&(enemy._enemyType==EnemyType.Giga|| enemy._enemyType == EnemyType.Final)&& Rule_no(10) )
            {
                enemy.gainShield(enemy._mhp * 4 / 100);
            }
        }
    }
    public void EnemyStatus_WhileDying(Enemy enemy)
    {
        if (!enemy._statusName.Equals("없음"))
        {
            StatusDatabase.Instance.Action_WhileDying(enemy._statusName, enemy);
        }
    }
    public void EnemyStatus_Reroll(Enemy enemy)
    {
        if (!enemy._statusName.Equals("없음"))
        {
            StatusDatabase.Instance.Action_Reroll(enemy._statusName, enemy);
        }
    }

    public void Summon_Enemy(EnemyStruct enemystruct)
    {
        if (EnemyPos.Count == Enemies.Count) return;
        Vector2 enemyposTmp = new Vector2(0, 0);
        foreach (Vector2 enemyPosTmp in EnemyPos)
        {
            bool chk = false;
            foreach (Enemy EnemyTmp in Enemies)
            {
                if (enemyPosTmp.Equals(EnemyTmp.transform.position))
                    chk = true;
            }
            if (!chk) enemyposTmp = enemyPosTmp;
        }

        var enemyTmp = Instantiate(_EnemyPrefab, enemyposTmp, Quaternion.identity);
        EffectManager.effectOn(EffectManager.EffectName.Summon, enemyTmp);

        enemyTmp.Set(enemystruct);

        var enemyHpBarTmp = Instantiate(_EnemyHPBarPrefab, enemyposTmp, Quaternion.identity);
        enemyHpBarTmp.Set(enemyTmp);
        Enemies.Add(enemyTmp);
    }
    public void Heal_Enemy(int a, Enemy enemy)
    {
        EffectManager.effectOn(EffectManager.EffectName.Heal, enemy); ;
        enemy.heal(a);
        DamagePopup.Create(enemy.transform.position, "+" + a, Color.green);


    }

    /// ---------------RULE---------------------

    public bool Rule_no(int n)
    {
        if (Resource.Instance.Rule_no(n))
        {
            DamagePopup.Create(new Vector2(0,0), DataManager.RuleName(n,Resource.Instance.Kor), Color.yellow);
            return true;
        }
        return false;
    }


    [ContextMenu("stageUp")]
    public void cont_stageEnd()
    {
        Change_State(BattleState.Reward);
    }
}
public delegate void Func();
public enum BattleState
{
    Set, TurnStart, Draw, WaitingReroll, Reroll, Attack, EnemyAttack, EndTurn, Reward, Lose
}