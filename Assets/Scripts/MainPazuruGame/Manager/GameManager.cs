using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ゲーム全体の進行を管理するクラス
/// ピース破壊演出タイミングなどを細かく調整可能にすることと、可動性を上げる目的で
/// 各工程（ピース生成➡ピース交換➡連鎖・コンボチェック➡ピースの破壊と演出➡ピース再生成）をコルーチンで分割。
/// </summary>
/// 
public class GameManager : MonoBehaviour
{
    //パズルピース関連---------------------------------------------------------------------------------------- 

    //パズルピースの生成・コントロール   
    [SerializeField]
    private GameObject _PazuruPrefab;

    //ピースオブジェクトへのアクセス（ピース破壊する際のDestroy指示の際に使用）
    private GameObject[,] _pieceGameObjects = new GameObject[Cols, Rows];

    //ピースクラスへのアクセス
    private Piece[,] _pieces = new Piece[Cols, Rows];    
    
    //タップしたピースの保存する
    private Piece _mouseDown;
    private Piece _mouseUp;
    private Piece _inputMemoryPiece;

    //3マッチチェック用・同じ色のピースが3つ以上揃っているか、上下左右に確認するための配列。又ピースを原点に十字方向に破壊できる特殊パズルの破壊時にも使用
    private int[] _Same_Block_Check_Direction_X = new int[] { 0, 1, 0, -1 };
    private int[] _Same_Block_Check_Direction_Y = new int[] { 1, 0, -1, 0 };

    //ピースを原点に3×3方向に破壊できる特殊パズルの破壊時に使用するチェック配列
    private int[] _Same_5_Block_Cheack_Direction_X = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 };
    private int[] _Same_5_Block_Cheack_Direction_Y = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };

    //削除するピースの座標位置を保存する（3マッチして破壊したパズルをリスト（座標位置保存）に入れる）
    private List<Blocks> _DestroyPieceList = new List<Blocks>();

    //何回連鎖が続いたかを記録する変数
    private int ComboCount;

    //コルーチン制御用・3マッチチェック後及び特殊パズルが破壊された場合、削除フラグをTrueにして削除処理のコルーチンに移動するためのフラグ
    private bool isDestroy = false;

    //3マッチしたピースが連鎖して破壊する際の速度調整（エディタ上で調整できるようにSerializeFieldにする）
    [SerializeField]
    private float WaitTime;

    //補充ピースがパズル盤に定位置に再生成される際の、ピースが落ちる速度（エディタ上で調整できるようにSerializeFieldにする）
    [SerializeField]
    private float FallTime;

    //ピースの行・列
    private const int Cols = 7;
    private const int Rows = 7;

    //ピースのマッチ数の定数
    private const int _piece_Match_3 = 3;
    private const int _piece_Match_4 = 4;
    private const int _piece_Match_5 = 5;

    //ピース再生時にどの地点からrecttransform上のどこ地点から生成するかの調整用変数
    private int FallNum = 9;

    //----------------------------------------------------------------------------------------------------------------------------

    //敵（セキュリティモンスター）関連---------------------------------------------------------------------------------------- 

    //敵の生成専用クラス（ここから敵を生成し、リターンでBaseEnemyに戻す）  
    [SerializeField]
    private EnemyFactory _EnemyFactroy;

    //敵のステータス（敵データが格納された構造体をそれぞれのクラス・変数に代入する）
    [SerializeField]
    private Enemy_Status _EnemyStatus;

    //敵の攻撃パターンを管理するクラスへのアクセス
    [SerializeField]
    private EnemyAttack _enemyAttack;

    //敵クラスへのアクセス
    private BaseEnemy _enemyConroll;

    //敵の出現回数（全3体、敵を倒すとCountが1プラスされる）
    private int _enemyCount = 0;

    //何体目の敵かの定数表示
    private const int enemy_1 = 0;  //1体目
    private const int enemy_2 = 1;  //2体目
    private const int enemy_3 = 2;　//3体目
    private const int enemyCount = 3;   //敵の総数
     
    //敵の総数分の敵ステータスの参照用配列
    private EnemyStatus[] Enemys = new EnemyStatus[enemyCount];

    //敵を撃破しなければならない時間制限をカウントするコルーチン
    private IEnumerator EnemyCountCol;

    //----------------------------------------------------------------------------------------------------------------------------

    //表示順（レイヤーの表示順管理のため）---------------------------------------------------------------------------------------- 
    [SerializeField]
    private RectTransform _PieceTrans;　//ピースの表示位置の固定
    [SerializeField]
    private RectTransform _EnemyTransform;　//敵の表示位置の固定
    [SerializeField]
    private RectTransform UIrectTransform; 　//UIの表示位置の固定
    [SerializeField]
    private RectTransform _SignalTransform; //スタート・クリア・第一層~3層までの表示位置の固定 
    [SerializeField]
    private RectTransform _LimitTransform;   //リミットボタンの表示位置の固定

    //-----------------------------------------------------------------------------------------------------------------------------

    //選択ステージ・クリア記録など設定したデータを呼び出す（PlayerPrefデータ）---------------------------------------------------------------------------------------- 
    //ステージナンバー
    private int StageNum;
    
    //クリアナンバー更新用
    private int ClearNum;

    //スペシャルアタックの攻撃
    private int SpecialAttack1;
    private int SpecialAttack2;

    //チュートリアル既読フラグ
    private int Is_Pazuru_Tutorial;

    //-----------------------------------------------------------------------------------------------------------------------------

    //その他---------------------------------------------------------------------------------------- 

    //座標変換クラス（インスタエイトの際にUI座標からワールド座標に変換する必要があるため）
    [SerializeField]
    private VectorReturn _InputClass;

    //パーティクルの生成
    [SerializeField]
    private RectTransform _ParticleTransform;
    [SerializeField]
    private Particle _Particles;

    //必殺技クラス
    [SerializeField]
    private SpecialPieceAttack _specialPieceAttack;   
    [SerializeField]
    private LimitManager _LimitManager;

    //UI関連へのアクセス  
    [SerializeField]
    private UIManager _UiManager;

    //チュートリアル表示
    [SerializeField]
    private Popup_Tutorial _PopupTutorial;

    //キャンバスサイズの調整用
    [SerializeField]
    private Canvas _Canvas; //Cameraクラスから画面サイズを受け取り、ピースの比率を調整をする    
    
    //画面比率サイズ
    private int Width;

    //-----------------------------------------------------------------------------------------------------------------------------

    //ここから関数---------------------------------------------------------------------------------------- 
    private void Awake()
    {
      //exeファイルにて出力するためにサイズを固定・apkで発行する場合、こちらの追加修正予定 
        Screen.SetResolution(432, 768, false, 60);
       
      //画面サイズを受け取る  
        Width = (Screen.width / Cols);
      
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
       InitBoard();
        
　　　//UIの初期化
       InitUI();

      //敵の初期化
       InitEnemys(_enemyCount);

　　　//[1]ゲームの開始コルーチン
       StartCoroutine(ArrartAnim());      
    }

    /// <summary>
    /// [1]警告ポップアップの表示演出
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public IEnumerator ArrartAnim()
    {
        yield return new WaitForSeconds(1.0f);
        _UiManager.WamingArrart(_SignalTransform, UIrectTransform);
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.Alert);
        yield return new WaitForSeconds(3.0f);
        _UiManager.ArrartClosePopup();
        yield return Popup_TutorialStart();
    }

    /// <summary>
    ///  [2]初めてパズルパートをプレイする際にチュートリアルを表示する・既読チェックがついてる場合は、スタート開始する
    /// </summary>
    /// <returns></returns>
    public IEnumerator Popup_TutorialStart() 
    {
        if (Is_Pazuru_Tutorial == (int)ISREAD.READ)
        {
            yield return StartSignal();
        }
        else 
        {
          _PopupTutorial.TutorialStart();
        }

        while (true) 
        {
            if (_PopupTutorial.GetIsTutrialFinish)
            {
                Is_Pazuru_Tutorial = (int)ISREAD.READ;
                PlayerPrefs.SetInt(SaveData_Manager.KEY_ISREAD_PAZURU_TUTORIAL, Is_Pazuru_Tutorial);
                yield return StartSignal();                
                break;
            }
            yield return null;
        }
        
    }

    /// <summary>
    ///  [3]スタート開始の合図のコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartSignal()
    {
        _UiManager.SignalPopup("FIRST 1/3", _SignalTransform);
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(EnemyCountCol);
        yield return new WaitForSeconds(2.0f);
        yield return TapMousePiece();
    }

    /// <summary>
    /// [4]タップした座標位置（Down,Up）を保存する
    /// </summary>
    /// <returns></returns>
    public IEnumerator TapMousePiece()
    {
        Debug.Log("TapMousePiece");       
        while (true)
        {           
            //リミットボタンの発動チェック
            for (var i = 0; i < 2; i++) 
            {
                if (_LimitManager[i]) 
                {
                    if (i == 0) { _specialPieceAttack.SpecialAttack((E_SpecialAttack)SpecialAttack1, _pieces); }
                    if (i == 1) { _specialPieceAttack.SpecialAttack((E_SpecialAttack)SpecialAttack2, _pieces); }
                    _LimitManager[i] = false;
                    isDestroy = true;
                    yield return null;
                    _LimitManager.LimitInitValues();
                    yield return ObstaclePieceCheck();
                }            
            }
         
            //ピースのタップ検知
            if (Input.GetMouseButtonDown(0))//押した瞬間のピースの情報を所得
            {                
                _mouseDown = _InputClass.ReturnRaycastPiece(_pieces[0, 0]);
                //選択したピースを大きくする
                _pieces[_mouseDown.GetPieceX, _mouseDown.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(1.5f, 1.5f) * Width;
                Debug.Log("MDX" + _mouseDown.GetPieceX + "MDY" + _mouseDown.GetPieceY);
            }
            else if (Input.GetMouseButtonUp(0)) //離したときのピースの情報を所得
            {
                _mouseUp = _InputClass.ReturnRaycastPiece(_pieces[6, 6]);
                _pieces[_mouseUp.GetPieceX, _mouseUp.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(1.5f, 1.5f) * Width;

                var xtap = (int)_mouseUp.GetPieceX - _mouseDown.GetPieceX;
                var ytap = (int)_mouseUp.GetPieceY - _mouseDown.GetPieceY;
                Vector2 poscheak = new Vector2(xtap, ytap);
              
                //ピースの離した座標からピースを最初に所得した座標を引いて、引いた座標がピースを中心に上下左右方向の座標であればピースの交換をする
                if (poscheak == new Vector2(1, 0) || poscheak == new Vector2(0, 1) || poscheak == new Vector2(-1, 0) || poscheak == new Vector2(0, -1))
                {
                    yield return ExchangePiece();
                }
                else
                {
                    //そうれなければタップしたピースの大きさを元に戻す
                    _pieces[_mouseDown.GetPieceX, _mouseDown.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
                    _pieces[_mouseUp.GetPieceX, _mouseUp.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
                }
            }
            yield return null;

            //スキル技 ・デバッグ用      
            if (Input.GetKeyDown(KeyCode.A)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_Red, _pieces);      
            if (Input.GetKeyDown(KeyCode.B)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_Blue, _pieces);          
            if (Input.GetKeyDown(KeyCode.C)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_Green, _pieces);          
            if (Input.GetKeyDown(KeyCode.D)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_White, _pieces);          
            if (Input.GetKeyDown(KeyCode.E)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_Yellow, _pieces);          
            if (Input.GetKeyDown(KeyCode.F)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_HorizontalOneArray, _pieces);           
            if (Input.GetKeyDown(KeyCode.G)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_VerticalOneArray, _pieces);          
            if (Input.GetKeyDown(KeyCode.H)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_Destroy_Cross, _pieces);           
            if (Input.GetKeyDown(KeyCode.I)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_ObliqueCross_Cross, _pieces);          
            if (Input.GetKeyDown(KeyCode.Q)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_BlackPiece_Destroy, _pieces);
            if (Input.GetKeyDown(KeyCode.Z)) _enemyConroll.Get_NowHpPoint = 0;
            if (Input.GetKeyDown(KeyCode.U)) _enemyConroll.GetEnemyTimeLimit = 0 ;
        }
    }

    /// <summary>
    ///  [5]ピースを交換する
    /// </summary>
    /// <returns></returns>
    IEnumerator ExchangePiece()
    {
        Debug.Log("Exchange Block");
        ComboCount = 0;
        yield return null;       
        //ピースの大きさを元に戻す
        _pieces[_mouseDown.GetPieceX, _mouseDown.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
        yield return null;
        _pieces[_mouseUp.GetPieceX, _mouseUp.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;       
        
        //mouseDownの座標を格納
        _inputMemoryPiece.GetPieceState = _pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState;
        yield return null;       
        
        //mouseDown ←　mouseUpに代入
        _pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState = _pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState;           
        yield return null;

        //mouseDown →　mouseUpに代入
        _pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState = _inputMemoryPiece.GetPieceState;
        yield return null;        
        
        //FEVER4 or FEVER5のピースを交換した場合は、FEVERチェックを通す      
        if (Fever_Exchange_Check(_mouseDown, _mouseUp)) {
          
            //敵からの攻撃までのカウントを更新
            _enemyConroll.GetEnemyAttackCount++;

            //ピースの破壊と下に詰める作業に移行
            yield return DownPackPiece();
        }        
        yield return ObstaclePieceCheck();
    }

    /// <summary>
    /// [6]アンチウイルスパズル（じゃまPiece）が一番下列にある場合、爆発処理をする
    /// </summary>
    /// <returns></returns>
    IEnumerator ObstaclePieceCheck() 
    {        
        for (var i = 0; i < Rows; i++) 
        {                           
            if (_pieces[i, 0].GetPieceState == Piece_Type.BLACK) 
            {
                PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyPieceDestroy);
                _pieces[i, 0].GetPieceState = Piece_Type.EMPTY;
                isDestroy = true;
                DestroyPiecePaticles(new Vector2(i, 0));
            }           
        }
        yield return DestroyPieceCheak();
    }

    /// <summary>
    ///[7]全ピースの状態を4方向に確認。３つ以上揃っていたらリストに入れてEMPTYフラグ（= Destroyフラグ）を立てる
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyPieceCheak()
    {
        Piece_Size_Back();
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {              
                for (var p = 0; p < _Same_Block_Check_Direction_X.Length; p++)
                {
                    bool _CanTurn = false;
                    
                    _DestroyPieceList.Clear();
                    
                    //ピースタイプにピースの種類を代入
                    Piece_Type piecetype = _pieces[i, k].GetPieceState;
                    
                    //リストに格納
                    _DestroyPieceList.Add(new Blocks(i, k, _pieces[i, k].GetPieceState));

                    int pazurux = i;
                    int pazuruy = k;

                    //上下左右4方向に同じ色のピースがあるかどうかを検索し、あった場合はリストに入れる
                    while (true)
                    {
                        //4方向の座標を入れてパズル盤の盤面の端まで検索をし続ける
                        pazurux += _Same_Block_Check_Direction_X[p];
                        pazuruy += _Same_Block_Check_Direction_Y[p];

                        //盤面外であれば処理を終了する
                        if (!(pazurux >= 0 && pazurux < Rows && pazuruy >= 0 && pazuruy < Cols)) break;

                        //現在検索しているピースが、piecetypeの色と一致しており、更にアンチウイルスパズルではない場合、リストに追加する
                        if (_pieces[pazurux, pazuruy].GetPieceState == piecetype && _pieces[pazurux, pazuruy].GetPieceState != Piece_Type.BLACK)
                        {
                            _DestroyPieceList.Add(new Blocks(pazurux, pazuruy, _pieces[pazuruy, pazurux].GetPieceState));
                            
                            //次に検索するピースを代入
                            int pazuruxplus = pazurux + _Same_Block_Check_Direction_X[p];
                            int pazuruyplus = pazuruy + _Same_Block_Check_Direction_Y[p];

                            //リストに3つ以上ピースがあり、次に検索するピースが盤外だったら
                           if (_DestroyPieceList.Count >= 3 && !(pazuruxplus >= 0 && pazuruxplus < Rows && pazuruyplus >= 0 && pazuruyplus < Cols))                          
                           {
                                //ピースの破壊演出を行う
                                _CanTurn = true;
                                break;
                           }
                            else { continue; }  //でなければ検索し続ける
                        }
                        else // 現在検索しているピースが一致しないが、既にリストに３つ入っている場合は破壊演出処理を行う
                        {
                            
                            if (_DestroyPieceList.Count >= 3)
                            {
                                _CanTurn = true;
                                break;
                            }
                            break;
                        }
                    }
                   
                    if (_CanTurn)
                    {
                        if (ComboCount == 0) //コンボカウントが0回の時のみ、敵からの攻撃カウントを減らす
                        {                          
                            _enemyConroll.GetEnemyAttackCount++;
                        }
                        if (piecetype != Piece_Type.EMPTY) //ピースが空じゃない場合、コンボ数をプラスする
                        {
                            ComboCount++;                  
                        }                        
                        if (piecetype == Piece_Type.WHITE) 
                        {
                            //ハート型ウイルスの場合、敵の制限時間を5秒延ばす処理を行う
                            _enemyConroll.GetEnemyTimeLimit += 5;                  
                            _UiManager.InitTextRecoveryDisplay(UIrectTransform);
                            _UiManager.DestroyRecoveryDisplay();
                        }                                          
                        
                        //敵への攻撃処理
                        _enemyConroll.AttackEnemy(piecetype,ComboCount);                          
                        _UiManager.InitTextComboDisplay(UIrectTransform, ComboCount);

                        //コンボチェックをし爆破演出行う
                        Pazuru_Combo_Count(_DestroyPieceList, _DestroyPieceList.Count);                             
                       
                        //破壊するピースが確定するため、破壊処理のフラグをオンする
                        isDestroy = true;

                        yield return new WaitForSeconds(0.2f);
                        //何コンボかを表示する
                        _UiManager.DestroyTextComboDisplay();
                    }
                }

            }

        }
        yield return null;
        
        
        if (isDestroy)
        {           
          yield return DownPackPiece();
        }
        else 
        {
            //コンボカウントが0の場合、3つ揃っているピースがない（＝交換しても3マッチが成立していない）ため、【4】のタップ交換のコルーチンに戻る
            if (ComboCount == 0)
            {
                //交換したピースを元に戻す
                _inputMemoryPiece.GetPieceState = _pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState;
                yield return null;
                //mouseDown ←　mouseUp
                _pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState = _pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState;
                yield return null;
                _pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState = _inputMemoryPiece.GetPieceState;
                yield return TapMousePiece();
            }
            else { yield return TapMousePiece(); }
            
        }
    }

    /// <summary>
    /// [8]残りのピースを下に詰める
    /// </summary>
    /// <returns></returns>
    IEnumerator DownPackPiece()
    {
        Debug.Log("DownPackPiece");
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                //該当ピースから上方向に検索をし続け、ピースが空だったら、交換処理を行う
                if (_pieces[i, k].GetPieceState == Piece_Type.EMPTY)
                {
                    int pazurux = i;
                    int pazuruy = k;
                    
                    while (pazurux >= 0 && pazurux < Rows && pazuruy >= 0 && pazuruy < Cols)
                    {
                        var exchangepazuru = _pieces[pazurux, pazuruy].GetPieceState;
                        if (exchangepazuru != Piece_Type.EMPTY)
                        {
                            _pieces[i, k].GetPieceState = exchangepazuru;
                            _pieces[pazurux, pazuruy].GetPieceState = Piece_Type.EMPTY;
                            break;
                        }
                        pazurux += _Same_Block_Check_Direction_X[0];
                        pazuruy += _Same_Block_Check_Direction_Y[0];
                        yield return new WaitForSeconds(WaitTime);
                    }
                }

            }
        }
        isDestroy = false;
        yield return DestroyPiece();
    }

    /// <summary>
    /// [9]削除フラグが立っているピースを削除する
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyPiece()
    {       
        Debug.Log("DestroyPiece");
        _DestroyPieceList.Clear();
       // DestroyPaticles();
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                if (_pieces[i, k].GetPieceState == Piece_Type.EMPTY)
                {
                    Destroy(_pieceGameObjects[i, k]);
                    yield return new WaitForSeconds(0.01f);
                    _DestroyPieceList.Add(new Blocks(i, k));
                }
            }
        }
        yield return CreateNewPiece();
    }

    /// <summary>
    /// [10]ピースを削除したあと新しくピースを生成する
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateNewPiece()
    {       
        foreach (var i in _DestroyPieceList)
        {
        　　var pos = _InputClass.FallGetPieceWorldPos(new Vector2(i.Getx(), i.Gety()), FallNum,Width);
            CreatePiece(new Vector2(i.Getx(), i.Gety()));

            _pieces[i.Getx(), i.Gety()].GetRectTransForm.position = pos;        
            var pos2 = _InputClass.GetPieceWorldPos(new Vector2(i.Getx(), i.Gety()),Width);
            while (pos2.y <= pos.y)
            {
                pos.y -= FallTime;      
                _pieces[i.Getx(), i.Gety()].GetRectTransForm.position = pos;
                yield return null;
            }
            _pieces[i.Getx(), i.Gety()].GetRectTransForm.position = pos2;
        }
        yield return Is_Enemy_Check();
    }

    /// <summary>
    /// [11]敵のHPを確認し、0になったらEnemyを消す
    /// </summary>
    /// <returns></returns>
    IEnumerator Is_Enemy_Check() 
    {
        //敵のHPがゼロだったら
        if (_enemyConroll.Get_NowHpPoint <= 0)
        {                              
            _enemyConroll.GetEnemyAttackCount = 0;
            _Particles.EnemyDestory(_ParticleTransform);
            StopCoroutine(EnemyCountCol);

            PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyDestroy);
            yield return new WaitForSeconds(2.0f);
            Destroy(_enemyConroll.gameObject);
            yield return new WaitForSeconds(1.0f);
            
            //敵の出現カウントが確認し、EnemyCount以下だったら次の敵を出現させる
            if (_enemyCount < enemy_3)
            {
                _enemyCount++;              
                _Particles.EnemyPop(_ParticleTransform);
                if (_enemyCount == enemy_2)
                {                                    
                    _UiManager.SignalPopup("SECOND 2/3", _SignalTransform);
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyAppearance);                 
                    yield return new WaitForSeconds(2.0f);                
                }
                if (_enemyCount == enemy_3) 
                {                    
                    _UiManager.SignalPopup("LAST 3/3", _SignalTransform);
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyAppearance);
                    yield return new WaitForSeconds(2.0f);
                    
                }
                yield return new WaitForSeconds(1.0f);              
                Enemys[_enemyCount] = _EnemyStatus.SetEnemyStatus(_enemyCount);   
                
                InitEnemys(_enemyCount);
                
                //敵のタイムカウントを開始する
                EnemyCountCol = _enemyConroll.TimeCount(_enemyConroll.GetEnemyTimeLimit);
                StartCoroutine(EnemyCountCol);
                yield return EnemyAttack();
            }//出現カウントを越えたらクリア画面へ
            else { yield return Clear(); }
        }
        else if (_enemyConroll.GetEnemyTimeOver) 
        {
            //秒数カウント以内に倒せなかったらGameOver
            yield return GameOver();
        }
        else　//それ以外なら【12】に進む
        {
            yield return EnemyAttack();
        }
    }

    /// <summary>
    /// [12]敵カウントが0になったら敵からの攻撃処理を行う
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyAttack()
    {      
        if (_enemyConroll.GetHindrancePiece == _enemyConroll.GetEnemyAttackCount)
        {            
            _enemyAttack.EnemyAttackType((Enemy_Attack_Type)_enemyConroll.GetAttackEnemyType, _pieces);
            _Particles.EnemyAll(_ParticleTransform);
            yield return new WaitForSeconds(3.0f);     
            _enemyConroll.GetEnemyAttackCount = 0;
        }
        //【6】に戻る
        yield return ObstaclePieceCheck();
    }

    /// <summary>
    /// [13]クリア画面
    /// </summary>
    /// <returns></returns>
    IEnumerator Clear()
    {
        Debug.Log("Clear");
        _UiManager.IsbestTimeCount();
        _UiManager.SignalPopup("CLEAR", _SignalTransform);
        if (StageNum >= ClearNum)
        {
           var num = StageNum + 1;
           PlayerPrefs.SetInt(SaveData_Manager.KEY_CLEAR_NUM, num);
        }
        yield return new WaitForSeconds(3.0f);
        _UiManager.GameClear();
        while (true) { yield return null; }
      
    }

    /// <summary>
    /// [14]ゲームオーバー画面
    /// </summary>
    /// <returns></returns>
    IEnumerator GameOver()
    {       
        Debug.Log("GameOver");
        _UiManager.SignalPopup("GAMEOVER", _SignalTransform);
        yield return new WaitForSeconds(3.0f);
        _UiManager.GameOver();        
        while (true) { yield return null; }
       
    }

    /// <summary>
    /// ピースが何個マッチしているかチェックし、4コンボの場合はPiece_TypeをFEVER4に、5コンボの場合FEVER5にする
    /// </summary>
    /// <param name="destroylist">マッチしているピースのリスト</param>
    /// <param name="listcount">リストの数</param>
    private void Pazuru_Combo_Count(List<Blocks> destroylist, int listcount)
    {      
        Debug.Log("Pazuru_Combo_Count");                      
        if (listcount == _piece_Match_5)
        {
            foreach (var i in destroylist)
            {
                 
                if (i == destroylist.First())
                {
                    _pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.FEVER_5;
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                    _LimitManager.LimitPlus();
                    _Particles.PlayerAttackToEnemy(_ParticleTransform);
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
                }
                else
                {
                    _pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.EMPTY;
                    _Particles.PlayerAttackToEnemy(_ParticleTransform);
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                }
                _LimitManager.LimitPlus();
            }
           
        }
        else if (listcount == _piece_Match_4)
        {
            foreach (var i in destroylist)
            {
                if (i == destroylist.First())
                {
                    _pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.FEVER_4;
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                   
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
                }
                else
                {
                    _pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.EMPTY;
                    _Particles.PlayerAttackToEnemy(_ParticleTransform);
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                }
                _LimitManager.LimitPlus();
                
            }
        }
        else if (listcount == _piece_Match_3)
        {
            foreach (var i in destroylist)
            {
                if (i == destroylist.First())
                {
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                    _Particles.PlayerAttackToEnemy(_ParticleTransform);
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.PuzzleDestroy);
                }
                _pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.EMPTY;
                _Particles.PlayerAttackToEnemy(_ParticleTransform);
                DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                _LimitManager.LimitPlus();              
            }
        }
       
    }

    /// <summary>
    /// タップして交換したピースにFEVER4、5があった場合は、削除と爆破アニメーションを再生する
    /// </summary>
    /// <param name="pazuru1">交換した2つのピース</param>
    /// <returns></returns>
    private bool Fever_Exchange_Check(params Piece[] pazuru1)
    {
        bool isConbo = false;
        for (var i = 0; i < pazuru1.Length; i++)
        {
            var p = pazuru1[i].GetComponent<Piece>();
            switch (_pieces[(int)p.GetPieceX, (int)p.GetPieceY].GetPieceState) 
            {
                case Piece_Type.FEVER_4:
                    DestroyCheak((int)p.GetPieceX, (int)p.GetPieceY, _Same_Block_Check_Direction_X, _Same_Block_Check_Direction_Y);
                    isConbo = true;
                    break;
                case Piece_Type.FEVER_5:
                    DestroyCheak((int)p.GetPieceX, (int)p.GetPieceY, _Same_5_Block_Cheack_Direction_X, _Same_5_Block_Cheack_Direction_Y);
                    isConbo = true;
                    break;
                default:
                    break;
            }
        }
        return isConbo;
    }

    /// <summary>
    /// FEVERピースを交換した場合、交換した4or8方向に対して削除フラグと爆破アニメーションを行う
    /// </summary>
    /// <param name="i">piece配列のx</param>
    /// <param name="k">piece配列のy</param>
    /// <param name="arrayx">爆破するxの座標</param>
    /// <param name="arrayy">爆破するyの座標</param>
    private void DestroyCheak(int i, int k, int[] arrayx, int[] arrayy)
    {
        for (var p = 0; p < arrayx.Length; p++)
        {
            int px = i + arrayx[p];
            int py = k + arrayy[p];
            if (!(px >= 0 && px < Rows && py >= 0 && py < Cols)) continue;
            _pieces[px, py].GetPieceState = Piece_Type.EMPTY;
        }
        DestroyPiecePaticles(new Vector2(i, k));
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.PuzzleDestroy);
        _pieces[i, k].GetPieceState = Piece_Type.EMPTY;
    }

    /// <summary>
    /// ピースの再生成
    /// </summary>
    /// <param name="pos">座標位置</param>
    private void CreatePiece(Vector2 pos)
    {
        //上から高速に落下させるため、FallNum分上にピースを表示させる
        var criatePos = _InputClass.GetPieceWorldPos(pos, Width);
        
        //ピースを生成する
        _pieceGameObjects[(int)pos.x, (int)pos.y] = Instantiate(_PazuruPrefab, criatePos, Quaternion.identity);
        var piece = _pieceGameObjects[(int)pos.x, (int)pos.y].GetComponent<Piece>();

        piece.GetPieceX = (int)pos.x;
        piece.GetPieceY = (int)pos.y;
        piece.transform.SetParent(_PieceTrans);
        piece.SetSize(Width); 
           
        //ピース種類の乱数
        var randomNum = Random.Range((int)Piece_Type.RED, (int)Piece_Type.BLACK);
        piece.GetPieceState = (Piece_Type)randomNum;
        piece.name = "piece" + (int)pos.x + " " + (int)pos.y;
        _pieces[(int)pos.x, (int)pos.y] = piece;
        _InputClass.GetRaycastResults.Clear();
    }

    /// <summary>
    /// ピースの破壊パーティクル
    /// </summary>
    /// <param name="pos"></param>
    private void DestroyPiecePaticles(Vector2 pos)
    {      
        var criatePos = _InputClass.GetPieceWorldPos(pos, Width);
        _Particles.PlayerDestroyParticles(criatePos, _ParticleTransform);
    }
   
    /// <summary>
    /// パズルボードにピースを生成する
    /// </summary>
    private void InitBoard()
    {      
        Debug.Log(Width);                
        //交換するピースの情報を保存するインスタンス
        var obj = Instantiate(_PazuruPrefab, new Vector3(10, 10, 10), Quaternion.identity);
        _inputMemoryPiece = obj.GetComponent<Piece>();        
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                CreatePiece(new Vector2(i, k));
            }
        }
    }

    /// <summary>
    /// UIの初期化
    /// </summary>
    private void InitUI() 
    {
        _LimitManager.InitLimitObject(_LimitTransform);
        _mouseDown = _pieces[0, 0];
        _mouseUp = _pieces[6, 6];

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
    }

    /// <summary>
    /// 敵の初期化処理
    /// </summary>
    private void InitEnemys(int num)
    {
        //敵生成クラスを生成する
        _EnemyFactroy = Instantiate(_EnemyFactroy);
        
        //入力データと何体目の敵なのかを判定し、該当の敵の構造体を受け渡す
        Enemys[_enemyCount] = _EnemyStatus.SetEnemyStatus(_enemyCount);

        //受け渡した構造体を敵クラスに代入する
        _enemyConroll = _EnemyFactroy.EnemyInstance(Enemys[num]);        
        _enemyConroll.transform.SetParent(_EnemyTransform);

        Instantiate(_EnemyStatus);             
        EnemyCountCol =  _enemyConroll.TimeCount(_enemyConroll.GetEnemyTimeLimit);



    }

    /// <summary>
    /// ピースのサイズを元に戻す
    /// </summary>
    private void Piece_Size_Back() 
    {
        for (var i = 0; i < Rows; i++) 
        {
            for (var k = 0; k < Cols; k++) 
            {
                _pieces[i, k].SetSize(Width);            
            }        
        }    
    }
}

