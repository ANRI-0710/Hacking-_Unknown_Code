using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// チュートリアル(マイページ/パズルパートそれぞれ2つ)のポップアップ文言・画像を管理するクラス
/// </summary>
public class Popup_Tutorial : Popup
{
    [SerializeField]
    private GameObject _TutorialPopup;
    [SerializeField]
    private Sprite[] _TutorialPopupImages;
    [SerializeField]
    private string[] _TutorialStrings;

    //画像と文言のコントロール用の変数
    private Image _tutrialImageControll;
    private Text _tutrialStringControll;
    
    //タップで次の画像・文言に変更するための管理変数
    private int _nextNum = 0;
    
    //既読チェック
    private bool _isTutrialFinish;
    public bool GetIsTutrialFinish { get => _isTutrialFinish; set { _isTutrialFinish  = value; }  }

    private void Start()
    {
        //イメージ画像の変更管理
        var tutrialobj = _TutorialPopup.transform.GetChild(0).gameObject;
        _tutrialImageControll = tutrialobj.GetComponent<Image>();
        _tutrialImageControll.sprite = _TutorialPopupImages[_nextNum];
        
        //説明文言管理変数
        var tutrialtext = _TutorialPopup.transform.GetChild(1).gameObject;
        _tutrialStringControll = tutrialtext.GetComponent<Text>();
        _tutrialStringControll.text = _TutorialStrings[_nextNum];
    }

    /// <summary>
    /// チュートリアルをスタートする（ポップアップを開始する）
    /// </summary>
    public void TutorialStart() 
    {
        _TutorialPopup.gameObject.SetActive(true);
        base.PopupStart(_TutorialPopup);
    }

    /// <summary>
    /// 次のポップアップを押した際にタップ音を鳴らす
    /// </summary>
    public void TapSound() { Common_Sound_Manager.Instance.SE_Play(SE.Popup_Tap); }
   
    /// <summary>
    /// 次のポップアップに進む
    /// </summary>
    public void NextPopupImage() 
    {
        //現在いるページ（NextNum）がポップアップ画像の枚数分達したらポップアップを閉じ、既読フラグを付ける
        if (_nextNum == _TutorialPopupImages.Length -1)
        {
            base.Popup_Close(_TutorialPopup.gameObject);
            GetIsTutrialFinish = true;
            Debug.Log("NextNum" + _nextNum);
        }
        else //達していない場合は、配列をプラス1して次のページに進む
        {
            if (_nextNum < _TutorialPopupImages.Length) { _nextNum++; }
            Debug.Log("NextNum" + _nextNum);
            _tutrialImageControll.sprite = _TutorialPopupImages[_nextNum];
            _tutrialStringControll.text = _TutorialStrings[_nextNum];           
        }        
    }

}
