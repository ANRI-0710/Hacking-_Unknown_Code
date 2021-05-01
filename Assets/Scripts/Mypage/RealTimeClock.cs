using UnityEngine;
using UnityEngine.UI;

/// <summary>
///現在時刻を表示するクラス 
/// </summary>
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
