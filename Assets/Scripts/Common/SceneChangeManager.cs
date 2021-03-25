using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class SceneChangeManager : MonoBehaviour
{
    [SerializeField]
    private string SceneName;

    //フェードのパネル
    [SerializeField]
    private GameObject _PanelObject;

    private Image _Panel;

    private float fadespeed = 0.1f;

    //パネルの色、不透明度
    private float red;
    private float green;
    private float blue;
    private float alfa;

    //フェードイン・アウトのフラグ管理
    private bool isFadeOut = false;
    public bool GetisFadeOut { get => isFadeOut; set { isFadeOut = value; } }

    private bool isFadeIn = false;
    public bool GetisFadeIn { get => isFadeIn; set { isFadeIn = value; } }

    private void Start()
    {
        _Panel = _PanelObject.GetComponent<Image>();
        
        red = _Panel.color.r;
        green = _Panel.color.g;
        blue = _Panel.color.b;
        alfa = _Panel.color.a;

        Debug.Log("startalpa" + alfa);

        StartCoroutine(FadeIn());
        //FadeOut();
    }

    public void StartFadeIn() 
    {
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut() {
       
        StartCoroutine(FadeOut());
    }

    public void Retry_Button() 
    {
        StartCoroutine(FadeOut());

    }

    public IEnumerator FadeIn() 
    {
        while (true) 
        {
            alfa -= fadespeed;
            _Panel.color = new Color(red, green, blue, alfa);
            yield return new WaitForSeconds(0.05f);
            //yield return null;
            Debug.Log("alfa" + alfa);

            if (alfa <= 0)
            {
                _Panel.enabled = false;
                //yield return new WaitForSeconds(3f);
               // SceneChange("ListMessengerScene");
                break;
            }

        }

    }


    public IEnumerator FadeOut()
    {
        _Panel.enabled = true;
        while (true)
        {
            alfa += fadespeed;
            _Panel.color = new Color(red, green, blue, alfa);
            yield return new WaitForSeconds(0.05f);            
            Debug.Log("alfa" + alfa);

            if (alfa >= 1)
            {              
                //yield return new WaitForSeconds(3f);
                SceneChange();
                break;
            }

        }

    }

    public IEnumerator Rerty()
    {
        _Panel.enabled = true;
        while (true)
        {
            alfa += fadespeed;
            _Panel.color = new Color(red, green, blue, alfa);
            yield return new WaitForSeconds(0.05f);
            Debug.Log("alfa" + alfa);

            if (alfa >= 1)
            {
                //yield return new WaitForSeconds(3f);
                SceneRetry();
                break;
            }
        }

    }

    public void SceneChange() 
    {
        SceneManager.LoadScene(SceneName);
    }

    public void SceneRetry() 
    {
        SceneManager.LoadScene("PazuruGameScene");
    }

    public void ChangeMypageBGM() 
    {
        Common_Sound_Manager.Instance.Sound_Play(BGM.Mypage);    
    }

    public void ButtonTap() 
    {

        Common_Sound_Manager.Instance.SE_Play(SE.Popup_Tap);
    }

    public void MessengerPop()
    {

        Common_Sound_Manager.Instance.SE_Play(SE.Messeage);
    }

}
