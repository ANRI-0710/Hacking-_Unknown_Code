using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
///パズルピースの盤面及びピースの処理を管理するクラス 
/// </summary>
public class PieceManager : MonoBehaviour
{
    //パズルピースの生成   
    [SerializeField]
    private GameObject _PazuruPrefab;

    //ピースオブジェクトへのアクセス（ピース破壊する際Destroy指示の際に使用）
    private GameObject[,] _pieceGameObjects = new GameObject[Cols, Rows];

    //ピースクラスへのアクセス
    private Piece[,] _pieces = new Piece[Cols, Rows];
    public Piece[,] GetPieces { get => _pieces; set { _pieces = value; } }

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

    //画面比率サイズ
    private int Width;

    //座標変換クラス（インスタエイトの際にUI座標からワールド座標に変換する必要があるため）
    [SerializeField]
    private VectorReturn _InputClass;

    //ピースの表示位置の固定
    [SerializeField]
    private RectTransform _PieceTrans;

    //パーティクルの生成
    [SerializeField]
    private RectTransform _ParticleTransform;

    public void Awake()
    {
        //画面サイズを受け取る  
        Width = (Screen.width / Cols);
    }

    /// <summary>
    /// パズルボードにピースを生成する
    /// </summary>
    /// 
    public void InitBoard()
    {       
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
        _mouseDown = _pieces[0, 0];
        _mouseUp = _pieces[6, 6];
    }


    /// <summary>
    /// ピースの再生成
    /// </summary>
    /// <param name="pos">座標位置</param>
    public void CreatePiece(Vector2 pos)
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
        piece.name = "piecetest" + (int)pos.x + " " + (int)pos.y;
        _pieces[(int)pos.x, (int)pos.y] = piece;
        _InputClass.GetRaycastResults.Clear();
    }

    /// <summary>
    /// ピースが十字方向に交換されたか、リミットボタンが押されたかをループでチェックする
    /// </summary>
    /// <returns></returns>
    public IEnumerator PieceTapCheckLoop() 
    {
        while (true) 
        {
            ////リミットボタンの発動チェック
            for (var i = 0; i < 2; i++)
            {
                if (GameManager.Instance.GetLimitManager[i])
                {
                    if (i == 0) {GameManager.Instance.GetSpecialAttack.SpecialAttack((E_SpecialAttack)GameManager.Instance[0], _pieces); }
                    if (i == 1) { GameManager.Instance.GetSpecialAttack.SpecialAttack((E_SpecialAttack)GameManager.Instance[1], _pieces); }
                    GameManager.Instance.GetLimitManager[i] = false;
                    GameManager.Instance.GetLimitInvocating = true;
                    yield return null;
                    GameManager.Instance.GetLimitManager.LimitInitValues();
                    yield break;
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
                   GameManager.Instance.GetexchangeSatisfied = true;
                    yield break;
                }
                else
                {
                    //そうれなければタップしたピースの大きさを元に戻す
                    _pieces[_mouseDown.GetPieceX, _mouseDown.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
                    _pieces[_mouseUp.GetPieceX, _mouseUp.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
                }
            }
            yield return null;
        }

    }

    /// <summary>
    ///  ピースを交換する
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExchangePiece()
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
        if (Fever_Exchange_Check(_mouseDown, _mouseUp))
        {
            GameManager.Instance.GetFeverCheck = true;
            //敵からの攻撃までのカウントを更新
            GameManager.Instance.GetEnemyManager.GetenemyConroll.GetEnemyAttackCount++;
            yield break;        
        }

    }

    /// <summary>
    /// アンチウイルスパズル（じゃまPiece）が一番下列にある場合、爆発処理をする
    /// </summary>
    /// <returns></returns>
    public IEnumerator ObstaclePieceCheck()
    {
        for (var i = 0; i < Rows; i++)
        {
            if (_pieces[i, 0].GetPieceState == Piece_Type.BLACK)
            {
                PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyPieceDestroy);
                _pieces[i, 0].GetPieceState = Piece_Type.EMPTY;
                GameManager.Instance.GetisDestroy = true;
            }
            if (_pieces[i, 0].GetPieceState == Piece_Type.EMPTY) { GameManager.Instance.GetisDestroy = true; }
        }
        yield return null;
    }

    /// <summary>
    ///全ピースの状態を4方向に確認。３つ以上揃っていたらリストに入れてEMPTYフラグ（= Destroyフラグ）を立てる
    /// </summary>
    /// <returns></returns>
    public IEnumerator DestroyPieceCheak()
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
                        if (_pieces[pazurux, pazuruy].GetPieceState == piecetype && _pieces[pazurux, pazuruy].GetPieceState != Piece_Type.BLACK )
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
                        yield return PieceChangeAction(piecetype);
                    }
                }

            }

        }
        yield return null;
        yield return ExchangeCancelPiece();
        yield break;
    }

    /// <summary>
    /// 残りのピースを下に詰める
    /// </summary>
    /// <returns></returns>
    public IEnumerator DownPackPiece()
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
        yield break;
    }

    /// <summary>
    /// 削除フラグが立っているピースを削除する
    /// </summary>
    /// <returns></returns>
    public IEnumerator DestroyPiece()
    {
        Debug.Log("DestroyPiece");
        _DestroyPieceList.Clear();
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
    }

    /// <summary>
    /// ピースを削除したあと新しくピースを生成する
    /// </summary>
    /// <returns></returns>
    public IEnumerator CreateNewPiece()
    {
        Debug.Log("CreateNewPiece()");
        foreach (var i in _DestroyPieceList)
        {
            var pos = _InputClass.FallGetPieceWorldPos(new Vector2(i.Getx(), i.Gety()), FallNum, Width);
            CreatePiece(new Vector2(i.Getx(), i.Gety()));

            _pieces[i.Getx(), i.Gety()].GetRectTransForm.position = pos;
            var pos2 = _InputClass.GetPieceWorldPos(new Vector2(i.Getx(), i.Gety()), Width);
            while (pos2.y <= pos.y)
            {
                pos.y -= FallTime;
                _pieces[i.Getx(), i.Gety()].GetRectTransForm.position = pos;
                yield return null;
            }
            _pieces[i.Getx(), i.Gety()].GetRectTransForm.position = pos2;
        }
    }

    /// <summary>
    /// タップして交換したピースにFEVER4、5があった場合は、削除と爆破アニメーションを再生する
    /// </summary>
    /// <param name="pazuru1">交換した2つのピース</param>
    /// <returns></returns>
    public bool Fever_Exchange_Check(params Piece[] pazuru1)
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
    /// ピースが何個マッチしているかチェックし、4コンボの場合はPiece_TypeをFEVER4に、5コンボの場合FEVER5にする
    /// </summary>
    /// <param name="destroylist">マッチしているピースのリスト</param>
    /// <param name="listcount">リストの数</param>
    private void Pazuru_Combo_Count(List<Blocks> destroylist, int listcount)
    {
        Debug.Log("Pazuru_Combo_Count");
        var piecetype = FeverComboPiece(listcount);       
        foreach (var i in destroylist)
        {
            if (i == destroylist.First()) 
            { 
                _pieces[i.Getx(), i.Gety()].GetPieceState = piecetype;
                if (piecetype != Piece_Type.EMPTY) { PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor); }
                else { PuzzleSoundManager.Instance.SE_Selection(SE_Now.PuzzleDestroy); }                
            }
            else { _pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.EMPTY; }
            DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
            GameManager.Instance.GetParticle.PlayerAttackToEnemy(_ParticleTransform);
            
            GameManager.Instance.GetLimitManager.LimitPlus();
        }
    }

    /// <summary>
    /// リストカウントに応じてたピースタイプを返す
    /// </summary>
    /// <param name="listcount"></param>
    /// <returns></returns>
    private Piece_Type FeverComboPiece(int listcount) 
    {
        Piece_Type piece_Type;       
        switch (listcount) 
        {
            case _piece_Match_5:
                piece_Type = Piece_Type.FEVER_5;
                return piece_Type;          
            case _piece_Match_4:
                piece_Type = Piece_Type.FEVER_4;
                return piece_Type;
            case _piece_Match_3:
                piece_Type = Piece_Type.EMPTY;
                return piece_Type;
            default:
                piece_Type = Piece_Type.EMPTY;
                return piece_Type;
        }
    }

    /// <summary>
    /// コンボカウントが0の場合、3つ揃っているピースがない（＝交換しても3マッチが成立していない）ため、タップ交換のコルーチンに戻る
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExchangeCancelPiece() 
    {
        if (!GameManager.Instance.GetisDestroy)
        {     
            if (ComboCount == 0)
            {
                //交換したピースを元に戻す
                _inputMemoryPiece.GetPieceState = _pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState;
                yield return null;
                //mouseDown ←　mouseUp
                _pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState = _pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState;
                yield return null;
                _pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState = _inputMemoryPiece.GetPieceState;
            }
        }
        yield break;
    }

    /// <summary>
    /// ピースが3つ揃っている場合、揃ったピースの攻撃カウント/コンボ表示/爆破演出を行う
    /// </summary>
    /// <param name="piecetype"></param>
    /// <returns></returns>
    private IEnumerator PieceChangeAction(Piece_Type piecetype)
    {
        //コンボカウントが0回の時のみ、敵からの攻撃カウントを減らす
        if (ComboCount == 0) { GameManager.Instance.GetEnemyManager.GetenemyConroll.GetEnemyAttackCount++; }
        //ピースが空じゃない場合、コンボ数をプラスするs
        if (piecetype != Piece_Type.EMPTY) { ComboCount++; }
        //ハート型ウイルスの場合、敵の制限時間を5秒延ばす処理を行う
        if (piecetype == Piece_Type.WHITE)
        {
            GameManager.Instance.GetEnemyManager.GetenemyConroll.GetEnemyTimeLimit += 5;
            GameManager.Instance.GetUIManager.InitTextRecoveryDisplay(GameManager.Instance.GetUIManager.GetrectTransform);
            yield return new WaitForSeconds(0.4f);
            GameManager.Instance.GetUIManager.DestroyRecoveryDisplay();
        }

        //敵への攻撃処理
        GameManager.Instance.GetEnemyManager.GetenemyConroll.AttackEnemy(piecetype, ComboCount);

        //コンボチェックをし爆破演出行う
        Pazuru_Combo_Count(_DestroyPieceList, _DestroyPieceList.Count);

        //破壊するピースが確定するため、破壊処理のフラグをオンする
        GameManager.Instance.GetisDestroy = true;
        yield return new WaitForSeconds(0.2f);

        //何コンボかを表示する
        GameManager.Instance.GetUIManager.InitTextComboDisplay(GameManager.Instance.GetUIManager.GetrectTransform, ComboCount);
        yield return null;
        GameManager.Instance.GetUIManager.DestroyTextComboDisplay();
        yield break;       

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
    /// ピースの破壊パーティクル
    /// </summary>
    /// <param name="pos"></param>
    private void DestroyPiecePaticles(Vector2 pos)
    {
        var criatePos = _InputClass.GetPieceWorldPos(pos, Width);
       GameManager.Instance.GetParticle.PlayerDestroyParticles(criatePos, _ParticleTransform);
    }

    /// <summary>
    /// ピースのサイズを元に戻す
    /// </summary>
    private void Piece_Size_Back()
    {
        for (var i = 0; i < Rows; i++)
        {
            for (var k = 0; k < Cols; k++) { _pieces[i, k].SetSize(Width); }            
        }
    }
}
