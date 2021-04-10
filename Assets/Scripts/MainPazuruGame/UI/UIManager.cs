using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// UIの位置調整・クリア・ゲームオーバーなどのUI表示・演出を管理するクラス
/// </summary>
public class UIManager : Popup
{    
    //UIオブジェクト
    [SerializeField]
    private GameObject _Crear_Popup;
    [SerializeField]
    private GameObject _GameOver_Popup;
    [SerializeField]
    SceneChangeManager sceneChangeManager;
    [SerializeField]
    private TextDisplay _ComboTextDisplay;
    [SerializeField]
    private TextDisplay _RecoveryText;
    [SerializeField]
    private GameObject Stop_Popup;
    [SerializeField]
    private Button _UI_Setting_Button;
    [SerializeField]
    private GameObject _TypeImage;
    [SerializeField]
    private GameObject _ClearTimeText;

    //タイムカウント
    [SerializeField]
    BestTimeCount bestTimeCount;
  
    //スタート・クリア・第一層~3層までの合図アニメーション
    [SerializeField]
    private Signal _Signal;
    [SerializeField]
    private GameObject _WarningPopup;

    //コンボテキスト表示
    private TextDisplay _comboTextObject;
    private TextDisplay _recoveryTextObject;


    /// <summary>
    /// 警告ポップアップを出す
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="particletransform"></param>
    public void WamingArrart(RectTransform rectTransform,RectTransform particletransform)
    {
        var obj = Instantiate(_WarningPopup);
        obj.transform.position = new Vector3(0, 0, 0);
        obj.transform.SetParent(rectTransform);
        var arart = _WarningPopup.GetComponent<WarningPopup>();
        var pos1 = new Vector2(0, 0);
        var pos2 = new Vector2(Screen.width / 3, Screen.height / 2);
        var pos3 = new Vector2(Screen.width / 2, Screen.height / 3);
        var pos4 = new Vector2(Screen.width / 4, Screen.height / 5);
        var pos5 = new Vector2(Screen.width / 2, -Screen.height / 2);
        var pos6 = new Vector2(-Screen.width / 4, -Screen.height / 3);
        var pos7 = new Vector2(-Screen.width / 5, -Screen.height / 4f);
        arart.PopupStartArart(particletransform, pos1, pos2, pos3, pos4, pos5, pos6, pos7);
    }

    /// <summary>
    /// 警告ポップアップを閉じる
    /// </summary>
    public void ArrartClosePopup() 
    {
        var arart = _WarningPopup.GetComponent<WarningPopup>();
        arart.Popup_Close(arart.Popup);
    }

    /// <summary>
    /// クリア表示
    /// </summary>
    public void GameClear()
    {
        _Crear_Popup.SetActive(true);
        PopupStart(_Crear_Popup);
    }

    /// <summary>
    /// ゲームオーバー表示
    /// </summary>
    public void GameOver()
    {
        _GameOver_Popup.SetActive(true);
        PopupStart(_GameOver_Popup);
    }

    /// <summary>
    /// テキスト表示
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="combocount"></param>
    public void InitTextComboDisplay(RectTransform rectTransform,int combocount)
    {
        _comboTextObject = Instantiate(_ComboTextDisplay);
        _comboTextObject.transform.SetParent(rectTransform);
        _comboTextObject.transform.localPosition = new Vector3(100, 100);
        _comboTextObject.transform.localRotation = Quaternion.Euler(0, 0, 20);
        _comboTextObject.GetCount = combocount;
        _comboTextObject.GetTextDisplayString = "Combo!!";
        _comboTextObject.UI_Text_Display();
    }

    /// <summary>
    /// 5秒回復するカウント表示UIの初期化
    /// </summary>
    /// <param name="rectTransform"></param>
    public void InitTextRecoveryDisplay(RectTransform rectTransform)
    {
        _recoveryTextObject = Instantiate(_RecoveryText);
        _recoveryTextObject.transform.SetParent(rectTransform);
        _recoveryTextObject.GetTextDisplayString = "+5";
        _recoveryTextObject.transform.localPosition = new Vector3(105, 260, 0);
        _recoveryTextObject.transform.localScale = new Vector3(1, 1, 1);
        _recoveryTextObject.UI_Text_Display_Recovery();
    }

    /// <summary>
    /// コンボテキストの破壊処理
    /// </summary>
    public void DestroyTextComboDisplay() { Destroy(_comboTextObject.gameObject); }

    /// <summary>
    ///  5秒回復するカウント表示UIの初期化
    /// </summary>
    public void DestroyRecoveryDisplay() { Destroy(_recoveryTextObject.gameObject,0.5f); }

    /// <summary>
    /// 最速クリアタイムの保存
    /// </summary>
    public void IsbestTimeCount() { bestTimeCount.GetTimeStop = true; }

    /// <summary>
    /// ゲームスタート・クリア・何体目の敵かを表示するクラス
    /// </summary>
    /// <param name="str"></param>
    /// <param name="rectTransform"></param>
    public void  SignalPopup(string str, RectTransform rectTransform) 
    {
        var obj = _Signal.SignalObject(str, rectTransform);
        Destroy(obj,2.0f);
    }
    
    /// <summary>
    ///リトライボタン（ボタン登録用） 
    /// </summary>
    public void Retry()
    {
        SceneManager.LoadScene("MypageScene");
    }

}
