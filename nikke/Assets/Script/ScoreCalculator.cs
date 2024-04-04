using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{


    public TMPro.TMP_Text text;

    public bool Clear;
    public int Level;
    public int HighestDam;
    public int Time_Min;
    public int Time_Sec;

    public int Score_Level => Level*100;
    public int Score_HighestDam => (HighestDam>1000?1000+(int)System.Math.Sqrt(HighestDam-1000):HighestDam);
    public int Score_Time => System.Math.Max(1000 - ((System.Math.Min(Time_Min,20) - 20) * 50),0);
    public int Score_Total => Score_Level + Score_Time + Score_HighestDam;

    public int HighestScore => PlayerPrefs.GetInt("HighestScore", 0);
    public bool isNew;
    public int newHighestScore => isNew ? Score_Total : HighestScore;
    public string Text_New => isNew ? "New" : "";

    public  void Set(bool clear,int level,int HighestDam,int Time_min,int Time_sec)
    {
        this.Clear = clear;
        this.Level = level;
        this.HighestDam = HighestDam;
        this.Time_Min = Time_min;
        this.Time_Sec = Time_sec;
    }


    private void OnEnable()
    {
        isNew = HighestScore < Score_Total;
        text.text = ("" +
                "<color=red><size=100>THE END</size></color>\n\n"+

                "난이도 점수 - "+Level+" - <color=red>"+Score_Level+"</color>\n"+
                "시간점수 - "+Time_Min+":"+Time_Sec+" - <color=red>"+Score_Time+"</color>\n"+
                "최고 대미지 점수 - "+HighestDam+" - <color=red> "+Score_HighestDam+" </color>\n\n"+

                "총 점수: "+Score_Total+"\n"+
                "최고 점수: <color=red>" + newHighestScore + "</color> <color=blue> " + Text_New+" </color> ").Replace("\\n", "\n");
        ;
        PlayerPrefs.SetInt("HighestScore", newHighestScore);
    }
}
