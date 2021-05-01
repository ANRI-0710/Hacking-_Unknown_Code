using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/// <summary>
/// 各パズルパートの最速タイムを保存するクラス
/// </summary>
public class BestTimeCount : MonoBehaviour
{      
    //パズルパートをクリアしたかどうかの判定
    private bool _timeStop;    
    public bool GetTimeStop {　set { _timeStop = value; } }

    //ベストタイムスコアを更新
    private int _bestTimeScore;
    public int GetBestTimeScore
    {
        get => _bestTimeScore;
        set
        {
            _bestTimeScore = value;
        }
    }
    
    //ベストスコアを格納する
    private int _save_Best_Time_Score;
    private int _stageNum;

    //ベストスコアのタイム表示
    private Text _bestTimeCount;

    void Start()
    {
        //各ステージのベストスコアを入れる
        InitBestTime();
        
        //タイムカウントを開始する
        StartCoroutine(TimeCount());
    }
 
    /// <summary>
    /// クリアタイマーの開始
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeCount() 
    {
        while (true) 
        {        
            //クリアしたタイムが登録しているタイムより早かったら、そのベストタイムを登録処理を行う
            if (_timeStop) 
            {
                if (_save_Best_Time_Score == 0)
                {
                    SetBestTime(_bestTimeScore);
                }
                else if (_bestTimeScore < 0) 
                {
                    if (_bestTimeScore > _save_Best_Time_Score)
                    {
                        SetBestTime(_bestTimeScore);
                    }
                }
                break;            
            }
            _bestTimeScore++;
            TimeDisplay();
            yield return new WaitForSeconds(1.0f);
        }
    
    }

    /// <summary>
    /// クリアタイムを反映する
    /// </summary>
    public void TimeDisplay()
    {
        var span = new TimeSpan(0, 0, _bestTimeScore);
        _bestTimeCount = this.GetComponent<Text>();
        _bestTimeCount.text = "CLEARTIME "+span.ToString();
    }

    /// <summary>
    /// ステージに応じたベストスコアを変数に格納する
    /// </summary>
    private void InitBestTime() 
    {
        _bestTimeCount = this.GetComponent<Text>();
        _stageNum = PlayerPrefs.GetInt(SaveData_Manager.KEY_STAGE_NUM);

        switch (_stageNum)
        {
            case 2:
                _save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_1);
                break;
            case 3:
                _save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_2);
                break;
            case 4:
                _save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_3);
                break;
            case 5:
                _save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_4);
                break;
            case 6:
                _save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_5);
                break;
            case 7:
                _save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_6);
                break;
            case 8:
                _save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_7);
                break;
            case 9:
                _save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_8);
                break;
            case 10:
                _save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_9);
                break;
            case 11:
                _save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_10);
                break;
        }

    }

    /// <summary>
    /// PlayerPrefで過去に登録したクリアタイムよりも早かったら、更新する
    /// </summary>
    /// <param name="bestTimeScore"></param>
    private void SetBestTime(int bestTimeScore) 
    {

        //登録した記録を代入処理
        switch (_stageNum)
        {
            case 2:
                PlayerPrefs.SetInt(SaveData_Manager.KEY_BESTTIMESTAGE_1, bestTimeScore);
                break;
            case 3:
                PlayerPrefs.SetInt(SaveData_Manager.KEY_BESTTIMESTAGE_2, bestTimeScore);
                break;
            case 4:
                PlayerPrefs.SetInt(SaveData_Manager.KEY_BESTTIMESTAGE_3, bestTimeScore);
                break;
            case 5:
                PlayerPrefs.SetInt(SaveData_Manager.KEY_BESTTIMESTAGE_4, bestTimeScore);
                break;
            case 6:
                PlayerPrefs.SetInt(SaveData_Manager.KEY_BESTTIMESTAGE_5, bestTimeScore);
                break;
            case 7:
                PlayerPrefs.SetInt(SaveData_Manager.KEY_BESTTIMESTAGE_6, bestTimeScore);
                break;
            case 8:
                PlayerPrefs.SetInt(SaveData_Manager.KEY_BESTTIMESTAGE_7, bestTimeScore);
                break;
            case 9:
                PlayerPrefs.SetInt(SaveData_Manager.KEY_BESTTIMESTAGE_8, bestTimeScore);
                break;
            case 10:
                PlayerPrefs.SetInt(SaveData_Manager.KEY_BESTTIMESTAGE_9, bestTimeScore);
                break;
            case 11:
                PlayerPrefs.SetInt(SaveData_Manager.KEY_BESTTIMESTAGE_10, bestTimeScore);
                break;
        }

    }

}
