using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckCount : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    
    List<Card> cards;
    bool isOver;
    public void Update()
    {
        if (!cards.Count.ToString().Equals(_text.text))
        {
            _text.text = cards.Count.ToString();
        }
        if (isOver && Input.GetMouseButtonDown(0))
        {
            BattleManager.Instance.Deck_chkCnt(cards);
        }
    }
    public void Set(ref List<Card> list)
    {
        this.cards = list;
    }
    void OnMouseOver()
    {
        isOver = true;

    }

    void OnMouseExit()
    {
        isOver = false;
    }
}
