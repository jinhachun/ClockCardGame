using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Village : MonoBehaviour
{
    [SerializeField] TMP_Text _jewel;

    [SerializeField] TMP_Text _FarmTxt;
    [SerializeField] TMP_Text _HouseTxt;
    [SerializeField] TMP_Text _ChurchTxt;
    [SerializeField] TMP_Text _BathTxt;
    public string _FarmTxtInfo;
    public string _HouseTxtInfo;
    public string _ChurchTxtInfo;
    public string _BathTxtInfo;

    static public int Value_Farm = 2;
    static public int Value_House = 5;
    static public int Value_Church = 4;
    static public int Value_Bath = 15;

    public void Awake()
    {
        TextUpdate();
    }
    private void FixedUpdate()
    {
        Invoke("TextUpdate", 0.3f);
    }
    private void TextUpdate()
    {
        _jewel.text = Resource.Instance.jewel+"";
        _FarmTxt.text = _FarmTxtInfo.Replace("@",Resource.Instance.VillageLevel["Farm"]*Village.Value_Farm+"");
        _HouseTxt.text = _HouseTxtInfo.Replace("@", Resource.Instance.VillageLevel["House"]* Village.Value_House + "");
        _ChurchTxt.text = _ChurchTxtInfo.Replace("@",(5+ Resource.Instance.VillageLevel["Church"] * Village.Value_Church) + "");
        _BathTxt.text = _BathTxtInfo.Replace("@", Resource.Instance.VillageLevel["Bath"] * Village.Value_Bath + "");
    }
}
