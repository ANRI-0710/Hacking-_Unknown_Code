using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///メッセージリストの既読チェックとどのステージのメッセージページ移動するかを記録するクラス
/// </summary>
public class ListObject : MonoBehaviour
{    
    //ステージ番号
    private int StageNum;
    public int GetStageNum { get => StageNum;  set { StageNum = value; } }
    
    //既読チェック
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
   
    //既読していない場合新着メッセージマークを表示させる
    private void isReadCheck() 
    {
        if (isRead)
        {
            var exobj = transform.GetChild(3).gameObject;
            exobj.SetActive(true);       
        }    
    }

    //どのステージNoをタップしたかを感知し、記録し、該当番号のメッセージを再生する
    public void OnClickButton() 
    {                
        Debug.Log("StageNum" + StageNum + "が押された");
        //2をセット
        PlayerPrefs.SetInt(SaveData_Manager.KEY_STAGE_NUM, StageNum);
        SceneManager.LoadScene("MessengerScene");
    }

}
