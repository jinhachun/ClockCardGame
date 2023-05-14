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
        int newSortingOrder = 52;
        _cardBack.sortingOrder = newSortingOrder + a;
        _cardBody.sortingOrder = newSortingOrder;
        _img.sortingOrder = newSortingOrder-1;
        _cardBody.sortingOrder = newSortingOrder;
        _company.sortingOrder = newSortingOrder;
        _rank.sortingOrder = newSortingOrder;
        MeshRenderer meshRenderer = _name.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
         meshRenderer = _text.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
        meshRenderer = _number.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
    }
    public void setLayer()
    {
        int newSortingOrder = 50;
        _cardBack.sortingOrder = newSortingOrder+1;
        _cardBody.sortingOrder = newSortingOrder;
        _img.sortingOrder = newSortingOrder-1;
        _cardBody.sortingOrder = newSortingOrder;
        _company.sortingOrder = newSortingOrder;
        _rank.sortingOrder = newSortingOrder;
        MeshRenderer meshRenderer = _name.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
        meshRenderer = _text.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
        meshRenderer = _number.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
    }
    void OnMouseOver()
    {
        if (_cardBack.gameObject.activeInHierarchy) return;
        _visual.transform.localScale = new Vector2(1.5f, 1.5f);
        setLayer(1);

    }

    void OnMouseExit()
    {
        if (_cardBack.gameObject.activeInHierarchy) return;
        _visual.transform.localScale = new Vector2(0.8f, 0.8f);
        setLayer();
    }
}
