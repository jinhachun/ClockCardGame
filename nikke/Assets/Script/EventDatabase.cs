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
            case "동글이와 두더지씨":
                {
                    if (selectIndex == 0)
                    {
                        if (Resource.Instance.Hp <= 20) return false;
                    }
                    return true;
                }
            case "안동글이와의 만남":
                {
                    if (selectIndex == 0)
                    {
                        if (Resource.Instance.Hp <= 10) return false;
                    }
                    return true;
                }
            case "위험한 도박":
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
            case "치명적인 고백":
                {
                    if (selectIndex == 1)
                    {
                        if (!Resource.Instance.haveCard("좀비")) return false;
                    }
                    return true;
                }
            case "고철상":
                {
                    if (selectIndex == 0 || selectIndex == 1)
                    {
                        if (!Resource.Instance.haveCard("깡통몬")) return false;
                    }
                    return true;
                }
            case "너 해고야!":
                {
                    if (selectIndex == 0)
                    {
                        if (!Resource.Instance.haveCard("동글이")) return false;
                    }
                    return true;
                }
            case "극적인 진화":
                {
                    if (selectIndex == 0)
                    {
                        if (!Resource.Instance.haveCard("먹깨비")) return false;
                    }
                    return true;
                }
            case "괴수 티라노":
                {
                    if (selectIndex == 0)
                    {
                        if (Resource.Instance.Hp*100/Resource.Instance.mHp>50) return false;
                    }
                    return true;
                }
            case "보물탐험":
                {
                    if (selectIndex == 0)
                    {
                        if (Resource.Instance.Hp <= 30) return false;
                    }
                    return true;
                }
            case "전쟁 속 작은 생명":
                {
                    if (selectIndex == 0)
                    {
                        if (!Resource.Instance.haveCard("크루세돌이")) return false;
                    }
                    else if (selectIndex == 2)
                    {
                        if (Resource.Instance.Deck.Where(x=>x._tier==2).ToList().Count==0) return false;
                    }
                    return true;
                }
            case "여행의 끝":
                {
                    return false;
                }
        }
        return true;
    }
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
            case "신병받아라!":
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
            case "너 해고야!":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(0);
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
                            Resource.Instance.Event_MoneyEarn(50);
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
            case "위험한 도박":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(Resource.Instance.Deck[Random.Range(0,Resource.Instance.Deck.Count)].NUM);
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
            case "숲의 지배자":
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
            case "케르베로삼의 간택":
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
                            Resource.Instance.Deck_Add(31);
                        });
                    }
                    return null;
                }
            case "치명적인 고백":
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
                            Resource.Instance.Deck_Remove(2);
                        });
                    }
                    return null;
                }
            case "고철상":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(4);
                            Resource.Instance.Event_Heal(15);
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Deck_Remove(4);
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
            case "극적인 진화":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            var _tmpStruct = CardDatabase.Instance.card("먹깨비");
                            var _tmpEvolveStruct = CardDatabase.Instance.card(_tmpStruct.evol[Random.Range(0, _tmpStruct.evol.Count)]);
                            Resource.Instance.Deck_Remove(_tmpStruct.NUM);
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
            case "안동글이와의 만남":
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
                            Resource.Instance.Deck_Add(7,true);
                        });
                    }
                    return null;
                }
            case "괴수 티라노":
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
            case "보물탐험":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Damage(30);
                            for (int i = 0; i < Resource.Instance.combiRate.Count; i++)
                                Resource.Instance.combiRate[i] += 0.1f;
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
            case "전쟁 속 작은 생명":
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
                            Resource.Instance.Deck_Remove(targetCard.NUM);

                        });
                    }
                    return null;
                }
            case "마지막 전투":
                {
                    if (selectIndex == 0)
                    {
                        return (() =>
                        {
                            Resource.Instance.Event_Heal(Resource.Instance.mHp);

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
