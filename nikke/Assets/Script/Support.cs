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

    public int Price => 50;
    public int RerollPrice => Price;
    public int HealPrice => Price* (Resource.Instance.SupportPrice["Heal"]+2);
    public int AddPrice => Price* (Resource.Instance.SupportPrice["Add"] + 2);
    public int DeletePrice => Price  * (Resource.Instance.SupportPrice["Delete"]* 2 + 3);
    public int EvolvePrice => Price  * (Resource.Instance.SupportPrice["Evolve"]+ _cardEvolvePrefab.Str._tier*2 + 2);
    void Start()
    {
        Set();
    }
    public void Set()
    {
        _cardAddPrefab.Set(CardDatabase.Instance.cardByTier(3));
        _cardDeletePrefab.Set(Resource.Instance.Deck[Random.Range(0, Resource.Instance.Deck.Count)]);
        var rare = Random.Range(0, 101) > 70;
        if (rare) {
            var rareCardDeck = Resource.Instance.Deck.Where(x => x.isRare).ToList();
            if(rareCardDeck.Count!=0)
                _cardEvolvePrefab.Set(rareCardDeck[Random.Range(0,rareCardDeck.Count)]);
            else
                _cardEvolvePrefab.Set(Resource.Instance.Deck[Random.Range(0, Resource.Instance.Deck.Count)]);
        }
        else
            _cardEvolvePrefab.Set(Resource.Instance.Deck[Random.Range(0, Resource.Instance.Deck.Count)]);
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
    public void ButtonAction_Reroll()
    {
        if (Resource.Instance.money >= RerollPrice)
        {
            Resource.Instance.money -= RerollPrice;
            Set();
        }
    }
    public void ButtonAction_Add()
    {
        if (Resource.Instance.money >= AddPrice)
        {
            Resource.Instance.money -= AddPrice;
            Resource.Instance.Deck_Add(_cardAddPrefab.name);
            Resource.Instance.SupportPrice["Add"]++;
            Quit();
        }
    }
    public void ButtonAction_Delete()
    {
        if (Resource.Instance.money >= DeletePrice)
        {
            Resource.Instance.money -= DeletePrice;
            Resource.Instance.Deck_Remove(_cardDeletePrefab.name);
            Resource.Instance.SupportPrice["Delete"]++;
            Quit();
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
            Quit();
        }
    }
    public void ButtonAction_Evolve()
    {
        if (Resource.Instance.money >= EvolvePrice)
        {
            Resource.Instance.money -= EvolvePrice;
            var _tmpStruct = _cardEvolvePrefab.Str;
            var _tmpEvolveStruct = CardDatabase.Instance.card(_tmpStruct.evol[Random.Range(0,_tmpStruct.evol.Count)]);
            Resource.Instance.Deck_Remove(_cardEvolvePrefab.name);
            Resource.Instance.Deck_Add(_tmpEvolveStruct._name);
            Resource.Instance.SupportPrice["Evolve"]++;
            Quit();
        }
    }
}
