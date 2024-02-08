using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] Image _image;
    public int progress;
    [SerializeField] List<Sprite> _sprites;

    public void Start()
    {
        if (PlayerPrefs.GetInt("Tutorial",0) == 1)
        {
            this.gameObject.SetActive(false);
        }
        progress = -1;
        PlayerPrefs.SetInt("Tutorial", 1);
        Next();
    }
    public void Next()
    {
        if (_sprites.Count-1 == progress)
        {
            this.gameObject.SetActive(false);
            return;
        }
        progress++;
        _image.sprite = _sprites[progress];
    }
    public void Back()
    {
        if (progress <= 0) return;
        progress--;
        _image.sprite = _sprites[progress];
    }
}
