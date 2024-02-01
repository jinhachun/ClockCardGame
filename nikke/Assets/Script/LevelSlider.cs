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
    public TMP_Text _text_load;
    public GameObject _LoadFileBox;
    private Slider _slider;
    public ResourceSaveData saveData;
    public void Start()
    {
        
        _slider = _sliderObject.GetComponent<Slider>();
        _slider.maxValue = PlayerPrefs.GetInt("maxLevel",0);
        TextUpdate();

        saveData = Resource.Instance.Load();
        if (saveData == null) _LoadFileBox.SetActive(false);
        else
        {
            _text_load.text = ("" +
                $"Level : {saveData.LEVEL}\n" +
                $"Stage: {saveData.Area} - {saveData.Stage}\n" +
                $"Hp: {saveData.Hp}\n" +
                $"DeckCount: {saveData.Deck.Count}\n").Replace("\\n", "\n");

        }
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
    public void LoadGame()
    {
        if (saveData != null)
        {
            Resource.Instance.StartGame();
            Resource.Instance.LoadData();
            SceneManager.LoadScene("MainScene");
        }
    }
}
