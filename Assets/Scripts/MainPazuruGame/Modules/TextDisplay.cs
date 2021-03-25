using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コンボ表示や敵の攻撃までのカウントをテキスト表示するクラス
/// </summary>
public class TextDisplay : MonoBehaviour
{     
    //テキスト表示
    private string _Text_Display_String;
    public string GetTextDisplayString { get => _Text_Display_String; set { _Text_Display_String = value; } }

    //カウント表示
    private int Count;
    public int GetCount { get => Count; set { Count = value; } }        
    private RectTransform rectTransform;

    //オブジェクト
    private Text _EnemyTextObject;

    //Enemyカウント
    private int EnemyCount;
    public int GetEnemyCount 
    {
       get => EnemyCount;
       set 
       {            
         EnemyCount = value;
         UI_Text_EnemyAttack_Play();
       }          
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();        
        _EnemyTextObject = this.GetComponent<Text>();
    }

    public void UI_Text_Display()
    {       
        _EnemyTextObject.text = Count.ToString() + _Text_Display_String;
    }

    public void UI_Text_Display_Recovery()
    {       
        _EnemyTextObject.text = _Text_Display_String;
    }

    public void UI_Text_EnemyAttack_Play() 
    {       
        _EnemyTextObject.text = EnemyCount.ToString();
    }

    public void InitSetsize(float Reductionwidth, float Reductionheight)
    {
        var sizewidth = (int)((int)Screen.width * Reductionwidth);
        var sizeheight = (int)((int)Screen.height * Reductionheight);
        rectTransform.sizeDelta = new Vector2(sizewidth, sizeheight);
    }

}
