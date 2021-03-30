using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ListObject : MonoBehaviour
{

    
    
    
    

    private int StageNum;
    public int GetStageNum { get => StageNum;  set { StageNum = value; } }
    
    private bool isRead = false;
    public bool GetisRead 
    {        
      get => isRead;        
      set
      {             
        isRead = value;
        isReadCheck();
      }         
    }

    private void Start()
    {

       

    }


    private void isReadCheck() 
    {
        if (isRead)
        {
            var exobj = transform.GetChild(3).gameObject;
            exobj.SetActive(true);       
        }    
    }

    public void OnClickButton() 
    {                
        Debug.Log("StageNum" + StageNum + "が押された");
        //2をセット
        PlayerPrefs.SetInt(SaveData_Manager.KEY_STAGE_NUM, StageNum);
        SceneManager.LoadScene("MessengerScene");
    }


   














}
