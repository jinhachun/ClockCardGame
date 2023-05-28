using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;
    public static BattleManager Instance => instance;
    public void Awake()
    {
        instance = this;
    }
    float DeckPosX = 11f, DeckPoxY = -5, HandPosX = -7, HandPosY = -2.75f, HandPosBlank = 3f, GravePosX = -12f, GravePosY = -5f;
    float ButtonPosX = 8f, ButtonPosY = -1f, ButtonPosBlank = 1.25f;
    float EnemyPosX = -8f, EnemyPosY = 3f, EnemyPosLength = 16f, EnemyPosBlank;

    [SerializeField] Card _cardPrefab;
    [SerializeField] RerollButton _rerollButtonPrefab;
    [SerializeField] RerollButton _chkButtonPrefab;
    [SerializeField] RerollButton _attackButtonPrefab;
    [SerializeField] TMP_Text _textPrefab;
    [SerializeField] SpriteRenderer _HPbarPrefab;
    [SerializeField] AttDefCal _AttDefCalPrefab;
    [SerializeField] Enemy _EnemyPrefab;
    [SerializeField] EnemyHpBar _EnemyHPBarPrefab;
    Vector2 DeckPos(int i) => new Vector2(DeckPosX, DeckPoxY + 0.15f * (Deck.Count - i));
    List<Vector2> HandPos;
    Vector2 GravePos => new Vector2(GravePosX, GravePosY);
    Vector2 ButtonPos(float i) => new Vector2(ButtonPosX, ButtonPosY - ButtonPosBlank * i);
    List<Vector2> EnemyPos;
    List<CardStruct> BaseDeck;
    public List<Enemy> Enemies;
    public List<Card> Deck;
    public List<Card> Hand;
    List<Card> Grave;
    List<Card> Cards;
    Sequence mySequence;

    public List<Enemy> targetEnemy => Enemies.Where(x => x.isTarget).ToList();

    public int area;
    public EnemyType enemyType;
    public int reward => Random.Range(50, 100) * (3 + area) / 3;


    public int Hp, Mhp, Shield, MShield = 50;
    public int Att;
    public int Def;
    public double Rate;
    public int RerollChance;
    TMP_Text RerollText;
    TMP_Text DeckCntTxt;
    TMP_Text GraveCntTxt;

    public static BattleState GameState;
    public void Start()
    {
        mySequence = DOTween.Sequence().SetAutoKill(false);
        BaseDeck = new List<CardStruct>();
        for (int i = 0; i < 20; i++)
        {
            var tmp = (CardDatabase.Instance.RandomCard());
            BaseDeck.Add(tmp);
        }
        Hp = 100; Mhp = 100; Shield = 0;
        area = 1; enemyType = EnemyType.SERVANT;
        RerollChance = 1;
        ChangeState(BattleState.Set);
    }
    private void ChangeState(BattleState battleState)
    {
        GameState = battleState;
        switch (battleState)
        {
            case BattleState.Set:
                setBase();
                break;
            case BattleState.TurnStart:
                TurnStartPhase();
                break;
            case BattleState.Draw:
                DrawPhase();
                break;
            case BattleState.WaitingReroll:
                WatingRerollPhase();
                break;
            case BattleState.Reroll:
                RerollPhase();
                break;
            case BattleState.Attack:
                AttackPhase();
                break;
            case BattleState.EnemyAttack:
                EnemyAttackPhase();
                break;
            case BattleState.EndTurn:
                EndTurnPhase();
                break;
            case BattleState.Reward:
                break;
            case BattleState.Lose:
                break;
        }
    }
    public void FixedUpdate()
    {
        if (DeckCntTxt == null && GraveCntTxt == null) return;
        if (!DeckCntTxt.text.Equals(Deck.Count.ToString()))
            DeckCntTxt.text = Deck.Count.ToString();
        if (!GraveCntTxt.text.Equals(Grave.Count.ToString()))
            GraveCntTxt.text = Grave.Count.ToString();
        
    }
    public void setBase()
    {
        Att = 0; Def = 0; Rate = 1;
        tmpAtt = 0; tmpDef = 0; tmpRate = 1;
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

        DeckCntTxt = Instantiate(_textPrefab, new Vector2(DeckPos(0).x+0.5f,DeckPos(0).y-0.5f), Quaternion.identity);
        DeckCntTxt.fontSize =  9;
        MeshRenderer meshRenderer = DeckCntTxt.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = 200;

        GraveCntTxt = Instantiate(_textPrefab, new Vector2(GravePosX, GravePosY-0.5f), Quaternion.identity);
        DeckCntTxt.fontSize = 9;
        meshRenderer = DeckCntTxt.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = 200;

        RerollText = Instantiate(_textPrefab, ButtonPos(1.6f), Quaternion.identity);
        RerollText.text = "Chance : " + RerollChance.ToString();
        RerollText.fontSize = 2;
        var cnt = 0;
        Sequence sq = DOTween.Sequence().SetAutoKill(false);
        foreach (var tmpCard in BaseDeck)
        {
            var tmp = Instantiate(_cardPrefab, new Vector2(0, 0), Quaternion.identity);
            int Layer = 50 + (BaseDeck.Count - cnt) * 3;
            tmp.setLayer(0, Layer);
            tmp.Set(tmpCard);
            Deck.Add(tmp);
            moveCard(sq, tmp, DeckPos(0), 1f/(float)BaseDeck.Count, true, false);
            cnt++;
        }
        var final_enemylist = EnemyManager.Instance.final_enemylist(area, enemyType);
        EnemyPosBlank = EnemyPosLength / (final_enemylist.Count + 1);
        cnt = 0;
        foreach (var enemy in final_enemylist)
        {
            cnt++;
            var enemyposTmp = new Vector2(EnemyPosX + EnemyPosBlank * cnt, EnemyPosY);
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
            ChangeState(BattleState.TurnStart);
        });
    }
    
    public void ShuffleDeck(Sequence sq)
    {
        var randomized = Deck.OrderBy(item => Random.value).ToList();
        Deck = randomized;
        deckMoveCard(sq);
    }
    public void TurnStartPhase()
    {
        Sequence sq = DOTween.Sequence();
        Shield = 0;
        foreach (var enemy in Enemies)
        {
            enemy.SetPatternText();
        }
        sq.AppendCallback(() =>
        {
            ChangeState(BattleState.Draw);
        });
    }

    public int tmpAtt;
    public int tmpDef;
    public double tmpRate;
    public void DrawPhase()
    {
        Att = tmpAtt; Def = tmpDef; Rate = tmpRate;
        Sequence sq = DOTween.Sequence();
        while (Hand.Count < 5)
        {
            DrawCard(sq);
            if (Hand.Count + Deck.Count + Grave.Count < 5) break;
        }
        deckMoveCard(sq);
        sq.AppendCallback(() =>
        {
            ChangeState(BattleState.WaitingReroll);
            sq.Kill();
        });
    }
    private List<RerollButton> ChkButtons;
    private List<int> NumberCombi;
    private List<COMPANY> COMCombi;
    private List<TMP_Text> CombiText;
    public void WatingRerollPhase()
    {
        ChkButtons = new List<RerollButton>();
        NumberCombi = new List<int>();
        COMCombi = new List<COMPANY>();
        CombiText = new List<TMP_Text>();
        List<RerollButton> Btns = new List<RerollButton>();
        var attackbtn = Instantiate(_attackButtonPrefab, ButtonPos(1), Quaternion.identity);
        attackbtn.ActionSet(() =>
        {
            foreach (var btn in Btns) Destroy(btn.gameObject);
            foreach (var txt in CombiText) Destroy(txt.gameObject);
            ChangeState(BattleState.Attack);
        });
        Btns.Add(attackbtn);
        var rerollbtn = Instantiate(_rerollButtonPrefab, ButtonPos(0), Quaternion.identity);
        rerollbtn.ActionSet(() =>
        {
            if (RerollChance <= 0) return;
            int chk = 0;
            for (int i = 0; i < Hand.Count; i++) if (isBtnChk(ChkButtons[i])) chk++;
            if (chk == 0) return;
            RerollChance -= 1;
            RerollText.text = "Chance : " + RerollChance.ToString();
            foreach (var btn in Btns) Destroy(btn.gameObject);
            foreach (var txt in CombiText) Destroy(txt.gameObject);
            ChangeState(BattleState.Reroll);
        });
        Btns.Add(rerollbtn);
        for (int i = 0; i < Hand.Count; i++)
        {
            var chkBtn = Instantiate(_chkButtonPrefab, new Vector2(HandPos[i].x, HandPos[i].y - 2.25f), Quaternion.identity);
            chkBtn.ActionSet(() =>
            {
                chkBtn.SpriteChange(isBtnChk(chkBtn) ? CardDatabase.Instance.btn(1) : CardDatabase.Instance.btn(0));
            });
            if (Hand[i].isFixed) { chkBtn.SpriteChange(CardDatabase.Instance.btn(2)); chkBtn.enabled = false; }
            ChkButtons.Add(chkBtn);
            Btns.Add(chkBtn);
            NumberCombi.Add(Hand[i].number);
            COMCombi.Add(Hand[i].Company);
            CardDatabase.Instance.BeforeCardActionFunc(Hand[i].name, Hand[i].Value)();
        }
        var NumberCombiText = Instantiate(_textPrefab, new Vector2(HandPos[1].x - 1f, HandPos[0].y + 2.25f), Quaternion.identity);
        NumberCombiText.text = CardDatabase.Instance.NumCombinationText(NumberCombi);
        int numberCombi = CardDatabase.Instance.NumCombination(NumberCombi);
        switch (numberCombi)
        {
            case 1:
                NumberCombiText.color = Color.gray;
                break;
            case 2:
                NumberCombiText.fontSize = 5;
                break;
            case 3:
                NumberCombiText.fontSize = 6;
                NumberCombiText.color = Color.green;
                break;
            case 4:
                NumberCombiText.fontSize = 7;
                NumberCombiText.color = Color.blue;
                break;
            case 6:
                NumberCombiText.fontSize = 7;
                NumberCombiText.color = Color.blue;
                break;
            case 5:
                NumberCombiText.fontSize = 8;
                NumberCombiText.color = Color.red;
                break;
            case 10:
                NumberCombiText.fontSize = 8;
                NumberCombiText.color = Color.red;
                break;
        }

        CombiText.Add(NumberCombiText);

        var COMCombiText = Instantiate(_textPrefab, new Vector2(HandPos[4].x - 0.5f, HandPos[0].y + 2.25f), Quaternion.identity);
        COMCombiText.text = CardDatabase.Instance.ComCombinationText(COMCombi);
        COMCombiText.fontSize = 7;
        COMCombiText.color = Color.cyan;
        CombiText.Add(COMCombiText);

        Debug.Log(tmpRate);
        AttDefCal(tmpAtt, tmpDef, tmpRate);
    }

    public void AttDefCal(int a, int b, double r)
    {
        Att = 0 + a; Def = 0 + b; Rate = 1;
        Rate = (double)Mathf.Round(((float)CardDatabase.Instance.ComCombination(COMCombi) * (float)CardDatabase.Instance.NumCombiRate(NumberCombi) * (float)r * 100)) / 100;
        foreach (var i in Hand) CardDatabase.Instance.CalCardActionFunc(i.name, i.Value)();
        this.Att = (int)(Rate * Att);
        this.Def = (int)(Rate * Def);
    }
    public bool isBtnChk(RerollButton btn) => btn.btnSprite.Equals(CardDatabase.Instance.btn(0));

    public void RerollPhase()
    {
        Sequence sq = DOTween.Sequence();
        List<Card> rerollCard = new List<Card>();
        for (int i = 0; i < Hand.Count; i++) if (isBtnChk(ChkButtons[i])) rerollCard.Add(Hand[i]);
        foreach (var card in rerollCard)
        {
            Hand.Remove(card);
            if (card.isEthereal) sq.Append(card.transform.DOScale(0, 0.5f).OnComplete(() => { Destroy(card.gameObject); }));
            else
            {
                Grave.Add(card);
                moveCard(sq, card, GravePos, 0.2f, true);
            }
        }
        sq.AppendCallback(() =>
        {
            ChangeState(BattleState.Draw);
            sq.Kill();
        });
    }
    public void AttackPhase()
    {
        Sequence sq = DOTween.Sequence();
        foreach (var card in Hand) CardDatabase.Instance.CardActionFunc(card.name, card.Value)();
        AttDefCal(tmpAtt, tmpDef, tmpRate);
        foreach (var card in Hand)
        {
            sq.Append(card.transform.DOMoveY(card.transform.position.y + 1f, 0.2f));
            if (card.isExhaust)
            {
                sq.Append(card.transform.DOScale(0, 0.2f).OnComplete(() => { Destroy(card.gameObject); }));
            }
            else
            {
                Grave.Add(card);
                moveCard(sq, card, GravePos, 0.2f, true);
            }
        }
        Hand.Clear();
        Shield = Def;
        var AttLeft = Att;
        var target = targetEnemy.Count == 0 ? Enemies[Random.Range(0, Enemies.Count)] : targetEnemy[0];


        int loopcnt = 0;
        sq.AppendCallback(() =>
        {
            while (Enemies.Count > 0 && AttLeft > 0)
            {

                target = targetEnemy.Count == 0 ? Enemies[Random.Range(0, Enemies.Count)] : targetEnemy[0];
                var tmpAttLeft = AttLeft - target._hp;

                target._hp -= AttLeft;
                if (target._hp <= 0)
                {
                    var deadEnemy = target;
                    target._hp = 0;
                    Enemies.Remove(deadEnemy);
                    deadEnemy.transform.DOScale(0, 0.3f).OnComplete(() =>
                    {
                        Destroy(deadEnemy._hpBar.gameObject);
                        Destroy(deadEnemy.gameObject);
                    });
                }
                AttLeft = tmpAttLeft;

                loopcnt++;
                if (loopcnt > 1000)
                {
                    Debug.Log("????");
                    break;
                }
            }
        });

        sq.AppendInterval(0.5f);
        sq.AppendCallback(() =>
        {
            tmpAtt = 0; tmpDef = 0; tmpRate = 1;
            if (RerollChance <= 0) RerollChance++;
            RerollText.text = "Chance : " + RerollChance.ToString();
            if (Enemies.Count == 0)
                ChangeState(BattleState.Reward);
            else
                ChangeState(BattleState.EnemyAttack);
        });
    }
    public int patternCnt;
    public void EnemyAttackPhase()
    {
        Sequence sq = DOTween.Sequence();
        List<int> DamageList = new List<int>();
        foreach (var Enemy in Enemies)
        {
            if (Enemy.Pattern._enemyPattern == EnemyPattern.ATT)
            {
                var dam = Enemy.Pattern._Value;

                int FinalDamage = Shield > dam ? 0 : dam - Shield;
                Shield = Shield > dam ? Shield - dam : 0;
                Debug.Log(dam+"/"+FinalDamage + "/" + Shield);
                sq.AppendCallback(() =>
                {
                    Enemy.transform.DOScale(4f, 0.15f).SetLoops(2, LoopType.Yoyo);
                    Hp -= FinalDamage;
                });
                sq.AppendInterval(0.8f);
            }
        }
        
            
        
        sq.AppendCallback(() => {
            if (Hp <= 0) ChangeState(BattleState.Lose);
            ChangeState(BattleState.EndTurn);
        });
    }
    public void EndTurnPhase()
    {
        foreach (var Enemy in Enemies) Enemy.TurnEnd();
        ChangeState(BattleState.TurnStart);
    }
    public void DrawCard(Sequence sq)
    {
        Card tmpCard;
        if (Deck.Count <= 0)
        {
            foreach (var card in Grave) { moveCard(sq, card, DeckPos(0), 0.1f, true, false); }
            Deck = Grave;
            ShuffleDeck(sq);
            Grave.Clear();
        }
        tmpCard = Deck[0];
        Hand.Add(tmpCard);
        Deck.Remove(tmpCard);
        for (int i = 0; i < Hand.Count; i++) moveCard(sq, Hand[i], HandPos[i], 0.8f, false, true);
    }
    public void moveCard(Sequence seq, Card card, Vector2 v, float duration, bool one)
    {

        if (one)
            seq.Append(card.transform.DOMove(v, duration));
        else
            seq.Join(card.transform.DOMove(v, duration));
    }
    public void moveCard(Sequence seq, Card card, Vector2 v, float duration, bool one, bool cardFlip)
    {
        if (one)
            seq.Append(card.transform.DOMove(v, duration).OnPlay(() => { card.flip(cardFlip); })).AppendInterval(0.1f);
        else
            seq.Join(card.transform.DOMove(v, duration).OnPlay(() => { card.flip(cardFlip); }));
    }
    public void deckMoveCard(Sequence sq)
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].setLayer(Deck.Count - i, 50);
            moveCard(sq, Deck[i], DeckPos(i), 0.05f, true);
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) Hp -= 10;
    }
}
public delegate void Func();
public enum BattleState
{
    Set, TurnStart, Draw, WaitingReroll, Reroll, Attack, EnemyAttack,EndTurn, Reward, Lose
}