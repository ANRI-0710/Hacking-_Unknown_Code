using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// シーン遷移時のフェードイン・アウトを管理するクラス
/// </summary>
public class SceneChangeManager : MonoBehaviour
{
    //インスペクタ上から遷移したいシーン名を記入
    [SerializeField]
    private string _SceneName;

    //フェードのパネル
    [SerializeField]
    private GameObject _PanelObject;    
    //操作用のパネル
    private Image _panel;
    
    //フェードインするスピード
    private float _fadeSpeed = 0.1f;
    //パネルの色、不透明度
    private float _red;
    private float _green;
    private float _blue;
    private float _alfa;    
    
    //フェードイン・アウトのフラグ管理
    private bool isFadeOut = false;
    public bool GetisFadeOut { get => isFadeOut; set { isFadeOut = value; } }
    private bool isFadeIn = false;
    public bool GetisFadeIn { get => isFadeIn; set { isFadeIn = value; } }

    private void Start()
    {
        _panel = _PanelObject.GetComponent<Image>();       
        _red = _panel.color.r;
        _green = _panel.color.g;
        _blue = _panel.color.b;
        _alfa = _panel.color.a;       
        
        StartCoroutine(FadeIn());       
    }

    //フェードイン開始のコルーチン（インスペクタボタンから呼び出すため）
    public void StartFadeIn() 
    {
        StartCoroutine(FadeIn());
    }

    //フェードイン終了のコルーチン（インスペクタボタンから呼び出すため）
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
            _alfa -= _fadeSpeed;
            _panel.color = new Color(_red, _green, _blue, _alfa);
            yield return new WaitForSeconds(0.05f);
            Debug.Log("alfa" + _alfa);

            if (_alfa <= 0)
            {
                _panel.enabled = false;         
                break;
            }
        }
    }

    public IEnumerator FadeOut()
    {
        _panel.enabled = true;
        while (true)
        {
            _alfa += _fadeSpeed;
            _panel.color = new Color(_red, _green, _blue, _alfa);
            yield return new WaitForSeconds(0.05f);            
            Debug.Log("alfa" + _alfa);

            if (_alfa >= 1)
            {              
                //yield return new WaitForSeconds(3f);
                SceneChange();
                break;
            }
        }
    }

    public IEnumerator Rerty()
    {
        _panel.enabled = true;
        while (true)
        {
            _alfa += _fadeSpeed;
            _panel.color = new Color(_red, _green, _blue, _alfa);
            yield return new WaitForSeconds(0.05f);
            Debug.Log("alfa" + _alfa);

            if (_alfa >= 1)
            {
                //yield return new WaitForSeconds(3f);
                SceneRetry();
                break;
            }
        }

    }

    public void SceneChange() 
    {
        SceneManager.LoadScene(_SceneName);
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
