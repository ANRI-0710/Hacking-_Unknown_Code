﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Popup
{    
    [SerializeField]
    private GameObject _Crear_Popup;
    [SerializeField]
    private GameObject _GameOver_Popup;
    [SerializeField]
    private GameObject Stop_Popup;
    [SerializeField]
    SceneChangeManager sceneChangeManager;

    public void GameClear()
    {
        _Crear_Popup.SetActive(true);
        PopupStart(_Crear_Popup);
    }

    public void GameOver()
    {
        _GameOver_Popup.SetActive(true);
        PopupStart(_GameOver_Popup);
    }

    
    public void Retry()
    {
        SceneManager.LoadScene("MaypageScene");
    }






}
