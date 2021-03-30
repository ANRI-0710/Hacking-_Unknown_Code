using UnityEngine;
using DG.Tweening;

/// <summary>
/// ポップアップのオープン・クローズのアニメーション
/// </summary>
public class Popup : MonoBehaviour
{
    public virtual void PopupStart(GameObject popupcanvas)
    {
        Common_Sound_Manager.Instance.SE_Play(SE.Popup_Tap);
        popupcanvas.transform.localScale = new Vector3(0f, 0f, 0f);
        popupcanvas.transform.DOScale(1f, 0.2f);
    }

    public virtual void Popup_Close(GameObject popupcanvas)
    {
        Common_Sound_Manager.Instance.SE_Play(SE.Popup_Close);
        Sequence seq = DOTween.Sequence();
        seq.Append(popupcanvas.transform.DOScale(0f, 0.2f));
        seq.OnComplete(() => FalseWindow(popupcanvas));
        seq.Play();
    }

    public virtual void FalseWindow(GameObject popupcanvas)
    {
        popupcanvas.SetActive(false);
    }

}
