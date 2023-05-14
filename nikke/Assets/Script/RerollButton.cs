using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollButton : MonoBehaviour
{
    [SerializeField] SpriteRenderer _rerollButtonPrefab;
    bool isOver = false;

    void Update()
    {
        if (isOver && Input.GetMouseButtonDown(0))
        {
            //��Ŭ�� �̺�Ʈ
        }
        if (isOver && Input.GetMouseButtonDown(1))
        {
            //��Ŭ�� �̺�Ʈ
        }
    }

    void OnMouseOver()
    {
        isOver = true;
        _rerollButtonPrefab.color = new Color(152,152,152);
        
    }

    void OnMouseExit()
    {
        isOver = false;
    }
}
