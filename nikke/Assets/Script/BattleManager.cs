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
    float DeckPosX = 11f, DeckPoxY = -5, HandPosX = -7, HandPosY = -2.75f, HandPosBlank = 3f, GravePosX = -11f, GravePosY = -5f;
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
    [SerializeField] ScrollViewCardBunch _scrollViewCardPrefab;
    [SerializeField] DeckCount _deckCountPrefab;
    [SerializeField] public GameObject _canvas;
    [SerializeField] public Transform _damagePopupPrefab;
    Vector2 DeckPos(int i) => new Vector2(DeckPosX, DeckPoxY + 0.15f * (Deck.Count - i));
    List<Vector2> HandPos;
    Vector2 GravePos => new Vector2(GravePosX, GravePosY);
    Vector2 ButtonPos(float i) => new Vector2(ButtonPosX, ButtonPosY - ButtonPosBlank * i);
    List<Vector2> EnemyPos;
    List<CardStruct> BaseDeck;
    public List<Enemy> Enemies;
    public List<Card> Deck;
    public List<Card> Hand;
    public List<Card> Grave;
    List<Card> Cards;
    Sequence mySequence;
    public List<Enemy> targetEnemy => Enemies.Where(x => x.isTarget).ToList();

    public int area;
    public EnemyType enemyType;
    public int reward =>  Random.Range(10*area+30,30*area+90);


    public int Hp, Mhp, Shield;
    public int Att;
    public int Def;
    public double Rate;
    public int RerollChance;
    TMP_Text RerollText;
    DeckCount DeckCntTxt;
    DeckCount GraveCntTxt;
    ScrollViewCardBunch scrollViewCard;
    Sequence cardAddSequence;

    public static BattleState GameState;
    public void Start()
    {
        mySequence = DOTween.Sequence().SetAutoKill(false);
        Random.InitState(System.DateTime.Now.Millisecond);
        var randomized = Resource.Instance.Deck.OrderBy(item => Random.Range(0,999)).ToList();
        BaseDeck = randomized;

        Hp = Resource.Instance.Hp; Mhp = Resource.Instance.mHp; Shield = 0;
        area = Resource.Instance.Area; enemyType = EnemyType.Mini;
        RerollChance = 1;
        addcardQueue_Deck = new Queue<CardStruct>();
        ChangeState(BattleState.Set);
    }
    public void FixedUpdate()
    {
        if (Mhp < Hp) Hp = Mhp;

        while (addcardQueue_Deck.Count > 0)
        {
            cardAddSequence = DOTween.Sequence().SetAutoKill(false);
            CardStruct str = addcardQueue_Deck.Dequeue();
            var card = Instantiate(_cardPrefab, new Vector2(Random.Range(-3f,3f), Random.Range(-3f,3f)), Quaternion.Euler(0,0,Random.Range(-90,90)));
            card.flip(true);
            card.TouchableChange(false);
            card.Set(str);
            cardAddSequence.AppendInterval(0.3f);
            int Layer = 50 + Deck.Count*3;
            card.setLayer(0, Layer);
            Deck.Add(card);
            moveCard(cardAddSequence, card, DeckPos(0), 0.4f, true, false);
            cardAddSequence.AppendInterval(0.3f);
        }
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
                RewardPhase();
                break;
            case BattleState.Lose:
                break;
        }
    }

    public void setBase()
    {
        Att = 0; Def = 0; Rate = 1;
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

        DeckCntTxt = Instantiate(_deckCountPrefab, new Vector2(DeckPos(0).x+0.85f,DeckPos(0).y-1.2f), Quaternion.identity);
        DeckCntTxt.Set(ref Deck);

        GraveCntTxt = Instantiate(_deckCountPrefab, new Vector2(GravePosX + 0.85f, GravePosY-1.2f), Quaternion.identity);
        GraveCntTxt.Set(ref Grave);

        RerollText = Instantiate(_textPrefab, ButtonPos(1.6f), Quaternion.identity);
        RerollText.text = "Chance : " + RerollChance.ToString();
        RerollText.fontSize = 2;
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
            moveCard(sq, tmp, DeckPos(0), 0.5f/(float)BaseDeck.Count, true, false);
            
            cnt++;
        }
        var final_enemylist = EnemyDatabase.Instance.final_enemylist(area, enemyType);
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
        foreach (Card tmp in Deck)
            tmp.flip(false);
        DeckCntTxt.Set(ref Deck);
        GraveCntTxt.Set(ref Grave);
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
    public int tmpReroll;
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
            foreach(var card in Hand) card.setLayer(0, 50);
            ChangeState(BattleState.WaitingReroll);
            sq.Kill();
        });

    }
    private List<RerollButton> ChkButtons;
    private List<SPECIES> SpeciesCombi;
    private List<TYPE> TypeCombi;
    private List<TMP_Text> CombiText;
    public void WatingRerollPhase()
    {
        
        ChkButtons = new List<RerollButton>();
        SpeciesCombi = new List<SPECIES>();
        TypeCombi = new List<TYPE>();
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
            for (int i = 0; i < Hand.Count; i++) if (RerollPhase_isBtnChk(ChkButtons[i])) chk++;
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
                chkBtn.SpriteChange(RerollPhase_isBtnChk(chkBtn) ? CardDatabase.Instance.btn(1) : CardDatabase.Instance.btn(0));
            });
            if (Hand[i].isFixed) { chkBtn.SpriteChange(CardDatabase.Instance.btn(2)); chkBtn.enabled = false; }
            ChkButtons.Add(chkBtn);
            Btns.Add(chkBtn);
            SpeciesCombi.Add(Hand[i].Species);
            TypeCombi.Add(Hand[i].Type);
            CardDatabase.Instance.BeforeCardActionFunc(Hand[i])();
        }
        var SpeciesCombiText = Instantiate(_textPrefab, new Vector2(HandPos[1].x - 1f, HandPos[0].y + 2.25f), Quaternion.identity);
        SpeciesCombiText.text = CardDatabase.Instance.SpeciesCombinationText(SpeciesCombi);
        int speciesCombi = CardDatabase.Instance.SpeciesCombination(SpeciesCombi);
        WatingRerollPhase_TextSet(speciesCombi, SpeciesCombiText);
        CombiText.Add(SpeciesCombiText);

        var TypeCombiText = Instantiate(_textPrefab, new Vector2(HandPos[4].x - 0.5f, HandPos[0].y + 2.25f), Quaternion.identity);
        TypeCombiText.text = CardDatabase.Instance.TypeCombinationText(TypeCombi);
        int typeCombi = CardDatabase.Instance.TypeCombination(TypeCombi);
        WatingRerollPhase_TextSet(typeCombi, TypeCombiText);
        CombiText.Add(TypeCombiText);
        
        AttDefCal(tmpAtt, tmpDef, tmpRate);
    }
    public void WatingRerollPhase_TextSet(int combi,TMP_Text text)
    {
        switch (combi)
        {
            case 0:
                text.color = Color.gray;
                break;
            case 1:
                break;
            case 2:
                text.fontSize = 5;
                break;
            case 3:
                text.fontSize = 6;
                text.color = Color.green;
                break;
            case 4:
                text.fontSize = 7;
                text.color = Color.blue;
                break;
            case 5:
                text.fontSize = 7;
                text.color = Color.blue;
                break;
            case 6:
                text.fontSize = 8;
                text.color = Color.red;
                break;
        }
    }
    
    public void rerolladd(int n,bool tmp)
    {
        if (tmp)
            tmpReroll++;
        else
            RerollChance++;
        RerollText.text = "Chance : " + RerollChance.ToString();
    }

    public void AttDefCal(int a, int b, double r)
    {
        Att = 0 + a; Def = 0 + b; Rate = 1;
        float typeRate = CardDatabase.Instance.TypeCombiRate(TypeCombi);
        float speciesRate = CardDatabase.Instance.SpeciesCombiRate(SpeciesCombi);
        float tmpRate = typeRate * speciesRate * (float)r;
        Rate *= (double)Mathf.Round((tmpRate*100f))/100f;
        foreach (var i in Hand) { Att += (int)(Rate*i.Stat.attack); Def += i.Stat.defence; }
        this.Def = (int)(Rate * (double)Def);
    }
    public bool RerollPhase_isBtnChk(RerollButton btn) => btn.btnSprite.Equals(CardDatabase.Instance.btn(0));

    public void RerollPhase()
    {
        Sequence sq = DOTween.Sequence();
        List<Card> rerollCard = new List<Card>();
        for (int i = 0; i < Hand.Count; i++) if (RerollPhase_isBtnChk(ChkButtons[i])) rerollCard.Add(Hand[i]);
        foreach (var card in rerollCard)
        {
            Hand.Remove(card);
            if (card.isEthereal) sq.Append(card.transform.DOScale(0, 0.5f).OnComplete(() => { Destroy(card.gameObject); }));
            else
            {
                Grave.Add(card);
                card.setLayer(0,50 + Grave.Count*3);
                card.TouchableChange(false);
                float ranTmpx = Random.Range(-30, 30) / 100f+ GravePosX;
                float ranTmpy = Random.Range(-30, 30) / 100f+ GravePosY;
                moveCard(sq, card, new Vector2(ranTmpx, ranTmpy), 0.2f, true);
                sq.Join(card.transform.DORotate(new Vector3(0, 0, Random.Range(-60, 60)), 0.2f));
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
        foreach (var card in Hand) CardDatabase.Instance.CardActionFunc(card)();
        AttDefCal(tmpAtt, tmpDef, tmpRate);
        foreach (var card in Hand)
        {
            sq.Append(card.transform.DOMoveY(card.transform.position.y + 1f, 0.2f));
            bool critical = (10+Resource.Instance.VillageLevel["Church"] * 2 )>= Random.Range(0, 100);
            
                sq.AppendCallback(() =>
                {
                if (Enemies.Count > 0)
                    {
                         var target = targetEnemy.Count == 0 ? Enemies[Random.Range(0, Enemies.Count)] : targetEnemy[0];
                        enemyDamage((int)(card.Stat.attack * Rate), critical, target);
                    }
                });
            
            if (card.isExhaust)
            {
                sq.Append(card.transform.DOScale(0, 0.2f).OnComplete(() => { Destroy(card.gameObject); }));
            }
            else
            {
                Grave.Add(card);
                card.TouchableChange(false);
                moveCard(sq, card, GravePos, 0.2f, true);
                card.setLayer(0, 50 + Grave.Count*3);
            }
            
        }
        Hand.Clear();
        Shield = Def;
        

        sq.AppendInterval(0.5f);
        sq.AppendCallback(() =>
        {
            foreach(var enem in Enemies)
            {
                if (enem._hp <= 0) Enemies.Remove(enem);
            }
            tmpAtt = 0; tmpDef = 0; tmpRate = 1;
            if(RerollChance<=0) RerollChance++;
            RerollChance += tmpReroll;
            tmpReroll = 0;
            RerollText.text = "Chance : " + RerollChance.ToString();
            if (Enemies.Count == 0)
                ChangeState(BattleState.Reward);
            else
                ChangeState(BattleState.EnemyAttack);
        });
    }
    public void enemyDamage(int dam,bool critical,Enemy target)
    {
        var AttLeft = !critical ? dam : dam * 2;
        int loopcnt = 0;

        while (Enemies.Count > 0 && AttLeft > 0)
        {

            target = targetEnemy.Count == 0 ? Enemies[Random.Range(0, Enemies.Count)] : targetEnemy[0];
            var tmpAttLeft = AttLeft - target._hp;

            target._hp -= AttLeft;
            bool lethal = (target._hp <= 0);
            if (lethal)
            {
                DamagePopup.Create(target.transform.position, AttLeft + target._hp, critical, lethal);
                var deadEnemy = target;
                target._hp = 0;
                Enemies.Remove(deadEnemy);
                deadEnemy._img.transform.DOScale(0, 0.3f).OnComplete(() =>
                {
                    Destroy(deadEnemy._hpBar.gameObject);
                    Destroy(deadEnemy.gameObject);
                });
            }
            else
                DamagePopup.Create(target.transform.position, AttLeft, critical, lethal);
            AttLeft = tmpAttLeft;

            loopcnt++;
            if (loopcnt > 1000)
            {
                Debug.Log("????");
                break;
            }
        }
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
                takeDamage(sq, Enemy);
            }else if(Enemy.Pattern._enemyPattern == EnemyPattern.BUFF)
            {
                Enemy.setAttackBuff();
                sq.AppendCallback(() =>
                {
                    Enemy.transform.DOMoveY(Enemy.transform.position.y+0.35f, 0.15f).SetLoops(4, LoopType.Yoyo);
                });
                sq.AppendInterval(0.8f);
            }
        }
        
            
        
        sq.AppendCallback(() => {
            if (Hp <= 0) ChangeState(BattleState.Lose);
            ChangeState(BattleState.EndTurn);
        });
    }
    public void takeDamage(int dam)
    {
        int FinalDamage = Shield > dam ? 0 : dam - Shield;
        Shield = Shield > dam ? Shield - dam : 0;
        if (FinalDamage > 0)
        {
             Hp -= FinalDamage;
        }
        if (Hp <= 0) ChangeState(BattleState.Lose);
    }
    public void takeDamage(Sequence sq, Enemy Enemy)
    {
        var dam = Enemy.damage;

        int FinalDamage = Shield > dam ? 0 : dam - Shield;
        Shield = Shield > dam ? Shield - dam : 0;
        if (FinalDamage > 0)
        {
            sq.AppendCallback(() =>
            {
                Enemy.transform.DOScale(4f, 0.15f).SetLoops(2, LoopType.Yoyo);
                Hp -= FinalDamage;
            });
            sq.AppendInterval(0.8f);
        }
        else
        {
            sq.AppendCallback(() =>
            {
                Enemy.transform.DOScale(2f, 0.2f).SetLoops(2, LoopType.Yoyo);
            });
            sq.AppendInterval(0.2f);
        }
    }
    public void EndTurnPhase()
    {
        foreach (var Enemy in Enemies) Enemy.TurnEnd();
        if (Hp <= 0) ChangeState(BattleState.Lose);
        else if (Enemies.Count == 0) ChangeState(BattleState.Reward);
        else
            ChangeState(BattleState.TurnStart);
    }
    public void RewardPhase()
    {
        Resource.Instance.money += reward;
        Resource.Instance.StageUp();
        Resource.Instance.setHp(Hp);
        SceneManager.LoadScene("MainScene");
        SceneManager.LoadScene("EventScene", LoadSceneMode.Additive);

    }
    Queue<CardStruct> addcardQueue_Deck;
    public void AddCard(Queue<CardStruct> list, bool deck)
    {
        Sequence sq = DOTween.Sequence();
        while (list.Count > 0)
        {
            addcardQueue_Deck.Enqueue(list.Dequeue());
        }
        
    }
    public void DrawCard(Sequence sq)
    {
        Card tmpCard;
        if (Deck.Count <= 0)
        {
            foreach (var card in Grave) { moveCard(sq, card, DeckPos(0), 0.5f, false, false); card.TouchableChange(true); }
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
        if (v.Equals(GravePos))
        {
            seq.Join(card.transform.DORotate(new Vector3(0, 0, Random.Range(-60,60)), 0.2f));
        }
        else {
            if (card.transform.rotation.z != 0)
                card.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void moveCard(Sequence seq, Card card, Vector2 v, float duration, bool one, bool cardFlip)
    {
        if (one) {
            seq.Append(card.transform.DOMove(v, duration));
            if(card.transform.rotation.z!=0)
                seq.Join(card.transform.DORotate(new Vector3(0,0,0), 0.2f));
            seq.AppendCallback(()=>card.flip(cardFlip));
            seq.AppendInterval(0.05f);
        }
        else
            seq.Join(card.transform.DOMove(v, duration).OnPlay(() => { card.flip(cardFlip); }));
    }
    public void deckMoveCard(Sequence sq)
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].setLayer(Deck.Count - i, 50);
            moveCard(sq, Deck[i], DeckPos(i), 0.02f, true);
        }
    }
    public void deckCntClick(List<Card> list)
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
}
public delegate void Func();
public enum BattleState
{
    Set, TurnStart, Draw, WaitingReroll, Reroll, Attack, EnemyAttack,EndTurn, Reward, Lose
}