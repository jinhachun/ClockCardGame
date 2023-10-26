using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardShop : MonoBehaviour
{
    [SerializeField] GameObject BackGround;
    [SerializeField] GameObject BackGround_evol;
    [SerializeField] Card _CardPrefab;
    [SerializeField] TMP_Text _evolPer;
    [SerializeField] TMP_Text _evolPer2;
    // Start is called before the first frame update
    public int BuyPrice;
    public int EvolPrice
    {
        get
        {
            if (card == null) return 0;
            else return BuyPrice * card.tier * card.tier * card.tier * (50 - Resource.Instance.VillageLevel["House"]*4)/100;
        }
    }
    [SerializeField] public Card card;
    public Card card_evol;
    public Card card_evol2;
    public int evolPer;
    public int evolPer2;

    [SerializeField] TMP_Text _text;
    [SerializeField] TMP_Text _buyButtonTxt;
    [SerializeField] TMP_Text _evolButtonTxt;
    [SerializeField] TMP_Text _recruitButtonTxt;
    [SerializeField] TMP_Text _delButtonTxt;
    public string text;
    public string buyButtonTxt;
    public string evolButtonTxt;
    public string recruitButtonTxt;
    public string delButtonTxt;

    public void Start()
    {
        evolveUI(false);
        UpdateTxt();
    }
    public void Buy()
    {
        if (card.gameObject.activeInHierarchy) return;
        if (Resource.Instance.money >= BuyPrice) Resource.Instance.money -= BuyPrice;
        else return;
        card.gameObject.SetActive(true);
        evolve(CardDatabase.Instance.card("동글이"));
        evolveUI(true);
        card.setLayer(1,5);
        card_evol.setLayer(1, 5);
        card_evol2.setLayer(1, 5);
        card.Touchable = false;
        card_evol.Touchable = false;
        card_evol2.Touchable = false;
        

        int random = Random.Range(0, 100);
        percentTxt(random);
        UpdateTxt();
    }
    public void percentTxt(int random)
    {
        evolPer = random;
        evolPer2 = 100 - evolPer;
        _evolPer.text = evolPer + "%";
        _evolPer2.text = evolPer2 + "%";
    }
    public void Evolve()
    {
        if (!card.gameObject.activeInHierarchy) return;
        if (Resource.Instance.money < EvolPrice) return;
        if (card.Str.evol.Count == 0) return;

        Resource.Instance.money -= BuyPrice;
        int random = Random.Range(0, 101);
        if (random <= evolPer) evolve(card_evol.Str);
        else evolve(card_evol2.Str);
        UpdateTxt();
    }
    private void evolve(CardStruct c)
    {
        card.Set(c);
        if (card.Str.evol.Count == 0) {
            evolveUI(false);
            return ;
        }
        int random = Random.Range(0, card.Str.evol.Count);
        int tmp = random;
        card_evol.Set(CardDatabase.Instance.card(card.Str.evol[random]));
        while (card.Str.evol.Count > 1 && random == tmp)
            random = Random.Range(0, card.Str.evol.Count);
        card_evol2.Set(CardDatabase.Instance.card(card.Str.evol[random]));
    }
    private void evolveUI(bool a)
    {
        card_evol.gameObject.SetActive(a);
        card_evol2.gameObject.SetActive(a);
        _evolPer.enabled = a;
        _evolPer2.enabled = a;

    }
    public void Recruit()
    {
        if (!card.gameObject.activeInHierarchy) return;
        Resource.Instance.Deck.Add(card.Str);
        Delete();
    }
    public void Delete()
    {
        if (!card.gameObject.activeInHierarchy) return;
        card.gameObject.SetActive(false);
        card_evol.Set(CardDatabase.Instance.card("동글이"));
        card_evol2.Set(CardDatabase.Instance.card("동글이"));
        percentTxt(100);
        UpdateTxt();
    }
    public void UpdateTxt()
    {
        _text.text = text;
        _buyButtonTxt.text = buyButtonTxt+$" (${BuyPrice})";
        _evolButtonTxt.text = evolButtonTxt + $" (${EvolPrice})";
        _recruitButtonTxt.text = recruitButtonTxt;
        _delButtonTxt.text = delButtonTxt;
    }

}
