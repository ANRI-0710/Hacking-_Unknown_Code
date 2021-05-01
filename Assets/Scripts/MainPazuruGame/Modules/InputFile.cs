using UnityEngine;


public enum Column //列番号
{
    Col_0,
    Col_1,
    Col_2,
    Col_3,
    Col_4,
    Col_5
}

public enum Row //行番号
{
    Row_0,
    Row_1,
    Row_2,
    Row_3,
    Row_4,
    Row_5
}

<<<<<<< HEAD
=======

>>>>>>> f061488f36f51569aa254191514fb7a1a159ec79
/// <summary>
/// テキストファイルから敵のステータス・メッセンジャー風会話を読み込むクラス
/// </summary>
public class InputFile : MonoBehaviour
{
    //テキストの加工前の一行を入れる・インデクサーに変更予定
    public string[] textMessage;

    //複数列入れる
    private string[,] _textWords;
    public string[,] GetStrings
    {
        get => _textWords;        
    }

    //行・列の所得
    private int Rows;
    private int Cols;  
    public int GetInputRows { get => Rows; }
    public int GetInputCols { get => Cols; }
   
    /// <summary>
    /// ステージ番号を受け取って、ステージ番号のファイル名を所得する
    /// </summary>
    /// <param name="num">ステージナンバー</param>
    /// <returns></returns>
    public string Text_File_Get(int num)
    {
        var str = "Stage"+ num;
        return str;
    }

    /// <summary>
    /// 該当ファイルを読み込んでtextWordsにデータを入れる
    /// </summary>
    /// <param name="str"></param>
    public void Input_File(string str)
    {   
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
        _textWords = new string[Rows, Cols];
        
        for (var i = 0; i < Rows; i++)
        {
            //textMasseageをカンマごとに分けたものを一時的にtempWordsに代入           
           　string[] tempWords = textMessage[i].Split('\t');
            
            for (var k = 0; k < Cols; k++)
            {
                _textWords[i, k] = tempWords[k];
            }
        }
    }
   
}
