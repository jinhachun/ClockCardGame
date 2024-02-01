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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (GetComponent<Collider2D>().OverlapPoint(wp))
            {
                OnTouchDown();
            }else
                OnTouchUp();

        }

            
    }
    private void OnMouseOver()
    {
        OnTouchDown();
    }
    private void OnMouseExit()
    {
        OnTouchUp();
    }
    void OnTouchDown()
    {
        if (!active)
        {
            _prefab.color = Color.red;
        }
        else
        {
            isOver = true;
            _prefab.color = Color.gray;

        }
    }

    void OnTouchUp()
    {
        isOver = false;
        _prefab.color = Color.white;
    }
}
