using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 必殺技（リミット）のボタンを管理するクラス
/// </summary>

public class LimitButton : MonoBehaviour
{
    //リミットボタン
    [SerializeField]
    private Button _LimitButtonPrefab;
    private Button[] _ButtonControll = new Button[2];
    private LimitButton[] _LimitButtonControll = new LimitButton[2];

    private Image _testImage;

    //スペシャルアタックの攻撃
    private int SpecialAttack1;
    private int SpecialAttack2;

    //Limitボタン
    [SerializeField]
    private Sprite[] Sprite;


    //
    [SerializeField]
    private Button _Button;
    private Image _AttackImage;

    private bool isButtonPress;
    public bool GetisButtonPress { get => isButtonPress; set { isButtonPress = value; } }
      
    private E_SpecialAttack SpecialAttackType;
    public E_SpecialAttack GetSpecialAttackType
    {
        get => SpecialAttackType;
        set 
        {
            SpecialAttackType = value;
            SetSpecialAttackImage(SpecialAttackType);
        }     
    }

     void Awake()
   　{
        
        //var obj = transform.GetChild(0).gameObject;
        //_AttackImage = obj.GetComponent<Image>();
        //_Button = this.GetComponent<Button>();
        //_Button.onClick.AddListener(ButtonPressed);
    }


    private void Start()
    {

        SpecialAttack1 = PlayerPrefs.GetInt("ATTACK1", 0);
        SpecialAttack2 = PlayerPrefs.GetInt("ATTACK2", 0);

    }


    public void InitLimitButton(RectTransform limitTransform) 
    {

        var obj = Instantiate(_LimitButtonPrefab);
        obj.name = "test";
        obj.transform.SetParent(limitTransform);
        var obj2 = obj.transform.GetChild(0).gameObject;
        Debug.Log("obj2" + obj2.name);

        _AttackImage = obj2.GetComponent<Image>();
        _Button = obj.GetComponent<Button>();
        _Button.onClick.AddListener(ButtonPressed);

        _ButtonControll[0] = obj.GetComponent<Button>();
        _ButtonControll[0].interactable = false;
        _LimitButtonControll[0] = obj.GetComponent<LimitButton>();

        _AttackImage.sprite = _LimitButtonControll[0].Sprite[1];

        //_LimitButtonControll[0].SpecialAttackType = (E_SpecialAttack)SpecialAttack1;
        // Debug.Log("LimitButtonC" + _LimitButtonControll[0].SpecialAttackType);

        //_LimitButtonControll[0].GetComponent<LimitButton>().SpecialAttackType = (E_SpecialAttack)SpecialAttack1;
        //SetSpecialAttackImage(_LimitButtonControll[0].GetComponent<LimitButton>().SpecialAttackType);
        obj.transform.localPosition = new Vector3(Screen.width / 13, Screen.height / 12, 0);

        //for (var i = 0; i < 2; i++)
        //{
        //    var obj = Instantiate(_LimitButtonPrefab);
        //    obj.name = "test";
        //    obj.transform.SetParent(limitTransform);
        //    var obj2 = obj.transform.GetChild(0).gameObject;

        //    _AttackImage = obj2.GetComponent<Image>();
        //    _Button = obj.GetComponent<Button>();
        //    _Button.onClick.AddListener(ButtonPressed);

        //    _ButtonControll[i] = obj.GetComponent<Button>();
        //    _ButtonControll[i].interactable = false;            
        //    _LimitButtonControll[i] = obj.GetComponent<LimitButton>();
         
        //    if (i == 0)
        //    {
        //        _LimitButtonControll[i].SpecialAttackType = (E_SpecialAttack)SpecialAttack1;
        //        Debug.Log("LimitButtonC"+_LimitButtonControll[i].SpecialAttackType);

        //        //SetSpecialAttackImage(_LimitButtonControll[i].SpecialAttackType);
        //        obj.transform.localPosition = new Vector3(Screen.width / 13, Screen.height / 12, 0);
        //    }
        //    else if (i == 1)
        //    {
        //       //_LimitButtonControll[i].SpecialAttackType = (E_SpecialAttack)SpecialAttack2;
        //        //SetSpecialAttackImage(_LimitButtonControll[i].SpecialAttackType);
        //        obj.transform.localPosition = new Vector3(Screen.width / 10 + Screen.width / 9, Screen.height / 12, 0);
        //    }

        //}

    }


    public void LimitAttack()
    {
        _LimitButtonControll[0].GetisButtonPress = true;
        _LimitButtonControll[1].GetisButtonPress = true;
    }

    public void LimitInteractable() 
    {
        _ButtonControll[0].interactable = true;
        _ButtonControll[1].interactable = true;
    }

    private void SetSpecialAttackImage(E_SpecialAttack SpecialAttackType) 
    {
        switch (SpecialAttackType) 
        {            
            case E_SpecialAttack.SP_Red:
                _AttackImage.sprite = Sprite[1];
                break;
            case E_SpecialAttack.SP_Blue:
                _AttackImage.sprite = Sprite[1];
                break;

            case E_SpecialAttack.SP_Yellow:
                _AttackImage.sprite = Sprite[1];
                break;

            case E_SpecialAttack.SP_Green:
                _AttackImage.sprite = Sprite[1];
                break;

            case E_SpecialAttack.SP_White:
                _AttackImage.sprite = Sprite[1];
                break;

            case E_SpecialAttack.SP_HorizontalOneArray:
                _AttackImage.sprite = Sprite[1];
                break;

            case E_SpecialAttack.SP_VerticalOneArray:
                _AttackImage.sprite = Sprite[1];
                break;

            case E_SpecialAttack.SP_Destroy_Cross:
                _AttackImage.sprite = Sprite[1];
                break;

            case E_SpecialAttack.SP_ObliqueCross_Cross:
                _AttackImage.sprite = Sprite[1];
                break;

            case E_SpecialAttack.SP_BlackPiece_Destroy:
                _AttackImage.sprite = Sprite[1];
                break;
        }
    }

    public void ButtonPressed() 
    {
        GetisButtonPress = true;
        Debug.Log("GetisButtonPress" + GetisButtonPress);

    } 


}
