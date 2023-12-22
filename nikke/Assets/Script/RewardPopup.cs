using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RewardPopup : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text winText;
    void Start()
    {

    }
    void Set(int reward)
    {
        text.text = "+" + reward;
    }
    void Set(int reward,bool winText)
    {
        this.winText.gameObject.SetActive(winText);
        Set(reward);
    }
    static public Tween Create(int reward)
    {
        var tmp = Instantiate(BattleManager.Instance._rewardPopupPrefab);
        RewardPopup rewardPopup = tmp.GetComponent<RewardPopup>();
        rewardPopup.Set(reward);
        tmp.localScale = new Vector2(0f, 0f);
        return tmp.DOScale(1f, 1f).SetEase(Ease.OutCubic);
    }
    static public void Create(int reward, bool winText)
    {
        var tmp = Instantiate(BattleManager.Instance._rewardPopupPrefab);
        RewardPopup rewardPopup = tmp.GetComponent<RewardPopup>();
        rewardPopup.Set(reward,winText);
        tmp.localScale = new Vector2(0f, 0f);
        tmp.DOScale(1f, 1f).SetEase(Ease.OutCubic).OnComplete(() => { Destroy(tmp.gameObject); });

    }

}

