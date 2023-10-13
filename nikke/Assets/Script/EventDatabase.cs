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
    public List<EventStruct> EventList_area(int area) => EventList.Where(x => x._area.Equals(area)).ToList();
    public EventStruct Event_area(int area) => EventList_area(area)[Random.Range(0, EventList_area(area).Count)];

    
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
                            Resource.Instance.Hp -= 10;
                            Resource.Instance.money += 20;
                        });
                    }
                    else if (selectIndex == 1)
                    {
                        return (() =>
                        {
                            Resource.Instance.Hp += 10;
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
    public string _eventText;
    public Sprite _eventSprite;
    public List<EventSelection> _eventSelect;
}
[Serializable]
public struct EventSelection
{
    public string _text;
}
