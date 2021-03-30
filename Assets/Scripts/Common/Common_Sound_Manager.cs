using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サウンド管理クラス
/// </summary>
/// 
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
    
    [SerializeField]
    private AudioClip BGM_Title_Page;
    [SerializeField]
    private AudioClip BGM_Mypage;
    [SerializeField]
    private AudioClip PazuruGameSccene;

    //ポップアップ
    [SerializeField]
    private AudioClip SE_Popup_Tap;
    [SerializeField]
    private AudioClip SE_Cancel_Tap;

    [SerializeField]
    private AudioClip Start_Tap;

    //メッセンジャー
    [SerializeField]
    private AudioClip Messeage;

    [SerializeField]
    private AudioSource _audioSource;


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

        Instance._audioSource = this.GetComponent<AudioSource>();

    }


    public void Sound_Play(BGM bgm) 
    {

        switch (bgm) 
        {

            case BGM.Title:
                Instance._audioSource.clip = BGM_Title_Page;
                Instance._audioSource.Play();
                break;

            case BGM.Mypage:
                Instance._audioSource.clip = BGM_Mypage;
                Instance._audioSource.Play();
                break;

            case BGM.Pazuru:
                break;

            case BGM.Stop:              
                Instance._audioSource.Stop();
                break;
        
        }
    
    }

    public void SE_Play(SE se)
    {
        switch (se) 
        {
            case SE.Start:
                Instance._audioSource.PlayOneShot(Start_Tap);
                break;

            case SE.Popup_Tap:
                Instance._audioSource.PlayOneShot(SE_Popup_Tap);
                break;
            case SE.Popup_Close:
                Instance._audioSource.PlayOneShot(SE_Cancel_Tap);
                break;
            case SE.Messeage:
                Instance._audioSource.PlayOneShot(Messeage);
                break;
        }
    
    }





   
}
