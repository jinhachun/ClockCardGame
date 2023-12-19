using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RewardPopup : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    void Start()
    {
        
    }
    void Set(int reward)
    {
        text.text = "+" + reward;
    }
    static public Tween Create(int reward)
    {
        var tmp = Instantiate(BattleManager.Instance._rewardPopupPrefab);
        RewardPopup rewardPopup = tmp.GetComponent<RewardPopup>();
        rewardPopup.Set(reward);
        tmp.localScale = new Vector2(0f, 0f);
        return tmp.DOScale(1f, 1f).SetEase(Ease.OutCubic);
    }
}
