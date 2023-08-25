using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _visual;
    [SerializeField] private SpriteRenderer _cardBody;
    [SerializeField] private SpriteRenderer _img;
    [SerializeField] private SpriteRenderer _species;
    [SerializeField] private SpriteRenderer _type;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private TMP_Text _tier;
    [SerializeField] private SpriteRenderer _cardBack;


    [SerializeField] private string Exhaust;
    [SerializeField] private string Ethereal;
    [SerializeField] private string Fixed;
    Collider2D collider => this.gameObject.GetComponent<BoxCollider2D>();
    public CardStruct Str;
    public SPECIES Species { get; private set; }
    public TYPE Type { get; private set; }
    public string name { get; private set; }
    public STAT Stat { get; private set; }
    public int tier { get; private set; }
    public bool isExhaust { get; private set; }
    public bool isEthereal { get; private set; }
    public bool isFixed{ get; private set; }
    public int layer = 0;
    public CardAction BeforeAction;
    public CardAction Action;
    public void ValueChange(string statVar,int a)
    {
        switch (statVar)
        {
            case "Attack":
                Stat.attack = a;
                break;
            case "Defence":
                Stat.defence = a;
                break;
        }
    }
public void Set(CardStruct str)
    {
        this.Str = str;
        this._name.text = str._name;
        this.name = str._name;
        this._tier.text = str._tier.ToString();
        this.tier = str._tier;
        this._img.sprite = str._img;
        this._species.sprite = CardDatabase.Instance.speciesSprite(str._species);
        this._type.sprite = CardDatabase.Instance.typeSprite(str._type);

        this.Species = str._species;
        this.Type = str._type;
        this.Type = str._type;
        this.Stat = str._stat;
        this._text.text = str._text;

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
        _species.sortingOrder = newSortingOrder+1;
        _type.sortingOrder = newSortingOrder+1;
        MeshRenderer meshRenderer = _name.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
         meshRenderer = _text.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
        meshRenderer = _tier.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
        meshRenderer = _infoText.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
    }
    
    void OnMouseOver()
    {
        _visual.transform.localScale = new Vector2(1.5f, 1.5f);
        setLayer(3,53+layer);

    }

    void OnMouseExit()
    {
        _visual.transform.localScale = new Vector2(0.8f, 0.8f);
        setLayer(0,50+ layer);
    }
    private void Update()
    {
        collider.enabled = !_cardBack.gameObject.activeInHierarchy;
    }
}
