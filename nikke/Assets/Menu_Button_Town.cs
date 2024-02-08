using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Button_Town : MonoBehaviour
{
    [SerializeField] Image _circle;

    void FixedUpdate()
    {
        if (Resource.Instance.jewel > 0) _circle.gameObject.SetActive(true);
        else _circle.gameObject.SetActive(false);
    }
}
