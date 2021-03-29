using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ベストタイムを保存するクラス
/// </summary>

public class BestTimeCount : MonoBehaviour
{  
    private Text _BestTimeCount;
    private bool TimeStop;
    public bool GetTimeStop { get => _BestTimeCount; set { TimeStop = value; } }

    private int Save_Best_Time_Score;
    private int StageNum;

    private int BestTimeScore;
    public int GetBestTimeScore
    {
        get => BestTimeScore;
        set
        {
            BestTimeScore = value;
        }
    }


    void Start()
    {
        var num = Save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_7);
        Debug.Log("Stage77777" + num);
               
        _BestTimeCount = this.GetComponent<Text>();

       StageNum =  PlayerPrefs.GetInt(SaveData_Manager.KEY_STAGE_NUM);

        switch (StageNum)         
        {
            case 2:
                Save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_1);         
                break;
            case 3:
                Save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_2);
                break;
            case 4:
                Save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_3);
                break;
            case 5:
                Save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_4);
                break;
            case 6:
                Save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_5);
                break;
            case 7:
                Save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_6);
                break;
            case 8:
                Save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_7);
                break;
            case 9:
                Save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_8);
                break;
            case 10:
                Save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_9);
                break;
            case 11:
                Save_Best_Time_Score = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_10);
                break;
        }

        StartCoroutine(TimeCount());
    }

  
    IEnumerator TimeCount() 
    {
        while (true) 
        {        
            if (TimeStop) 
            {
                if (Save_Best_Time_Score == 0)
                {
                    SetBestTime(BestTimeScore);
                }
                else if (BestTimeScore < 0) 
                {

                    if (BestTimeScore > Save_Best_Time_Score)
                    {
                        SetBestTime(BestTimeScore);
                    }
                }
                break;            
            }
            BestTimeScore++;
            TimeDisplay();
            yield return new WaitForSeconds(1.0f);
        }
    
    }

    public void TimeDisplay()
    {
        var span = new TimeSpan(0, 0, BestTimeScore);
        _BestTimeCount = this.GetComponent<Text>();
        _BestTimeCount.text = "CLEARTIME "+span.ToString();

    }


    private void SetBestTime(int bestTimeScore) 
    {

        switch (StageNum)
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
