using UnityEngine;

public class DataInitialization : MonoBehaviour
{
    public void SetData()
    {
        PlayerPrefs.SetInt(SaveData_Manager.KEY_STAGE_NUM, 0);
        PlayerPrefs.SetInt(SaveData_Manager.KEY_CLEAR_NUM, 0);

        Debug.Log(SaveData_Manager.KEY_STAGE_NUM);
    }
}
