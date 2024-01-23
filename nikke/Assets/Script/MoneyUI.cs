using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public TMPro.TMP_Text text;

    public void FixedUpdate()
    {
        Invoke("TextUpdate", 0.5f);
    }
    public void TextUpdate()
    {
        text.text = Resource.Instance.money.ToString();
    }
}
