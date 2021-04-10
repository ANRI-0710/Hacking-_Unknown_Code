using UnityEngine;
using DG.Tweening;

/// <summary>
/// パズルスタート時のWamingポップアップを出すクラス
/// </summary>
public class WarningPopup : Popup
{
    [SerializeField]
    private GameObject _WamingPopup1;
    public GameObject[] Popup;

  /// <summary>
  /// 受け取ったVector2の数分警告ポップアップを表示する
  /// </summary>
  /// <param name="transforms"></param>
  /// <param name="vectors"></param>
    public void PopupStartArart( RectTransform transforms, params Vector2[]  vectors)
    {
        Popup = new GameObject[vectors.Length];
        for (var i = 0; i < vectors.Length; i++) 
        {
            Popup[i] = Instantiate(_WamingPopup1);
            Popup[i].transform.position = new Vector3(Screen.width + vectors[i].x, Screen.height + vectors[i].y, 0);
            Popup[i].transform.DOScale(0.8f, 0.2f);          
            Popup[i].transform.SetParent(transforms);
        }      
    }

    
    public void Popup_Close(GameObject[] popupcanvas)
    {
        for (var i = 0; i < popupcanvas.Length; i++) 
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(popupcanvas[i].transform.DOScale(0f, 0.2f));
            seq.OnComplete(() => FalseWindow(popupcanvas[i]));
            seq.Play();
            Destroy(popupcanvas[i]);
        }
        
    }

}
