using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 必殺技セットとパズルステージへの遷移のポップアップ
/// </summary>

public class Pazuru_Setup_Popup :Popup
{
    [SerializeField]
    private GameObject SetUpPanel;
    
    [SerializeField]
    private Dropdown _DropDown;

    [SerializeField]
    private Sprite[] _SetupAttackType;
    private Image image;

    [SerializeField]
    private Button _StartButton;

    [SerializeField]
    private E_SpecialAttack SelectSpecialNum;

    [SerializeField]
    private int SelectNum;

    [SerializeField]
    private Text _DescriptionText;

    void Awake()
    {
        var obj = this.transform.GetChild(0).gameObject;
        image = obj.GetComponent<Image>();

        var obj2 = this.transform.GetChild(1).gameObject;
        _DescriptionText = obj2.GetComponent<Text>();

    }

    private void Start()
    {
        _DropDown.value = 0;
    }

   /// <summary>
   /// ドロップダウンメニューから必殺技を選択し、選んだ必殺技の番号を記録しパズルステージで反映させる
   /// </summary>
    public void SpecialAttack_Select1() 
    {
        var selectnow = _DropDown.value;
        switch ((E_SpecialAttack)selectnow)
        {
            case E_SpecialAttack.SP_Red:
                image.sprite = _SetupAttackType[(int)E_SpecialAttack.SP_Red];
                SetAttackNumber(E_SpecialAttack.SP_Red);
                _DescriptionText.text = "レッドウイルスをランダムで10個発生させる";
                break;
            case E_SpecialAttack.SP_Blue:
                image.sprite = _SetupAttackType[(int)E_SpecialAttack.SP_Blue];
                SetAttackNumber(E_SpecialAttack.SP_Blue);
                _DescriptionText.text = "ブルーウイルスをランダムで10個発生させる";
                break;
            case E_SpecialAttack.SP_Yellow:
                image.sprite = _SetupAttackType[(int)E_SpecialAttack.SP_Yellow];
                SetAttackNumber(E_SpecialAttack.SP_Yellow);
                _DescriptionText.text = "イエローウイルスをランダムで10個発生させる";
                break;
            case E_SpecialAttack.SP_Green:
                image.sprite = _SetupAttackType[(int)E_SpecialAttack.SP_Green];
                SetAttackNumber(E_SpecialAttack.SP_Green);
                _DescriptionText.text = "グリーンウイルスをランダムで10個発生させる";
                break;
            case E_SpecialAttack.SP_White:
                image.sprite = _SetupAttackType[(int)E_SpecialAttack.SP_White];
                SetAttackNumber(E_SpecialAttack.SP_White);
                _DescriptionText.text = "ハート型ウイルスをランダムで10個発生させる";
                break;
            case E_SpecialAttack.SP_HorizontalOneArray:
                image.sprite = _SetupAttackType[(int)E_SpecialAttack.SP_HorizontalOneArray];
                SetAttackNumber(E_SpecialAttack.SP_HorizontalOneArray);
                _DescriptionText.text = "真ん中垂直一列のウイルスを破壊";
                break;
            case E_SpecialAttack.SP_VerticalOneArray:
                image.sprite = _SetupAttackType[(int)E_SpecialAttack.SP_VerticalOneArray];
                SetAttackNumber(E_SpecialAttack.SP_VerticalOneArray);
                _DescriptionText.text = "縦一列のウイルスを破壊";
                break;
            case E_SpecialAttack.SP_Destroy_Cross:
                image.sprite = _SetupAttackType[(int)E_SpecialAttack.SP_Destroy_Cross];
                SetAttackNumber(E_SpecialAttack.SP_Destroy_Cross);
                _DescriptionText.text = "十字にウイルスを破壊";
                break;
            case E_SpecialAttack.SP_ObliqueCross_Cross:
                image.sprite = _SetupAttackType[(int)E_SpecialAttack.SP_ObliqueCross_Cross];
                SetAttackNumber(E_SpecialAttack.SP_ObliqueCross_Cross);
                _DescriptionText.text = "斜め十字方向にウイルスを破壊";
                break;
            case E_SpecialAttack.SP_BlackPiece_Destroy:
                image.sprite = _SetupAttackType[(int)E_SpecialAttack.SP_BlackPiece_Destroy];
                SetAttackNumber(E_SpecialAttack.SP_BlackPiece_Destroy);
                _DescriptionText.text = "アンチウイルスをすべて破壊";
                break;
        }

    }

    /// <summary>
    /// 必殺技を登録する
    /// </summary>
    /// <param name="attacktype"></param>
    private void SetAttackNumber(E_SpecialAttack attacktype) 
    {

        if (SelectNum == 1)
        {
            PlayerPrefs.SetInt(SaveData_Manager.KEY_ATTACK_1, (int)attacktype);
            var num = PlayerPrefs.GetInt(SaveData_Manager.KEY_ATTACK_1, 0);
            Debug.Log(num + "を設定");
        }

        if (SelectNum == 2)
        {
            PlayerPrefs.SetInt(SaveData_Manager.KEY_ATTACK_2, (int)attacktype);
            var num = PlayerPrefs.GetInt(SaveData_Manager.KEY_ATTACK_2, 0);
            Debug.Log(num + "を設定");
        }

    }



    /// <summary>
    /// ゲームスタートボタンの開始
    /// </summary>
    public void GameStart()
    {
        // SceneManager.LoadScene("3_Reversi_GameScene");
        SetUpPanel.SetActive(true);
    }

    /// <summary>
    /// ☓ボタンを押すとポップアップが非表示になる・今後DoTweenでアニメーションを入れる予定
    /// </summary>
    public void Cancel()
    {
       // Popup_Close(_GamePlay_PopupPanel);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public void TapPlayButton()
    {
       
       // PopupStart(_GamePlay_PopupPanel);
        //_GamePlay_PopupPanel.SetActive(true);
    }



}
