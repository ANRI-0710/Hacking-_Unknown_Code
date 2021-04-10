using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 最速クリアタイムを更新するクラス
/// </summary>

public class ResultSceneBestTimes : MonoBehaviour
{
    [SerializeField]
    private Text[] TimeScoreText;
    private Text[] TimeScoreTextLine;

    private int[] ScoreTimeStageNum;

    void Start()
    {

        TimeScoreTextLine = new Text[TimeScoreText.Length];
        ScoreTimeStageNum = new int[TimeScoreText.Length];

        for (var i = 0; i < TimeScoreText.Length; i++) 
        {
            TimeScoreTextLine[i] = TimeScoreText[i].GetComponent<Text>();
        }

       //クリアステージの出力と更新
        ScoreTimeStageNum[0] = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_1, 0);
        ScoreTimeStageNum[1] = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_2, 0);
        ScoreTimeStageNum[2] = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_3, 0);
        ScoreTimeStageNum[3] = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_4, 0);
        ScoreTimeStageNum[4] = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_5, 0);
        ScoreTimeStageNum[5] = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_6, 0);
        ScoreTimeStageNum[6] = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_7, 0);
        ScoreTimeStageNum[7] = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_8, 0);
        ScoreTimeStageNum[8] = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_9, 0);
        ScoreTimeStageNum[9] = PlayerPrefs.GetInt(SaveData_Manager.KEY_BESTTIMESTAGE_10, 0);

        for (var k = 0; k < TimeScoreText.Length; k++) 
        {
            var str = new TimeSpan(0, 0, ScoreTimeStageNum[k]);
            TimeScoreTextLine[k].text = str.ToString();
        }

    }

   
  





}
