using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

[Serializable]
public class Card : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _visual;
    [SerializeField] private SpriteRenderer _cardBody;
    [SerializeField] protected SpriteRenderer _img;
    [SerializeField] private SpriteRenderer _species;
    [SerializeField] private SpriteRenderer _type;
    [SerializeField] private SpriteRenderer _outLine;
    [SerializeField] public TMP_Text _name;
    [SerializeField] protected TMP_Text _text;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] protected TMP_Text _tier;
    [SerializeField] private TMP_Text _attText;
    [SerializeField] private TMP_Text _defText;
    [SerializeField] private SpriteRenderer _cardBack;

    protected float Size => this is BonusRule ? 1f : 0.8f;

    private string Exhaust => Resource.Instance.Kor?"�Ҹ�":"Exhaust";
    private string Ethereal => Resource.Instance.Kor ? "�ֹ߼�" : "Ethereal";
    private string Fixed => Resource.Instance.Kor ? "����" : "Fixed";
    Collider2D collider => this.gameObject.GetComponent<BoxCollider2D>();
    public CardStruct Str;
    public SPECIES Species { get; private set; }
    public TYPE Type { get; private set; }
    public  string name { get; protected set; }
    public STAT Stat { get; private set; }
    private int attack_before;
    private int defence_before;
    public int tier { get; protected set; }
    public bool isExhaust { get; private set; }
    public bool isEthereal { get; private set; }
    public bool isFixed{ get; private set; }
    public int layer = 0;
    public bool Touchable = false;

    public bool combiSpeices;
    public bool combiType;
    public void StatChange(string statVar,int a)
    {
        int b = a;
        if (b < 0) b = 0;
        switch (statVar)
        {
            case "Attack":
                if (b > Str._stat.attack)
                    this._attText.color = Color.red;
                else if (b < Str._stat.attack)
                    this._attText.color = Color.gray;
                else this._attText.color = Color.white;
                Stat.attack = b;
                this._attText.text = Stat.attack.ToString();
                break;
            case "Defence":
                if (b > Str._stat.defence)
                    this._defText.color = Color.red;
                else if (b < Str._stat.defence)
                    this._defText.color = Color.gray;
                else this._defText.color = Color.white;
                Stat.defence = b;
                this._defText.text = Stat.defence.ToString();
                break;
        }
    }
    public void StatChange(string statVar,int a,bool effect)
    {
        int b = a;
        if (b < 0) b = 0;
        if (effect) StatChangeEffect(b, statVar.Equals("Attack") ? Stat.attack : Stat.defence);
        StatChange(statVar, a);
    }
    public void StatChangeEffect(int changestat, int beforestat)
    {
        if (changestat > beforestat)
        {
            EffectManager.effectOn(EffectManager.EffectName.Rage, this);
        }
        else if (changestat < beforestat)
        {
            EffectManager.effectOn(EffectManager.EffectName.Debuff, this);
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
        this._name.text = DataManager.CardName(str.NUM,Resource.Instance.Kor,str._Token);
        this.name = str._name;

        string[] RomanNumeral = {"0","I","II","III","IV","V"};
        this._tier.text = RomanNumeral[str._tier];
        this.tier = str._tier;
        this._img.sprite = str._img;
        this._species.sprite = CardDatabase.Instance.speciesSprite(str._species);
        this._type.sprite = CardDatabase.Instance.typeSprite(str._type);

        this.Species = str._species;
        this.Type = str._type;
        this.Type = str._type;

        this.Stat = new STAT();
        var Rule_no13_0 = Resource.Instance.Rule_no(13) ? (this.Species==SPECIES.MECH? 3: -1) : 0;
        var Rule_no17_0 = Resource.Instance.Rule_no(17) ? (this.Species == SPECIES.UNDEAD ? 1 : 0) : 0;
        this.Stat.attack = str._stat.attack + Rule_no13_0 + Rule_no17_0;
        this.Stat.defence = str._stat.defence;

        StatChange("Attack", this.Stat.attack, false);
        StatChange("Defence", this.Stat.defence, false);
        this._text.text = DataManager.CardInfo(str.NUM, Resource.Instance.Kor, str._Token);

        this.isEthereal = str.isEthereal;
        this.isExhaust = str.isExhaust;
        this.isFixed = str.isFixed;
        List<string> infoText = new List<string>();
        if (isExhaust) infoText.Add(Exhaust);
        if (isEthereal) infoText.Add(Ethereal);
        if (isFixed) infoText.Add(Fixed);

        this._infoText.text = string.Join(", ", infoText.ToArray());

        if(str.isRare)
            this._tier.color = Color.yellow;
        else
            this._tier.color = Color.white;

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
        _img.sortingOrder = newSortingOrder-2;
        if(_outLine!=null)
            _outLine.sortingOrder = newSortingOrder - 3;
        _cardBody.sortingOrder = newSortingOrder-1;

        MeshRenderer meshRenderer = _name.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
         meshRenderer = _text.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;

        meshRenderer = _tier.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;

        if (this is BonusRule) return;
        meshRenderer = _infoText.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
        meshRenderer = _attText.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;
        meshRenderer = _defText.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = newSortingOrder;

        _species.sortingOrder = newSortingOrder + 1;
        _type.sortingOrder = newSortingOrder;
    }
    public void setLayer(string A)
    {
        _cardBack.sortingLayerName = A;
        _cardBody.sortingLayerName = A;
        _img.sortingLayerName = A;
        if (_outLine != null)
            _outLine.sortingLayerName = A;
        MeshRenderer meshRenderer = _name.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;
        meshRenderer = _text.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;
        meshRenderer = _tier.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;

        if (this is BonusRule) return;
        meshRenderer = _infoText.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;
        meshRenderer = _attText.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;
        meshRenderer = _defText.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = A;
        _species.sortingLayerName = A;
        _type.sortingLayerName = A;

    }
    public void deleteCard()
    {
        BattleManager.Instance.waitDelay = true;
        this.flip(true);
        this.setLayer(0,100);
        this.transform.DOScale(0, BattleManager.GameState==BattleState.Attack?0.2f:1f).SetEase(Ease.InCubic).OnComplete(() => {
            BattleManager.Instance.waitDelay = false;
            Destroy(this.gameObject); 
        });
    }
    public void deleteCard(float time)
    {
        BattleManager.Instance.waitDelay = true;
        this.flip(true);
        this.setLayer(0, 100);
        this.transform.DOScale(0, time).SetEase(Ease.InCubic).OnComplete(() => {
            BattleManager.Instance.waitDelay = false;
            Destroy(this.gameObject);
        });
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (GetComponent<Collider2D>().OverlapPoint(wp))
            {
                OnTouchDown();
            }
            else
                OnTouchUp();

        }
    }
    private void OnMouseOver()
    {
        OnTouchDown();
    }
    private void OnMouseExit()
    {
        OnTouchUp();   
    }
    void OnTouchDown()
    {
        if (!Touchable) return;
        _visual.transform.localScale = new Vector2(Size * 2, Size * 2);
        setLayer(3, 53 + layer);
    }

    void OnTouchUp()
    {
        if (!Touchable && _visual.transform.localScale.x == Size) return;
        _visual.transform.localScale = new Vector2(Size, Size);
        setLayer(0, 50 + layer);
    }

}
