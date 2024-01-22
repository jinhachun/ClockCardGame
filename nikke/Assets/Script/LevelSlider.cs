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
    }
    public void StartGame()
    {
        Resource.Instance.LEVEL = (int)_slider.value;
        Resource.Instance.StartGame();
        SceneManager.LoadScene("MainScene");

    }
}
