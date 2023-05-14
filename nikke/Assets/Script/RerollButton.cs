using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void buttonAction();
public class RerollButton : MonoBehaviour
{
    [SerializeField] SpriteRenderer _rerollButtonPrefab;
    [SerializeField] string _Name;
    bool isOver = false;
    buttonAction Action;
    public Sprite btnSprite => _rerollButtonPrefab.sprite;
    public void SpriteChange(Sprite s)
    {
        _rerollButtonPrefab.sprite = s;
    }
    public void ActionSet(buttonAction bt)
    {
        Action = bt;
    }
    void Update()
    {
        if (isOver && Input.GetMouseButtonDown(0))
        {
            //좌클릭 이벤트
            Action();
        }
    }

    void OnMouseOver()
    {
        isOver = true;
        _rerollButtonPrefab.color = Color.gray;
        
    }

    void OnMouseExit()
    {
        isOver = false;
        _rerollButtonPrefab.color = Color.white;
    }
}