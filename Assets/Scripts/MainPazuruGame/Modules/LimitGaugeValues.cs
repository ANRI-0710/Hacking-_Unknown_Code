using UnityEngine;

/// <summary>
/// �K�E�Z�i���~�b�g�j�Q�[�W���Ǘ�����N���X
/// </summary>
public class LimitGaugeValues : MonoBehaviour
{
    //Limit�Q�[�W
    [SerializeField]
    private GameObject _LimitGauge;
    [SerializeField]
    private RectTransform _LimitTransform;
    [SerializeField]
    private GameObject _LimitButton;
    private GameObject LimitGaugeControll;

    //���~�b�g�Q�[�W�̕\��
    private HP _HPLimitGauge;

    //�Q�[�W�̐��l�̃v���p�e�B
    private int gaugeValue;
    public int _GetHpLimitValue { get => _HPLimitGauge.GetHp; set { _HPLimitGauge.GetHp = value; } }

    //���~�b�g��Max�ɂȂ�Q�[�W�̒l
    private int LimitMax = 50;

    /// <summary>
    /// Limit�Q�[�W�̏���������
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
