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
    public int Score_Time => System.Math.Max(1000 - (System.Math.Min(Time_Min,20) - 20) * 50,0);
    public int Score_Total => Score_Level + Score_Time + Score_HighestDam;

    public int HighestScore = PlayerPrefs.GetInt("HighestScore", 0);
    public bool isNew => HighestScore < Score_Total;
    public int newHighestScore => isNew ? Score_Total : HighestScore;
    public string Text_New => isNew ? "New" : "";

    private void OnEnable()
    {
        PlayerPrefs.SetInt("HighestScore", newHighestScore);
        text.text = ("" +
                "<color=red><size=100>GAME OVER</size></color>\n\n"+

                "���̵� ���� - "+Level+" - <color=red >"+Score_Level+"</color >\n"+
                "�ð����� - "+Time_Min+":"+Time_Sec+" -<color=red >"+Score_Time+"</color >\n"+
                "�ְ� ����� ���� - "+HighestDam+" - <color=red > "+Score_HighestDam+" </color >\n\n"+

                "�� ����: "+Score_Total+"\n"+
                "�ְ� ����: "+ newHighestScore +" <color=blue> "+Text_New+" </color> ").Replace("\\n", "\n");
            ;
    }
}
