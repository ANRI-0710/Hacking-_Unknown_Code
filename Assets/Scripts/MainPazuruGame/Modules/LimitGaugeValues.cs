using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 必殺技（リミット）ゲージを管理するクラス
/// </summary>

public class LimitGaugeValues : MonoBehaviour
{
    //Limitゲージ
    [SerializeField]
    private GameObject _LimitGauge;
    [SerializeField]
    private RectTransform _LimitTransform;
    [SerializeField]
    private GameObject _LimitButton;

    private GameObject LimitGaugeControll;

    //リミットゲージの表示
    private HP _HPLimitGauge;

    //ゲージの数値のプロパティ
    private int gaugeValue;
    public int _GetHpLimitValue { get => _HPLimitGauge.GetHp; set { _HPLimitGauge.GetHp = value; } }

    //リミットがMaxになるゲージの値
    private int LimitMax = 50;

    /// <summary>
    /// Limitゲージの初期化処理
    /// </summary>
    public void InitLimitGauge(RectTransform rectTransform)
    {
        LimitGaugeControll = Instantiate(_LimitGauge);
        LimitGaugeControll.transform.localPosition = new Vector3(Screen.width - Screen.width / 4, Screen.height + Screen.height / 8, 0);
        LimitGaugeControll.transform.SetParent(rectTransform);
        _HPLimitGauge = LimitGaugeControll.GetComponent<HP>();
        _HPLimitGauge.InitSetsize(0.4f, 0.1f);
        _HPLimitGauge.InitSetHp(100, 0);
    }


    //public void LimitPlus()
    //{
    //    _HPLimitGauge.GetHp++;
    //    if (_HPLimitGauge.GetHp == LimitMax)
    //    {
    //        Debug.Log(" 1000ninatta" + _HPLimitGauge.GetHp);
    //        _ButtonControll[0].interactable = true;
    //        _ButtonControll[1].interactable = true;
    //        PuzzleSoundManager.Instance.SE_Selection(SE_Now.LimitMax);
    //    }
    //}
}
