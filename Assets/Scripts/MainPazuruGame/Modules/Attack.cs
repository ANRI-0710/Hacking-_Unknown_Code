using UnityEngine;

/// <summary>
/// playerのパズル破壊時のダメージを管理するクラス
/// </summary>
public class Attack : MonoBehaviour
{
    //攻撃ポイント
    private const int _advantageousPoint = 300;
    private const int _nomalPoint = 50;
    private const int _disadvantagePoint = 25;

    /// <summary>
    /// 敵の属性と破壊したピースの属性によって攻撃を振り分ける
    /// </summary>
    /// <param name="piece">3マッチしたパズル</param>
    /// <param name="enemy"></param>
    /// <param name="comboCount"></param>
<<<<<<< HEAD
    public void PieceAttack(Piece_Type piece,Enemy enemy,int comboCount) 
=======
    public void PieceAttack(Piece_Type piece,BaseEnemy enemy,int comboCount) 
>>>>>>> f061488f36f51569aa254191514fb7a1a159ec79
    {       
        switch (piece) 
        {
            case Piece_Type.RED:                
                enemy.EnemyDamage(RedPiece_Attack(comboCount, enemy.GetEnemyType));
                break;
            case Piece_Type.GREEN:
                enemy.EnemyDamage(Green_Piece_Attack(comboCount, enemy.GetEnemyType));
                break;
            case Piece_Type.YELLOW:
                enemy.EnemyDamage(Yellow_Piece_Attack(comboCount, enemy.GetEnemyType));
                break;
            case Piece_Type.BLUE:
                enemy.EnemyDamage(Blue_Piece_Attack(comboCount, enemy.GetEnemyType));
                break;
            default:
                NoDamageAttack();
                break;
        }
    }

    /// <summary>
    /// 赤属性の攻撃、敵属性に対して有利だった場合は300、不利だったら25、それ以外だったら50とコンボしただけ掛けた攻撃数値を返す
    /// </summary>
    /// <param name="comboCount">コンボ数</param>
    /// <param name="piece">敵の属性</param>
    /// <returns></returns>
    public int RedPiece_Attack(int comboCount, Piece_Type piece) 
    {
        int attackPoint;
        if (piece == Piece_Type.GREEN)
        {
            attackPoint = _advantageousPoint * comboCount;
        }
        else if (piece == Piece_Type.BLUE)
        {
            attackPoint = _disadvantagePoint * comboCount;
        }
        else 
        {
            attackPoint = _nomalPoint * comboCount;
        }
        Debug.Log(attackPoint);
        return attackPoint;
    }

    /// <summary>
    /// 緑属性の攻撃、敵属性に対して有利だった場合は300、不利だったら25、それ以外だったら50とコンボしただけ掛けた攻撃数値を返す
    /// </summary>
    /// <param name="comboCount">コンボ数</param>
    /// <param name="piece">敵の属性</param>
    /// <returns></returns>
    public int Green_Piece_Attack(int comboCount, Piece_Type piece)
    {
        int attackPoint;
        if (piece == Piece_Type.YELLOW)
        {
            attackPoint = _advantageousPoint * comboCount;
        }
        else if (piece == Piece_Type.RED)
        {
            attackPoint = _disadvantagePoint * comboCount;
        }
        else
        {
            attackPoint = _nomalPoint * comboCount;
        }
        Debug.Log(attackPoint);
        return attackPoint;
    }

    /// <summary>
    /// 黄色属性の攻撃、敵属性に対して有利だった場合は300、不利だったら25、それ以外だったら50とコンボしただけ掛けた攻撃数値を返す
    /// </summary>
    /// <param name="comboCount">コンボ数</param>
    /// <param name="piece">敵の属性</param>
    /// <returns></returns>
    public int Yellow_Piece_Attack(int comboCount, Piece_Type piece)
    {
        int attackPoint;
        if (piece == Piece_Type.BLUE)
        {
            attackPoint = _advantageousPoint * comboCount;
        }
        else if (piece == Piece_Type.GREEN)
        {
            attackPoint = _disadvantagePoint * comboCount; 
        }
        else
        {
            attackPoint = _nomalPoint * comboCount;
        }
        Debug.Log(attackPoint);
        return attackPoint;
    }

    /// <summary>
    /// 青属性の攻撃、敵属性に対して有利だった場合は300、不利だったら25、それ以外だったら50とコンボしただけ掛けた攻撃数値を返す
    /// </summary>
    /// <param name="comboCount">コンボ数</param>
    /// <param name="piece">敵の属性</param>
    /// <returns></returns>
    public int Blue_Piece_Attack(int comboCount, Piece_Type piece)
    {
        int attackPoint;
        if (piece == Piece_Type.RED)
        {
            attackPoint = _advantageousPoint * comboCount;
        }
        else if (piece == Piece_Type.YELLOW)
        {
            attackPoint = _disadvantagePoint * comboCount;
        }
        else
        {
            attackPoint = _nomalPoint * comboCount;
        }
        Debug.Log(attackPoint);
        return attackPoint;
    }

    /// <summary>
    /// ハート型ウイルスなど属性値以外の攻撃処理は50ダメージを与える
    /// </summary>
    /// <returns></returns>
    private int NoDamageAttack() { return _nomalPoint; }

}
