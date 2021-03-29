using UnityEngine;

/// <summary>
/// 共通で使用するサウンド管理クラス（パズルパート以外のSE・BGMの再生）
/// </summary>
/// 
    //１つの関数から呼び出しできるようにenum変数を使用
    public enum BGM 
    {
        Title,
        Mypage,
        Pazuru,
        Stop
    }

    public enum SE
    { 
        Popup_Tap,
        Popup_Close,
        Start,
        Cancel,
        Messeage,
    }

public class Common_Sound_Manager : Singleton<Common_Sound_Manager>
{   
    //BGM
    [SerializeField]
    private AudioClip _BGM_Title_Page;
    [SerializeField]
    private AudioClip _BGM_Mypage;
    [SerializeField]
    private AudioClip _BGM_PazuruGameSccene;

    //ポップアップ
    [SerializeField]
    private AudioClip _SE_Popup_Tap;
    [SerializeField]
    private AudioClip _SE_Cancel_Tap;
    [SerializeField]
    private AudioClip _SE_Start_Tap;

    //メッセンジャー
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
            case BGM.Pazuru:
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
