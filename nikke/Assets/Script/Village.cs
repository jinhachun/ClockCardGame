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
    public void Start()
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
        _FarmTxt.text = _FarmTxtInfo.Replace("@",Resource.Instance.VillageLevel["Farm"]*5+"");
        _HouseTxt.text = _HouseTxtInfo.Replace("@", Resource.Instance.VillageLevel["House"]*5 + "");
        _ChurchTxt.text = _ChurchTxtInfo.Replace("@",(5+ Resource.Instance.VillageLevel["Church"] * 4) + "");
        _BathTxt.text = _BathTxtInfo.Replace("@", Resource.Instance.VillageLevel["Bath"] * 10 + "");
    }
}
