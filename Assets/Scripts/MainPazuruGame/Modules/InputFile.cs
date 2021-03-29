using UnityEngine;

/// <summary>
/// 敵のステータス・メッセンジャー風会話をテキストファイルから読み込むクラス
/// </summary>

public class InputFile : MonoBehaviour
{
    public string[] textMessage;    //テキストの加工前の一行を入れる       
    private string[,] textWords; //複数列入れる
    public string[] tempWords;
    private string[] textWordstruct;

    public string[,] GetStrings 
    {
        get => textWords;        
    }

    private int Rows;   //行
    private int Cols;    //列   
    //行・列の所得
    public int GetInputRows { get => Rows; }
    public int GetInputCols { get => Cols; }
   
    public string[,] str0 = new string[1,4];
    public string[,] str1 = new string[1, 4];
    public string[,] str2 = new string[1, 4];
   
    public string Text_File_Get(int num)
    {
        var str = "Stage"+ num;
        return str;
    }

    public void Input_File(string str)
    {
        //var enemystaus = new EnemyStatus();      
        TextAsset textAsset = new TextAsset();
        textAsset = Resources.Load(str, typeof(TextAsset)) as TextAsset;
        string TextLines = textAsset.text;

        //Splitで一行ずつ代入した一次元配列を作成
        textMessage = TextLines.Split('\n');
        Debug.Log("TextMe" + textMessage[0]);

        //行数と列数の所得
        Cols = textMessage[0].Split('\t').Length;
        Rows = textMessage.Length;
        //2次元配列を定義
        textWords = new string[Rows, Cols];
        
        for (var i = 0; i < Rows; i++)
        {
            //textMasseageをカンマごとに分けたものを一時的にtempWordsに代入
           
           string[] tempWords = textMessage[i].Split('\t');
            
            for (var k = 0; k < Cols; k++)
            {
                textWords[i, k] = tempWords[k];
            }
        }
    }
   
}
