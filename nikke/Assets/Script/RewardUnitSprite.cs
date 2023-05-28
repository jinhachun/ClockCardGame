using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardUnitSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer _prefab;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] TMP_Text text;
    bool isOver;
    bool get = false;
    public void Set(RewardUnit unit)
    {
        sprite.sprite = unit.sprite;
        text.text = unit._name;
    }
    void Update()
    {
        if (isOver && Input.GetMouseButtonDown(0))
        {
            get = true;
            _prefab.color = Color.black;
        }
    }

    void OnMouseOver()
    {
        if (get) return;
        isOver = true;
        _prefab.color = Color.gray;

    }

    void OnMouseExit()
    {
        if (get) return;
        isOver = false;
        _prefab.color = Color.white;
    }
}
