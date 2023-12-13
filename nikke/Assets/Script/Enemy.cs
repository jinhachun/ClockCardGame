using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] StatusUnit _status;
    [SerializeField] public SpriteRenderer _img;
    [SerializeField] private SpriteRenderer _targetImg;
    [SerializeField] private TMP_Text _statusText;


    public EnemyStruct Str;
    public string _name;
    public int _hp;
    public int _mhp;
    public int _shield;
    public int _area;
    public int _statusValue;
    
    public EnemyType _enemyType;
    public List<pattern> _enemyPatterns;
    public string _statusName;
    public EnemyHpBar _hpBar;
    bool isOver = false;
    public bool isTarget = false;
    int PatternIndex;
    public double attackBuff = 0;
    public int damage => (int)attackBuff + Pattern._Value;
    public void setAttackBuff()
    {
        attackBuff += Pattern._Value; 
    }
    public void setAttackBuff(int a)
    {
        attackBuff += a; 
    }
    public void gainShield(int a)
    {
        BattleManager.Instance.effectOn(BattleManager.Instance._shieldEffectPrefab, this);
        _shield += a;
    }
    public void lossShield(int a)
    {
        _shield -= a;
        if (_shield <= 0) _shield = 0;
    }
    public void heal(int a)
    {
        _hp += a;
        if (_hp > _mhp) _hp = _mhp;
    }
    public void statusValueChange(int a)
    {
        this._statusValue = a;
        if (this._statusValue == 0) _statusText.gameObject.SetActive(false);
        else _statusText.gameObject.SetActive(true);
        _statusText.text = this._statusValue + "";
    }
    public void Set(EnemyStruct enemyStruct)
    {
        this.Str = enemyStruct;
        this._name = enemyStruct._name;
        this._hp = enemyStruct._hp;
        this._mhp = this._hp;
        this._area = enemyStruct._area;
        this._img.sprite = enemyStruct._sprite;
        this._enemyType = enemyStruct._enemyType;
        Debug.Log(this._enemyType + " " + EnemyDatabase.Instance.spriteSize(this._enemyType));
        this._img.transform.parent.localScale = EnemyDatabase.Instance.spriteSize(this._enemyType);
        this._enemyPatterns = enemyStruct._enemyPatterns;
        this._statusValue = enemyStruct._statusValue;
        if (this._statusValue == 0) _statusText.gameObject.SetActive(false);
        _statusText.text = this._statusValue+"";
        if (!enemyStruct._statusStruct.Equals("¾øÀ½"))
        {
            this._statusName = enemyStruct._statusStruct;
            this._status.set(StatusDatabase.Instance.Status(_statusName));
        }
        else
        {
            this._status.gameObject.SetActive(false);
        }


        PatternIndex = 0;
    }
    
    void Update()
    {
        if (isOver && Input.GetMouseButtonDown(0))
        {
            if (BattleManager.GameState != BattleState.WaitingReroll) return;
            if (!isTarget && BattleManager.Instance.targetEnemy.Count > 0) BattleManager.Instance.targetEnemy[0].isTarget=false;
            isTarget = isTarget ? false : true;
        }
        if(isTarget!=_targetImg.gameObject.activeInHierarchy)
            _targetImg.gameObject.SetActive(isTarget);

    }
    public pattern Pattern => _enemyPatterns[PatternIndex];
    public void SetPatternText()
    {
        switch (Pattern._enemyPattern)
        {
            case EnemyPattern.ATT:
                {
                    string dammTxt = Pattern._Value + (attackBuff == 0 ? "" : "+" + attackBuff);
                    this._hpBar.setText_Pattern(dammTxt, Color.red);
                    return;
                }
            case EnemyPattern.BUFF:
                {
                    this._hpBar.setText_Pattern("+", Color.blue);
                    return;
                }
            case EnemyPattern.SLEEP:
                {
                    this._hpBar.setText_Pattern("Zzz", Color.green);
                    return;
                }
              
            default:
                this._hpBar.setText_Pattern("?", Color.green);
                return;
        }

    }
    public void TurnEnd()
    {
        PatternIndex++;
        if (PatternIndex == _enemyPatterns.Count) PatternIndex = 0;
    }
    void OnMouseOver()
    {
        isOver = true;

    }

    void OnMouseExit()
    {
        isOver = false;
    }
}
