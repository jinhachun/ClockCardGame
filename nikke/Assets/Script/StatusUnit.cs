using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUnit : MonoBehaviour
{
    string _name;
    Sprite _sprite;
    int _value;
    public void Set(StatusStruct struc,int value)
    {
        this._name = struc._name;
        this._sprite = struc.sprite;
        this._value = value;
    }
}
