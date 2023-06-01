using System.Collections.Generic;
using UnityEngine;

public struct StatusStruct
{
    public string _name;
    public Sprite sprite;
}

public class StatusDataBase : MonoBehaviour
{
    [SerializeField] List<StatusStruct> StatusDatabase;

    Func Action_TurnStart(string name,int value)
    {
        return null;
    }
    Func Action_WhileAttack(string name, int value)
    {
        return null;
    }
    Func Action_TurnEnd(string name,int value)
    {
        return null;
    }
}