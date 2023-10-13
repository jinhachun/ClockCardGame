using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VillageContent : MonoBehaviour
{
    [SerializeField] SpriteRenderer _image;
    [SerializeField] List<Sprite> _imageList;

    [SerializeField] TMP_Text _text;
    private int level => Resource.Instance.VillageLevel[this.gameObject.name];
    public void TextUpdate()
    {
        _image.sprite = _imageList[level >= 5 ? 4 : level];
        _text.text = level + "�ܰ�";
    }
    private void FixedUpdate()
    {
        Invoke("TextUpdate", 0.5f);
    }
    public void Upgrade()
    {
        if (Resource.Instance.jewel > 0)
        {
            Resource.Instance.jewel--;
            Resource.Instance.VillageLevel[this.gameObject.name]++;
        }
    }
}