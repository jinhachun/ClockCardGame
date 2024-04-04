using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private static Timer instance;  
    public static Timer Instance => instance;
    public void Awake()
    {
        instance = this;
    }
    public bool End => (Resource.Instance.Area == 4 && Resource.Instance.Stage != 1);

    static public int totalSec => (int)Mathf.Round(Resource.Instance.time);
    static public int Min => (int)(totalSec / 60);
    static public int Sec => (totalSec - 60 * Min);

    static public string TIME()
    {
        return $"{Timer.Min}MIN {Timer.Sec}SEC";
    }
    void Update()
    {

        if (!End)
        {
            Resource.Instance.time += Time.deltaTime;
        }
    }

}
