using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// パズルスタート時のWamingポップアップを出すクラス
/// </summary>

public class WarningPopup : Popup
{
    [SerializeField]
    private GameObject _WamingPopup1;

    public GameObject[] Popup;

    public void PopAlert(Vector3 pos, RectTransform transform) 
    {
        var obj = Instantiate(_WamingPopup1, pos, Quaternion.identity);
       // PopupStartArart(obj);
    }

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
       // Popup.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
      
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

    public IEnumerator ArlertStart() 
    {
        //PopAlert(new Vector3(0,0,0),);
        yield return null;

        //yield return WaitForSeconds(3.0f);

    }




}
