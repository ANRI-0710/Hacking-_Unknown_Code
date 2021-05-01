using UnityEngine;
/// <summary>
/// マイページのチュートリアル表示、データ初期化を担当するクラス
/// </summary>
public class MypageManager : Popup
{
    [SerializeField]
    private GameObject _TutorialPopup;
    [SerializeField]
    private Popup_Tutorial _Popup_Tutorial;
    
    private int is_Mypage_Tutorial;

    private void Start()
    {
        //既読フラグがついていなければマイページを再生する
        is_Mypage_Tutorial = PlayerPrefs.GetInt("KEY_ISREAD_MYPAGE_TUTORUAL", 0);
        if (is_Mypage_Tutorial != (int)ISREAD.READ) 
        {           
            _TutorialPopup.gameObject.SetActive(true);
            base.PopupStart(_TutorialPopup);
        }
    }

    private void Update()
    {
        if (_Popup_Tutorial.GetIsTutrialFinish) 
        {
            is_Mypage_Tutorial = (int)ISREAD.READ;
            PlayerPrefs.SetInt(SaveData_Manager.KEY_ISREAD_MYPAGE_TUTORIAL, is_Mypage_Tutorial);
            _Popup_Tutorial.GetIsTutrialFinish = false;

        }
    }

   /// <summary>
   /// データ削除
   /// </summary>
    public void SetData()
    {
        PlayerPrefs.DeleteAll();
    }
}
