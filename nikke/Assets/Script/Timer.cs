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
   
    public string TIME()
    {
        var totalSec = Mathf.Round(Resource.Instance.time);
        Debug.Log(totalSec+" "+ Resource.Instance.time);
        var Min = (int)(totalSec / 60);
        var Sec = (totalSec - 60 * Min);
        Debug.Log(Min + " : " + Sec);
        return $"{Min}MIN {Sec}SEC";
    }
    void Update()
    {

        if (!End)
        {
            Resource.Instance.time += Time.deltaTime;
        }
    }

}
