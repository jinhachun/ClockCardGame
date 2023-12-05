using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class StatusDatabase : MonoBehaviour
{
    private static StatusDatabase instance;
    public static StatusDatabase Instance => instance;
    public void Awake()
    {
        instance = this;
    }
    [SerializeField] List<StatusStruct> StatusList;

    public StatusStruct Status(string a) => StatusList.Where(e => e._name.Equals(a)).FirstOrDefault();

    public void Action_TurnStart(string name, Enemy enemy)
    {
        switch (name)
        {
            case ("�̸�"):
                return;
        }
        return;
    }
    public void Action_WhileAttack(string name, Enemy enemy)
    {
        switch (name)
        {
            case ("�̸�"):
                return ;
        }
        return ;
    }
    public void Action_TurnEnd(string name, Enemy enemy)
    {
        switch (name)
        {
            case "�г�":
                {
                    BattleManager.Instance.effectOn(BattleManager.Instance._rageEffectPrefab, enemy);
                    enemy.setAttackBuff(enemy._area*enemy._area);
                    return;
                }
            case "�θ�":
                {
                    if(UnityEngine.Random.Range(0,10)<3)
                        BattleManager.Instance.summonEnemy(EnemyDatabase.Instance.enemy("�ѱ�"));
                    return;
                }
        }
    }
}
[Serializable]
public struct StatusStruct
{
    public string _name;
    public Sprite sprite;
    public string _info;
}
