using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームマネージャークラス・UI系のもの分割させてすっきりさせる（2021/03/28）
/// </summary>
/// 
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Canvas _Canvas;
    [SerializeField]
    private WarningPopup warningPopup;

    //ピースの生成・コントロール
    [SerializeField]
    private RectTransform _PieceTrans;
    [SerializeField]
    private GameObject _PazuruPrefab;
    private Piece[,] _Pieces = new Piece[Cols, Rows];
    private GameObject[,] _GameObjects = new GameObject[Cols, Rows];

   //座標クラス
    [SerializeField]
    private VectorReturn _InputClass;

    //パーティクルの生成
    [SerializeField]
    private RectTransform _ParticleTransform;     
    [SerializeField]
    private Particle _Particles;

    //Enemyの生成  
    [SerializeField]
    private EnemyFactory _EnemyFactroy;
    [SerializeField]
    private RectTransform _EnemyTransform;      
    private BaseEnemy EnemyConroll;
    [SerializeField]
    private EnemyAttack _enemyAttack;

    //攻撃クラス
    [SerializeField]
    private SpecialPieceAttack _specialPieceAttack;
    [SerializeField]
    private GameObject _WarningPopup;

    //リミットボタン
    [SerializeField]
    private RectTransform _LimitTransform; 
    [SerializeField]
    private LimitManager _LimitManager;

    //UI
    [SerializeField]
    private RectTransform UIrectTransform;
    [SerializeField]
    private TextDisplay _ComboTextDisplay;

    [SerializeField]
    private TextDisplay _RecoveryText;
    
    //コンボテキスト表示
    private TextDisplay _ComboTextObject;
    private TextDisplay _RecoveryTextObject;
   
    //スタート・クリア・第一層~3層までの合図アニメーション
    [SerializeField]
    private Signal _Signal;
    [SerializeField]
    private RectTransform _SignalTransform;

    //コルーチンの待機時間
    [SerializeField]
    private float WaitTime;
    //補充ピースが落ちる速度
    [SerializeField]
    private float FallTime;

    //行・列
    private const int Cols = 7;
    private const int Rows = 7;
    private int FallNum = 9;

    //画面比率サイズ
    private int Width;

    //タップしたピースの保存変数
    private Piece _mouseDown;
    private Piece _mouseUp;
    private Piece inputMemoryPiece;

    //上下左右チェック用
    private int[] Same_Block_Check_Direction_X = new int[] { 0, 1, 0, -1 };
    private int[] Same_Block_Check_Direction_Y = new int[] { 1, 0, -1, 0 };

    //原点から8方向チェック用
    private int[] Same_5_Block_Cheack_Direction_X = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 };
    private int[] Same_5_Block_Cheack_Direction_Y = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };

    //削除するピースの座標位置を保存する
    private List<Blocks> _DestroyPieceList = new List<Blocks>();
    private int ComboCount;
   
    //コルーチン引数
    private IEnumerator EnemyCountCol;

    //削除するピーズがあるかのフラグ
    private bool isDestroy = false;

    //敵の出現回数
    private int EnemyCount = 0;

    //敵のステータス
    [SerializeField]
    private Enemy_Status _EnemyStatus;
    private EnemyStatus[] Enemys = new EnemyStatus[3];
    
    //ステージナンバー
    private int StageNum;
    //クリアナンバー更新用
    private int ClearNum;
    private bool Combo5 = false;

    //タイムカウント
    [SerializeField]
    BestTimeCount bestTimeCount;    
    [SerializeField]
    SceneChangeManager _sceneChange;

    [SerializeField]
    private UIManager _UiManager;

    //リミットゲージの値
    private int LimitMax = 50;

    //スペシャルアタックの攻撃
    private int SpecialAttack1;
    private int SpecialAttack2;


    private void Awake()
    {
       Screen.SetResolution(432, 768, false, 60);
    }

    void Start()
    {
        Common_Sound_Manager.Instance.Sound_Play(BGM.Stop);
        ClearNum = PlayerPrefs.GetInt("STAGECLEARNUM", 0);
        StageNum = PlayerPrefs.GetInt("STAGENUM", 0);
        Debug.Log("StageNum"+StageNum);
        Debug.Log("ClearNum" + ClearNum);

        SpecialAttack1 = PlayerPrefs.GetInt("ATTACK1", 0);
        SpecialAttack2 = PlayerPrefs.GetInt("ATTACK2", 0);

        InitScreenSize();
       InitBoard();
        //InitLimitGauge();
        _LimitManager.InitLimitObject(_LimitTransform);

       _EnemyFactroy = Instantiate(_EnemyFactroy);
        Enemys[EnemyCount] = _EnemyStatus.SetEnemyStatus(StageNum, EnemyCount);
       InitEnemys(EnemyCount);
       Instantiate(_EnemyStatus);
             
       EnemyCountCol =  EnemyConroll.TimeCount(EnemyConroll.GetEnemyTimeLimit);
        StartCoroutine(EnemyCountCol);
        StartCoroutine(ArrartAnim(3.0f));
      
        Debug.Log("GetHindrancePiece" + EnemyConroll.GetHindrancePiece);
    }

    /// <summary>
    /// アラート音の調整
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public IEnumerator ArrartAnim(float a)
    {
        yield return new WaitForSeconds(1.0f);
        var obj = Instantiate(_WarningPopup);      
        obj.transform.position = new Vector3(0, 0, 0);
        obj.transform.SetParent(_SignalTransform);
        var arart = _WarningPopup.GetComponent<WarningPopup>();        
        var pos1 = new Vector2(0, 0);
        var pos2 = new Vector2(Screen.width / 3, Screen.height / 2);
        var pos3 = new Vector2(Screen.width / 2, Screen.height / 3);
        var pos4 = new Vector2(Screen.width / 4, Screen.height / 5);
        var pos5 = new Vector2(Screen.width / 2, -Screen.height / 2);
        var pos6 = new Vector2(-Screen.width / 4, -Screen.height / 3);
        var pos7 = new Vector2(-Screen.width / 5, -Screen.height / 4f);
        arart.PopupStartArart( _ParticleTransform, pos1, pos2, pos3, pos4,pos5, pos6, pos7);
       PuzzleSoundManager.Instance.SE_Selection(SE_Now.Alert);
        yield return new WaitForSeconds(3.0f);            
        arart.Popup_Close(arart.Popup);
        yield return StartSignal();           
    }

    /// <summary>
    /// スタート
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartSignal()
    {
        var obj =  _Signal.SignalObject("FIRST 1/3",_SignalTransform);
        yield return new WaitForSeconds(2.0f);
        Destroy(obj);
        yield return TapMousePiece();
    }

/// <summary>
    /// タップした座標位置（Down,Up）を保存する
    /// </summary>
    /// <returns></returns>
    public IEnumerator TapMousePiece()
    {
        Debug.Log("TapMousePiece");

        while (true)
        {
            //for (var p = 0; p < 2; p++) 
            //{
            //    if (_LimitButtonControll[p].GetisButtonPress)
            //    {
            //        _Particles.SpecialAttack_Tate(_ParticleTransform);                  
            //        if (_LimitButtonControll[0].GetisButtonPress) _specialPieceAttack.SpecialAttack((E_SpecialAttack)SpecialAttack1, _Pieces);
            //        if (_LimitButtonControll[1].GetisButtonPress) _specialPieceAttack.SpecialAttack((E_SpecialAttack)SpecialAttack2, _Pieces);

            //        _HPLimitGauge.GetHp = 0;
            //        _ButtonControll[p].interactable = false;
            //        _LimitButtonControll[p].GetisButtonPress = false;
            //        isDestroy = true;
            //        yield return ObstaclePieceCheck();
            //    }
            //}
            // _LimitManager.GetisButtonPress

            //リミットボタンの発動チェック
            if (_LimitManager.GetisButtonPress)
            {
                _specialPieceAttack.SpecialAttack((E_SpecialAttack)SpecialAttack1, _Pieces);
                isDestroy = true;
                _LimitManager.GetisButtonPress = false;
                yield return null;
                _LimitManager.LimitInitValues();
                yield return ObstaclePieceCheck();
            }

            //ピースのタップ検知
            if (Input.GetMouseButtonDown(0))//押した瞬間のピースの情報を所得
            {
                _mouseDown = _InputClass.ReturnRaycastPiece(_Pieces[0, 0]);
                _Pieces[_mouseDown.GetPieceX, _mouseDown.GetPieceY]._GetRectTransForm.sizeDelta = new Vector2(1.5f, 1.5f) * Width;
                Debug.Log("MDX" + _mouseDown.GetPieceX + "MDY" + _mouseDown.GetPieceY);
            }
            else if (Input.GetMouseButtonUp(0)) //離したときのピースの情報を所得
            {
                _mouseUp = _InputClass.ReturnRaycastPiece(_Pieces[6, 6]);
                _Pieces[_mouseUp.GetPieceX, _mouseUp.GetPieceY]._GetRectTransForm.sizeDelta = new Vector2(1.5f, 1.5f) * Width;

                var xa = (int)_mouseUp.GetPieceX - _mouseDown.GetPieceX;
                var ya = (int)_mouseUp.GetPieceY - _mouseDown.GetPieceY;
                Vector2 poscheak = new Vector2(xa, ya);
                Debug.Log(" poscheak" + poscheak);

                //yield return ExchangePiece();

                if (poscheak == new Vector2(1, 0) || poscheak == new Vector2(0, 1) || poscheak == new Vector2(-1, 0) || poscheak == new Vector2(0, -1))
                {
                    // Func = false;
                    yield return ExchangePiece();
                }
                else
                {
                    _Pieces[_mouseDown.GetPieceX, _mouseDown.GetPieceY]._GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
                    _Pieces[_mouseUp.GetPieceX, _mouseUp.GetPieceY]._GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
                    //break;
                }
                Debug.Log("MUX" + _mouseUp.GetPieceX + "MUY" + _mouseUp.GetPieceY);
            }
            yield return null;

            //スキル技 ・デバッグ用      
            if (Input.GetKeyDown(KeyCode.A)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_Red, _Pieces);      
            if (Input.GetKeyDown(KeyCode.B)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_Blue, _Pieces);          
            if (Input.GetKeyDown(KeyCode.C)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_Green, _Pieces);          
            if (Input.GetKeyDown(KeyCode.D)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_White, _Pieces);          
            if (Input.GetKeyDown(KeyCode.E)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_Yellow, _Pieces);          
            if (Input.GetKeyDown(KeyCode.F)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_HorizontalOneArray, _Pieces);           
            if (Input.GetKeyDown(KeyCode.G)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_VerticalOneArray, _Pieces);          
            if (Input.GetKeyDown(KeyCode.H)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_Destroy_Cross, _Pieces);           
            if (Input.GetKeyDown(KeyCode.I)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_ObliqueCross_Cross, _Pieces);          
            if (Input.GetKeyDown(KeyCode.Q)) _specialPieceAttack.SpecialAttack(E_SpecialAttack.SP_BlackPiece_Destroy, _Pieces);
            if (Input.GetKeyDown(KeyCode.Z)) EnemyConroll.Get_NowHpPoint = 0;
            if (Input.GetKeyDown(KeyCode.U)) EnemyConroll.GetEnemyTimeLimit = 0 ;

            if (Input.GetKeyDown(KeyCode.T)) 
            {
              _LimitManager.GetisButtonPress = true;
            }




        }
    }

    /// <summary>
    /// ピースを交換する
    /// </summary>
    /// <returns></returns>
    IEnumerator ExchangePiece()
    {
        Debug.Log("Exchange Block");
        ComboCount = 0;

        yield return null;       
        _Pieces[_mouseDown.GetPieceX, _mouseDown.GetPieceY]._GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
        yield return null;
        _Pieces[_mouseUp.GetPieceX, _mouseUp.GetPieceY]._GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;       
        
        //mouseDownの座標を格納
        inputMemoryPiece.GetPieceState = _Pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState;
        yield return null;       
        
        //mouseDown ←　mouseUp
        _Pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState = _Pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState;           
        yield return null;
        _Pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState = inputMemoryPiece.GetPieceState;
        yield return null;        
        
        //FEVER4 or FEVER5のピースを交換した場合は、FEVERチェックを通す      
        if (Fever_Exchange_Check(_mouseDown, _mouseUp)) {
          
            EnemyConroll.GetEnemyAttackCount++;
            yield return DownPackPiece();
        }        
        yield return ObstaclePieceCheck();

    }

    /// <summary>
    /// じゃまPieceが一番下列にある場合、爆発処理をする
    /// </summary>
    /// <returns></returns>
    IEnumerator ObstaclePieceCheck() 
    {
        
        for (var i = 0; i < Rows; i++) 
        {                           
            if (_Pieces[i, 0].GetPieceState == Piece_Type.BLACK) 
            {
                PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyPieceDestroy);
                _Pieces[i, 0].GetPieceState = Piece_Type.EMPTY;
                isDestroy = true;
                DestroyPiecePaticles(new Vector2(i, 0));
            }           
        }
        yield return DestroyPieceCheak();
    }

    /// <summary>
    ///全ピースの状態を4方向に確認。３つ以上揃っていたらリストに入れてEMPTYフラグ（= Destroyフラグ）を立てる
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyPieceCheak()
    {
        Piece_Size_Back();
        Debug.Log("DestroyPieceCheak");     
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {              
                for (var p = 0; p < Same_Block_Check_Direction_X.Length; p++)
                {
                    bool _CanTurn = false;
                    _DestroyPieceList.Clear();
                    Piece_Type piecetype = _Pieces[i, k].GetPieceState;
                    _DestroyPieceList.Add(new Blocks(i, k, _Pieces[i, k].GetPieceState));

                    int pazurux = i;
                    int pazuruy = k;

                    while (true)
                    {
                        pazurux += Same_Block_Check_Direction_X[p];
                        pazuruy += Same_Block_Check_Direction_Y[p];

                        if (!(pazurux >= 0 && pazurux < Rows && pazuruy >= 0 && pazuruy < Cols)) break;

                        if (_Pieces[pazurux, pazuruy].GetPieceState == piecetype && _Pieces[pazurux, pazuruy].GetPieceState != Piece_Type.BLACK)
                        {
                            _DestroyPieceList.Add(new Blocks(pazurux, pazuruy, _Pieces[pazuruy, pazurux].GetPieceState));
                            int pazuruxplus = pazurux + Same_Block_Check_Direction_X[p];
                            int pazuruyplus = pazuruy + Same_Block_Check_Direction_Y[p];
                            
                           if (_DestroyPieceList.Count >= 3 && !(pazuruxplus >= 0 && pazuruxplus < Rows && pazuruyplus >= 0 && pazuruyplus < Cols))                          
                           {
                                _CanTurn = true;
                                break;
                           }
                            else { continue; }
                        }
                        else
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
                        if (ComboCount == 0)
                        {                          
                            EnemyConroll.GetEnemyAttackCount++;
                        }
                        if (piecetype != Piece_Type.EMPTY) 
                        {
                            ComboCount++;
                            Debug.Log("ComboCount" + ComboCount);                        
                        }                        
                        if (piecetype == Piece_Type.WHITE) 
                        { 
                            EnemyConroll.GetEnemyTimeLimit += 5;
                            InitTextRecoveryDisplay();                            
                            Destroy(_RecoveryTextObject.gameObject,0.5f);
                        }                       
                        //if (piecetype != Piece_Type.EMPTY)
                        //{
                          EnemyConroll.AttackEnemy(ComboCount);
                          InitTextComboDisplay();
                          Pazuru_Combo_Count(_DestroyPieceList, _DestroyPieceList.Count);
                            
                       // }                        
                        Debug.Log("_DestroyPieceList.Count" + _DestroyPieceList.Count);
                        isDestroy = true;
                        yield return new WaitForSeconds(0.2f);
                        Destroy(_ComboTextObject.gameObject);                        
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
            //_Pieces[_mouseDown.GetPieceX, _mouseDown.GetPieceY]._GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
            //_Pieces[_mouseUp.GetPieceX, _mouseUp.GetPieceY]._GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;

            if (ComboCount == 0)
            {
                inputMemoryPiece.GetPieceState = _Pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState;
                yield return null;
                //mouseDown ←　mouseUp
                _Pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState = _Pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState;
                yield return null;
                _Pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState = inputMemoryPiece.GetPieceState;
               
            }
            yield return TapMousePiece();
        }
    }

    /// <summary>
    /// 残りのピースを下に詰める
    /// </summary>
    /// <returns></returns>
    IEnumerator DownPackPiece()
    {
        Debug.Log("DownPackPiece");
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                if (_Pieces[i, k].GetPieceState == Piece_Type.EMPTY)
                {
                    int pazurux = i;
                    int pazuruy = k;
                    while (pazurux >= 0 && pazurux < Rows && pazuruy >= 0 && pazuruy < Cols)
                    {
                        var testpazuru = _Pieces[pazurux, pazuruy].GetPieceState;
                        if (testpazuru != Piece_Type.EMPTY)
                        {
                            _Pieces[i, k].GetPieceState = testpazuru;
                            //yield return new WaitForSeconds(WaitTime);
                            _Pieces[pazurux, pazuruy].GetPieceState = Piece_Type.EMPTY;
                            break;
                        }
                        pazurux += Same_Block_Check_Direction_X[0];
                        pazuruy += Same_Block_Check_Direction_Y[0];
                        yield return new WaitForSeconds(WaitTime);
                    }
                }

            }
        }
        isDestroy = false;
        yield return DestroyPiece();
    }

    /// <summary>
    /// 削除フラグが立っているピースを削除する
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
                if (_Pieces[i, k].GetPieceState == Piece_Type.EMPTY)
                {
                    Destroy(_GameObjects[i, k]);
                    yield return new WaitForSeconds(0.01f);
                    _DestroyPieceList.Add(new Blocks(i, k));
                }
            }
        }
        yield return CreateNewPiece();
    }

    /// <summary>
    /// ピースを削除したあと新しくピースを生成する
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateNewPiece()
    {       
        foreach (var i in _DestroyPieceList)
        {
        　　var pos = _InputClass.FallGetPieceWorldPos(new Vector2(i.Getx(), i.Gety()), FallNum,Width);
            CreatePiece(new Vector2(i.Getx(), i.Gety()));

            _Pieces[i.Getx(), i.Gety()]._GetRectTransForm.position = pos;        
            var pos2 = _InputClass.GetPieceWorldPos(new Vector2(i.Getx(), i.Gety()),Width);
            while (pos2.y <= pos.y)
            {
                pos.y -= FallTime;      
                _Pieces[i.Getx(), i.Gety()]._GetRectTransForm.position = pos;
                yield return null;
            }
            _Pieces[i.Getx(), i.Gety()]._GetRectTransForm.position = pos2;
        }
        yield return Is_Enemy_Check();
    }

    /// <summary>
    /// 敵のHPを確認し、0になったらEnemyを消す
    /// </summary>
    /// <returns></returns>
    IEnumerator Is_Enemy_Check() 
    {
        //敵のHPがゼロだったら
        if (EnemyConroll.Get_NowHpPoint <= 0)
        {                   
           
            EnemyConroll.GetEnemyAttackCount = 0;
            _Particles.EnemyDestory(_ParticleTransform);
            StopCoroutine(EnemyCountCol);
            PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyDestroy);
            yield return new WaitForSeconds(2f);
            Destroy(EnemyConroll.gameObject);
            yield return new WaitForSeconds(1f);
            //敵の出現カウントが確認し、EnemyCount以下だったら次の敵を出現させる
            if (EnemyCount < 2)
            {
                EnemyCount++;              
                _Particles.EnemyPop(_ParticleTransform);
                if (EnemyCount == 1)
                {                  
                    var obj = _Signal.SignalObject("SECOND 2/3", _SignalTransform);
                   PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyAppearance);                 
                    yield return new WaitForSeconds(2.0f);
                    Destroy(obj);
                    //_HPLimitGauge.GetHp++;
                    

                }
                if (EnemyCount == 2) 
                {                    
                    var obj = _Signal.SignalObject("LAST 3/3", _SignalTransform);
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyAppearance);
                    yield return new WaitForSeconds(2.0f);
                    Destroy(obj);
                    //_HPLimitGauge.GetHp++;
                    
                }
                yield return new WaitForSeconds(1.0f);              
                Enemys[EnemyCount] = _EnemyStatus.SetEnemyStatus(StageNum, EnemyCount);   
                
                InitEnemys(EnemyCount);
                
                //敵のタイムカウントを開始する
                EnemyCountCol = EnemyConroll.TimeCount(EnemyConroll.GetEnemyTimeLimit);
                StartCoroutine(EnemyCountCol);

                //_HPLimitGauge.GetHp++;
                yield return EnemyAttack();
            }//出現カウントを越えたらクリア画面へ
            else { yield return Clear(); }
        }
        else if (EnemyConroll.GetEnemyTimeOver) 
        {
            //秒数カウント以内に倒せなかったらGameOver
            yield return GameOver();
        }
        else　//それ以外なら次にいく
        {
            yield return EnemyAttack();
        }
    }

    /// <summary>
    /// x回のサイクル目に敵がお邪魔ピースを輩出する
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyAttack()
    {      
        Debug.Log("EnemyAttackCount"+ EnemyConroll.GetEnemyAttackCount);
        if (EnemyConroll.GetHindrancePiece == EnemyConroll.GetEnemyAttackCount)
        {            
            _enemyAttack.EnemyAttackType((Enemy_Attack_Type)EnemyConroll.GetAttackEnemyType, _Pieces);
            _Particles.EnemyAll(_ParticleTransform);
            yield return new WaitForSeconds(3.0f);     
            EnemyConroll.GetEnemyAttackCount = 0;
        }

        yield return ObstaclePieceCheck();
    }

    /// <summary>
    /// クリア画面
    /// </summary>
    /// <returns></returns>
    IEnumerator Clear()
    {
        Debug.Log("Clear");        
        bestTimeCount.GetTimeStop = true;
        var obj = _Signal.SignalObject("CLEAR", _SignalTransform);
        if (StageNum >= ClearNum)
        {
           var num = StageNum + 1;
           PlayerPrefs.SetInt(SaveData_Manager.KEY_CLEAR_NUM, num);
        }
        yield return new WaitForSeconds(3.0f);
        _UiManager.GameClear();

        while (true) 
        {
            yield return null;
        }
       
    }

    /// <summary>
    /// ゲームオーバー画面
    /// </summary>
    /// <returns></returns>
    IEnumerator GameOver()
    {       
        //_PopupManager.ClearPopup();
        Debug.Log("GameOver");
        var obj = _Signal.SignalObject("GAMEOVER", _SignalTransform);
        yield return new WaitForSeconds(3.0f);
        _UiManager.GameOver();
        while (true)
        {
            yield return null;
        }

    }

    /// <summary>
    /// ピースが何個マッチしているかチェックし、4コンボの場合はPiece_TypeをFEVER4に、5コンボの場合FEVER5にする
    /// </summary>
    /// <param name="destroylist">マッチしているピースのリスト</param>
    /// <param name="listcount">リストの数</param>
    private void Pazuru_Combo_Count(List<Blocks> destroylist, int listcount)
    {      
        Debug.Log("Pazuru_Combo_Count");              
        
        if (listcount == 5)
        {
            Combo5 = true;
            foreach (var i in destroylist)
            {
                //ComboCount++;
                if (i == destroylist.First())
                {
                    _Pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.FEVER_5;
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));

                    //LimitPlus();
                    _LimitManager.LimitPlus();
                    _Particles.PlayerAttackToEnemy(_ParticleTransform);
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
                }
                else
                {
                    _Pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.EMPTY;
                    _Particles.PlayerAttackToEnemy(_ParticleTransform);
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                }
                //LimitPlus();
                _LimitManager.LimitPlus();
            }
           
        }
        else if (listcount == 4 && !Combo5)
        {
            foreach (var i in destroylist)
            {
                //ComboCount++;
                if (i == destroylist.First())
                {
                    _Pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.FEVER_4;
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                   
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
                }
                else
                {
                    _Pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.EMPTY;
                    _Particles.PlayerAttackToEnemy(_ParticleTransform);
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                }
                //LimitPlus();
                _LimitManager.LimitPlus();
                Combo5 = false;
            }
        }
        else if (listcount == 3)
        {
            //ComboCount++;
            foreach (var i in destroylist)
            {
                if (i == destroylist.First())
                {
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                    _Particles.PlayerAttackToEnemy(_ParticleTransform);
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.PuzzleDestroy);
                }
                _Pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.EMPTY;
                _Particles.PlayerAttackToEnemy(_ParticleTransform);
                DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                _LimitManager.LimitPlus();
                //LimitPlus();                
            }
        }
        Combo5 = false;
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
            switch (_Pieces[(int)p.GetPieceX, (int)p.GetPieceY].GetPieceState) 
            {
                case Piece_Type.FEVER_4:
                    DestroyCheak((int)p.GetPieceX, (int)p.GetPieceY, Same_Block_Check_Direction_X, Same_Block_Check_Direction_Y);
                    isConbo = true;
                    break;
                case Piece_Type.FEVER_5:
                    DestroyCheak((int)p.GetPieceX, (int)p.GetPieceY, Same_5_Block_Cheack_Direction_X, Same_5_Block_Cheack_Direction_Y);
                    isConbo = true;
                    break;
                default:
                   // isConbo = false;
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
            _Pieces[px, py].GetPieceState = Piece_Type.EMPTY;
        }
        DestroyPiecePaticles(new Vector2(i, k));
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.PuzzleDestroy);
        _Pieces[i, k].GetPieceState = Piece_Type.EMPTY;
    }

    /// <summary>
    /// ピースの生成
    /// </summary>
    /// <param name="pos">座標位置</param>
    private void CreatePiece(Vector2 pos)
    {
        //↑から落下させるため、FallNum分上に表示させる
        var criatePos = _InputClass.GetPieceWorldPos(pos, Width);      
        _GameObjects[(int)pos.x, (int)pos.y] = Instantiate(_PazuruPrefab, criatePos, Quaternion.identity);
        var piece = _GameObjects[(int)pos.x, (int)pos.y].GetComponent<Piece>();
        piece.GetPieceX = (int)pos.x;
        piece.GetPieceY = (int)pos.y;
        piece.transform.SetParent(_PieceTrans);
        piece.SetSize(Width); 
      
        
        var randomNum = Random.Range((int)Piece_Type.RED, (int)Piece_Type.BLACK);
        piece.GetPieceState = (Piece_Type)randomNum;
        piece.name = "piece" + (int)pos.x + " " + (int)pos.y;
        _Pieces[(int)pos.x, (int)pos.y] = piece;
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
        //_Particles.EnemyParticles(criatePos, _ParticleTransform);
    }
   
    /// <summary>
    /// 敵の攻撃
    /// </summary>
    /// <param name="PieceCount"></param>
    private void Enemy_Damage_Piece(int PieceCount) 
    {
        for (var i = 0; i < PieceCount; i++) 
        {
            var x = Random.Range(0, Cols);
            var y = Random.Range(0, Rows);
            DestroyPiecePaticles(new Vector2(x, y));
            _Pieces[x, y].GetPieceState = Piece_Type.BLACK;
        }   
    }

    /// <summary>
    /// パズルボードにピースを生成する
    /// </summary>
    public void InitBoard()
    {      
        Debug.Log(Width);                
        //交換するピースの情報を保存するインスタンス
        var obj = Instantiate(_PazuruPrefab, new Vector3(10, 10, 10), Quaternion.identity);
        inputMemoryPiece = obj.GetComponent<Piece>();
        
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                CreatePiece(new Vector2(i, k));
            }
        }
    }

    /// <summary>
    ///端末サイズによってピースの比率を変更する
    /// </summary>
    private void InitScreenSize()
    {
        Width = (Screen.width / Cols);
        var w = (float)Screen.width;
        var h = (float)Screen.height;
        
        Camera.main.transform.position = new Vector3(w, h, -10);
        Camera.main.orthographicSize = w;
        var canvas = _Canvas.transform.position;      
        var z = canvas.z;

        canvas = new Vector3(w, h, z);
        _Canvas.transform.position = canvas;
        var canvas2 = _Canvas.GetComponent<RectTransform>();
        canvas2.sizeDelta = new Vector2(w, h);
    }

    /// <summary>
    /// 敵1の初期化処理
    /// </summary>
    private void InitEnemys(int num)
    {       
        EnemyConroll = _EnemyFactroy.EnemyInstance(num, Enemys[num]);        
        EnemyConroll.transform.SetParent(_EnemyTransform);
       // EnemyConroll.transform.localPosition = new Vector3(0, 300, 0);                     
        Debug.Log("ENemy1" + Enemys[num].GetenemyHp);       
    }

   /// <summary>
   /// リミットゲージの初期化
   /// </summary>
    //private void InitLimitGauge() 
    //{
    //    LimitGaugeControll = Instantiate(_LimitGauge);
    //    LimitGaugeControll.transform.localPosition = new Vector3(Screen.width  - Screen.width/4, Screen.height + Screen.height / 8, 0);
    //    LimitGaugeControll.transform.SetParent(_LimitTransform);      
    //    _HPLimitGauge = LimitGaugeControll.GetComponent<HP>();
    //    _HPLimitGauge.InitSetsize(0.4f, 0.1f);
    //    _HPLimitGauge.InitSetHp(100, 0);

    //    for (var i = 0; i < 2; i++) 
    //    {
    //        var obj = Instantiate(_LimitButtonPrefab);
    //        _ButtonControll[i] = obj.GetComponent<Button>();
    //        _ButtonControll[i].interactable = false;
    //        _LimitButtonControll[i] = obj.GetComponent<LimitButton>();
    //        if (i == 0)
    //        {
    //           _LimitButtonControll[i].GetComponent<LimitButton>().GetSpecialAttackType = (E_SpecialAttack)SpecialAttack1;
    //            obj.transform.localPosition = new Vector3(Screen.width + Screen.width / 7, Screen.height + Screen.height / 7, 0);
    //        }
    //        else if (i == 1) 
    //        {
    //            _LimitButtonControll[i].GetComponent<LimitButton>().GetSpecialAttackType = (E_SpecialAttack)SpecialAttack2;
    //            obj.transform.localPosition = new Vector3(Screen.width + Screen.width / 3, Screen.height + Screen.height / 7, 0);
    //        }                       
    //        obj.transform.SetParent(_LimitTransform);
    //    }      
    //}

    private void InitTextComboDisplay()
    {
        _ComboTextObject = Instantiate(_ComboTextDisplay);
        _ComboTextObject.transform.SetParent(UIrectTransform);
        _ComboTextObject.transform.localPosition = new Vector3(100, 100);
        _ComboTextObject.transform.localRotation = Quaternion.Euler(0, 0, 20);
        _ComboTextObject.GetCount = ComboCount;
        _ComboTextObject.GetTextDisplayString = "Combo!!";
        _ComboTextObject.UI_Text_Display();
    }

    private void InitTextRecoveryDisplay() 
    {
        _RecoveryTextObject = Instantiate(_RecoveryText);
        _RecoveryTextObject.transform.SetParent(UIrectTransform);
        _RecoveryTextObject.GetTextDisplayString = "+5";        
        _RecoveryTextObject.transform.localPosition = new Vector3(105, 260,0);
        _RecoveryTextObject.transform.localScale = new Vector3(1, 1, 1);
        _RecoveryTextObject.UI_Text_Display_Recovery();

    }

  
    //private void LimitPlus() 
    //{
    //    _HPLimitGauge.GetHp++;
    //    if (_HPLimitGauge.GetHp == LimitMax) 
    //    {
    //       Debug.Log(" 1000ninatta" + _HPLimitGauge.GetHp);
    //       _ButtonControll[0].interactable = true;
    //       _ButtonControll[1].interactable = true;
    //        PuzzleSoundManager.Instance.SE_Selection(SE_Now.LimitMax);           
    //    }
    //}

    //public void LimitAttack() 
    //{      
        
    //    _LimitButtonControll[0].GetisButtonPress = true;
    //    _LimitButtonControll[1].GetisButtonPress = true;
    //}

    private void Piece_Size_Back() 
    {
        for (var i = 0; i < Rows; i++) 
        {
            for (var k = 0; k < Cols; k++) 
            {
                _Pieces[i, k].SetSize(Width);            
            }        
        }    
    }


}

