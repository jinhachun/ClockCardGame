using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardShop : MonoBehaviour
{
    [SerializeField] Transform _evolveEffectPrefab;
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
            switch (card.tier)
            {
                case (0): return BuyPrice;
                case (1): return BuyPrice*2;
                case (2): return BuyPrice*7;
                case (3): return BuyPrice*20;
                case (4): return BuyPrice*60;

                default:  return 0;
            }
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
    [SerializeField] GameObject _buyButton;
    [SerializeField] GameObject _evolButton;
    [SerializeField] GameObject _recruitButton;
    [SerializeField] GameObject _delButton;

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
        var EvolveEffect = Instantiate(_evolveEffectPrefab, new Vector2(card.transform.position.x, card.transform.position.y-2f), Quaternion.identity);
        int random = Random.Range(0, card.Str.evol.Count);
        int tmp = random;
        card_evol.Set(CardDatabase.Instance.card(card.Str.evol[random]));
        while (card.Str.evol.Count > 1 && random == tmp)
            random = Random.Range(0, card.Str.evol.Count);
        card_evol2.Set(CardDatabase.Instance.card(card.Str.evol[random]));
        if (2*Resource.Instance.VillageLevel["House"] > Random.Range(0, 101))
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
        setFirstCard();
    }
    public void UpdateTxt()
    {
        _text.text = text;
        _buyButtonTxt.text = buyButtonTxt+$" (${BuyPrice})";
        _evolButtonTxt.text = evolButtonTxt + $" (${EvolPrice})";
        _recruitButtonTxt.text = recruitButtonTxt;
        _delButtonTxt.text = delButtonTxt;

        if (!card.gameObject.activeInHierarchy)
        {
            _recruitButton.SetActive(false);
            _delButton.SetActive(false);
            _evolButton.SetActive(false);
            _buyButton.SetActive(true);
        }
        else
        {
            _recruitButton.SetActive(true);
            _delButton.SetActive(true);
            _evolButton.SetActive(true);
            if (card.tier == 5) _evolButton.SetActive(false);
            _buyButton.SetActive(false);
        }
        
    }
    public void setCard(CardStruct cardStruct)
    {
        card.gameObject.SetActive(true);
        Resource.Instance.shopcard = cardStruct;
        evolve(cardStruct);
        evolveUI(cardStruct.evol.Count);

    }
    public void setFirstCard()
    {
        evolveUI(1);
        card_evol.Set(FIRSTCARD);
        percentTxt(100);
        
        
        UpdateTxt();
    }

}
