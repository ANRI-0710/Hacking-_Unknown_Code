using UnityEngine;

/// <summary>
/// テキストファイルから敵のステータスを格納するクラス
/// </summary>

public struct EnemyStatus 
{    
    private int enemyHp;    
    private Piece_Type enemyPiece_Type;
    private int enemyImageNum;
    private int hindrance_Piece;
    private int EnemyAttackType;
    private int EnemyTimeLimit;

    public int GetenemyHp { get => enemyHp; set { enemyHp = value; } }
    public Piece_Type GetPiece_Type { get => enemyPiece_Type; set { enemyPiece_Type = value; } }
    public int GetenemyImageNum { get => enemyImageNum; set { enemyImageNum = value; } }
    public int Gethindrance_Piece { get => hindrance_Piece; set { hindrance_Piece = value; } }
    public int GetEnemyAttackType { get => EnemyAttackType; set { EnemyAttackType = value; } }
    public int GetEnemyTimeLimit { get => EnemyTimeLimit; set { EnemyTimeLimit = value; } }

    //コンストラクタ
    public EnemyStatus(int enemyHp, Piece_Type enemypiece_Type, int enemyImageNum, int hindrance_piece,int attackenemy,int enemytimelimit)
    {
        this.enemyHp = enemyHp;
        this.enemyPiece_Type = enemypiece_Type;
        this.enemyImageNum = enemyImageNum;
        this.hindrance_Piece = hindrance_piece;
        this.EnemyAttackType = attackenemy;
        this.EnemyTimeLimit = enemytimelimit;
    }
}

public class Enemy_Status : MonoBehaviour
{
    [SerializeField]
    InputFile _inputFile;
       
    //ステージナンバーの選択
    private int StageNum;

    //テキストファイルから読み取ったデータを構造体に入れるための変数
    private EnemyStatus[] EnemyInput;

    //テキストファイルからの呼び出し
    private void Start()
    {
        StageNum = PlayerPrefs.GetInt("STAGENUM", 0);            
        var str = _inputFile.Text_File_Get(StageNum);
        _inputFile.Input_File(str);
        EnemyInput = new EnemyStatus[_inputFile.GetInputRows];       
        for (var i = 0; i < _inputFile.GetInputRows; i++) 
        {
            for (var k = 0; k < _inputFile.GetInputCols; k++)
            {
                if (i != 0) 
                {
                    if (k == 0) 
                    {
                        EnemyInput[i].GetenemyHp = int.Parse(_inputFile.GetStrings[i, k]);
                    }

                    if(k == 1)
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
                    if(k == 2)
                    {
                        EnemyInput[i].GetenemyImageNum = int.Parse(_inputFile.GetStrings[i, k]);

                    }
                    if (k == 3)
                    {
                        EnemyInput[i].Gethindrance_Piece = int.Parse(_inputFile.GetStrings[i, k]);
                    }

                    if (k == 4) 
                    {
                        EnemyInput[i].GetEnemyAttackType = int.Parse(_inputFile.GetStrings[i, k]);

                    }

                    if(k == 5)
                    {
                        EnemyInput[i].GetEnemyTimeLimit = int.Parse(_inputFile.GetStrings[i, k]);

                    }
                }
            }        
       
        }

    }

    public EnemyStatus SetEnemyStatus(int stageno,int num)
    {
        switch (num) 
        {
            case 0:
                var result = EnemyInput[1];
                return result;
            case 1:
                result = EnemyInput[2];
                return result;
            case 2:
                result = EnemyInput[3];
                return result;
            default:
                result = EnemyInput[1];
                return result;
        }
    }
}












