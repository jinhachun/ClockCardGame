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
    private void OnMouseOver()
    {
        alarm.gameObject.SetActive(true);
    }
    private void OnMouseExit()
    {
        alarm.gameObject.SetActive(false);
    }
}
