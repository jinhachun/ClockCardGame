using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _cardBody;
    [SerializeField] private SpriteRenderer _img;
    [SerializeField] private SpriteRenderer _company;
    [SerializeField] private SpriteRenderer _rank;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _number;
    [SerializeField] private SpriteRenderer _cardBack;
    public CardStruct Str;
    private COMPANY Company;
    private RANK Rank;
    private int Value;
    private CARDTYPE Type;

    public void Set(CardStruct str)
    {
        this.Str = str;
        this._name.text = str._name;
        this._text.text = str._text;
        this._number.text = str._number.ToString();
        this._img.sprite = str._img;
        this._company.sprite = CardDatabase.Instance.companySprite(str._company);
        this._rank.sprite = CardDatabase.Instance.rankSprite(str._rank);

        this.Company = str._company;
        this.Rank = str._rank;
        this.Type = str._type;
        this.Value = str._value;
    }
    public void flip()
    {
        _cardBack.gameObject.SetActive(_cardBack.gameObject.activeInHierarchy?false:true);
    }
    public void flip(bool a)
    {
        _cardBack.gameObject.SetActive(a ? false:true) ;
    }
    public void setLayer(int a)
    {
        _cardBack.sortingOrder = 52+a;
    }
}
