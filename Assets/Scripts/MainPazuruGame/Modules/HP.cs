using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HPvarクラス
/// </summary>

public class HP : MonoBehaviour
{   
    [SerializeField]
    public Slider _HpGauge;

    [SerializeField]
    private GameObject _EnemyTypeImage;

    [SerializeField]
    private Sprite[] enemyType;

    [SerializeField]
    private RectTransform _EnemyTransform;
    private RectTransform rectTransform;
       
    //HPゲージのプロパティ
    private int Hp;
    public int GetHp
    {
        get => Hp;
        set
        {
            Hp = value;
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
    
    public void Decrease_Hp() 
    {
        _HpGauge.value = Hp;
    }

    public void InitSetHp(int maxHp, int nowHp)
    {        
        //MaxHPの最大値
        _HpGauge.maxValue = maxHp;
        //スライダーの現在地の設定
        _HpGauge.value = nowHp;
    }

    public bool LimitMax() 
    {
        var limiton = false;       
        if (_HpGauge.maxValue == _HpGauge.value) 
        {
            limiton = true;        
        }
        return limiton;    
    }

    public void InitSetsize(float Reductionwidth,float Reductionheight) 
    {       
        var sizewidth = (int)((int)Screen.width * Reductionwidth);
        var sizeheight = (int)((int)Screen.height * Reductionheight);
        rectTransform.sizeDelta = new Vector2(sizewidth, sizeheight);
    }

    public void EnemyTypeImageSet() 
    {
        //enemyImageObject = _EnemyTypeImage.GetComponent<Image>();
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
