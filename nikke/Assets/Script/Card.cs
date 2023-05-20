using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _visual;
    [SerializeField] private SpriteRenderer _cardBody;
    [SerializeField] private SpriteRenderer _img;
    [SerializeField] private SpriteRenderer _company;
    [SerializeField] private SpriteRenderer _rank;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private TMP_Text _number;
    [SerializeField] private SpriteRenderer _cardBack;


    [SerializeField] private string Exhaust;
    [SerializeField] private string Ethereal;
    [SerializeField] private string Fixed;
    public CardStruct Str;
    public COMPANY Company { get; private set; }
    private RANK Rank;
    public CARDTYPE Type { get; private set; }
    public TARGET Target { get; private set; }
    public string name { get; private set; }
    public int Value { get; private set; }
    public int number { get; private set; }
    public bool isExhaust { get; private set; }
    public bool isEthereal { get; private set; }
    public bool isFixed{ get; private set; }
    public CardAction BeforeAction;
    public CardAction Action;
    public void ValueChange(int a)
    {
        this.Value = a;
        this._text.text = textSet(Str._text);
    }
    public string textSet(string txt)
    {
        return txt.Replace("[]", Value.ToString());
    }
public void Set(CardStruct str)
    {
        this.Str = str;
        this._name.text = str._name;
        this.name = str._name;
        this._number.text = str._number.ToString();
        this.number = str._number;
        this._img.sprite = str._img;
        this._company.sprite = CardDatabase.Instance.companySprite(str._company);
        this._rank.sprite = CardDatabase.Instance.rankSprite(str._rank);

        this.Company = str._company;
        this.Rank = str._rank;
        this.Type = str._type;
        this.Target = str._target;
        this.Value = str._value;
        this._text.text = textSet(str._text);

        this.isEthereal = str.isEthereal;
        this.isExhaust = str.isExhaust;
        this.isFixed = str.isFixed;
        List<string> infoText = new List<string>();
        if (isExhaust) infoText.Add(Exhaust);
        if (isEthereal) infoText.Add(Ethereal);
        if (isFixed) infoText.Add(Fixed);

        this._infoText.text = string.Join(", ", infoText.ToArray());
    }
    public void flip()
    {
        _cardBack.gameObject.SetActive(_cardBack.gameObject.activeInHierarchy?false:true);
    }
    public void flip(bool a)
    {
        _cardBack.gameObject.SetActive(a ? false:true) ;
    }

    public void setLayer(int a,int b)
    {
        int newSortingOrder = b;
        _cardBack.sortingOrder = newSortingOrder+2 + a;
        _cardBody.sortingOrder = newSortingOrder;
        _img.sortingOrder = newSortingOrder-1;
        _cardBody.sortingOrder = newSortingOrder;
        _company.sortingOrder = newSortingOrder+1;
        _rank.sortingOrder = newSortingOrder+1;
        MeshRenderer meshRenderer = _name.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
         meshRenderer = _text.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
        meshRenderer = _number.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
        meshRenderer = _infoText.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
    }
    
    void OnMouseOver()
    {
        if (_cardBack.gameObject.activeInHierarchy) return;
        _visual.transform.localScale = new Vector2(1.5f, 1.5f);
        setLayer(3,53);

    }

    void OnMouseExit()
    {
        if (_cardBack.gameObject.activeInHierarchy) return;
        _visual.transform.localScale = new Vector2(0.8f, 0.8f);
        setLayer(0,50);
    }
}
