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
    [SerializeField] private SpriteRenderer _outLine;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private TMP_Text _tier;
    [SerializeField] private TMP_Text _attText;
    [SerializeField] private TMP_Text _defText;
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
    private int attack_before;
    private int defence_before;
    public int tier { get; private set; }
    public bool isExhaust { get; private set; }
    public bool isEthereal { get; private set; }
    public bool isFixed{ get; private set; }
    public int layer = 0;
    public bool Touchable = false;
    public CardAction BeforeAction;
    public CardAction Action;

    public bool combiSpeices;
    public bool combiType;
    public void StatChange(string statVar,int a)
    {
        int b = a;
        if (b < 0) b = 0;
        switch (statVar)
        {
            case "Attack":
                Stat.attack = b;
                if (Stat.attack > Str._stat.attack)
                {
                    if(BattleManager.GameState == BattleState.Attack)
                        BattleManager.Instance.effectOn(BattleManager.Instance._rageEffectPrefab, this);
                    this._attText.color = Color.red;
                }
                else if (Stat.attack < Str._stat.attack) this._attText.color = Color.gray;
                else this._attText.color = Color.white;
                this._attText.text = Stat.attack.ToString();
                break;
            case "Defence":
                Stat.defence = b;
                if (Stat.defence > Str._stat.defence) {
                    BattleManager.Instance.effectOn(BattleManager.Instance._rageEffectPrefab, this);
                    this._defText.color = Color.red; 
                }
                else if (Stat.defence < Str._stat.defence) this._defText.color = Color.gray;
                else this._defText.color = Color.white;
                this._defText.text = Stat.defence.ToString();
                break;
        }
    }
    public void TouchableChange(bool a)
    {
        Touchable = a;
        collider.enabled = a;
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

        this.Stat = new STAT();
        this.Stat.attack = str._stat.attack;
        this.Stat.defence = str._stat.defence;

        StatChange("Attack", this.Stat.attack);
        StatChange("Defence", this.Stat.defence);
        this._text.text = str._text;

        this.isEthereal = str.isEthereal;
        this.isExhaust = str.isExhaust;
        this.isFixed = str.isFixed;
        List<string> infoText = new List<string>();
        if (isExhaust) infoText.Add(Exhaust);
        if (isEthereal) infoText.Add(Ethereal);
        if (isFixed) infoText.Add(Fixed);

        this._infoText.text = string.Join(", ", infoText.ToArray());

        if(str.isRare)
            this._tier.color = Color.red;
        else
            this._tier.color = Color.black;

        combiReset();

    }
    public void combiReset()
    {
        combiSpeices = false;
        combiType = false;
    }
    public void flip()
    {
        _cardBack.gameObject.SetActive(_cardBack.gameObject.activeInHierarchy?false:true);
        TouchableChange(_cardBack.gameObject.activeInHierarchy ? false : true);
    }
    public void flip(bool a)
    {
        _cardBack.gameObject.SetActive(a ? false:true) ;
        TouchableChange(a);
    }
    public void glow(bool a)
    {
        if (a)
        {
            if (!combiSpeices && !combiType) return;
            _outLine.transform.localScale = new Vector2(1.05f, 1.05f);
            if (combiSpeices && combiType) _outLine.color = new Color32(255, 0, 255, 255);
            else if (combiSpeices) _outLine.color = Color.cyan;
            else if (combiType) _outLine.color = Color.red;
        }
        else
        {
            _outLine.transform.localScale = new Vector2(1f, 1f);
            combiReset();
            _outLine.color = Color.black; 
        }
    }
    public void setLayer(int a,int b)
    {
        int newSortingOrder = b;
        _cardBack.sortingOrder = newSortingOrder+2 + a;
        _cardBody.sortingOrder = newSortingOrder;
        _img.sortingOrder = newSortingOrder-1;
        if(_outLine!=null)
            _outLine.sortingOrder = newSortingOrder - 2;
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
        meshRenderer = _attText.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
        meshRenderer = _defText.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
    }
    public void setLayer(string A)
    {
        _cardBack.sortingLayerName = A;
        _cardBody.sortingLayerName = A;
        _img.sortingLayerName = A;
        if (_outLine != null)
            _outLine.sortingLayerName = A;
        _species.sortingLayerName = A;
        _type.sortingLayerName = A;
        MeshRenderer meshRenderer = _name.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;
        meshRenderer = _text.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;
        meshRenderer = _tier.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;
        meshRenderer = _infoText.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;
        meshRenderer = _attText.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;
        meshRenderer = _defText.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;

    }
    void OnMouseOver()
    {
        if (!Touchable) return;
        _visual.transform.localScale = new Vector2(1.8f, 1.8f);
        setLayer(3,53+layer);

    }

    void OnMouseExit()
    {
        if (!Touchable && _visual.transform.localScale.x == 0.8f) return;
        _visual.transform.localScale = new Vector2(0.8f, 0.8f);
        setLayer(0,50+ layer);
    }
}
