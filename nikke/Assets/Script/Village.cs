using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Village : MonoBehaviour
{
    [SerializeField] TMP_Text _jewel;
    private void FixedUpdate()
    {
        Invoke("TextUpdate", 0.5f);
    }
    private void TextUpdate()
    {
        _jewel.text = Resource.Instance.jewel+"";
    }
}
