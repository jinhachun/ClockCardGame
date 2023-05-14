using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
    float DeckPosX=11f, DeckPoxY=-5,HandPosX=-7,HandPosY=-3f,HandPosBlank=3f,GravePosX=-12f,GravePosY=-5f;

    [SerializeField] Card _cardPrefab;
    [SerializeField] SpriteRenderer _rerollButtonPrefab;
    Vector2 DeckPos(int i)=> new Vector2(DeckPosX,DeckPoxY+0.15f*(Deck.Count-i));
    List<Vector2> HandPos;
    Vector2 GravePos => new Vector2(GravePosX, GravePosY);
    List<Card> BaseDeck;
    List<Card> Deck;
    List<Card> Hand;
    List<Card> Grave;
    List<Card> Cards;
    Sequence mySequence;
    public void Start()
    {
        mySequence = DOTween.Sequence().SetAutoKill(false);
        BaseDeck = new List<Card>();
        for(int i = 0; i < 10; i++)
        {
            var tmp = _cardPrefab;
            BaseDeck.Add(tmp);
            tmp.Set(CardDatabase.Instance.card("½Ã¿øÇÑ ¸ÆÁÖ"));
        }
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
                WatingReroll();
                break;
            case BattleState.Reroll:
                break;
            case BattleState.Attack:
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
        Deck = new List<Card>();
        Hand = new List<Card>();
        Grave = new List<Card>();
        HandPos = new List<Vector2>();
        for (int i = 0; i < 5; i++)
            HandPos.Add(new Vector2(HandPosX + i * HandPosBlank, HandPosY));

        var cnt = 0 ;
        foreach(var tmpCard in BaseDeck)
        {
            var tmp = Instantiate(_cardPrefab, new Vector2(0,0), Quaternion.identity);
            tmp.Set(tmpCard.Str);
            Deck.Add(tmp);
            tmp.flip();
            moveCard(tmp, DeckPos(0),0.2f, true,false);
            cnt++;
        }
        ShuffleDeck();

        ChangeState(BattleState.Draw);
    }
    public void ShuffleDeck()
    {
        var randomized = Deck.OrderBy(item => Random.value).ToList();
        Deck = randomized;
        deckMoveCard();
    }
    
    public void DrawPhase()
    {
        while (Hand.Count < 5)
        {
            DrawCard();
            if (Hand.Count + Deck.Count + Grave.Count < 5) break;
        }
        deckMoveCard();
        ChangeState(BattleState.WaitingReroll);
    }
    public void WatingReroll()
    {
        var rerollbtn = Instantiate(_rerollButtonPrefab);
    }
    public void DrawCard()
    {
        Card tmpCard;
        if (Deck.Count == 0)
        {
            foreach (var card in Grave) { moveCard(card, DeckPos(0),0.2f,true,false); }
            Deck = Grave;
            ShuffleDeck();
            Grave.Clear();
        }
        tmpCard = Deck[0];
        Hand.Add(tmpCard);
        Deck.Remove(tmpCard);
        for (int i = 0; i < Hand.Count; i++) moveCard(Hand[i], HandPos[i],0.7f,false,true); 
    }
    public void moveCard(Card card,Vector2 v,float duration,bool one)
    {
        if(one)
            mySequence.Append(card.transform.DOMove(v, duration));
        else
            mySequence.Join(card.transform.DOMove(v, duration));
    }
    public void moveCard(Card card, Vector2 v, float duration, bool one,bool cardFlip)
    {
        if (one)
            mySequence.Append(card.transform.DOMove(v, duration).OnPlay(() => { card.flip(cardFlip); }));
        else
            mySequence.Join(card.transform.DOMove(v, duration).OnPlay(() => { card.flip(cardFlip); }));
    }
    public void deckMoveCard()
    {

        for (int i = 0; i < Deck.Count; i++)
        {
            Deck[i].setLayer(Deck.Count - i);
            moveCard(Deck[i], DeckPos(i), 0.05f, true);
        }
    }
}
public enum BattleState
{
    Set, Draw, WaitingReroll, Reroll, Attack, EnemyAttack, Reward, Lose
}