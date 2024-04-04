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
                    StatusPopup(enemy);
                    enemy.gainShield(enemy._statusValue);
                    return;
                }
            case ("��������ź"):
                {
                    var enemyName = "�����뽽����";
                    enemy.statusValueChange(enemy._statusValue-1);
                    if (enemy._statusValue == 0)
                    {
                        enemy.statusValueChange(2);
                        StatusPopup(enemy);
                        BattleManager.Instance.Summon_Enemy(EnemyDatabase.Instance.enemy(enemyName));
                    }
                    return;
                }
            case ("������"):
                {
                    enemy.statusValueChange(enemy._statusValue - 1);
                    if (enemy._statusValue == 0)
                    {
                        StatusPopup(enemy);
                        BattleManager.Instance.Damage_toPlayer(30);
                        BattleManager.Instance.Damage_toEnemy(enemy._hp, false, enemy);

                    }
                    return;
                }
            case ("����"):
                {
                    enemy.statusValueChange(enemy._hp-200);
                    return;
                }
        }
        return;
    }
    public void Action_WhileAttacked(string name, Enemy enemy)
    {
        switch (name)
        {
            case ("����"):
                {
                    StatusPopup(enemy);
                    enemy.setAttackBuff(enemy._statusValue);
                    enemy.SetPatternText();
                    return;
                }
            case ("�ݰ�"):
                {
                    if (BattleManager.Instance.pureAttack == false) return;
                    StatusPopup(enemy);
                    BattleManager.Instance.Damage_toPlayer(enemy._statusValue);
                    return;
                }
            case ("�һ��Ǹ�"):
                {
                    StatusPopup(enemy);
                    BattleManager.Instance.Heal_Enemy(enemy._statusValue, enemy);
                    return;
                }
            case ("����"):
                {
                    if (enemy._hp >= enemy._statusValue) return;
                    StatusPopup(enemy);
                    BattleManager.Instance.Heal_Enemy(enemy._statusValue-enemy._hp, enemy);

                    return;
                }
            case ("����"):
                {
                    var cardName = "Ű����ũ";
                    if (BattleManager.Instance.pureAttack == false) return;
                    Debug.Log("���� : " + CardDatabase.Instance.SpeciesCombination(BattleManager.Instance.SpeciesCombi));
                    if (CardDatabase.Instance.SpeciesCombination(BattleManager.Instance.SpeciesCombi) >= 3) return;
                    StatusPopup(enemy);
                    BattleManager.Instance.Heal_Enemy(enemy._statusValue, enemy);
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.Instance.card_token(cardName));
                    BattleManager.Instance.Card_Add_toDeckOrGrave(queue, true);

                    return;
                }
        }
        return ;
    }
    public void Action_TurnEnd(string name, Enemy enemy)
    {
        switch (name)
        {
            case "�����ӱ�":
                {
                    if (BattleManager.Instance.Deck.Count <= 0) return;
                    StatusPopup(enemy);
                    var deleteTarget = BattleManager.Instance.Deck[0];
                    BattleManager.Instance.Deck.RemoveAt(0);
                    deleteTarget.deleteCard();
                    return;
                }
            case "�г�":
                {
                    StatusPopup(enemy);
                    enemy.setAttackBuff(enemy._statusValue);
                    return;
                }
            case "�θ�":
                {
                    var enemyName = "�ѱ�";
                    if (UnityEngine.Random.Range(0, 10) < 2)
                    {
                        StatusPopup(enemy);
                        BattleManager.Instance.Summon_Enemy(EnemyDatabase.Instance.enemy(enemyName));
                    }
                    return;
                }
            case "��������":
                {
                    StatusPopup(enemy);
                    int RandomValue = Random.Range(0, 5);
                    Debug.Log(RandomValue);
                    if (RandomValue == 0)
                    {
                        enemy.gainShield(enemy._statusValue);
                    }
                    else if (RandomValue == 1)
                    {
                        enemy.setAttackBuff(enemy._statusValue);
                    }
                    else if (RandomValue == 2)
                        BattleManager.Instance.Damage_toEnemy(enemy._statusValue, false, enemy);
                    else if (RandomValue == 3)
                    {
                        enemy.statusValueChange(enemy._statusValue+Random.Range(1, 11));
                    }
                    else if (RandomValue == 4)
                    {
                        BattleManager.Instance.Heal_Enemy(enemy._statusValue, enemy);
                    }

                    return;
                }
            case "������":
                {
                    if ((float)enemy._hp*100/(float)enemy._mhp<50)
                    {
                        StatusPopup(enemy);
                        enemy.setAttackBuff(enemy._statusValue);
                        enemy.statusValueChange(enemy._statusValue+5);

                    }
                    return;
                }
            case "ȭ�Ʒ�":
                {
                    enemy.statusValueChange(enemy._statusValue - 1);
                    if (enemy._statusValue > 0) return; 
                    StatusPopup(enemy);
                    EffectManager.effectOn(EffectManager.EffectName.Fire, new Vector2(BattleManager.Instance.HandPos[2].x, BattleManager.Instance.HandPos[0].y));
                    BattleManager.Instance.Damage_toPlayer(50);
                    enemy.statusValueChange(3);
                    return;
                }
            case "������":
                {
                    if (BattleManager.Instance.RerollChance != 0) return;
                    StatusPopup(enemy);
                    enemy.setAttackBuff(enemy._statusValue);
                    return;
                }
            case "�����":
                {
                    if (enemy._statusValue == 0) return;
                    StatusPopup(enemy);
                    BattleManager.Instance.Heal_Enemy(enemy._statusValue, enemy);
                    enemy.statusValueChange(enemy._statusValue - 1);
                    return;
                }
            case "������":
                {
                    StatusPopup(enemy);
                    EffectManager.effectOn(EffectManager.EffectName.Fracture, new Vector2(BattleManager.Instance.HandPos[2].x, BattleManager.Instance.HandPos[0].y));
                    BattleManager.Instance.Damage_toPlayer(enemy._statusValue);
                    enemy.statusValueChange(enemy._statusValue + 1);
                    return;
                }
            case "������ �����":
                {
                    var cardName = "����";
                    var tmpValue = BattleManager.Instance.howManyCardsinDeck(cardName) + BattleManager.Instance.howManyCardsinGrave(cardName);
                    enemy.statusValueChange(tmpValue);
                    if (tmpValue == 0) return;
                    StatusPopup(enemy);
                    foreach(Enemy tmpEnemy in BattleManager.Instance.Enemies)
                        tmpEnemy.setAttackBuff(enemy._statusValue);
                    return;
                }

        }

    }
    public void Action_Reroll(string name, Enemy enemy)
    {
        switch (name)
        {
            case "���� �������":
                {
                    var cardName = "����";
                    StatusPopup(enemy);
                    Queue<CardStruct> queue = new Queue<CardStruct>();
                    queue.Enqueue(CardDatabase.Instance.card_token(cardName));
                    BattleManager.Instance.Card_Add_toDeckOrGrave(queue, false);
                    return;
                }
        }

    }
    public void Action_WhileDying(string name, Enemy enemy)
    {
        switch (name)
        {
            case ("��������ź"):
                {
                    StatusPopup(enemy);
                    BattleManager.Instance.Damage_toEnemy_Wide(20);
                    return;
                }
            case ("��"):
                {
                    var enemyName = "������ų����";
                    StatusPopup(enemy);
                    BattleManager.Instance.Summon_Enemy(EnemyDatabase.Instance.enemy(enemyName));
                    return;
                }
            case ("����"):
                {
                    StatusPopup(enemy);
                    foreach (var tmpEnemy in BattleManager.Instance.Enemies)
                        tmpEnemy.setAttackBuff(enemy._statusValue);
                    return;
                }
            case ("������"):
                {
                    var enemyName = "�����ϼ���";
                    StatusPopup(enemy);
                    BattleManager.Instance.Summon_Enemy(EnemyDatabase.Instance.enemy(enemyName));
                    return;
                }
        }
        return;
    }
    public void StatusPopup(Enemy enemy)
    {
        DamagePopup.Create(enemy.transform.position, enemy._statusName, Color.white);
    }
}
[Serializable]
public struct StatusStruct
{
    public string _name;
    public Sprite sprite;
    public string _info;
}
