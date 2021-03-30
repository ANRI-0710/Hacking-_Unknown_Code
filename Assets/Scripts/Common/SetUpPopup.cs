using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
