using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;


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
            case ("�ܴ���"):
                {
                    DamagePopup.Create(enemy.transform.position, enemy._statusName, Color.white);
                    enemy.gainShield(enemy._statusValue);
                    return;
                }
        }
        return;
    }
    public void Action_WhileAttack(string name, Enemy enemy)
    {
        switch (name)
        {
            case ("����"):
                {
                    DamagePopup.Create(enemy.transform.position, enemy._statusName, Color.white);
                    BattleManager.Instance.effectOn(BattleManager.Instance._rageEffectPrefab, enemy);
                    enemy.setAttackBuff(enemy._statusValue);
                    return;
                }
        }
        return ;
    }
    public void Action_TurnEnd(string name, Enemy enemy)
    {
        switch (name)
        {
            case "�г�":
                {
                    DamagePopup.Create(enemy.transform.position, enemy._statusName, Color.white);
                    BattleManager.Instance.effectOn(BattleManager.Instance._rageEffectPrefab, enemy);
                    enemy.setAttackBuff(enemy._statusValue);
                    return;
                }
            case "�θ�":
                {
                    if (UnityEngine.Random.Range(0, 10) < 2)
                    {
                        DamagePopup.Create(enemy.transform.position, enemy._statusName, Color.white);
                        BattleManager.Instance.summonEnemy(EnemyDatabase.Instance.enemy("�ѱ�"));
                    }
                    return;
                }
            case "��������":
                {
                    DamagePopup.Create(enemy.transform.position, enemy._statusName, Color.white);
                    int RandomValue = Random.Range(0, 5);
                    Debug.Log(RandomValue);
                    if (RandomValue == 0)
                    {
                        enemy.gainShield(enemy._statusValue);
                    }
                    else if (RandomValue == 1)
                    {
                        BattleManager.Instance.effectOn(BattleManager.Instance._rageEffectPrefab, enemy);
                        enemy.setAttackBuff(enemy._statusValue);
                    }
                    else if (RandomValue == 2)
                        BattleManager.Instance.enemyDamage(enemy._statusValue, false, enemy);
                    else if (RandomValue == 3)
                    {
                        enemy.statusValueChange(enemy._statusValue+Random.Range(1, 11));
                    }
                    else if (RandomValue == 4)
                    {
                        BattleManager.Instance.healEnemy(enemy._statusValue, enemy);
                    }

                    return;
                }
            case "������":
                {
                    if ((float)enemy._hp*100/(float)enemy._mhp<50)
                    {
                        DamagePopup.Create(enemy.transform.position, enemy._statusName, Color.white);
                        BattleManager.Instance.effectOn(BattleManager.Instance._rageEffectPrefab, enemy);
                        enemy.setAttackBuff(enemy._statusValue);
                        enemy.statusValueChange(enemy._statusValue+5);

                    }
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
