using UnityEngine;

    public enum BGM //１つの関数から呼び出しできるようにenum変数を使用
    {
        Title,
        Mypage,
        Pazuru,
        Stop
    }

    public enum SE //１つの関数から呼び出しできるようにenum変数を使用
    { 
        Popup_Tap,
        Popup_Close,
        Start,
        Cancel,
        Messeage,
    }
/// <summary>
/// パズルパート以外のSE・BGM管理クラス
/// </summary>
/// 
public class Common_Sound_Manager : Singleton<Common_Sound_Manager>
{   
    //BGM
    [SerializeField]
    private AudioClip _BGM_Title_Page;
    [SerializeField]
    private AudioClip _BGM_Mypage;
    [SerializeField]
    private AudioClip _BGM_PazuruGameSccene;

    //タップ時のSE
    [SerializeField]
    private AudioClip _SE_Popup_Tap;
    [SerializeField]
    private AudioClip _SE_Cancel_Tap;
    [SerializeField]
    private AudioClip _SE_Start_Tap;

    //メッセンジャー風アプリの効果音
    [SerializeField]
    private AudioClip _SE_Messeage;
    
    //オーディオソース
    [SerializeField]
    private AudioSource _AudioSource;

    /// <summary>
    /// シングルトンの起動
    /// </summary>
    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        Instance._AudioSource = this.GetComponent<AudioSource>();
    }

    /// <summary>
    /// 共通BGMの呼び出し
    /// </summary>
    /// <param name="bgm"></param>
    public void Sound_Play(BGM bgm) 
    {
        switch (bgm) 
        {
            case BGM.Title:
                Instance._AudioSource.clip = _BGM_Title_Page;
                Instance._AudioSource.Play();
                break;
            case BGM.Mypage:
                Instance._AudioSource.clip = _BGM_Mypage;
                Instance._AudioSource.Play();
                break;
            case BGM.Stop:              
                Instance._AudioSource.Stop();
                break;      
        }    
    }

    /// <summary>
    /// 共通SEの呼び出し
    /// </summary>
    /// <param name="se"></param>
    public void SE_Play(SE se)
    {
        switch (se) 
        {
            case SE.Start:
                Instance._AudioSource.PlayOneShot(_SE_Start_Tap);
                break;
            case SE.Popup_Tap:
                Instance._AudioSource.PlayOneShot(_SE_Popup_Tap);
                break;
            case SE.Popup_Close:
                Instance._AudioSource.PlayOneShot(_SE_Cancel_Tap);
                break;
            case SE.Messeage:
                Instance._AudioSource.PlayOneShot(_SE_Messeage);
                break;
        }    
    }
}
