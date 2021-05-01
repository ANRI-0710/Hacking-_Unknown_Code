using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// パズルパートの敵のイメージ・HP・時間制限・攻撃カウント・攻撃パターンを管理するクラス
/// </summary>

public class BaseEnemy : Attack
{
  
    //---------------------------------------------------------------------------------------- 
    //敵のHP関連
    //---------------------------------------------------------------------------------------- 

    //敵のHPのプレハブ
    [SerializeField]
    private HP _HP;
       
    //現在の敵のHP
    private int _nowHpPoint;
    public int Get_NowHpPoint
    {
        get => _nowHpPoint;
        set { _nowHpPoint = value; }
    }

    //----------------------------------------------------------------------------------------
    //敵のタイムリミット関連
    //----------------------------------------------------------------------------------------

    //敵の時間制限管理
    [SerializeField]
    private TImeLimit _TimeLimit;
    //残り時間の管理
    private int _enemyTimeLimit;
    public int GetEnemyTimeLimit
    {
        get => _enemyTimeLimit;
        set
        {
            _enemyTimeLimit = value;
        }
    }
    //制限時間内か、制限越えたかの判定フラグ
    private bool _EnemyTimeOver = false;
    public bool GetEnemyTimeOver { get => _EnemyTimeOver; }
    //制限時間（ EnemyTimeLimit - 経過時間を代入し、残り時間を表示クラスに渡す）
    private int enemyTimeLimitNowTime = 0;

    //----------------------------------------------------------------------------------------
    //敵のイメージ関連
    //----------------------------------------------------------------------------------------

    [SerializeField]
    private GameObject _EnemyImage;

    //----------------------------------------------------------------------------------------
    //敵の攻撃カウント関連
    //----------------------------------------------------------------------------------------

    ///敵がするまでの攻撃カウント変数（テキストデータから情報を取ってくる）
    private int _enemyHindrancePieceCount;
    public int GetHindrancePiece
    {
        get => _enemyHindrancePieceCount;
        set { _enemyHindrancePieceCount = value; }
    }
    
    //敵の現在の攻撃カウント（攻撃までの回数　－　EnemyAttackCountをし、0になったらアンチウイルスパズルを生成する）
    private int _enemyAttackCount;
    public int GetEnemyAttackCount 
    {         
      get => _enemyAttackCount; 
      set 
      {
         _enemyAttackCount = value;
         EnemyAttackCountUpdate();
      }           
    }
    //敵の攻撃カウント管理の表示管理
    [SerializeField]
    private GameObject _EnemyAttackCountObject;
    private TextDisplay _enemyAttackCountText;

    //----------------------------------------------------------------------------------------
    //その他
    //----------------------------------------------------------------------------------------

    //敵の攻撃パターンの種類
    private int _attackEnemyType;
    public int GetAttackEnemyType { get => _attackEnemyType;}

    //敵の属性（赤・青・緑・黄色のいずれか）
    private Piece_Type _enemy_Type;
    public Piece_Type GetEnemyType { get => _enemy_Type; }

    //敵の座標固定（Canvas上生成しているため、GameManagerCanvasのオブジェクトに生成位置を固定し、レイヤー表示順を管理する）
    [SerializeField]
    private RectTransform _EnemyTransform;

    //---------------------------------------------------------------------------------------- 
    //ここから関数
    //----------------------------------------------------------------------------------------

    /// <summary>
    ///敵の初期化処理、構造体EnemyStatusの情報をBaseEnemyクラスに格納する役割
    /// </summary>
    public void EnemyInit(int maxHppoint,int nowHppoint,float setwidth,float sethight, Piece_Type enemytype, int hindrancepiececount,int enemyattacktype,int enemytimeLimit) 
    {                   
        //HPバーの初期化
        var enemyhpvar = Instantiate(_HP);       
        _HP = enemyhpvar.GetComponent<HP>();        
        _nowHpPoint = nowHppoint;
        _HP.InitSetHp(maxHppoint, nowHppoint);
        _HP.InitSetsize(setwidth, sethight);
        _HP.GetenemyType = enemytype;
            
        enemyhpvar.transform.SetParent(_EnemyTransform);
        var enemyhpvartransform = _HP.GetComponent<RectTransform>();
        enemyhpvartransform.transform.localPosition = new Vector3(Screen.width, Screen.height + Screen.height / 3.5f, 0);

        //時間制限の初期化
        _enemyTimeLimit = enemytimeLimit;
        _TimeLimit = Instantiate(_TimeLimit);
        _TimeLimit.transform.SetParent(_EnemyTransform);
        _TimeLimit.transform.localPosition = new Vector3(Screen.width, Screen.height + Screen.height / 1.9f, 0);
        _TimeLimit.InitSetsize(0.2f, 0.05f);
               
        //敵の属性値の初期化
        _enemy_Type = enemytype;

        //敵の攻撃パターンの初期化
        _attackEnemyType = enemyattacktype;

        //敵の攻撃カウントの初期化
        _enemyHindrancePieceCount = hindrancepiececount;
        var enemyattackcountTextobject = Instantiate(_EnemyAttackCountObject);
        enemyattackcountTextobject.transform.SetParent(_EnemyTransform);
        enemyattackcountTextobject.transform.localPosition = new Vector3(Screen.width + Screen.width/ 2.8f, Screen.height + Screen.height/3.5f, 0);
        _enemyAttackCountText = enemyattackcountTextobject.GetComponent<TextDisplay>();      
        _enemyAttackCountText.GetCount = _enemyAttackCount;
        GetEnemyAttackCount = _enemyAttackCount;
        _enemyAttackCountText.UI_Text_EnemyAttack_Play();
    }

    /// <summary>
    /// 3マッチしたピース種類と現在のコンボ数を引数として攻撃処理を行う
    /// </summary>
    /// <param name="Combo">コンボ連鎖数に応じてHPダメージを倍々にする</param>
    public void AttackEnemy(Piece_Type piece_Type, int Combo) { PieceAttack(piece_Type, this, Combo); }

    /// <summary>
    /// PieceAttackでピースの種類を判定したダメージ数値を引数にDamageHPを発動させる
    /// </summary>
    /// <param name="Damage"></param>
    public void EnemyDamage(int Damage) { DamageHP(Damage); }
    
    /// <summary>
    /// PieceAttackでピースの種類を判定したダメージ数値をHPゲージに反映させる
    /// </summary>
    /// <param name="hp">Attackクラスでダメージ計算したあとのダメージ数値</param>
    public void DamageHP(int hp)
    {
        _nowHpPoint -= hp;
        _HP.GetHp = _nowHpPoint;
    }

    /// <summary>
    ///敵からの攻撃カウントの表示更新 
    /// </summary>
    private void EnemyAttackCountUpdate()
    {
        var count = _enemyHindrancePieceCount - _enemyAttackCount;
        _enemyAttackCountText.GetEnemyCount = count;
    }

    /// <summary>
    /// 敵の画像の所得・テキストファイルから敵の画像番号を受け取り、その配列番号の敵画像を表示させる
    /// </summary>
    /// <param name="sprite"></param>
    public void EnemyInitImage(Sprite sprite)
    {
        //画像を変更する
        var enemyimage = Instantiate(_EnemyImage);
        var imagechange = enemyimage.GetComponent<Image>();
        imagechange.sprite = sprite;
        
        //座標位置調整
        enemyimage.transform.SetParent(_EnemyTransform);
        enemyimage.transform.localPosition = new Vector3(Screen.width, Screen.height+ Screen.height/2.5f, 0);
        var sizewidth = (int)((int)Screen.width * 0.3);
        var sizeheight = (int)((int)Screen.height * 0.3);
        var objrect = enemyimage.GetComponent<RectTransform>();
        objrect.sizeDelta = new Vector2(sizewidth, sizeheight);
    }

    /// <summary>
    /// 敵のHPがゼロになったかの判定
    /// </summary>
    /// <returns></returns>
    public bool EnemyDefeat()
    {
        var enemy = false;
        if (_nowHpPoint <= 0) { enemy = true; }
        return enemy;
    }

    /// <summary>
    /// 敵を撃破しなければいけない制限時間のカウント処理。ハート型ウイルスを破壊した場合、_enemyTimeLimitに5秒プラスする。
    /// </summary>
    /// <param name="count"></param>
    /// <param name="countdown"></param>
    /// <returns></returns>
    public IEnumerator TimeCount(int countdown)
    {        
        _enemyTimeLimit = countdown;       
        for (var i = 0; i < _enemyTimeLimit; i++)
        {                                                       
            enemyTimeLimitNowTime = _enemyTimeLimit - i -1;

            yield return new WaitForSeconds(1f);
            ReturnCountDown();           
            Debug.Log(_enemyTimeLimit + "EnemyTimeLimit");
       
        }
        Debug.Log("終了");
        _EnemyTimeOver = true;
    }

    /// <summary>
    /// 制限時間の表示処理
    /// </summary>
    public void ReturnCountDown() 
    {
        _TimeLimit.GetTimeLimitint = enemyTimeLimitNowTime;
    }

}
