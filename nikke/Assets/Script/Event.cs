using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Event : MonoBehaviour
{
    [SerializeField] Sprite ExitButtonSprites1;
    [SerializeField] Sprite ExitButtonSprites2;
    Sprite ExitButtonSprites(bool exit) => exit ? ExitButtonSprites1 : ExitButtonSprites2;
    [SerializeField] RerollButton _ExitButton;
    [SerializeField] GameObject _Text;
    [SerializeField] TMP_Text _Text_eventname;
    [SerializeField] TMP_Text _Text_eventtext;
    [SerializeField] GameObject _Image;
    [SerializeField] SpriteRenderer _Image_eventimage;
    [SerializeField] GameObject _Button;
    [SerializeField] RewardUnitSprite _ButtonPrefab;
    [SerializeField] Transform _ButtonParent;
    List<RewardUnitSprite> buttonList;
    bool exit;
    float blank = 0.6f;
    public void Start()
    {
        exit = false;
        _ExitButton.ActionSet(()=> {
            _Text.SetActive(exit);
            _Image.SetActive(exit);
            _Button.SetActive(exit);
            _ExitButton.SpriteChange(ExitButtonSprites(exit));
            exit = !exit;
        });
        OnEnable();
    }
    public void OnEnable()
    {
        Debug.Log(Resource.Instance.Area);
        Debug.Log(EventDatabase.Instance.EventList_area(Resource.Instance.Area).Count);
        EventSet(EventDatabase.Instance.Event_area(Resource.Instance.Area));
    }
    public void EventSet(EventStruct a)
    {
        buttonList = new List<RewardUnitSprite>();
        _Text_eventname.text = a._eventName;
        _Text_eventtext.text = a._eventText;
        _Image_eventimage.sprite = a._eventSprite;
        for(int i=0;i<a._eventSelect.Count;i++)
        {
            EventSelection es = a._eventSelect[i];
            var Button = Instantiate(_ButtonPrefab,new Vector2(_ButtonParent.position.x,_ButtonParent.position.y-blank*(i+1)),Quaternion.identity);
            Button.transform.SetParent(_ButtonParent);
            Button.Set(es._text,EventDatabase.Instance.eventSelect(a._eventName,i));

            buttonList.Add(Button);
        }
    }
}