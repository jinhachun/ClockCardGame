using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct pattern
{
    public EnemyPattern _enemyPattern;
    public int _Value;
}
[Serializable]
public struct EnemyStruct {
    public Sprite _sprite;
    public string _name;
    public int _hp;
    public int _area;
    public EnemyType _enemyType;
    public List<pattern> _enemyPatterns;
};
[Serializable]
public struct EnemySquadStruct
{
    public List<string> enemySquad;
    public int area;
    public EnemyType enemyType;
}

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance => instance;
    public void Awake()
    {
        instance = this;
    }
    [SerializeField] List<EnemyStruct> enemyList;
    [SerializeField] List<EnemySquadStruct> sqdList;

    public List<EnemyStruct> enemylist(List<EnemyStruct> enemyStructs, int area) => enemyStructs.Where(x => x._area == area).ToList();
    public EnemyStruct enemylist(List<EnemyStruct> enemyStructs, string name) => enemyStructs.Where(x => x._name.Equals(name)).FirstOrDefault();
    public EnemyStruct randomEnemy(List<EnemyStruct> enemyStructs, int area) => enemylist(enemyStructs, area)[Random.Range(0, enemylist(enemyStructs, area).Count)];

    public List<EnemyStruct> final_enemylist(int area,EnemyType type)
    {
        var sqdListByArea = sqdList.Where(x => x.area == area).ToList().Where(x=>x.enemyType==type).ToList();
        var sqdByArea = sqdListByArea[Random.Range(0, sqdListByArea.Count)];

        List<EnemyStruct> enemyStructs = new List<EnemyStruct>();
        foreach(var enemy in sqdByArea.enemySquad)
        {
            enemyStructs.Add(enemylist(enemyList,enemy));
        }
        return enemyStructs;
    }
}
[Serializable]
public enum EnemyPattern
{
    ATT, BUFF, deBUFF, CARDINSRT
}
[Serializable]
public enum EnemyType
{
    SERVANT,EXCEED,MASTER,LORD,TYRANT,QUEEN
}
