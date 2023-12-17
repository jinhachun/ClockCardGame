using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public delegate void rewardAction();
public class RewardUnitSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer _prefab;
    [SerializeField] TMP_Text text;
    rewardAction Action;
    bool isOver;
    bool get = false;
    public bool active;
    public void Set(string a, rewardAction ac)
    {
        text.text = a;
        Action = ac;
        active = true;
    }
    void Update()
    {
        if (active && isOver && Input.GetMouseButtonDown(0)&&!get)
        {
            Action();
            get = true;
            _prefab.color = Color.black;
        }
        if (get)
        {
            SceneManager.UnloadSceneAsync("EventScene");
        }
    }

    void OnMouseOver()
    {
        if (!active) {
            _prefab.color = Color.red;
        }
        else
        {
            isOver = true;
            _prefab.color = Color.gray;

        }
    }

    void OnMouseExit()
    {
        isOver = false;
        _prefab.color = Color.white;
    }
}
