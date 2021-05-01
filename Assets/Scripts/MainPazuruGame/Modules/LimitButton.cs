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
    public Button this[int index] //インデクサ
    {
        get => _ButtonControll[index];
        set { _ButtonControll[index] = value; }    
    }
    
    //リミットボタンへのアクセス
    private LimitButton[] _LimitButtonControll = new LimitButton[2];

    //Limitボタンのイメージ画像
    [SerializeField]
    private Sprite[] Sprite;

    //ボタン
    [SerializeField]
    public Button _Button;   
    private Image[] _AttackImage = new Image[2];

    //ボタンが押されたかの判定フラグ
    private bool isButtonPress;
    public bool GetisButtonPress { get => isButtonPress; set { isButtonPress = value; } }

    //リミット技セットした際のセット番号
    private E_SpecialAttack SpecialAttackType;

    //ボタン番号
    private const int Button1 = 0;
    private const int Button2 = 1;

    /// <summary>
    /// リミットボタンの初期化
    /// </summary>
    /// <param name="limitTransform"></param>
    public void InitLimitButton(RectTransform limitTransform) 
    {        
        for (var i = 0; i < _ButtonControll.Length; i++) 
        {
            var obj = Instantiate(_LimitButtonPrefab);
            obj.name = "test";
            obj.transform.SetParent(limitTransform);
            
            _Button = obj.GetComponent<Button>();
            //_Button.onClick.AddListener(ButtonPressed);
            _ButtonControll[i] = obj.GetComponent<Button>();
            _ButtonControll[i].interactable = false;
            _LimitButtonControll[i] = obj.GetComponent<LimitButton>();
            

            var obj2 = obj.transform.GetChild(0).gameObject;
            _AttackImage[i] = obj2.GetComponent<Image>();
         
            if (i == 0)
            {
                obj.transform.localPosition = new Vector3(Screen.width / 11, Screen.height / 9, 0);
                _LimitButtonControll[i].SpecialAttackType = (E_SpecialAttack)GameManager.Instance[0];
                _AttackImage[i].sprite = _LimitButtonControll[i].Sprite[(int)GameManager.Instance[0]];
            }
            else 
            {
                obj.transform.localPosition = new Vector3(Screen.width/ 3.5f , Screen.height /9, 0);
                _LimitButtonControll[i].SpecialAttackType = (E_SpecialAttack)GameManager.Instance[1];
                _AttackImage[i].sprite = _LimitButtonControll[i].Sprite[(int)GameManager.Instance[1]];
            }
        }

    }

    /// <summary>
    /// リミット攻撃フラグをONにする
    /// </summary>
    public void LimitAttack()
    {
        _LimitButtonControll[Button1].isButtonPress = true;
        _LimitButtonControll[Button2].isButtonPress = true;
    }

   /// <summary>
   /// ボタンを押せるようにする
   /// </summary>
    public void LimitInteractable() 
    {
        _ButtonControll[Button1].interactable = true;
       _ButtonControll[Button2].interactable = true;
    }

   /// <summary>
   /// ボタンを押せないようにする
   /// </summary>
    public void LimitUnInteractable()
    {
        _ButtonControll[Button1].interactable = false;
        _ButtonControll[Button2].interactable = false;
    }

}
