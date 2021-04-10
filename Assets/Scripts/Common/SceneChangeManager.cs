using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// シーン遷移時及びフェードイン・アウト（暗転処理）を管理するクラス
/// </summary>
public class SceneChangeManager : MonoBehaviour
{
    //インスペクタ上から遷移したいシーン名を記入する
    [SerializeField]
    private string _SceneName;

    //フェードのパネル
    [SerializeField]
    private GameObject _PanelObject;    
    
    //操作用のパネル
    private Image _panel;   
    
    //暗転処理するスピード
    private float _fadeSpeed = 0.1f;
    
    //パネルの色、不透明度
    private float _red;
    private float _green;
    private float _blue;
    private float _alfa;    
    
    //フェードイン・アウトのフラグ管理
    private bool _isFadeOut = false;
    public bool GetisFadeOut { get => _isFadeOut; set { _isFadeOut = value; } }
    private bool _isFadeIn = false;
    public bool GetisFadeIn { get => _isFadeIn; set { _isFadeIn = value; } }

    private void Start()
    {
        //パネルへアクセスし、RGBそれぞれを変数に代入
        _panel = _PanelObject.GetComponent<Image>();               
        _red = _panel.color.r;
        _green = _panel.color.g;
        _blue = _panel.color.b;
        _alfa = _panel.color.a;       
        
        //フェードイン開始
        StartCoroutine(FadeIn());       
    }

    /// <summary>
    /// フェードイン開始のコルーチン（エディタ上のOnclickに登録するため）
    /// </summary>
    public void StartFadeIn() { StartCoroutine(FadeIn()); }

    /// <summary>
    /// フェードイン終了のコルーチン（エディタ上のOnclickに登録するため）
    /// </summary>
    public void StartFadeOut() { StartCoroutine(FadeOut()); }

    /// <summary>
    /// リトライボタンタップ時のコルーチンスタート（エディタ上のOnclickに登録するため）
    /// </summary>
    public void Retry_Button() { StartCoroutine(Rerty()); }
    
    /// <summary>
    /// フェードイン
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeIn() 
    {
        while (true) 
        {
            //黒パネルのアルファ値を0にする
            _alfa -= _fadeSpeed;
            _panel.color = new Color(_red, _green, _blue, _alfa);
            yield return new WaitForSeconds(0.05f);

            if (_alfa <= 0)
            {
                _panel.enabled = false;         
                break;
            }
        }
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOut()
    {
        _panel.enabled = true;
        while (true)
        {
            //黒パネルのアルファ値をMAXにする
            _alfa += _fadeSpeed;
            _panel.color = new Color(_red, _green, _blue, _alfa);
            yield return new WaitForSeconds(0.05f);            
            Debug.Log("alfa" + _alfa);

            if (_alfa >= 1)
            {              
                SceneChange();
                break;
            }
        }
    }

    /// <summary>
    /// リトライ処理（デリゲードでフェードアウト処理にまとめられると思うので今後修正予定）
    /// </summary>
    /// <returns></returns>
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
                SceneRetry();
                break;
            }
        }

    }

    /// <summary>
    /// 遷移するシーンへ移動する
    /// </summary>
    public void SceneChange() { SceneManager.LoadScene(_SceneName); }

    /// <summary>
    /// パズルシーンへリトライする
    /// </summary>
    public void SceneRetry() { SceneManager.LoadScene("PazuruGameScene"); }

    /// <summary>
    /// マイページに遷移する際、BGMの変更処理を同時に行う（戻るボタンと一緒に登録する）
    /// </summary>
    public void ChangeMypageBGM() { Common_Sound_Manager.Instance.Sound_Play(BGM.Mypage); }

    /// <summary>
    /// シーン管理のポップアップボタンをタップする際にSEを再生する
    /// </summary>
    public void ButtonTap() { Common_Sound_Manager.Instance.SE_Play(SE.Popup_Tap); }

}
