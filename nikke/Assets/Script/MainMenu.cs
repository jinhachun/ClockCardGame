using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] ScrollViewCardBunch _scrollViewCardPrefab;
    [SerializeField] TMP_Text __DeckShowText;
    public string _DeckShowText_showdeck;
    public string _DeckShowText_not_showdeck;
    [SerializeField] TMP_Text __BattleButtonText;
    public string __BattleButtonText_Stage;
    string __BattleButtonText_Stage_Full => Resource.Instance.Stage != 6 ? (Resource.Instance.Area + " -" + Resource.Instance.Stage + "\n" + __BattleButtonText_Stage):  "BOSS - "+Resource.Instance.Area;
    string _BattleButtonText_Area4 => "FINAL BOSS";
    string _BattleButtonText_End => "END";


    [SerializeField] TMP_Text _MoneyText;
    [SerializeField] Card _cardPrefab;

    List<GameObject> MenuList;
    List<GameObject> ButtonList;
    [SerializeField] GameObject _Main;
    [SerializeField] GameObject _Shop;
    [SerializeField] GameObject _Village;
    [SerializeField] GameObject _Support;
    [SerializeField] GameObject _MainButton;
    [SerializeField] GameObject _ShopButton;
    [SerializeField] GameObject _VillageButton;
    [SerializeField] GameObject _SupportButton;

    private static MainMenu instance;
    public static MainMenu Instance => instance;
    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        MenuList = new List<GameObject>();
        MenuList.Add(_Main);
        MenuList.Add(_Shop);
        MenuList.Add(_Village);
        MenuList.Add(_Support);
        ButtonList = new List<GameObject>();
        ButtonList.Add(_MainButton);
        ButtonList.Add(_ShopButton);
        ButtonList.Add(_VillageButton);
        ButtonList.Add(_SupportButton);
        TextUpdate();
    }
    public void TextUpdate()
    {
        _MoneyText.text = Resource.Instance.money.ToString();
        if (Resource.Instance.Area == 5)
        {
            __BattleButtonText.text = _BattleButtonText_End;
            return;
        }
        if (Resource.Instance.Area == 4)
        {
            __BattleButtonText.text = _BattleButtonText_Area4;
            return;
        }
        __BattleButtonText.text = __BattleButtonText_Stage_Full;
    }
    public void FixedUpdate()
    {
        Invoke("TextUpdate",0.5f);
    }
    public void showDeck()
    {
        if (_scrollViewCardPrefab.gameObject.activeInHierarchy)
        {
            __DeckShowText.text = _DeckShowText_showdeck;
            _scrollViewCardPrefab.delete();
            return;
        }
        List<Card> tmpDeck = new List<Card>();
        foreach(CardStruct tmpStruct in Resource.Instance.Deck)
        {
            Card tmpCard = Instantiate(_cardPrefab);
            tmpCard.Set(tmpStruct);
            tmpDeck.Add(tmpCard);
            Destroy(tmpCard.gameObject);
        }
        _scrollViewCardPrefab.gameObject.SetActive(true);
        __DeckShowText.text = _DeckShowText_not_showdeck;
        _scrollViewCardPrefab.Set(tmpDeck);
    }
    public void battleStart()
    {
        if (Resource.Instance.Area >= 4 && Resource.Instance.Stage!=1) return;
        DOTween.KillAll();
        SceneManager.LoadScene("BattleScene");
    }
    public void ButtonAction(int n)
    {
        for(int i = 1; i <= MenuList.Count; i++)
        {
            if (i == n) { 
                MenuList[i - 1].SetActive(true);
                ButtonList[i - 1].GetComponent<Image>().color = new Color32(185, 193, 233,255);
            }
            else { 
                MenuList[i - 1].SetActive(false);
                ButtonList[i - 1].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
    }
}
