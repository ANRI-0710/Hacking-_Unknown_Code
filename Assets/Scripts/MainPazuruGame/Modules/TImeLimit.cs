using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// タイムリミットの秒数画像を表示するクラス
/// </summary>
public class TImeLimit : MonoBehaviour
{
    [SerializeField]
    private GameObject _TimeLimit;

    //子オブジェクト所得用変数
    private GameObject _timeLimitObject;

    //タイムカウントの表示へのアクセス
    private Text _timeCountString;
    private RectTransform _rectTransform;

    //制限時間の所得
    private int _timeLimitNum;
    public int GetTimeLimitint 
    {
        get => _timeLimitNum;
        set 
        {
            _timeLimitNum = value;
            TimeCount();
        }        
    }


    void Awake()
    {       
        _rectTransform = GetComponent<RectTransform>();       
    }

    private void Start()
    {
        _timeLimitObject = _TimeLimit.transform.GetChild(0).gameObject;     
    }

    /// <summary>
    /// タイムカウントの表示
    /// </summary>
    public void TimeCount() 
    {
        var span = new TimeSpan(0, 0, _timeLimitNum);
        _timeCountString = _timeLimitObject.GetComponent<Text>();
        _timeCountString.text = span.ToString();
      
    }

    /// <summary>
    /// 座標調整
    /// </summary>
    /// <param name="Reductionwidth"></param>
    /// <param name="Reductionheight"></param>
    public void InitSetsize(float Reductionwidth, float Reductionheight)
    {
        var sizewidth = (int)((int)Screen.width * Reductionwidth);
        var sizeheight = (int)((int)Screen.height * Reductionheight);
        _rectTransform.sizeDelta = new Vector2(sizewidth, sizeheight);
    }

}
