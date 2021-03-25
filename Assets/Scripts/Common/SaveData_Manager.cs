using UnityEngine;

/// <summary>
/// セーブデータ管理クラス
/// </summary>

public class SaveData_Manager : Singleton<SaveData_Manager>
{  
    //ステージ記録フラグ
    public const string KEY_STAGE_NUM = "STAGENUM";
    public const string KEY_LIST_NUM = "LISTNUM";
   
    //既読フラグ
    public const string KEY_ISREAD_NUM = "KEY_ISREAD_NUM"; 

    //クリアフラグ
    public const string KEY_CLEAR_NUM = "STAGECLEARNUM";

    //スペシャル攻撃の技の登録
    public const string KEY_ATTACK_1 = "ATTACK1";
    public const string KEY_ATTACK_2 = "ATTACK2";

    //クリアタイムの保存（最速記録のものだけ表示）
    public const string KEY_BESTTIMESTAGE_1 = "KEY_BESTTIMESTAGE_1";
    public const string KEY_BESTTIMESTAGE_2 = "KEY_BESTTIMESTAGE_2";
    public const string KEY_BESTTIMESTAGE_3 = "KEY_BESTTIMESTAGE_3";
    public const string KEY_BESTTIMESTAGE_4 = "KEY_BESTTIMESTAGE_4";
    public const string KEY_BESTTIMESTAGE_5 = "KEY_BESTTIMESTAGE_5";
    public const string KEY_BESTTIMESTAGE_6 = "KEY_BESTTIMESTAGE_6";
    public const string KEY_BESTTIMESTAGE_7 = "KEY_BESTTIMESTAGE_7";
    public const string KEY_BESTTIMESTAGE_8 = "KEY_BESTTIMESTAGE_8";
    public const string KEY_BESTTIMESTAGE_9 = "KEY_BESTTIMESTAGE_9";
    public const string KEY_BESTTIMESTAGE_10 = "KEY_BESTTIMESTAGE_10";

    /// <summary>
    /// シングルトンの起動
    /// </summary>
    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    

}
