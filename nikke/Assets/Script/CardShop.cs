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
    public int BuyPrice;
    public int EvolPrice
    {
        get
        {
            if (card == null) return 0;
            else return BuyPrice * card.tier * card.tier * card.tier ;
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

    CardStruct FIRSTCARD => CardDatabase.Instance.card("µ¿±ÛÀÌ");
    public void Start()
    {
        evolveUI(1);
        if (Resource.Instance.shopcard._name.Equals(FIRSTCARD._name))
        {
            card_evol.Set(Resource.Instance.shopcard);
            percentTxt(100);
        }
        else
        {
            setCard(Resource.Instance.shopcard);
            int random = Random.Range(50, 100);
            if (card.Str.evol.Count == 1)
                random = 100;
            percentTxt(random);
            evolveUI(card.Str.evol.Count);
        }
        UpdateTxt();
        card.setLayer(1, 5);
        card_evol.setLayer(1, 5);
        card_evol2.setLayer(1, 5);
        card.Touchable = false;
        card_evol.Touchable = false;
        card_evol2.Touchable = false;
    }
    public void Buy()
    {
        if (card.gameObject.activeInHierarchy) return;
        if (Resource.Instance.money >= BuyPrice) Resource.Instance.money -= BuyPrice;
        else return;
        card.gameObject.SetActive(true);
        var tmp = FIRSTCARD;
        setCard(tmp);
        

        int random = Random.Range(50, 100);
        if (card.Str.evol.Count == 1)
            random = 100;
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

        Resource.Instance.money -= EvolPrice;
        int random = Random.Range(0, 101);
        if (random <= evolPer) setCard(card_evol.Str);
        else setCard(card_evol2.Str);

        random = Random.Range(50, 100);
        if (card.Str.evol.Count == 1)
            random = 100;
        percentTxt(random);
        evolveUI(card.Str.evol.Count);
        UpdateTxt();
    }
    private void evolve(CardStruct c)
    {
        card.Set(c);
        if (card.Str.evol.Count == 0) {
            evolveUI(0);
            return;
        }
        
        int random = Random.Range(0, card.Str.evol.Count);
        int tmp = random;
        card_evol.Set(CardDatabase.Instance.card(card.Str.evol[random]));
        while (card.Str.evol.Count > 1 && random == tmp)
            random = Random.Range(0, card.Str.evol.Count);
        card_evol2.Set(CardDatabase.Instance.card(card.Str.evol[random]));
        if (3*Resource.Instance.VillageLevel["House"] > Random.Range(0, 101))
        {
            evolve(CardDatabase.Instance.card(card.Str.evol[random]));
        }

    }
    private void evolveUI(int n)
    {
        bool a = false;
        bool b = false;
        if (n >= 1) a = true;
        if (n >= 2) b = true;
        card_evol.gameObject.SetActive(a);
        card_evol2.gameObject.SetActive(b);
        _evolPer.enabled = a;
        _evolPer2.enabled = b;

    }
    public void Recruit()
    {
        if (!card.gameObject.activeInHierarchy) return;
        Resource.Instance.Deck_Add(card.Str._name);
        Resource.Instance.shopcard = FIRSTCARD;
        Delete();
    }
    public void Delete()
    {
        if (!card.gameObject.activeInHierarchy) return;
        card.gameObject.SetActive(false);
        card_evol.Set(FIRSTCARD);
        card_evol2.Set(FIRSTCARD);
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
    public void setCard(CardStruct cardStruct)
    {
        card.gameObject.SetActive(true);
        Resource.Instance.shopcard = cardStruct;
        evolve(cardStruct);
        evolveUI(cardStruct.evol.Count);

    }

}
