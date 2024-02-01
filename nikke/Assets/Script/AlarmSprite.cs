using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSprite : MonoBehaviour
{
    [SerializeField] InfoAlarm alarm;
    [SerializeField] SpriteRenderer spriteRenderer;
    
    public void set(StatusStruct statusStruct)
    {
        spriteRenderer.sprite = statusStruct.sprite;
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (GetComponent<Collider2D>().OverlapPoint(wp))
            {
                OnTouchDown();
            }
            else
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
        alarm.gameObject.SetActive(true);
    }

    void OnTouchUp()
    {
        alarm.gameObject.SetActive(false);
    }

}
