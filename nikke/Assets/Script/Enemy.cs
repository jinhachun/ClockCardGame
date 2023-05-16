using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EnemyPattern();
[Serializable]
public struct pattern
{
    EnemyPattern patternFunc;
    int Value;
}
[Serializable]
public struct EnemyStruct {
    Sprite sprite;
    string name;
    int hp;
    int area;
    List<pattern> patterns;
};
public class Enemy : MonoBehaviour
{
    [SerializeField] public List<EnemyStruct> enemyList;
}
