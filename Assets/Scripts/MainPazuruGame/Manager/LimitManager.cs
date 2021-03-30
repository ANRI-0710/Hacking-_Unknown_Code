using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リミットゲージ及び必殺技ボタンの管理クラス
/// </summary>

public class LimitManager : MonoBehaviour
{
    [SerializeField]
    private LimitGaugeValues _LimitGaugeManager;    
  
    [SerializeField]
    private LimitButton _LimitButtonManager;

    //リミットゲージの値
    private int LimitMax = 50;

    //limitのゲージ数値、LimitMaxになったらLimitButtonをONにする
    private int LimitHP;
    public int GetLimitGaugeHP
    {
        get => LimitHP;
        set 
        {
            LimitHP = value;
            IncreaseLimitGauge();
        }
    }

    private void Awake()
    {
        //_LimitGaugeManager = 



    }


    public void InitLimitObject(RectTransform rectTransform) 
    {
        _LimitGaugeManager.InitLimitGauge(rectTransform);
        _LimitButtonManager.InitLimitButton(rectTransform);
    }

    public void IncreaseLimitGauge() 
    {
        _LimitGaugeManager._GetHpLimitValue = LimitHP;
    }


}
