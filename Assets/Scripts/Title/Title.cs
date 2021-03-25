using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Common_Sound_Manager.Instance.Sound_Play(BGM.Title);


    }


    public void StopTitleBGM() {
      
        Common_Sound_Manager.Instance.Sound_Play(BGM.Mypage);
        Common_Sound_Manager.Instance.SE_Play(SE.Start);
    }


    


}
