using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// メッセージクラス
/// </summary>
public struct MessageBoxListStruct
{
    private int TalkingPerson;   
    private int MesseageIconImgNum;
    private int MesseageImgNum;
    public string _messegae;

    public int GetTalkingPerson { get => TalkingPerson; set { TalkingPerson = value; } }
    public int GetMesseageIconImgNum { get => MesseageIconImgNum; set { MesseageIconImgNum = value; } }   
    public int GetMesseageImgNum { get => MesseageImgNum; set { MesseageImgNum = value; } }
    public string Getmesseage { get => _messegae; set { _messegae = value; } }
}

public class MessagePage : MonoBehaviour
{
    //メッセージプレハブ
    [SerializeField]
    private GameObject _MessengerPrefab;
    [SerializeField]
    private RectTransform Contentrecttransform;

    [SerializeField]
    private GameObject _button;
    private MessageBoxListStruct[] _MesseageList;

    private int listcount = 1;
    private int arraycount;

    //吹き出しのイメージ（相手か主人公か）
    [SerializeField]
    private Sprite[] _hukidasiImage;

    //話している相手の座標
    private Vector2[] iconPos =
    {  new Vector3(-130,-60,0),  //相手
      new Vector3(120,-60,0)　//主人公
    };

    //アイコンのイメージ画像
    [SerializeField]
    private Sprite[] _iconImage;

    //テキストファイル読み込み
    [SerializeField]
    private InputFile inputFile;

    //ステージ情報の記録
    private int stageNum;

    //既読フラグのチェック
    private int isReadCheckNum;


    private void Start()
    {
        stageNum = PlayerPrefs.GetInt(SaveData_Manager.KEY_STAGE_NUM, 0);
        isReadCheckNum = PlayerPrefs.GetInt(SaveData_Manager.KEY_ISREAD_NUM, 0);

        //SaveManagerから所得
        var str = "Messeage" + stageNum;
        Debug.Log("Messeage" + stageNum);

        //ファイル読み込み
        inputFile.Input_File(str);
        _MesseageList = new MessageBoxListStruct[inputFile.GetInputRows];
        arraycount = inputFile.GetInputRows;

        for (var i = 0; i < inputFile.GetInputRows; i++)
        {
            for (var k = 0; k < inputFile.GetInputCols; k++)
            {
                if (i != 0)
                {
                    if (k == 0)
                    {
                        if (inputFile.GetStrings[i, k] == "相手") { _MesseageList[i].GetTalkingPerson = 0; }
                        else if (inputFile.GetStrings[i, k] == "自分") { _MesseageList[i].GetTalkingPerson = 1; }
                    }
                    if (k == 1)
                    {
                        if (inputFile.GetStrings[i, k] == "自分") { _MesseageList[i].GetMesseageIconImgNum = 0; }
                        else if (inputFile.GetStrings[i, k] == "マサシ") { _MesseageList[i].GetMesseageIconImgNum = 1; }
                        else if (inputFile.GetStrings[i, k] == "レナ") { _MesseageList[i].GetMesseageIconImgNum = 2; }
                        else if (inputFile.GetStrings[i, k] == "かな") { _MesseageList[i].GetMesseageIconImgNum = 3; }
                        else if (inputFile.GetStrings[i, k] == "高田") { _MesseageList[i].GetMesseageIconImgNum = 4; }
                        else if (inputFile.GetStrings[i, k] == "警察") { _MesseageList[i].GetMesseageIconImgNum = 5; }
                        else if (inputFile.GetStrings[i, k] == "アンノウン") { _MesseageList[i].GetMesseageIconImgNum = 6; }
                      

                    }
                    if (k == 2)
                    {
                        _MesseageList[i].GetMesseageImgNum = int.Parse(inputFile.GetStrings[i, k]);
                    }
                    if (k == 3)
                    {
                        _MesseageList[i].Getmesseage = inputFile.GetStrings[i, k];
                    }
                }
            }
        }

        //既読チェック
        //if(isReadCheckNum <= stageNum) ReadMessegeFin();
        Debug.Log("isReadCheckNum"+isReadCheckNum);

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ListTap();
            NewMessenge();
            //ReadMessegeFin();
        }
    }

    /// <summary>
    /// タップでメッセージ表示
    /// </summary>
    public void NewMessenge()
    {
        if (listcount < arraycount)
        {
            var obj = Instantiate(_MessengerPrefab);
            obj.transform.SetParent(Contentrecttransform);

            //吹き出しの番号
            obj.GetComponent<Image>().sprite = _hukidasiImage[_MesseageList[listcount].GetTalkingPerson];

            //テキスト
            var textobj = obj.transform.GetChild(1).gameObject;
            textobj.GetComponent<Text>().text = _MesseageList[listcount]._messegae;

            //アイコンイメージ
            var imageobj = obj.transform.GetChild(0).gameObject;

            imageobj.GetComponent<Image>().sprite = _iconImage[_MesseageList[listcount].GetMesseageIconImgNum];

            if (_MesseageList[listcount].GetTalkingPerson == 1)
            { imageobj.GetComponent<RectTransform>().localPosition = new Vector3(120, -60, 0); }
            else { imageobj.GetComponent<RectTransform>().localPosition = new Vector3(-130, -60, 0); }
            listcount++;
        }
        else
        {
            if (stageNum != 0 && stageNum != 1)
            {
                _button.SetActive(true);
            }

            if (stageNum == 0 || stageNum == 1)
            {
                PlayerPrefs.SetInt(SaveData_Manager.KEY_ISREAD_NUM, 2);
                PlayerPrefs.SetInt(SaveData_Manager.KEY_CLEAR_NUM, 2);
            }     

        }
    }

    /// <summary>
    /// 既読フラグがついている場合は表示にする
    /// </summary>
    public void ReadMessegeFin()
    {
        for (var i = 1; i < arraycount; i++)
        {
            var obj = Instantiate(_MessengerPrefab);
            obj.transform.SetParent(Contentrecttransform);

            //吹き出しの番号
            obj.GetComponent<Image>().sprite = _hukidasiImage[_MesseageList[i].GetTalkingPerson];

            //テキスト
            var textobj = obj.transform.GetChild(1).gameObject;
            textobj.GetComponent<Text>().text = _MesseageList[i]._messegae;

            //アイコンイメージ
            var imageobj = obj.transform.GetChild(0).gameObject;
            imageobj.GetComponent<Image>().sprite = _iconImage[_MesseageList[i].GetMesseageIconImgNum];

            if (_MesseageList[listcount].GetTalkingPerson == 1)
            { imageobj.GetComponent<RectTransform>().localPosition = new Vector3(120, -60, 0); }
            else { imageobj.GetComponent<RectTransform>().localPosition = new Vector3(-130, -60, 0); }

        }

        _button.SetActive(true);

    }

    public void NextPazuruScene()
    {
        if (stageNum == 0 || stageNum == 1) 
        {

            if (isReadCheckNum != 0)
            {
                SceneManager.LoadScene("ListMessengerScene");
            }
            else {

                PlayerPrefs.SetInt(SaveData_Manager.KEY_ISREAD_NUM, 2);
                PlayerPrefs.SetInt(SaveData_Manager.KEY_CLEAR_NUM, 2);
                SceneManager.LoadScene("ListMessengerScene");

            }

        }
        else 
        {                         
           SceneManager.LoadScene("PazuruGameScene");
        }       
    }

    public void ListTap()
    {
        Common_Sound_Manager.Instance.SE_Play(SE.Messeage);
    }




}
