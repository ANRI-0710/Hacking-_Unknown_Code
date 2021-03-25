using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToListPage : Popup
{
    [SerializeField]
    private GameObject _Panel;

    private int stageNum;

    private void Start()
    {
        stageNum = PlayerPrefs.GetInt(SaveData_Manager.KEY_STAGE_NUM, 0);
    }

    public void AppearancePopup()
    {
        if (stageNum != 0 && stageNum != 1) {

            PopupStart(_Panel);
            _Panel.SetActive(true);
        }

    }
       
    public void Onclick() 
    {
        _Panel.SetActive(false);
        SceneManager.LoadScene("ListMessengerScene");    
    }


}
