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
    public int _GetHpLimitValue { get => _HPLimitGauge.GetHp; set { _HPLimitGauge.GetHp = value; } }

    /// <summary>
    /// Limitゲージの初期化処理
    /// </summary>
    public void InitLimitGauge(RectTransform rectTransform)
    {
        LimitGaugeControll = Instantiate(_LimitGauge);
        LimitGaugeControll.transform.localPosition = new Vector3(Screen.width - Screen.width / 4, Screen.height + Screen.height / 9, 0);
        LimitGaugeControll.transform.SetParent(rectTransform);
        _HPLimitGauge = LimitGaugeControll.GetComponent<HP>();
        _HPLimitGauge.InitSetsize(0.4f, 0.1f);
        _HPLimitGauge.InitSetHp(100, 0);
    }

}
