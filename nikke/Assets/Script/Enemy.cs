using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _img;

    public EnemyStruct Str;
    public string _name;
    public int _hp;
    public int _mhp;
    public int _shield;
    public int _area;
    public EnemyType _enemyType;
    public List<pattern> _enemyPatterns;

    public void Set(EnemyStruct enemyStruct)
    {
        this.Str = enemyStruct;
        this._name = enemyStruct._name;
        this._hp = enemyStruct._hp;
        this._mhp = this._hp;
        this._area = enemyStruct._area;
        this._enemyType = enemyStruct._enemyType;
        this._enemyPatterns = enemyStruct._enemyPatterns;
    }
}
