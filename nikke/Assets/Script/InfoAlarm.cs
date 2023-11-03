using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoAlarm : MonoBehaviour
{
    [SerializeField] TMP_Text _info;
    [SerializeField] SpriteRenderer _background;
    public string info;
    public void set(string info)
    {
        this.info = info;
        _info.text = this.info;
        _background.transform.localScale = new Vector2(_info.preferredWidth + 1, 1.5f);
    }
}
