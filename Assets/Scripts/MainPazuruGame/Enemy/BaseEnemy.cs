using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵クラス
/// </summary>

public class BaseEnemy : Attack
{
    [SerializeField]
    private HP _HP;

    [SerializeField]
    private TImeLimit _TimeLimit;
   
    [SerializeField]
    private Sprite[] _Sprite;

    [SerializeField]
    GameObject _EnemyImage;

    [SerializeField]
    private int _MaxHpPoint;

    [SerializeField]
    private int _NowHpPoint;
    public int  Get_NowHpPoint
    {
        get => _NowHpPoint;
        set { _NowHpPoint = value; }    
    }

    [SerializeField]
    private RectTransform _EnemyTransform;

    [SerializeField]
    private GameObject _EnemyAttackCountObject;
    private TextDisplay _textDisplay;
   
    private bool _EnemyTimeOver = false;
    public bool GetEnemyTimeOver { get => _EnemyTimeOver; }
       
    private int EnemyAttackCount;
    public int GetEnemyAttackCount 
    {         
      get => EnemyAttackCount; 
      set 
      {
         EnemyAttackCount = value;
         EnemyAttackCountUpdate();
      }           
    }

    private int AttackEnemyType;
    public int GetAttackEnemyType { get => AttackEnemyType;}

    //敵の属性
    private Piece_Type _Enemy_Type;
    public Piece_Type _Get_Enemy_Type { get => _Enemy_Type; }

    //おじゃまピースの発生回数
    private int HindrancePiece;
    public int GetHindrancePiece
    {         
        get => HindrancePiece;
        set { HindrancePiece = value;}
    }

    //TimeLimit
    private int EnemyTimeLimit;
    public int GetEnemyTimeLimit 
    {
        get => EnemyTimeLimit;
        set 
        {
            EnemyTimeLimit = value;           
        }    
    }
    public int NowTime = 0;
    private HP _Hps;

   /// <summary>
   ///敵の初期化処理
   /// </summary>
    public void EnemyInit(int maxHppoint,int nowHppoint,float setwidth,float sethight,float rocalposition, Piece_Type enemytype, int piececount,int enemyAttackType,int enemytimeLimit) 
    {
        _NowHpPoint = nowHppoint;       
        _Hps = Instantiate(_HP);
        _HP = _Hps.GetComponent<HP>();     
        _HP.InitSetHp(maxHppoint, nowHppoint);
        _HP.InitSetsize(setwidth, sethight);
        _HP.GetenemyType = enemytype;

        EnemyTimeLimit = enemytimeLimit;       
        _Enemy_Type = enemytype;
        HindrancePiece = piececount;
        AttackEnemyType = enemyAttackType;


        _Hps.transform.SetParent(_EnemyTransform);        
        var t = _HP.GetComponent<RectTransform>();
        t.transform.localPosition = new Vector3(Screen.width, Screen.height+ Screen.height/3.5f, 0);

        _TimeLimit = Instantiate(_TimeLimit);
        _TimeLimit.transform.SetParent(_EnemyTransform);
        _TimeLimit.transform.localPosition = new Vector3(Screen.width, Screen.height+ Screen.height / 1.9f, 0);
        _TimeLimit.InitSetsize(0.2f, 0.05f);

        var obj = Instantiate(_EnemyAttackCountObject);
        obj.transform.SetParent(_EnemyTransform);
        obj.transform.localPosition = new Vector3(Screen.width + Screen.width/ 2.8f, Screen.height + Screen.height/3.5f, 0);

        _textDisplay = obj.GetComponent<TextDisplay>();
       
        _textDisplay.GetCount = EnemyAttackCount;

        GetEnemyAttackCount = EnemyAttackCount;

        _textDisplay.UI_Text_EnemyAttack_Play();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Combo"></param>
    public void AttackEnemy(int Combo) 
    {
        PieceAttack(_Enemy_Type, this, Combo);
    }

    public void EnemyDamage(int Damage)
    {
        DamageHP(Damage);
    }

    private void EnemyAttackCountUpdate()
    {
        var count = HindrancePiece - EnemyAttackCount;
        _textDisplay.GetEnemyCount = count;
    }

    /// <summary>
    /// 画像の所得
    /// </summary>
    /// <param name="sprite"></param>
    public void EnemyInitImage(Sprite sprite)
    {
        var obj = Instantiate(_EnemyImage);
        var imagechange = obj.GetComponent<Image>();
        imagechange.sprite = sprite;
        obj.transform.SetParent(_EnemyTransform);
        obj.transform.localPosition = new Vector3(Screen.width, Screen.height+ Screen.height/2.5f, 0);
        var sizewidth = (int)((int)Screen.width * 0.3);
        var sizeheight = (int)((int)Screen.height * 0.3);
        var objrect = obj.GetComponent<RectTransform>();
        objrect.sizeDelta = new Vector2(sizewidth, sizeheight);

    }

    /// <summary>
    ///ダメージ処理 
    /// </summary>
    /// <param name="hp"></param>
    public void DamageHP(int hp)
    {                
        _NowHpPoint -= hp;
        _HP.GetHp = _NowHpPoint;        
    }

    /// <summary>
    /// 敵のHPがゼロになったかの判定
    /// </summary>
    /// <returns></returns>
    public bool EnemyDefeat()
    {
        var enemy = false;
        if (_NowHpPoint <= 0) { enemy = true; }
        return enemy;
    }

    /// <summary>
    /// 制限時間
    /// </summary>
    /// <param name="count"></param>
    /// <param name="countdown"></param>
    /// <returns></returns>
    public IEnumerator TimeCount(int countdown)
    {        
        EnemyTimeLimit = countdown;       
        for (var i = 0; i < EnemyTimeLimit; i++)
        {                                                       
            NowTime = EnemyTimeLimit - i -1;

            yield return new WaitForSeconds(1f);
            ReturnCountDown();           
            Debug.Log(EnemyTimeLimit + "EnemyTimeLimit");
            

        }
        Debug.Log("終了");
        _EnemyTimeOver = true;
    }


    public void ReturnCountDown() 
    {
        _TimeLimit.GetTimeLimitint = NowTime;
    }

}
