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
    public EventStruct eventNum(int num)=>EventList[num];
    public List<EventStruct> EventList_area(int area,bool boss) => EventList.Where(x => x._area.Equals(area) && x._boss==boss).ToList();
    public EventStruct Event_area(int area,bool boss) => EventList_area(area,boss)[Random.Range(0, EventList_area(area,boss).Count)];
    
    
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
        }
        return null;

        
    }
}
[Serializable]
public struct EventStruct
{
    public int _area;
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
