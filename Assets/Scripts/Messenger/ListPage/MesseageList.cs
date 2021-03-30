using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MesseageList : MonoBehaviour
{
    [SerializeField]
    private GameObject _MesseageList;

    [SerializeField]
    private RectTransform _MesseagerectTransform;

    private GameObject[] _MesseageObject;     
    private string StageName = "Messeage";
    private int StageListNo = 0;
    private int ClearNum;

    [SerializeField]
    private Image ExclamationMark;

    [SerializeField]
    private Sprite[] _iconSprite;

    private GameObject _iCon;
    private Image[] _iconImage;
    private Text[] _iconName;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        //クリアナンバーの更新
        ClearNum = PlayerPrefs.GetInt(SaveData_Manager.KEY_CLEAR_NUM, 0);

        //リスト表示（クリアナンバーの+1）
        StageListNo = ClearNum+1;
        _MesseageObject = new GameObject[StageListNo];

        //
        Debug.Log(StageListNo);
        NewMessageListAdditional();

    }

    /// <summary>
    /// ClearNumに応じて、ゲームオブジェクトを生成する
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

                //
                MesseageListSetting(_MesseageObject[i], i);
                Debug.Log("num" + _MesseageObject[i].GetComponent<ListObject>().GetStageNum);
                if (i == StageListNo - 1) { _MesseageObject[i].GetComponent<ListObject>().GetisRead = true; }

                //PlayerPrefs.DeleteAll();
                 var obj = _MesseageObject[i].transform.GetChild(0);
                 var objtext = _MesseageObject[i].transform.GetChild(1);

                _iconImage[i] = obj.GetComponent<Image>();
                _iconName[i] = objtext.GetComponent<Text>();

                if (i == 0) 
                {
                    _iconImage[i].sprite = _iconSprite[6];
                    _iconName[i].text = "Unknown";

                }

                if(i == 1 ){

                    _iconImage[i].sprite = _iconSprite[1];
                    _iconName[i].text = "マサト";
                }

                if (i == 2)　 
                {
                    _iconImage[i].sprite = _iconSprite[2];
                    _iconName[i].text = "レナ STAGE1";
                }

                if (i == 3 ) 
                {
                    _iconImage[i].sprite = _iconSprite[3];
                    _iconName[i].text = "かな　STAGE2";

                }

                if (i == 4) 
                {

                    _iconImage[i].sprite = _iconSprite[4];
                    _iconName[i].text = "高田　STAGE3";

                }

                if (i == 5) 
                {
                    _iconImage[i].sprite = _iconSprite[6];
                    _iconName[i].text = "匿名希望1 STAGE4";
                }

                if (i == 6)
                {
                    _iconImage[i].sprite = _iconSprite[2];
                    _iconName[i].text = "レナ　STAGE5";
                }

                if (i == 7)
                {

                    _iconImage[i].sprite = _iconSprite[5];
                    _iconName[i].text = "三好区警察署　STAGE6";

                }

                if (i == 8)
                {
                    _iconImage[i].sprite = _iconSprite[1];
                    _iconName[i].text = "マサト　STAGE7";

                }

            }
                             
        }
    }
    public void NextScene() 
    {
        SceneManager.LoadScene("PazuruGameScene");    
    }
    private void MesseageListSetting(GameObject obj, int objectnum) 
    {
        var obj2 = obj.GetComponent<ListObject>();
        obj2.GetStageNum = objectnum;
      
    }

    public void ListTap() 
    {
        Common_Sound_Manager.Instance.SE_Play(SE.Messeage);    
    }


}
