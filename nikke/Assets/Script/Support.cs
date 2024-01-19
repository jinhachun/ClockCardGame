using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using DG.Tweening;

public class Support : MonoBehaviour
{
    [SerializeField] GameObject _Content;
    [SerializeField] GameObject _Square;

    [SerializeField] Card _cardAddPrefab;
    [SerializeField] Card _cardDeletePrefab;
    [SerializeField] Card _cardEvolvePrefab;

    [SerializeField] TMP_Text _RerollPrice;
    [SerializeField] TMP_Text _HealPrice;
    [SerializeField] TMP_Text _AddPrice;
    [SerializeField] TMP_Text _DeletePrice;
    [SerializeField] TMP_Text _EvolvePrice;

    public int Price => 30;
    public int RerollPrice = 0;
    public int HealPrice => Price* (Resource.Instance.SupportPrice["Heal"]+2);
    public int AddPrice => Price* (Resource.Instance.SupportPrice["Add"] + 3);
    public int DeletePrice => Price  * (Resource.Instance.SupportPrice["Delete"]* 2 + 3);
    public int EvolvePrice => Price  * (Resource.Instance.SupportPrice["Evolve"]+ _cardEvolvePrefab.Str._tier*2);
    void Start()
    {
        RerollPrice = Price;
        Set();
    }
    public void Set()
    {
        var rare = Random.Range(0, 101) >= 75;
        if (rare)
        {
            List<CardStruct> rareList = CardDatabase.Instance.cardByRareTierList(3);
            CardStruct rareCard = rareList[Random.Range(0, rareList.Count)];
            _cardAddPrefab.Set(rareCard);
        }
        else
        {
            _cardAddPrefab.Set(CardDatabase.Instance.cardByTier(3));
        }
        var cardDeleteTmp = Resource.Instance.Deck.Count == 0 ? CardDatabase.Instance.card_token("X") : Resource.Instance.Deck[Random.Range(0, Resource.Instance.Deck.Count)];
        _cardDeletePrefab.Set(cardDeleteTmp);
        int rareChk = 0;
        rare = Random.Range(0, 101) >= 75;
        if (rare)
        {
            var rareCardDeck = Resource.Instance.Deck.Where(x => x.isRare && x._tier != 5).ToList();
            if (rareCardDeck.Count != 0)
                _cardEvolvePrefab.Set(rareCardDeck[Random.Range(0, rareCardDeck.Count)]);
            else
                rareChk = -1;
        }
        if(!rare || rareChk == -1)
        {
            var CardDeck = Resource.Instance.Deck.Where(x => x._tier != 5).ToList();
            if (CardDeck.Count != 0)
                _cardEvolvePrefab.Set(CardDeck[Random.Range(0, CardDeck.Count)]);
            else
            {
                _cardEvolvePrefab.Set(CardDatabase.Instance.card_token("X"));
                _Content.transform.Find("Button_Evolve").gameObject.SetActive(false);
            }
        }
        UpdateText();
    }
    public void UpdateText()
    {
        _RerollPrice.text = $"{RerollPrice}원";
        _HealPrice.text = $"{HealPrice}원";
        _AddPrice.text = $"{AddPrice}원";
        _DeletePrice.text = $"{DeletePrice}원";
        _EvolvePrice.text = $"{EvolvePrice}원";
    }
    public void Quit()
    {
        UpdateText();
        _Square.transform.DOMoveY(0.15f, 1.5f);
        _cardAddPrefab.TouchableChange(false);
        _cardDeletePrefab.TouchableChange(false);
        _cardEvolvePrefab.TouchableChange(false);
        _Content.SetActive(false);
    }
    public void QuitChk()
    {
        Set();
        bool chk = Random.Range(0, 101) > 50;
        UpdateText();
        if (chk) Quit();
    }
    public void ButtonAction_Reroll()
    {
        if (Resource.Instance.money >= RerollPrice)
        {
            Resource.Instance.money -= RerollPrice;
            RerollPrice += Price;
            Set();
            UpdateText();
        }
    }
    public void ButtonAction_Add()
    {
        if (Resource.Instance.money >= AddPrice)
        {
            Resource.Instance.money -= AddPrice;
            Resource.Instance.Deck_Add(_cardAddPrefab.Str.NUM);
            Resource.Instance.SupportPrice["Add"]++;
            _Content.transform.Find("Button_Add").gameObject.SetActive(false);
            QuitChk();
        }
    }
    public void ButtonAction_Delete()
    {
        if (Resource.Instance.money >= DeletePrice)
        {
            Resource.Instance.money -= DeletePrice;
            Resource.Instance.Deck_Remove(_cardDeletePrefab.Str.NUM);
            Resource.Instance.SupportPrice["Delete"]++;
            _Content.transform.Find("Button_Delete").gameObject.SetActive(false);
            QuitChk();
        }
    }
    public void ButtonAction_Heal()
    {
        if (Resource.Instance.money >= HealPrice)
        {
            Resource.Instance.money -= HealPrice;
            var HealValue = Random.Range(30, 71);
            Resource.Instance.Event_Heal(HealValue);
            Resource.Instance.SupportPrice["Heal"]++;
            _Content.transform.Find("Button_Heal").gameObject.SetActive(false);
            QuitChk();
        }
    }
    public void ButtonAction_Evolve()
    {
        if (Resource.Instance.money >= EvolvePrice)
        {
            Resource.Instance.money -= EvolvePrice;
            var _tmpStruct = _cardEvolvePrefab.Str;
            var _tmpEvolveStruct = CardDatabase.Instance.card(_tmpStruct.evol[Random.Range(0,_tmpStruct.evol.Count)]);
            Resource.Instance.Deck_Remove(_cardEvolvePrefab.Str.NUM);
            Resource.Instance.Deck_Add(_tmpEvolveStruct.NUM);
            Resource.Instance.SupportPrice["Evolve"]++;
            _Content.transform.Find("Button_Evolve").gameObject.SetActive(false);
            QuitChk();
        }
    }
}
