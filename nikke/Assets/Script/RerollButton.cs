using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RerollButton : MonoBehaviour
{
    [SerializeField] SpriteRenderer _rerollButtonPrefab;
    [SerializeField] string _Name;
    public int index;
    bool isOver = false;
    public delegate void buttonAction();
    buttonAction Action;
    public Sprite btnSprite => _rerollButtonPrefab.sprite;
    public void SpriteChange(Sprite s)
    {
        _rerollButtonPrefab.sprite = s;
    }
    public void rerollEnable(bool a)
    {
        if (a)
        {
            this.enabled = true;
            _rerollButtonPrefab.enabled = true;
        }
        else
        {
            this.enabled = false;
            _rerollButtonPrefab.enabled = false;
        }
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