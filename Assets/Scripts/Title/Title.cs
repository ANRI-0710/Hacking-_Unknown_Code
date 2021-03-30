using UnityEngine;

/// <summary>
/// タイトル画面を管理するクラス
/// </summary>
public class Title : MonoBehaviour
{  
    void Start()
    {
        Common_Sound_Manager.Instance.Sound_Play(BGM.Title);
    }

    public void StopTitleBGM() {
      
        Common_Sound_Manager.Instance.Sound_Play(BGM.Mypage);
        Common_Sound_Manager.Instance.SE_Play(SE.Start);
    }

}
