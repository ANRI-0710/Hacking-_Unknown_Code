﻿using UnityEngine;

/// <summary>
/// テキストファイルから敵のデータを格納するための構造体
/// </summary>
public struct EnemyStatus 
{    
    private int enemyHp;  //敵のHP
    private Piece_Type enemyPiece_Type; //敵の属性
    private int enemyImageNum;  //敵の画像番号
    private int hindrance_Piece;  //敵が何ターン目で攻撃してくるかの数
    private int EnemyAttackType;   //敵の攻撃パターン
    private int EnemyTimeLimit; //敵の撃破しなければ制限時間

    //プロパティ
    public int GetenemyHp { get => enemyHp; set { enemyHp = value; } }
    public Piece_Type GetPiece_Type { get => enemyPiece_Type; set { enemyPiece_Type = value; } }
    public int GetenemyImageNum { get => enemyImageNum; set { enemyImageNum = value; } }
    public int Gethindrance_Piece { get => hindrance_Piece; set { hindrance_Piece = value; } }
    public int GetEnemyAttackType { get => EnemyAttackType; set { EnemyAttackType = value; } }
    public int GetEnemyTimeLimit { get => EnemyTimeLimit; set { EnemyTimeLimit = value; } }
}

/// <summary>
/// テキストファイルから敵のステータスを格納するクラス
/// </summary>
public class Enemy_Status : MonoBehaviour
{
    [SerializeField]
    private InputFile _inputFile;

    //ステージナンバーの選択(playerprefか所得する)
    private int StageNum;

    //テキストファイルから読み取ったデータを構造体に入れるための変数
    private EnemyStatus[] EnemyInput;

    //テキストファイルからの呼び出し
    private void Start()
    {
        //playerplefから選択したパズルステージを受け取り、テキストデータを受けとる
        StageNum = PlayerPrefs.GetInt("STAGENUM", 0);          
        var str = _inputFile.Text_File_Get(StageNum);
        _inputFile.Input_File(str);

        //受け取ったtテキストファイルの行数分の構造体を生成する
        EnemyInput = new EnemyStatus[_inputFile.GetInputRows];
        
        //受け取ったテキストファイルをそれぞれの構造体の変数に代入する
        for (var i = 0; i < _inputFile.GetInputRows; i++) 
        {
            for (var k = 0; k < _inputFile.GetInputCols; k++)
            {
                if (i != (int)Row.Row_0) 
                {
                    if (k == (int)Column.Col_0) //HP代入
                    {
                        EnemyInput[i].GetenemyHp = int.Parse(_inputFile.GetStrings[i, k]);
                    }

                    if(k == (int)Column.Col_1)//属性値の代入
                    {
                        if (_inputFile.GetStrings[i, k] == "赤") 
                        {
                            EnemyInput[i].GetPiece_Type = Piece_Type.RED;
                        }
                        if (_inputFile.GetStrings[i, k] == "緑")
                        {
                            EnemyInput[i].GetPiece_Type = Piece_Type.GREEN;
                        }
                        if (_inputFile.GetStrings[i, k] == "青")
                        {
                            EnemyInput[i].GetPiece_Type = Piece_Type.BLUE;
                        }
                        if (_inputFile.GetStrings[i, k] == "黄")
                        {
                            EnemyInput[i].GetPiece_Type = Piece_Type.YELLOW;
                        }
                    }
                    if(k == (int)Column.Col_2) //敵の画像番号
                    {
                        EnemyInput[i].GetenemyImageNum = int.Parse(_inputFile.GetStrings[i, k]);

                    }
                    if (k == (int)Column.Col_3) //敵の攻撃までのカウント
                    {
                        EnemyInput[i].Gethindrance_Piece = int.Parse(_inputFile.GetStrings[i, k]);
                    }

                    if (k == (int)Column.Col_4) //敵の攻撃パターン番号
                    {
                        EnemyInput[i].GetEnemyAttackType = int.Parse(_inputFile.GetStrings[i, k]);

                    }
                    if(k == (int)Column.Col_5)//制限時間
                    {
                        EnemyInput[i].GetEnemyTimeLimit = int.Parse(_inputFile.GetStrings[i, k]);

                    }
                }
            }        
       
        }

    }

   /// <summary>
   /// 各ステージ3体の敵の何体目の敵なのかを引数にし、数値に応じた構造体を返す
   /// </summary>
   /// <param name="num"></param>
   /// <returns></returns>
    public EnemyStatus SetEnemyStatus(int num)
    {
        //敵の出現番号
        const int enemy1 = 0;
        const int enemy2 = 1;
        const int enemy3 = 2;

        switch (num) 
        {
            case enemy1:
                var result = EnemyInput[(int)Row.Row_1];
                return result;
            case enemy2:
                result = EnemyInput[(int)Row.Row_2];
                return result;
            case enemy3:
                result = EnemyInput[(int)Row.Row_3];
                return result;
            default:
                result = EnemyInput[(int)Row.Row_1];
                return result;
        }
    }
}












