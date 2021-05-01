using UnityEngine;
using UnityEngine.UI;

  public enum iconSprite    //メッセージアイコン
  {
      Zibun,
    　Masato,
      Rena,
      Kana,
      Takada,
      Keisatu,
      UnKnown,
  }

  public enum MesseageListNum //メッセージリスト
  {
       List_1,
       List_2,
       List_3,
       List_4,
       List_5,
       List_6,
       List_7,
       List_8,
       List_9,
  }

/// <summary>
/// メッセージリストを表示するクラス。クリアしたステージに応じて、表示するステージ数・既読マーク有無を変更する
/// </summary>
public class MesseageList : MonoBehaviour
{
    [SerializeField]
    private GameObject _MesseageList;
    [SerializeField]
    private RectTransform _MesseagerectTransform;   
    private GameObject[] _MesseageObject;     
    
    //メッセンジャーアイコン
    [SerializeField]
    private Sprite[] _iconSprite;
    private Image[] _iconImage;
    private Text[] _iconName;

    [SerializeField]
    private Image ExclamationMark;

    //登録ナンバーを格納する変数
    private int StageListNo = 0;
    private int ClearNum;


    private void Start()
    {
        //クリアナンバーの更新
        ClearNum = PlayerPrefs.GetInt(SaveData_Manager.KEY_CLEAR_NUM, 0);

        //リスト表示（クリアナンバーの+1）
        StageListNo = ClearNum+1;
        _MesseageObject = new GameObject[StageListNo];
        Debug.Log(StageListNo);
        
        //クリアナンバーの+1の数だけメッセージリストを表示する
        NewMessageListAdditional();

    }

    /// <summary>
    /// ClearNumに応じて、クリアした分のメッセージリスト＋1を生成する
    /// </summary>
    public void NewMessageListAdditional() 
    {
        _iconImage = new Image[StageListNo];
        _iconName = new Text[StageListNo];

        for (var i = 0; i < StageListNo; i++) 
        {
            //ステージの数が全部で9個のためオブジェクトを生成しないように制限を設ける
            if (i < 9) 
            {
                _MesseageObject[i] = Instantiate(_MesseageList);
                _MesseageObject[i].transform.SetParent(_MesseagerectTransform);

                MesseageListSetting(_MesseageObject[i], i);
                Debug.Log("num" + _MesseageObject[i].GetComponent<ListObject>().GetStageNum);
                
                if (i == StageListNo - 1) { _MesseageObject[i].GetComponent<ListObject>().GetisRead = true; }

                 var obj = _MesseageObject[i].transform.GetChild(0);
                 var objtext = _MesseageObject[i].transform.GetChild(1);

                _iconImage[i] = obj.GetComponent<Image>();
                _iconName[i] = objtext.GetComponent<Text>();

                if (i == (int)MesseageListNum.List_1) 
                {
                    _iconImage[i].sprite = _iconSprite[(int)iconSprite.UnKnown];
                    _iconName[i].text = "Unknown";

                }

                if(i == (int)MesseageListNum.List_2)
                {

                    _iconImage[i].sprite = _iconSprite[(int)iconSprite.Masato];
                    _iconName[i].text = "マサト";
                }

                if (i == (int)MesseageListNum.List_3)　 
                {
                    _iconImage[i].sprite = _iconSprite[(int)iconSprite.Rena];
                    _iconName[i].text = "レナ STAGE1";
                }

                if (i == (int)MesseageListNum.List_4) 
                {
                    _iconImage[i].sprite = _iconSprite[(int)iconSprite.Kana];
                    _iconName[i].text = "かな　STAGE2";

                }

                if (i == (int)MesseageListNum.List_5) 
                {

                    _iconImage[i].sprite = _iconSprite[(int)iconSprite.Takada];
                    _iconName[i].text = "高田　STAGE3";

                }

                if (i == (int)MesseageListNum.List_6) 
                {
                    _iconImage[i].sprite = _iconSprite[(int)iconSprite.UnKnown];
                    _iconName[i].text = "匿名希望1 STAGE4";
                }

                if (i == (int)MesseageListNum.List_7)
                {
                    _iconImage[i].sprite = _iconSprite[(int)iconSprite.Rena];
                    _iconName[i].text = "レナ　STAGE5";
                }

                if (i == (int)MesseageListNum.List_8)
                {

                    _iconImage[i].sprite = _iconSprite[(int)iconSprite.Keisatu];
                    _iconName[i].text = "三好区警察署　STAGE6";

                }

                if (i == (int)MesseageListNum.List_9)
                {
                    _iconImage[i].sprite = _iconSprite[(int)iconSprite.Masato];
                    _iconName[i].text = "マサト　STAGE7";

                }
            }
                             
        }
    }

    /// <summary>
    /// メッセージリストの既読チェックとListObjectクラスにステージ番号の登録を行う
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="objectnum"></param>
    private void MesseageListSetting(GameObject obj, int objectnum) 
    {
        var listobject = obj.GetComponent<ListObject>();
        listobject.GetStageNum = objectnum;
    }

    /// <summary>
    /// リストをタップしたときに音を再生させる
    /// </summary>
    public void ListTap() 
    {
        Common_Sound_Manager.Instance.SE_Play(SE.Messeage);    
    }

}
