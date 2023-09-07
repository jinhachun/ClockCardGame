using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] ScrollViewCardBunch _scrollViewCardPrefab;
    [SerializeField] TMP_Text __DeckShowText;
    public string _DeckShowText_showdeck;
    public string _DeckShowText_not_showdeck;
    [SerializeField] TMP_Text __BattleButtonText;
    public string __BattleButtonText_Stage;
    string __BattleButtonText_Stage_Full => Resource.Instance.Area + " -" + Resource.Instance.Stage + "\n" + __BattleButtonText_Stage;

    [SerializeField] TMP_Text _MoneyText;
    [SerializeField] Card _cardPrefab;
    
    private static MainMenu instance;
    public static MainMenu Instance => instance;
    public void Awake()
    {
        instance = this;
    }
    public void FixedUpdate()
    {
        __BattleButtonText.text = __BattleButtonText_Stage_Full;
        _MoneyText.text = Resource.Instance.money.ToString();
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
        SceneManager.LoadScene("BattleScene");
    }
}
