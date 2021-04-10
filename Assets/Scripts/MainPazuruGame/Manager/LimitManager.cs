using UnityEngine;

/// <summary>
/// ���~�b�g�Q�[�W�y�ѕK�E�Z�{�^���̊Ǘ��N���X
/// </summary>
public class LimitManager : MonoBehaviour
{    
    //���~�b�g�{�^���}�l�[�W���[�A�N�Z�X
    [SerializeField]
    private LimitGaugeValues _LimitGaugeManager;  
    [SerializeField]
    private LimitButton _LimitButtonManager;

    //�{�^���̐��̒萔
    private const int _button1 = 0;
    private const int _button2 = 1;
    private const int _buttonCount = 2;

    //�K�E�Z�{�^��2�̃C���f�N�T�[�A�K�E�Z�{�^�������������̔���t���O
    private bool[] isButtonPress = new bool[_buttonCount];
    public bool this[int index] 
    {
        get => isButtonPress[index];
        set { isButtonPress[index] = value; }    
    }
   
    //���~�b�g�Q�[�W��MAX�l
    private int LimitMax = 100;    
   
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

    /// <summary>
    /// InitLimitGauge��InitLimitButton�̏�����
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
    /// �{�^��1�̃��~�b�g�Z�𔭓��t���O��ON�ɂ���
    /// </summary>
    public void IsLimitButton() { isButtonPress[_button1] = true; }

    /// <summary>
    /// �{�^��2�̃��~�b�g�Z�𔭓��t���O��ON�ɂ���
    /// </summary>
    public void IsLimitButton2() { isButtonPress[_button2] = true; }
    
    /// <summary>
    /// HP�Q�[�W�𔽉f����
    /// </summary>
    public void IncreaseLimitGauge() { _LimitGaugeManager._GetHpLimitValue = LimitHP; }
   
    /// <summary>
    /// Limit�Q�[�W��1�v���X����i�j�󂷂�s�[�X�̐������v���X����j
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
    /// ���~�b�g�Z������A���~�b�g�Q�[�W��0�ɂ���
    /// </summary>
    public void LimitInitValues() 
    {
        LimitHP = 0;
       IncreaseLimitGauge();
       _LimitButtonManager.LimitUnInteractable();
    }

}
