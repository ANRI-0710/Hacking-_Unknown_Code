using UnityEngine;

/// <summary>
/// 敵の攻撃（アンチウイルス）種類を管理するクラス
/// </summary>
public enum Enemy_Attack_Type
{
    Attack_1_RamdomCriate,
    Attack_2_RamdomCriate,
    Attack_4_RamdomCriate,
    Attack_Center_Rows,
    Attack_Center_Cols,
    Attack_Cross,
    Attack_ObliqueCross,
}

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private RectTransform _ParticleTransform;
    [SerializeField]
    private Particle _Particles;

    [SerializeField]
    private VectorReturn vectorReturn;

    private int Width;
    private const int Cols = 7;
    private const int Rows = 7;

    void Start()
    {
        Width = (Screen.width / Cols);
    }

    /// <summary>
    /// Enemyの敵をだす
    /// </summary>
    /// <param name="enemyAttacknum"></param>
    /// <param name="Pieces"></param>
    public void EnemyAttackType(Enemy_Attack_Type enemyAttacknum, Piece[,] Pieces)
    {
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyAppearance);
        switch (enemyAttacknum)
        {
            case Enemy_Attack_Type.Attack_1_RamdomCriate:
                Attack_1_RamdomCriate(Pieces);
                break;
            case Enemy_Attack_Type.Attack_2_RamdomCriate:
                Attack_2_RamdomCriate(Pieces);
                break;
            case Enemy_Attack_Type.Attack_4_RamdomCriate:
                Attack_4_RamdomCriate(Pieces);
                break;
            case Enemy_Attack_Type.Attack_Center_Rows:
                Attack_Center_Rows(Pieces);
                break;
            case Enemy_Attack_Type.Attack_Center_Cols:
                Attack_Center_Cols(Pieces);
                break;
            case Enemy_Attack_Type.Attack_Cross:
                Attack_Cross_Piece(Pieces);
                break;
            case Enemy_Attack_Type.Attack_ObliqueCross:
                Attack_ObliqueCross(Pieces);
                break;
        }
    }

    public void Attack_1_RamdomCriate(Piece[,] Pieces)
    {        
        var x = Random.Range(0, Cols);
        var y = Random.Range(0, Rows);

        var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(x, y), Width);
        _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
        Pieces[x, y].GetPieceState = Piece_Type.BLACK;
    }

    public void Attack_2_RamdomCriate(Piece[,] Pieces)
    {        
        for (var i = 0; i < 2; i++)
        {
            var x = Random.Range(0, Cols);
            var y = Random.Range(0, Rows);
            
            var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(x, y), Width);
            _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
            Pieces[x, y].GetPieceState = Piece_Type.BLACK;          
        }
    }

    public void Attack_4_RamdomCriate(Piece[,] Pieces)
    {       
        for (var i = 0; i < 4; i++)
        {
            var x = Random.Range(0, Cols);
            var y = Random.Range(0, Rows);
            var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(x, y), Width);

            _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
            Pieces[x, y].GetPieceState = Piece_Type.BLACK;            
        }
    }
    
    public void Attack_Center_Rows(Piece[,] Pieces)
    {
        for (var i = 0; i < Rows; i++)
        {
            for (var k = 0; k < Cols; k++)
            {
                var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(k, 3), Width);
                _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
                Pieces[k, 3].GetPieceState = Piece_Type.BLACK;
            }
        }
    }

    public void Attack_Center_Cols(Piece[,] Pieces)
    {
        for (var i = 0; i < Rows; i++)
        {
            for (var k = 0; k < Cols; k++)
            {
                var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(3, k), Width);
                _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
                Pieces[3, k].GetPieceState = Piece_Type.BLACK;
            }
        }
    }

    public void Attack_Cross_Piece(Piece[,] Pieces)
    {
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                Pieces[3, k].GetPieceState = Piece_Type.EMPTY;

                var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(3, k), Width);
                _Particles.PlayerDestroyParticles(criatePos, _ParticleTransform);

                var criatePos1 = vectorReturn.GetPieceWorldPos(new Vector2(k, 3), Width);
                Pieces[k, 3].GetPieceState = Piece_Type.EMPTY;

                _Particles.PlayerDestroyParticles(criatePos, _ParticleTransform);
                _Particles.PlayerDestroyParticles(criatePos1, _ParticleTransform);
            }
        }
    }

    public void Attack_ObliqueCross(Piece[,] Pieces) 
    {
        for (var i = 0; i < Rows; i++)
        {
            for (var k = 0; k < Cols; k++)
            {
                var col = Cols - 1;
                var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(k, k), Width);
                var criatePos2 = vectorReturn.GetPieceWorldPos(new Vector2(col - i, k), Width);

                _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
                _Particles.EnemyAttackParticles(criatePos2, _ParticleTransform);

                Pieces[k, k].GetPieceState = Piece_Type.BLACK;
                Pieces[col - i, i].GetPieceState = Piece_Type.BLACK;
            }
        }
    }


}
    
















