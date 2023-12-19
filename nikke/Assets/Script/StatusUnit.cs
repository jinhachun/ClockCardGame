using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUnit : MonoBehaviour
{
    [SerializeField] AlarmSprite _alarmSprite;
    [SerializeField] InfoAlarm _infoAlarm;
    public StatusStruct _statusStruct;

    public void set(StatusStruct statusStruct,int value)
    {
        _statusStruct = statusStruct;
        _alarmSprite.set(statusStruct);
        _infoAlarm.set(_statusStruct._info,_statusStruct._name,value);
        
    }
    public void infoChange(int value)
    {
        _infoAlarm.set(_statusStruct._info, _statusStruct._name,value);

    }
}
