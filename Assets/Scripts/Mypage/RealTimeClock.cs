using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RealTimeClock : MonoBehaviour
{
    
    void Start()
    {
        var str = GetComponent<Text>();
        var datestrmonth = System.DateTime.Now.Hour.ToString();
        var dateday = System.DateTime.Now.Minute.ToString();

        str.text = datestrmonth + ":" + dateday;

    }

  
}
