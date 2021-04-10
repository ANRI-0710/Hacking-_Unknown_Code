using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HPvarクラス
/// </summary>
public class HP : MonoBehaviour
{   
   //HPゲージ
    [SerializeField]
    public Slider _HpGauge;

    //敵の属性値
    [SerializeField]
    private GameObject _EnemyTypeImage;
    
    //敵の属性
    [SerializeField]
    private Sprite[] enemyType;

    //座標
    [SerializeField]
    private RectTransform _EnemyTransform;
    private RectTransform rectTransform;
       
    //HPゲージのプロパティ
    private int _hp;
    public int GetHp
    {
        get => _hp;
        set
        {
            _hp = value;
            Decrease_Hp();
        }
    }
    
    //敵の属性プロパティ
    private Piece_Type enemyPiecetype;
    public Piece_Type GetenemyType
    {
        get => enemyPiecetype;
        set
        {             
          enemyPiecetype = value;
          EnemyTypeImageSet();
        }
    }

    void Awake()
    {                          
        _HpGauge = this.GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();        
    }
    
    /// <summary>
    /// HP数値に反映する
    /// </summary>
    public void Decrease_Hp() 
    {
        _HpGauge.value = _hp;
    }

    /// <summary>
    /// HPゲージの初期化
    /// </summary>
    /// <param name="maxHp"></param>
    /// <param name="nowHp"></param>
    public void InitSetHp(int maxHp, int nowHp)
    {        
        //MaxHPの最大値
        _HpGauge.maxValue = maxHp;
        //スライダーの現在地の設定
        _HpGauge.value = nowHp;
    }

    /// <summary>
    /// HPバーの初期化
    /// </summary>
    /// <param name="Reductionwidth"></param>
    /// <param name="Reductionheight"></param>
    public void InitSetsize(float Reductionwidth,float Reductionheight) 
    {       
        var sizewidth = (int)((int)Screen.width * Reductionwidth);
        var sizeheight = (int)((int)Screen.height * Reductionheight);
        rectTransform.sizeDelta = new Vector2(sizewidth, sizeheight);
    }

    /// <summary>
    /// 敵の属性のイメージを代入する
    /// </summary>
    public void EnemyTypeImageSet() 
    {
        var image = _EnemyTypeImage.GetComponent<Image>();
        switch (enemyPiecetype) 
        {
            case Piece_Type.RED:
                image.sprite = enemyType[(int)Piece_Type.RED];
                break;
            case Piece_Type.BLUE:
                image.sprite = enemyType[(int)Piece_Type.BLUE];
                break;
            case Piece_Type.GREEN:
                image.sprite = enemyType[(int)Piece_Type.GREEN];
                break;
            case Piece_Type.YELLOW:
                image.sprite = enemyType[(int)Piece_Type.YELLOW];
                break;           
        }   
    }

}
