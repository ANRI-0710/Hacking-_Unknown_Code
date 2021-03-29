using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���~�b�g�Q�[�W�y�ѕK�E�Z�{�^���̊Ǘ��N���X
/// </summary>

public class LimitManager : MonoBehaviour
{
    [SerializeField]
    private LimitGaugeValues _LimitGaugeManager;    
  
    [SerializeField]
    private LimitButton _LimitButtonManager;

    //���~�b�g�Q�[�W�̒l
    private int LimitMax = 50;

    //limit�̃Q�[�W���l�ALimitMax�ɂȂ�����LimitButton��ON�ɂ���
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
