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

    [SerializeField] Card _cardPrefab;
    [SerializeField] RerollButton _rerollButtonPrefab;
    [SerializeField] RerollButton _chkButtonPrefab;
    [SerializeField] RerollButton _attackButtonPrefab;
    [SerializeField] TMP_Text _textPrefab;
    [SerializeField] SpriteRenderer _HPbarPrefab;
    [SerializeField] AttDefCal _AttDefCalPrefab;
    Vector2 DeckPos(int i) => new Vector2(DeckPosX, DeckPoxY + 0.15f * (Deck.Count - i));
    List<Vector2> HandPos;
    Vector2 GravePos => new Vector2(GravePosX, GravePosY);
    Vector2 ButtonPos(int i) => new Vector2(ButtonPosX, ButtonPosY - ButtonPosBlank * i);
    List<CardStruct> BaseDeck;
    public List<Card> Deck;
    public List<Card> Hand;
    List<Card> Grave;
    List<Card> Cards;
    Sequence mySequence;

    public int Hp, Mhp, Shield,MShield = 100;
    public int Att;
    public int Def;
    public double Rate;
    public void Start()
    {
        mySequence = DOTween.Sequence().SetAutoKill(false);
        BaseDeck = new List<CardStruct>();
        for (int i = 0; i < 10; i++)
        {
            var tmp = (CardDatabase.Instance.RandomCard());
            BaseDeck.Add(tmp);
        }
        Hp = 100; Mhp = 100; Shield = 100;
        ChangeState(BattleState.Set);
    }
    private void ChangeState(BattleState battleState)
    {
        switch (battleState)
        {
            case BattleState.Set:
                setBase();
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
                break;
            case BattleState.Reward:
                break;
            case BattleState.Lose:
                break;
        }
    }
    public void setBase()
    {
        Att = 0; Def = 0; Rate = 1;
        Deck = new List<Card>();
        Hand = new List<Card>();
        Grave = new List<Card>();
        HandPos = new List<Vector2>();
        for (int i = 0; i < 5; i++)
            HandPos.Add(new Vector2(HandPosX + i * HandPosBlank, HandPosY));

        var HpBar = Instantiate(_HPbarPrefab, new Vector2(HandPos[0].x-1.5f, HandPos[0].y - 3.5f), Quaternion.identity);
        var AttDef = Instantiate(_AttDefCalPrefab, ButtonPos(3), Quaternion.identity); ;
        var cnt = 0;
        Sequence sq = DOTween.Sequence().SetAutoKill(false);
        foreach (var tmpCard in BaseDeck)
        {
            var tmp = Instantiate(_cardPrefab, new Vector2(0, 0), Quaternion.identity);
            int Layer = 50 + (BaseDeck.Count - cnt) * 3;
            tmp.setLayer(0, Layer);
            tmp.Set(tmpCard);
            Deck.Add(tmp);
            moveCard(sq, tmp, DeckPos(0), 0.1f, true, false);
            cnt++;
        }
        sq.OnComplete(() =>
        {
            ShuffleDeck(sq);
        });
        sq.OnComplete(() =>
        {
            sq.Kill();
            ChangeState(BattleState.Draw);
        });
    }
    public void ShuffleDeck(Sequence sq)
    {
        var randomized = Deck.OrderBy(item => Random.value).ToList();
        Deck = randomized;
        deckMoveCard(sq);
    }

    public void DrawPhase()
    {
        Att = 0; Def = 0; Rate = 1;
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
        }
        var NumberCombiText = Instantiate(_textPrefab, new Vector2(HandPos[1].x-1f, HandPos[0].y + 2.25f), Quaternion.identity);
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

        var COMCombiText = Instantiate(_textPrefab, new Vector2(HandPos[4].x-0.5f, HandPos[0].y + 2.25f), Quaternion.identity);
        COMCombiText.text = CardDatabase.Instance.ComCombinationText(COMCombi);
        COMCombiText.fontSize = 7;
        COMCombiText.color = Color.cyan;
        CombiText.Add(COMCombiText);


        AttDefCal();
    }
    public void AttDefCal()
    {
        Att = 0;Def = 0;Rate = 1;
        Rate = 1 * CardDatabase.Instance.ComCombination(COMCombi) * CardDatabase.Instance.NumCombiRate(NumberCombi);
        foreach (var i in Hand) CardDatabase.Instance.BeforeCardActionFunc(i.name, i.Value)();
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
            Grave.Add(card);
            moveCard(sq, card, GravePos, 0.2f, true);
        }
        sq.AppendCallback(()=>
        {
            ChangeState(BattleState.Draw);
            sq.Kill();
        });
    }
    public void AttackPhase()
    {
        Sequence sq = DOTween.Sequence();
        foreach(var card in Hand)
        {
            card.Action = CardDatabase.Instance.CardActionFunc(card.name, card.Value);
            card.Action();
        }
        AttDefCal();
        foreach (var card in Hand)
        {
            sq.Append(card.transform.DOMoveY(card.transform.position.y + 1f,0.5f));
            if (card.isExhaust)
            {
                sq.Append(card.transform.DOScale(0, 0.5f).OnComplete(() => { Destroy(card.gameObject); }));
            }
            else
            {
                moveCard(sq, card, GravePos, 0.2f, true);
            }
        }
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
    Set, Draw, WaitingReroll, Reroll, Attack, EnemyAttack, Reward, Lose
}