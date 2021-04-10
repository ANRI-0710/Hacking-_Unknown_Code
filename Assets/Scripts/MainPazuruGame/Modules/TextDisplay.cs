using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// コンボ表示や敵の攻撃までのカウントをテキスト表示するクラス
/// </summary>
public class TextDisplay : MonoBehaviour
{     
    //テキスト表示
    private string _text_Display_String;
    public string GetTextDisplayString { get => _text_Display_String; set { _text_Display_String = value; } }

    //テキスト更新
    private Text _enemyTextObject;

    //カウント表示
    private int _count;
    public int GetCount { get => _count; set { _count = value; } }        
    private RectTransform _rectTransform;

    //敵の攻撃までのカウント数
    private int _enemyCount;
    public int GetEnemyCount 
    {
       get => _enemyCount;
       set 
       {            
         _enemyCount = value;
         UI_Text_EnemyAttack_Play();
       }          
    }

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();        
        _enemyTextObject = this.GetComponent<Text>();
    }

    /// <summary>
    /// コンボカウント表示
    /// </summary>
    public void UI_Text_Display()
    {       
        _enemyTextObject.text = _count.ToString() + _text_Display_String;
    }

    /// <summary>
    /// 5秒回復表示
    /// </summary>
    public void UI_Text_Display_Recovery()
    {       
        _enemyTextObject.text = _text_Display_String;
    }

    /// <summary>
    /// 敵の攻撃までのカウント表示
    /// </summary>
    public void UI_Text_EnemyAttack_Play() 
    {       
        _enemyTextObject.text = _enemyCount.ToString();
    }

    /// <summary>
    /// 表示の大きさ調整
    /// </summary>
    /// <param name="Reductionwidth"></param>
    /// <param name="Reductionheight"></param>
    public void InitSetsize(float Reductionwidth, float Reductionheight)
    {
        var sizewidth = (int)((int)Screen.width * Reductionwidth);
        var sizeheight = (int)((int)Screen.height * Reductionheight);
        _rectTransform.sizeDelta = new Vector2(sizewidth, sizeheight);
    }

}
