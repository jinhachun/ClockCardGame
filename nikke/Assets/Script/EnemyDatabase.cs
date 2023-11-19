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
    public string _statusStruct;
};
[Serializable]
public struct EnemySquadStruct
{
    public List<string> enemySquad;
    public int area;
    public EnemyType enemyType;
}

public class EnemyDatabase : MonoBehaviour
{
    private static EnemyDatabase instance;
    public static EnemyDatabase Instance => instance;
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
        int randomNum = Random.Range(0, sqdListByArea.Count);
        var sqdByArea = sqdListByArea[randomNum];

        List<EnemyStruct> enemyStructs = new List<EnemyStruct>();
        foreach(var enemy in sqdByArea.enemySquad)
        {
            enemyStructs.Add(enemylist(enemyList,enemy));
        }
        return enemyStructs;
    }
    public Vector2 spriteSize(EnemyType et)
    {
        if (et == EnemyType.Mini) return new Vector2(1, 1);
        else if (et == EnemyType.Normal) return new Vector2(1.5f, 1.5f);
        else if (et == EnemyType.Giga) return new Vector2(2, 2);
        return new Vector2(1,1);
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
    Mini,Normal,Giga
}
