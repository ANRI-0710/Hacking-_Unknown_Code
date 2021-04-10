using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �`���[�g���A��(�}�C�y�[�W/�p�Y���p�[�g���ꂼ��2��)�̃|�b�v�A�b�v�����E�摜���Ǘ�����N���X
/// </summary>
public class Popup_Tutorial : Popup
{
    [SerializeField]
    private GameObject _TutorialPopup;
    [SerializeField]
    private Sprite[] _TutorialPopupImages;
    [SerializeField]
    private string[] _TutorialStrings;

    //�摜�ƕ����̃R���g���[���p�̕ϐ�
    private Image _tutrialImageControll;
    private Text _tutrialStringControll;
    
    //�^�b�v�Ŏ��̉摜�E�����ɕύX���邽�߂̊Ǘ��ϐ�
    private int _nextNum = 0;
    
    //���ǃ`�F�b�N
    private bool _isTutrialFinish;
    public bool GetIsTutrialFinish { get => _isTutrialFinish; set { _isTutrialFinish  = value; }  }

    private void Start()
    {
        //�C���[�W�摜�̕ύX�Ǘ�
        var tutrialobj = _TutorialPopup.transform.GetChild(0).gameObject;
        _tutrialImageControll = tutrialobj.GetComponent<Image>();
        _tutrialImageControll.sprite = _TutorialPopupImages[_nextNum];
        
        //���������Ǘ��ϐ�
        var tutrialtext = _TutorialPopup.transform.GetChild(1).gameObject;
        _tutrialStringControll = tutrialtext.GetComponent<Text>();
        _tutrialStringControll.text = _TutorialStrings[_nextNum];
    }

    /// <summary>
    /// �`���[�g���A�����X�^�[�g����i�|�b�v�A�b�v���J�n����j
    /// </summary>
    public void TutorialStart() 
    {
        _TutorialPopup.gameObject.SetActive(true);
        base.PopupStart(_TutorialPopup);
    }

    /// <summary>
    /// ���̃|�b�v�A�b�v���������ۂɃ^�b�v����炷
    /// </summary>
    public void TapSound() { Common_Sound_Manager.Instance.SE_Play(SE.Popup_Tap); }
   
    /// <summary>
    /// ���̃|�b�v�A�b�v�ɐi��
    /// </summary>
    public void NextPopupImage() 
    {
        //���݂���y�[�W�iNextNum�j���|�b�v�A�b�v�摜�̖������B������|�b�v�A�b�v����A���ǃt���O��t����
        if (_nextNum == _TutorialPopupImages.Length -1)
        {
            base.Popup_Close(_TutorialPopup.gameObject);
            GetIsTutrialFinish = true;
            Debug.Log("NextNum" + _nextNum);
        }
        else //�B���Ă��Ȃ��ꍇ�́A�z����v���X1���Ď��̃y�[�W�ɐi��
        {
            if (_nextNum < _TutorialPopupImages.Length) { _nextNum++; }
            Debug.Log("NextNum" + _nextNum);
            _tutrialImageControll.sprite = _TutorialPopupImages[_nextNum];
            _tutrialStringControll.text = _TutorialStrings[_nextNum];           
        }        
    }

}
