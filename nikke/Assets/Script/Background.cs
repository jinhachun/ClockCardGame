using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Background : MonoBehaviour
{
    private SpriteRenderer _image;
    public List<Sprite> _backgroundList;

    private void Awake()
    {
        _image = GetComponent<SpriteRenderer>();

    }
    void Start()
    {
        _image.sprite = _backgroundList[Resource.Instance.Area - 1];
    }

}
