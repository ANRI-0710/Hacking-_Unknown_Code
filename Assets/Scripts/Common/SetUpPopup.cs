using UnityEngine;

/// <summary>
/// ポップアップの呼び出しクラス（不必要と判断しPopUpクラスと統合予定　2021/03/29）
/// </summary>
public class SetUpPopup : Popup
{
    [SerializeField]
    GameObject _Paneru;

    public void SetUpButton() 
    {
        _Paneru.SetActive(true);
        PopupStart(_Paneru);    
    }
    public void SetUpClouse() 
    {
        Popup_Close(_Paneru);
    }

   


}
