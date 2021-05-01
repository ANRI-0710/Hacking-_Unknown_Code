using UnityEngine;
using DG.Tweening;

/// <summary>
/// ポップアップのオープン・クローズを管理するクラス
/// </summary>
public class Popup : MonoBehaviour
{
    //ポップアップアニメーションの速度管理
    private float _durationDoTweenTime = 0.2f;
    private float _openValue = 1.0f;
    private float _cloaseValue = 0f;

    /// <summary>
    /// ポップアップの表示開始アニメーション
    /// </summary>
    /// <param name="popupcanvas"></param>
    /// 
    public void PopupStart(GameObject popupcanvas)
    {
        Common_Sound_Manager.Instance.SE_Play(SE.Popup_Tap);
        popupcanvas.transform.localScale = new Vector3(0,0,0);
        popupcanvas.transform.DOScale(_openValue, _durationDoTweenTime);
    }
  
    /// <summary>
    /// ポップアップ表示終了アニメーション
    /// </summary>
    /// <param name="popupcanvas"></param>
    public void Popup_Close(GameObject popupcanvas)
    {
        Common_Sound_Manager.Instance.SE_Play(SE.Popup_Close);
        Sequence seq = DOTween.Sequence();
        seq.Append(popupcanvas.transform.DOScale(_cloaseValue, _durationDoTweenTime));
        seq.OnComplete(() => FalseWindow(popupcanvas));
        seq.Play();
    }

    /// <summary>
    /// 終了アニメーション終了後、ポップアップを閉じる
    /// </summary>
    /// <param name="popupcanvas"></param>
    public void FalseWindow(GameObject popupcanvas)
    {
        popupcanvas.SetActive(false);
    }
   
}
