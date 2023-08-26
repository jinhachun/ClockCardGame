using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _img;
    [SerializeField] private SpriteRenderer _targetImg;

    public EnemyStruct Str;
    public string _name;
    public int _hp;
    public int _mhp;
    public int _shield;
    public int _area;
    
    public EnemyType _enemyType;
    public List<pattern> _enemyPatterns;
    public EnemyHpBar _hpBar;
    bool isOver = false;
    public bool isTarget = false;
    int PatternIndex;
    private double attackBuff = 0;
    public int damage => (int)attackBuff + Pattern._Value;
    public void setAttackBuff() { attackBuff += Pattern._Value; }
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
        if (Pattern._enemyPattern == EnemyPattern.ATT)
        {
            string dammTxt = Pattern._Value + (attackBuff==0?"":"+"+attackBuff);
            this._hpBar.setText_Pattern(dammTxt, Color.red);
        }
        else if (Pattern._enemyPattern == EnemyPattern.BUFF)
            this._hpBar.setText_Pattern("+", Color.blue);
        else
            this._hpBar.setText_Pattern("?", Color.green);

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
