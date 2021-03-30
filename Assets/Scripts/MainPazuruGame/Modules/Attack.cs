using UnityEngine;

/// <summary>
/// 攻撃クラス
/// </summary>
public class Attack : MonoBehaviour
{    
    public void PieceAttack(Piece_Type piece,BaseEnemy enemy,int comboCount) 
    {       
        switch (piece) 
        {
            case Piece_Type.RED:                
                enemy.EnemyDamage(RedPiece_Attack(comboCount,piece));
                break;
            case Piece_Type.GREEN:
                enemy.EnemyDamage(Green_Piece_Attack(comboCount,piece));
                break;
            case Piece_Type.YELLOW:
                enemy.EnemyDamage(Yellow_Piece_Attack(comboCount,piece));
                break;
            case Piece_Type.BLUE:
                enemy.EnemyDamage(Blue_Piece_Attack(comboCount,piece));
                break;
            default:
                NoDamageAttack();
                break;
        }
    }

    public int RedPiece_Attack(int comboCount, Piece_Type piece) 
    {
        int attackPoint;
        if (piece == Piece_Type.GREEN)
        {
            attackPoint = 100* comboCount;
        }
        else if (piece == Piece_Type.BLUE)
        {
            attackPoint = 25;
        }
        else 
        {
            attackPoint = 50;
        }
        Debug.Log(attackPoint);
        return attackPoint;
    }

    public int Green_Piece_Attack(int comboCount, Piece_Type piece)
    {
        int attackPoint;
        if (piece == Piece_Type.YELLOW)
        {
           attackPoint = 100 * comboCount;
        }
        else if (piece == Piece_Type.RED)
        {
            attackPoint = 25 * comboCount;
        }
        else
        {
            attackPoint = 50 * comboCount;
        }
        Debug.Log(attackPoint);
        return attackPoint;
    }

    public int Yellow_Piece_Attack(int comboCount, Piece_Type piece)
    {
        int attackPoint;
        if (piece == Piece_Type.BLUE)
        {
            attackPoint = 100 * comboCount; 
        }
        else if (piece == Piece_Type.GREEN)
        {
            attackPoint = 25 * comboCount; 
        }
        else
        {
            attackPoint = 50 * comboCount;
        }
        Debug.Log(attackPoint);
        return attackPoint;
    }

    public int Blue_Piece_Attack(int comboCount, Piece_Type piece)
    {
        int attackPoint;
        if (piece == Piece_Type.RED)
        {
            attackPoint = 100 * comboCount;
        }
        else if (piece == Piece_Type.YELLOW)
        {
            attackPoint = 25 * comboCount;
        }
        else
        {
            attackPoint = 50 * comboCount;
        }
        Debug.Log(attackPoint);
        return attackPoint;
    }

    private int NoDamageAttack()
    {
        int attackPoint = 0;
        return attackPoint;
    }

}
