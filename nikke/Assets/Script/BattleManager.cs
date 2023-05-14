using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] Card _cardPrefab;
    List<Card> Deck;
    List<Card> Hand;
    public void Start()
    {
        var card = Instantiate(_cardPrefab);
        card.Set(CardDatabase.Instance.card("시원한 맥주"));
    }

}
