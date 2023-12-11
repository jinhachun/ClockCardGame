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
    bool isArea(int area,EventStruct eventStruct)
    {
        if (area == 1)
            return Resource.Instance.Area == 1 && eventStruct._isAreaOne;
        else if (area == 2)
            return Resource.Instance.Area == 2 && eventStruct._isAreaTwo;
        if (area == 3)
            return Resource.Instance.Area == 3 && eventStruct._isAreaThree;
        else return true;
    }
    public List<EventStruct> EventList_area(int area,bool boss) => EventList.Where(x => (isArea(area,x)) && x._boss==boss).ToList();
    public EventStruct Event_area(int area,bool boss) => EventList_area(area,boss)[Random.Range(0, EventList_area(area,boss).Count)];
    
    
    public rewardAction eventSelect(string eventName, int selectIndex)
    {
        switch (eventName)
        {
            case "동글이와 두더지씨":
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
            case "신병받아라!":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Add("기사돌이");
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
            case "너 해고야!":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove("동글이");
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
            case "동전수집가":
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
            case "드르렁~":
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
            case "빅둘기 격파!":
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
            case "위험한 도박":
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
