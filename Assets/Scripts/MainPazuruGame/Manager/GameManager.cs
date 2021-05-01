using UnityEngine;
using System.Collections;

/// <summary>
/// ゲーム全体の進行を管理するクラス
/// ピース破壊演出タイミングなどを細かく調整可能にすることと、可動性を上げる目的で
/// 各工程（ピース生成➡ピース交換➡連鎖・コンボチェック➡ピースの破壊と演出➡ピース再生成）をコルーチンで分割。
/// 但し、Update関数との併用も検討し、効率良いコーディングを目指したい
/// </summary>
/// 
public class GameManager : MonoBehaviour
{
    //---------------------------------------------------------------------------------------- 
    //Managerクラス（ピース・敵・リミットボタン・UIなど管理クラスからアクセスし、制御をする）
    //---------------------------------------------------------------------------------------- 
    
    /// <summary>
    /// ゲームコントロール用のインスタンス所得
    /// </summary>
    public static GameManager Instance { get; private set; }

    //パズルピースの盤面管理
    [SerializeField]
    private PieceManager _PieceManager;
    public PieceManager GetPieceManager { get => _PieceManager; set { _PieceManager = value; } }

    //敵の管理
    [SerializeField]
    private EnemyManager _EnemyManager;
    public EnemyManager GetEnemyManager { get => _EnemyManager; set { _EnemyManager = value; } }
   
    //必殺技ボタン・ゲージの管理
    [SerializeField]
    private LimitManager _LimitManager;
    public LimitManager GetLimitManager { get => _LimitManager; set { _LimitManager = value; } }

    //UI管理（ポップアップやクリア画面の表示等） 
    [SerializeField]
    private UIManager _UiManager;
    public UIManager GetUIManager { get => _UiManager; }


    //---------------------------------------------------------------------------------------- 
    //フラグ管理
    //---------------------------------------------------------------------------------------- 
    /// <summary>
    ///ピースの交換が成立したかのフラグチェック
    /// </summary>
    private bool _exchangeSatisfied = false;
    public bool GetexchangeSatisfied { get => _exchangeSatisfied; set { _exchangeSatisfied = value; } }

    /// <summary>
    /// コルーチン制御用・3マッチチェック後及び特殊パズルが破壊された場合、削除フラグをTrueにして削除処理のコルーチンに移動するためのフラグ
    /// </summary>
    private bool isDestroy = false;
    public bool GetisDestroy { get => isDestroy; set { isDestroy = value; } }
    
   /// <summary>
   /// リミット技が発動したかどうか
   /// </summary>
    private bool LimitInvocating = false;
    public bool GetLimitInvocating { get => LimitInvocating; set { LimitInvocating = value; } }
    
    /// <summary>
    /// 特殊パズルを交換したかどうかのフラグ
    /// </summary>
    private bool FeverCheck = false;
    public bool GetFeverCheck { get => FeverCheck; set { FeverCheck = value; } }

    //チュートリアル既読フラグ
    private int Is_Pazuru_Tutorial;
    public int GetIs_Pazuru_Tutorial { get => Is_Pazuru_Tutorial; set { Is_Pazuru_Tutorial = value; } }

  
    //---------------------------------------------------------------------------------------- 
    //選択ステージ・クリア記録など設定したデータを呼び出す（PlayerPrefデータ）
    //---------------------------------------------------------------------------------------- 

    //ステージナンバー
    private int StageNum;
    public int GetStageNum { get => StageNum; }
    
    //クリアナンバー更新用
    private int ClearNum;
    public int GetClearNum { get => ClearNum; }

    //スペシャルアタックの攻撃
    private int SpecialAttack1;
    private int SpecialAttack2;
    public int GetSpecialAttack1 { get => SpecialAttack1; }
    public int GetSpecialAttack2 { get => SpecialAttack2; }

    //---------------------------------------------------------------------------------------- 
    //その他
    //---------------------------------------------------------------------------------------- 

    //キャンバスサイズの調整用
    [SerializeField]
    private Canvas _Canvas; //Cameraクラスから画面サイズを受け取り、ピースの比率を調整をする    

  　//パーティクルへのアクセス
    [SerializeField]
    private Particle _Particles;
    public Particle GetParticle { get => _Particles; }

    //必殺技クラスへのアクセス
    [SerializeField]
    private SpecialPieceAttack _specialPieceAttack;   
    public SpecialPieceAttack GetSpecialAttack { get => _specialPieceAttack; }
  
    //---------------------------------------------------------------------------------------- 
    //ここから関数
    //---------------------------------------------------------------------------------------- 
    private void Awake()
    {
       //GameManagerのシングルトンの所得
        Instance = this;

      //exeファイルにて出力するためにサイズを固定・apkで発行する場合、こちらの追加修正予定 
       Screen.SetResolution(432, 768, false, 60);
      
      //受け取った画面サイズでピースサイズの初期化を行う 
      　InitScreenSize();

      //選択したステージ番号をセットする
        ClearNum = PlayerPrefs.GetInt("STAGECLEARNUM", 0);
        StageNum = PlayerPrefs.GetInt("STAGENUM", 0);

      //設定したウイルスセットの技の番号をセットする
        SpecialAttack1 = PlayerPrefs.GetInt("ATTACK1", 0);
        SpecialAttack2 = PlayerPrefs.GetInt("ATTACK2", 0);
        Is_Pazuru_Tutorial = PlayerPrefs.GetInt("KEY_ISREAD_PAZURU_TUTORUAL", 0);

        //パズル専用サウンドに切り替え
        Common_Sound_Manager.Instance.Sound_Play(BGM.Stop);
    }

    void Start()
    {
        //パズル盤の初期化
        _PieceManager.InitBoard();
     
        //リミットゲージの初期化
        _LimitManager.InitLimitObject(GameManager.Instance.GetLimitManager.GetLimitTransform);

        //敵の初期化
        _EnemyManager.InitEnemys(_EnemyManager.GetenemyCount);

        //[1]ゲームの開始コルーチン    
        StartCoroutine(StartAction());
    }

    /// <summary>
    ///[1]ゲームスタート時に必要なアクションを行う（ゲーム中1回のみ通過する） 
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartAction() 
    {        
        //警告ポップアップの出現
        yield return _UiManager.ArrartAnim();      
       
        //チュートリアルポップアップが既読でない場合は、出現させる
        if (Is_Pazuru_Tutorial != (int)ISREAD.READ)
        {
            yield return _UiManager.Popup_TutorialStart();
        }        
        yield return _UiManager.StartSignal(); //既読の場合はそのままスタートシグナルが開始

<<<<<<< HEAD
        StartCoroutine(PieceExchangeTurnLoop());
        StartCoroutine(DebugMenu());
=======
        StartCoroutine(PieceExchangeTurnLoop());        
>>>>>>> f061488f36f51569aa254191514fb7a1a159ec79
    }


    /// <summary>
    /// [2]ピースが十字4方向に交換されたか、必殺技ボタンが発動されたかをチェックするターン。
    /// </summary>
    /// <returns></returns>
    public IEnumerator PieceExchangeTurnLoop()
    {       
        //タップ座標を保存と必殺技ボタンが押されたかをwhileループでチェックする
        yield return _PieceManager.PieceTapCheckLoop();

        //必殺技が押されたらピースの交換成立フラグ（_exchangeSatisfied）をスキップしてPieceMatchingTurnに移行
        if (LimitInvocating) 
        {                  
            isDestroy = true;
            yield return PieceMatchingTurnLoop();
            LimitInvocating = false;
            yield break;
        }
        //もし、ピースの交換の条件に一致した場合、[3]ピース交換フェーズに入る
        if (_exchangeSatisfied)
        {
            yield return _PieceManager.ExchangePiece();      
            yield return PieceMatchingTurnLoop();
            yield break;
        }
        
        if(!_exchangeSatisfied)  //でなければ戻る
        {
            StartCoroutine(PieceExchangeTurnLoop());
            yield break;
        }
    }

    /// <summary>
    /// [3]パズルの3マッチ成立チェック➡ピース破壊➡盤面調整➡敵からの攻撃…のサイクルを行うターン
    /// </summary>
    /// <returns></returns>
    public IEnumerator PieceMatchingTurnLoop() 
    {

        //0行目内のピースにアンチウイルスパズル及びEMPTY状態の場合は、isDestroyフラグをオンにし、ピースを下に詰める
        //連鎖コンボが発生時、PieceMatchingTurnがループし続けるためまず最初にチェックを行う
        yield return _PieceManager.ObstaclePieceCheck();

        //特殊パズルが交換された場合は、3マッチ成立チェックを飛ばして、下に詰める作業を行う
        if (GetFeverCheck) { isDestroy = true; }
        else { yield return _PieceManager.DestroyPieceCheak(); } //3マッチしているかチェックする

        //ピース破壊処理。isDestroyがONになる＝ピースの削除フラグが発生するため下記の処理を行う
        if (isDestroy)
        {
            //ピースを下に詰める
            yield return _PieceManager.DownPackPiece();
            //ピースを破壊する
            yield return _PieceManager.DestroyPiece();
            //新しいピースを生成する
            yield return _PieceManager.CreateNewPiece();
            // 敵のHPが0かどうか確認する。0だったら次の敵を出すもしくはクリア判定/ゲームオーバー判定を出す
            yield return _EnemyManager.Is_Enemy_Check();
            //敵の攻撃カウントが0の場合、アンチウイルスパズルを送信する
            yield return _EnemyManager.EnemyAttack();

            //フラグの初期化をした後、ピース破壊された後の盤面で3マッチチェックを行うため(PieceMatchingTurnをループする
            GetFeverCheck = false;
            isDestroy = false;
            StartCoroutine(PieceMatchingTurnLoop());
            yield break;
        }
        else //OFFの場合は盤面上で3マッチしている盤面が発生していないため[2]PieceExchangeTurnに戻る
        {         
            //フラグの初期化
            _exchangeSatisfied = false;
            GetFeverCheck = false;
            isDestroy = false;
            StartCoroutine(PieceExchangeTurnLoop());
            yield break;
        }

    }

    /// <summary>
    ///端末サイズによってピースの比率を変更する
    /// </summary>
    private void InitScreenSize()
    {
        var w = (float)Screen.width;
        var h = (float)Screen.height;
        
        //カメラの位置の調整
        Camera.main.transform.position = new Vector3(w, h, -10);
        Camera.main.orthographicSize = w;
        var canvas = _Canvas.transform.position;      
        var z = canvas.z;

        //Canvasに代入
        canvas = new Vector3(w, h, z);
        _Canvas.transform.position = canvas;
        
        var canvassize = _Canvas.GetComponent<RectTransform>();
        canvassize.sizeDelta = new Vector2(w, h);
<<<<<<< HEAD
    }

    /// <summary>
    /// デバッグコマンド
    /// </summary>
    /// <returns></returns>
    private IEnumerator DebugMenu() 
    {
        while (true) 
        {
            if (Input.GetKeyDown(KeyCode.D)) { GameManager.Instance._EnemyManager.GetenemyConroll.Get_NowHpPoint = 0; }
            yield return null;       
        }
    
    }



=======
    }

>>>>>>> f061488f36f51569aa254191514fb7a1a159ec79
}

