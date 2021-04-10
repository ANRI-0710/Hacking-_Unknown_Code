using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///メッセージリストの既読チェックとどのステージのメッセージページ移動するかを記録するクラス
/// </summary>
public class ListObject : MonoBehaviour
{    
    //ステージ番号
    private int stageNum;
    public int GetStageNum { get => stageNum;  set { stageNum = value; } }
    
    //既読チェック
    private bool isRead = false;
    public bool GetisRead 
    {        
      get => isRead;        
      set
      {             
        isRead = value;
        IsReadCheck();
      }         
    }

    /// <summary>
    /// 既読していない場合新着メッセージマークを表示させる
    /// </summary>
    private void IsReadCheck() 
    {
        if (isRead)
        {
            var exobj = transform.GetChild(3).gameObject;
            exobj.SetActive(true);       
        }    
    }

    /// <summary>
    ///どのステージNoをタップしたかを感知し、記録し、該当番号のメッセージを再生する
    /// </summary>
    public void OnClickButton() 
    {                
        Debug.Log("StageNum" + stageNum + "が押された");
        //2をセット
        PlayerPrefs.SetInt(SaveData_Manager.KEY_STAGE_NUM, stageNum);
        SceneManager.LoadScene("MessengerScene");
    }

}
