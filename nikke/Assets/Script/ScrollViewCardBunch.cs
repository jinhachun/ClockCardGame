using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewCardBunch : MonoBehaviour
{
    [SerializeField] RectTransform _contents;
    [SerializeField] RectTransform _backGround;
    [SerializeField] RectTransform _Cards;
    [SerializeField] Card _card;
    List<Card> cardBunch;
    List<Card> cardsMade;
    int x = -400;
    int y = -175;
    int xBlank = 200;
    int yBlank = 250;
    public void OnEnable()
    {
        cardsMade = new List<Card>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) delete();
    }
    public void delete()
    {
        foreach (var card in cardsMade)
            Destroy(card.gameObject);
        cardsMade = new List<Card>();
        this.gameObject.SetActive(false);
    }
    public void Set(List<Card> cards)
    {
        this.cardBunch = cards.OrderBy(item => Random.value).ToList();
        var xpos = x; var ypos = y;
        foreach(var card in cardBunch)
        {
            var cardTmp = Instantiate(_card,new Vector2(xpos, ypos),Quaternion.identity);
            cardTmp.transform.localPosition = new Vector2(xpos, ypos);
            cardTmp.transform.localScale = new Vector2(70f, 70f);
            cardTmp.transform.SetParent(_contents.transform,false);
            xpos += xBlank;
            if (xpos >= x+5*xBlank) { xpos = x; ypos -= yBlank; }
            cardTmp.Set(card.Str);
            cardTmp.layer = 505;
            cardTmp.setLayer(1,505);
            cardTmp.transform.parent = _Cards.transform;

            cardsMade.Add(cardTmp);
        }
        _backGround.localScale = new Vector2(1100, ((cards.Count-1)/5+1)*yBlank + 150);
        _contents.sizeDelta = new Vector2(_contents.rect.height,_backGround.transform.localScale.y);

    }
}
