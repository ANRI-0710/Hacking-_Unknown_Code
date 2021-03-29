using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイムリミットの秒数画像を表示するクラス
/// </summary>

public class TImeLimit : MonoBehaviour
{
    [SerializeField]
    private GameObject _TimeLimit;
    private Text Str;
    private GameObject _Object;
    private RectTransform rectTransform;

    private int _TimeLimitNum;
    public int GetTimeLimitint 
    {
        get => _TimeLimitNum;
        set 
        {
            _TimeLimitNum = value;
            TimeCount();
        }        
    }


    void Awake()
    {       
        rectTransform = GetComponent<RectTransform>();       
    }

    private void Start()
    {
        _Object = _TimeLimit.transform.GetChild(0).gameObject;     
    }

    public void TimeCount() 
    {
        var span = new TimeSpan(0, 0, _TimeLimitNum);
        Str = _Object.GetComponent<Text>();
        Str.text = span.ToString();
      
    }

    public void InitSetsize(float Reductionwidth, float Reductionheight)
    {
        var sizewidth = (int)((int)Screen.width * Reductionwidth);
        var sizeheight = (int)((int)Screen.height * Reductionheight);
        rectTransform.sizeDelta = new Vector2(sizewidth, sizeheight);
    }

}
