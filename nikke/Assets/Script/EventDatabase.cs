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
            return eventStruct._isAreaThree;
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
                        if (Resource.Instance.Hp < 20) return false;
                    }
                    return true;
                }
            case "�ȵ����̿��� ����":
                {
                    if (selectIndex == 0)
                    {
                        if (Resource.Instance.Hp < 10) return false;
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
                            Resource.Instance.money += 100;
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
                            Resource.Instance.Deck_Add("��絹��");
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
                            Resource.Instance.Deck_Remove("������");
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
                            Resource.Instance.money += 50;
                        });
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
            case "��ѱ� ����!":
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
                            Resource.Instance.money += 300;
                        });
                    }
                    else if (selectIndex == 2)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Add(CardDatabase.Instance.cardByTier(4)._name);
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
                                Resource.Instance.money += Random.Range(0, 200);
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
                            Resource.Instance.money += 600;
                        });
                    }
                    else if (selectIndex == 2)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Add(CardDatabase.Instance.cardByTier(5)._name);
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
                            Resource.Instance.combiRate[3] = 2.5f;
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Add("�ɸ����λ�");
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
                            Resource.Instance.Deck_Remove("����");
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
                            Resource.Instance.Deck_Remove("�����");
                            Resource.Instance.Event_Heal(15);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove("�����");
                            Resource.Instance.money += 150;
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
                            Resource.Instance.Deck_Remove(_tmpStruct._name);
                            Resource.Instance.Deck_Add(_tmpEvolveStruct._name);
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
                            Resource.Instance.Event_Damage(10);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Add("�ȵ�����",true);
                        });
                    }
                    return null;
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
