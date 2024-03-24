using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EventDatabase : MonoBehaviour
{
    [SerializeField] List<EventStruct> EventList;
    private static EventDatabase instance;
    public static EventDatabase Instance => instance;
    public void Awake()
    {
        instance = this;
    }
    public EventStruct eventNum(int num) => EventList[num];
    bool isArea(int area, EventStruct eventStruct)
    {
        if (area == 1)
            return eventStruct._isAreaOne;
        else if (area == 2)
            return eventStruct._isAreaTwo;
        else if (area == 3)
            return eventStruct._isAreaThree;
        else if (area == 4)
            return eventStruct._isAreaFour;
        else return false;
    }
    public List<EventStruct> EventList_area(int area, bool boss) => EventList.Where(x => (isArea(area, x)) && x._boss == boss).ToList();
    public EventStruct Event_area(int area, bool boss) => EventList_area(area, boss)[Random.Range(0, EventList_area(area, boss).Count)];

    public bool eventCondition(string eventName, int selectIndex)
    {
        switch (eventName)
        {
            case "�����̿� �δ�����":
                {
                    if (selectIndex == 0)
                    {
                        if (Resource.Instance.Hp <= 20) return false;
                    }
                    return true;
                }
            case "������ ����":
                {
                    if (selectIndex == 0)
                    {
                        if (Resource.Instance.Deck.Count == 0) return false;
                    }
                    else if (selectIndex == 1)
                    {
                        if (Resource.Instance.money < 100) return false;
                    }
                    return true;
                }
            case "ġ������ ���":
                {
                    if (selectIndex == 1)
                    {
                        if (!Resource.Instance.haveCard("����")) return false;
                    }
                    return true;
                }
            case "��ö��":
                {
                    if (selectIndex == 0 || selectIndex == 1)
                    {
                        if (!Resource.Instance.haveCard("�����")) return false;
                    }
                    return true;
                }
            case "�� �ذ��!":
                {
                    if (selectIndex == 0)
                    {
                        if (!Resource.Instance.haveCard("������")) return false;
                    }
                    return true;
                }
            case "������ ��ȭ":
                {
                    if (selectIndex == 0)
                    {
                        if (!Resource.Instance.haveCard("�Ա���")) return false;
                    }
                    return true;
                }
            case "���� Ƽ���":
                {
                    if (selectIndex == 0)
                    {
                        if (Resource.Instance.Hp*100/Resource.Instance.mHp>50) return false;
                    }
                    return true;
                }
            case "����Ž��":
                {
                    if (selectIndex == 0)
                    {
                        if (Resource.Instance.Hp <= 30) return false;
                    }
                    return true;
                }
            case "���� �� ���� ����":
                {
                    if (selectIndex == 0)
                    {
                        if (!Resource.Instance.haveCard("ũ�缼����")) return false;
                    }
                    else if (selectIndex == 2)
                    {
                        if (Resource.Instance.Deck.Where(x=>x._tier==2).ToList().Count==0) return false;
                    }
                    return true;
                }
            case "[��ü] ������ ����":
                {
                    if (selectIndex == 0)
                    {
                        if (!Resource.Instance.haveCard("ũ�缼����")|| !Resource.Instance.haveCard("���絹��")) return false;
                    }
                    return true;
                }
            case "[��ü] ��ȥ��Ż��":
                {
                    if (selectIndex == 0)
                    {

                        if (!Resource.Instance.haveCard("���") || !Resource.Instance.haveCard("�����")) return false;
                    }
                    else if (selectIndex == 1)
                    {
                        if (Resource.Instance.Deck.Where(x => x._tier == 2).ToList().Count == 0) return false;
                    }
                    return true;
                }
            case "[��ü] �츮���� ������":
                {
                    if (selectIndex == 0)
                    {
                        if (!Resource.Instance.haveCard("�ɸ����λ�") || !Resource.Instance.haveCard("Ƽ���")) return false;
                    }
                    return true;
                }
            case "[��ü] ��ü�κ��� ����":
                {
                    if (selectIndex == 0)
                    {
                        if (!Resource.Instance.haveCard("���Ըӽ�+") || !Resource.Instance.haveCard("��ġ��")) return false;
                    }
                    return true;
                }
            case "������ ��":
                {
                    if (selectIndex == 0)
                    {
                        return false;
                    }
                    return true;
                }
            case "������ ����":
                {
                    if (selectIndex == 0)
                    {
                        return !Resource.Instance.Rule_no(16);
                    }
                    return true;
                }
        }
        return true;
    }
    public rewardAction eventSelect(string eventName, int selectIndex)
    {
        switch (eventName)
        {
            case "�����̿� �δ�����":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Damage(20);
                            Resource.Instance.Event_MoneyEarn(100);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Heal(20);
                        });
                    }
                    return null;
                }
            case "�ź��޾ƶ�!":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Add(5);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                        });
                    }
                    return null;
                }
            case "�� �ذ��!":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(0,false);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                        });
                    }
                    return null;
                }
            case "����������":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_MoneyEarn(50);
                        });
                    }
                    return null;
                }
            case "��ȭ�� ��":
                {
                    if (selectIndex == 0)
                    {
                        return (() => {
                            var CardDeck = Resource.Instance.Deck.Where(x => x._tier != 5 && !x._Token).ToList();
                            if (CardDeck.Count != 0)
                            {
                                var _tmpStruct = (CardDeck[Random.Range(0, CardDeck.Count)]);
                                var _tmpEvolveStruct = CardDatabase.Instance.card(_tmpStruct.evol[Random.Range(0, _tmpStruct.evol.Count)]);
                                Resource.Instance.Deck_Remove(_tmpStruct.NUM, false);
                                Resource.Instance.Deck_Add(_tmpEvolveStruct.NUM);
                            }
                        });
                    }else if(selectIndex == 1)
                    {
                        return (() => { });
                    }
                    return null;
                }
            case "�帣��~":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Heal(30);
                        });
                    }
                    return null;
                }
            case "���� ����!":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Heal(Resource.Instance.mHp);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_MoneyEarn(300);
                        });
                    }
                    else if (selectIndex == 2)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Add(CardDatabase.Instance.cardByTier(4).NUM);
                        });
                    }
                    return null;
                }
            case "������ ����":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(Resource.Instance.Deck[Random.Range(0,Resource.Instance.Deck.Count)]._name);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            if (Resource.Instance.money >= 100)
                            {
                                Resource.Instance.money -= 100;
                                Resource.Instance.Event_MoneyEarn(Random.Range(0,201));
                            }
                        });
                    }
                    else if (selectIndex == 2)
                    {
                        return (() =>
                        {
                        });
                    }
                    return null;
                }
            case "���� ������":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Heal(Resource.Instance.mHp);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_MoneyEarn(600);
                        });
                    }
                    else if (selectIndex == 2)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Add(CardDatabase.Instance.cardByTier(5).NUM);
                        });
                    }
                    return null;
                }
            case "�ɸ����λ��� ����":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.combiRate_Species[3] = 2.5f;
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Add(31);
                        });
                    }
                    return null;
                }
            case "ġ������ ���":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Heal(30);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(2,false);
                        });
                    }
                    return null;
                }
            case "��ö��":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(4,false);
                            Resource.Instance.Event_Heal(15);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(4,false);
                            Resource.Instance.Event_MoneyEarn(150);
                        });
                    }
                    else if (selectIndex == 2)
                    {
                        return (() =>
                        {
                        });
                    }
                    return null;
                }
            case "������ ��ȭ":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            var _tmpStruct = CardDatabase.Instance.card("�Ա���");
                            var _tmpEvolveStruct = CardDatabase.Instance.card(_tmpStruct.evol[Random.Range(0, _tmpStruct.evol.Count)]);
                            Resource.Instance.Deck_Remove(_tmpStruct.NUM,false);
                            Resource.Instance.Deck_Add(_tmpEvolveStruct.NUM);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                        });
                    }
                    return null;
                }
            case "�ȵ����̿��� ����":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Add(7,true);
                        });
                    }
                    return null;
                }
            case "���� Ƽ���":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_MoneyEarn(400);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_MoneyEarn(200);
                        });
                    }
                    return null;
                }
            case "����Ž��":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Damage(30);
                            for (int i = 0; i < Resource.Instance.combiRate_Species.Count; i++)
                                Resource.Instance.combiRate_Species[i] += 0.1f;
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                        });
                    }
                    return null;
                }
            case "��Ʃ���� ����":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Damage(Resource.Instance.Hp-1);
                            Resource.Instance.Event_MoneyEarn(999);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Heal(20);
                        });
                    }
                    return null;
                }
            case "���� �� ���� ����":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Heal(Resource.Instance.mHp);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Heal(15);
                        });
                    }
                    else if (selectIndex == 2)
                    {
                        return (() =>
                        {
                            var tmpList = Resource.Instance.Deck.Where(x => x._tier == 2).ToList();
                            var targetCard = tmpList[Random.Range(0, tmpList.Count)];
                            Resource.Instance.Deck_Remove(targetCard._name);

                        });
                    }
                    return null;
                }
            case "[��ü] ������ ����":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(6, false);
                            Resource.Instance.Deck_Remove(9, false);
                            Resource.Instance.Deck_Add(73);

                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Heal(15);
                        });
                    }
                    return null;
                }
            case "[��ü] ��ȥ��Ż��":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(13, false);
                            Resource.Instance.Deck_Remove(42, false);
                            Resource.Instance.Deck_Add(74);

                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            var tmpList = Resource.Instance.Deck.Where(x => x._tier == 2).ToList();
                            var targetCard = tmpList[Random.Range(0, tmpList.Count)];
                            Resource.Instance.Deck_Remove(targetCard._name);

                        });
                    }
                    return null;
                }
            case "[��ü] �츮���� ������":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(31, false);
                            Resource.Instance.Deck_Remove(19, false);
                            Resource.Instance.Deck_Add(75);

                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_MoneyEarn(300);
                        });
                    }
                    return null;
                }
            case "[��ü] ��ü�κ��� ����":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(58, false);
                            Resource.Instance.Deck_Remove(29, false);
                            Resource.Instance.Deck_Add(76);

                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                        });
                    }
                    return null;
                }
            case "������ ��":
                {
                    if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            DG.Tweening.DOTween.KillAll();
                            UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
                        });
                    }
                    return null;
                }
            case "������ ����":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Heal(Resource.Instance.mHp);
                        });
                    }
                    return null ;
                }
        }
        return null;

        
    }
}
[Serializable]
public struct EventStruct
{
    public bool _isAreaOne;
    public bool _isAreaTwo;
    public bool _isAreaThree;
    public bool _isAreaFour;
    public string _eventName;
    [Multiline(4)]
    public string _eventText;
    public Sprite _eventSprite;
    public List<EventSelection> _eventSelect;
    public bool _boss;
}
[Serializable]
public struct EventSelection
{
    public string _text;
}
