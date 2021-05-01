using UnityEngine;

/// <summary>
/// リミットゲージ及び必殺技ボタンの管理クラス
/// </summary>
public class LimitManager : MonoBehaviour
{    
    //リミットボタン・ゲージへのアクセス
    [SerializeField]
    private LimitGaugeValues _LimitGaugeManager;  
    [SerializeField]
    private LimitButton _LimitButtonManager;
   
    //リミットボタンの表示位置の固定
    [SerializeField]
    private RectTransform _LimitTransform;
    public RectTransform GetLimitTransform { get => _LimitTransform; }

    //ボタンの数の定数
    private const int _button1 = 0;
    private const int _button2 = 1;
    private const int _buttonCount = 2;

    //必殺技ボタン2つのインデクサー、必殺技ボタンを押したかの判定フラグ
    private bool[] isButtonPress = new bool[_buttonCount];
    public bool this[int index] 
    {
        get => isButtonPress[index];
        set { isButtonPress[index] = value; }    
    }

    //リミットゲージのMAX値
    private int LimitMax = 100;    
   
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

    /// <summary>
    /// InitLimitGaugeとInitLimitButtonの初期化
    /// </summary>
    /// <param name="rectTransform"></param>
    public void InitLimitObject(RectTransform rectTransform) 
    {       
        _LimitGaugeManager.InitLimitGauge(rectTransform);
        _LimitButtonManager.InitLimitButton(rectTransform);
        _LimitButtonManager[_button1].onClick.AddListener(IsLimitButton);
        _LimitButtonManager[_button2].onClick.AddListener(IsLimitButton2);
    }

    /// <summary>
    /// ボタン1のリミット技を発動フラグをONにする
    /// </summary>
    public void IsLimitButton() { isButtonPress[_button1] = true; }

    /// <summary>
    /// ボタン2のリミット技を発動フラグをONにする
    /// </summary>
    public void IsLimitButton2() { isButtonPress[_button2] = true; }
    
    /// <summary>
    /// HPゲージを反映する
    /// </summary>
    public void IncreaseLimitGauge() { _LimitGaugeManager._GetHpLimitValue = LimitHP; }
   
    /// <summary>
    /// Limitゲージを1プラスする（破壊するピースの数だけプラスする）
    /// </summary>
    public void LimitPlus()
    {
        GetLimitGaugeHP++;
        if (_LimitGaugeManager._GetHpLimitValue == LimitMax)
        {
            Debug.Log("100ninatta" + _LimitGaugeManager._GetHpLimitValue);          
            _LimitButtonManager.LimitInteractable();
            PuzzleSoundManager.Instance.SE_Selection(SE_Now.LimitMax);
        }
    }

    /// <summary>
    /// リミット技発動後、リミットゲージを0にする
    /// </summary>
    public void LimitInitValues() 
    {
        LimitHP = 0;
       IncreaseLimitGauge();
       _LimitButtonManager.LimitUnInteractable();
    }

}
