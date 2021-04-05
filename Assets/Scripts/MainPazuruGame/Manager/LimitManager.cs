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
    
    private bool isButtonPress;
    public bool GetisButtonPress 
    {
      get => isButtonPress; 
      set { 
            isButtonPress = value;
            //_LimitButtonManager.LimitAttack();
          } 
    
    }

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
   
    //
    public void InitLimitObject(RectTransform rectTransform) 
    {
        _LimitGaugeManager.InitLimitGauge(rectTransform);
        _LimitButtonManager.InitLimitButton(rectTransform);
        _LimitButtonManager._Button.onClick.AddListener(isLimitButton);
    }

    public void IncreaseLimitGauge() 
    {
        _LimitGaugeManager._GetHpLimitValue = LimitHP;
    }

    public void isLimitButton() 
    {
        GetisButtonPress = true;
    }

    public void LimitPlus()
    {
        GetLimitGaugeHP++;
        if (_LimitGaugeManager._GetHpLimitValue == LimitMax)
        {
            Debug.Log(" 100ninatta" + _LimitGaugeManager._GetHpLimitValue);          
            _LimitButtonManager.LimitInteractable();
            //_ButtonControll[1].interactable = true;
            PuzzleSoundManager.Instance.SE_Selection(SE_Now.LimitMax);
        }
    }

    public void LimitInitValues() 
    {
        _LimitGaugeManager._GetHpLimitValue = 0;
       _LimitButtonManager.LimitUnInteractable();
    }


}
