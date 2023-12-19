using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoAlarm : MonoBehaviour
{
    [SerializeField] TMP_Text _info;
    [SerializeField] TMP_Text _name;
    [SerializeField] SpriteRenderer _background;
    public string info;
    public string name;
    public void set(string info,string name,int value)
    {
        this.info = info;
        this.name = name;
        _info.text = this.info.Replace("@",value.ToString());
        _name.text = this.name+" : ";
        _background.transform.localScale = new Vector2(_info.preferredWidth + 1, 1.5f);
    }
    
}
