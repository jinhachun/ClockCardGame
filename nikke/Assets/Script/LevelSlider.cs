using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSlider : MonoBehaviour
{
    public GameObject _sliderObject;
    public TMP_Text _text;
    public TMP_Text _text_max;
    private Slider _slider;
    public void Start()
    {
        _slider = _sliderObject.GetComponent<Slider>();
        _slider.maxValue = PlayerPrefs.GetInt("maxLevel",0);
        TextUpdate();
    }
    public void TextUpdate()
    {
        _text.text = "LV."+((int)_slider.value).ToString();
        _text_max.text = "MaxLv : " + _slider.maxValue.ToString();
    }
    public void StartGame()
    {
        Resource.Instance.LEVEL = (int)_slider.value;
        Resource.Instance.StartGame();
        SceneManager.LoadScene("MainScene");

    }
}
