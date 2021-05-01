using UnityEngine;

public enum Enemy_Attack_Type   //敵の攻撃パターン変数
{
    Attack_1_RamdomCriate,
    Attack_2_RamdomCriate,
    Attack_4_RamdomCriate,
    Attack_Center_Rows,
    Attack_Center_Cols,
    Attack_Cross,
    Attack_ObliqueCross,
}

/// <summary>
/// 敵の攻撃パターンを管理するクラス（アンチウイルスパズル（お邪魔ピース）を輩出パターン）
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    //アンチウイルスパズル用のパーティクル演出
    [SerializeField]
    private RectTransform _ParticleTransform;
    [SerializeField]
    private Particle _Particles;
  
    //座標変換用
    [SerializeField]
    private VectorReturn _VectorReturn;

    //スクリーンサイズを所得しパーティクルの生成画面サイズに併せて調整するため
    private int _Width;

    //パズルの行・列の定数
    private const int _cols = 7;
    private const int _rows = 7;

    void Start()
    {
        //スクリーンサイズを所得しパーティクルの生成画面サイズに併せて調整
        _Width = (Screen.width / _cols);
    }

    /// <summary>
    /// テキストファイルから受け取った敵の攻撃パターン番号の攻撃を行う
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

   /// <summary>
   /// アンチウイルスパズルを1個ランダムで生成する
   /// </summary>
   /// <param name="Pieces">現在のパズルの盤面</param>
    public void Attack_1_RamdomCriate(Piece[,] Pieces)
    {        
        var x = Random.Range(0, _cols);
        var y = Random.Range(0, _rows);

        var criatePos = _VectorReturn.GetPieceWorldPos(new Vector2(x, y), _Width);
        _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
        Pieces[x, y].GetPieceState = Piece_Type.BLACK;
    }

    /// <summary>
    /// アンチウイルスを2個ランダムで生成する
    /// </summary>
    /// <param name="Pieces">現在のパズルの盤面</param>
    public void Attack_2_RamdomCriate(Piece[,] Pieces)
    {        
        for (var i = 0; i < 2; i++)
        {
            var x = Random.Range(0, _cols);
            var y = Random.Range(0, _rows);
            
            var criatePos = _VectorReturn.GetPieceWorldPos(new Vector2(x, y), _Width);
            _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
            Pieces[x, y].GetPieceState = Piece_Type.BLACK;          
        }
    }

    /// <summary>
    /// アンチウイルスパズルをを4個ランダムで生成する
    /// </summary>
    /// <param name="Pieces">現在のパズルの盤面</param>
    public void Attack_4_RamdomCriate(Piece[,] Pieces)
    {       
        for (var i = 0; i < 4; i++)
        {
            var x = Random.Range(0, _cols);
            var y = Random.Range(0, _rows);
            var criatePos = _VectorReturn.GetPieceWorldPos(new Vector2(x, y), _Width);

            _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
            Pieces[x, y].GetPieceState = Piece_Type.BLACK;            
        }
    }

    /// <summary>
    /// アンチウイルスパズルを縦4列目すべてに生成する
    /// </summary>
    /// <param name="Pieces">現在のパズルの盤面</param>
    public void Attack_Center_Rows(Piece[,] Pieces)
    {
        for (var i = 0; i < _rows; i++)
        {
            for (var k = 0; k < _cols; k++)
            {              
                var criatePos = _VectorReturn.GetPieceWorldPos(new Vector2(k, 3), _Width);
                _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
                Pieces[k, 3].GetPieceState = Piece_Type.BLACK;             
            }
        }
    }

    /// <summary>
    /// アンチウイルスパズルを横4列目すべてに生成する
    /// </summary>
    /// <param name="Pieces">現在のパズルの盤面</param>
    public void Attack_Center_Cols(Piece[,] Pieces)
    {
        for (var i = 0; i < _rows; i++)
        {
            for (var k = 0; k < _cols; k++)
            {

                if (k != 0) 
                {
                    var criatePos = _VectorReturn.GetPieceWorldPos(new Vector2(3, k), _Width);
                    _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
                    Pieces[3, k].GetPieceState = Piece_Type.BLACK;
                }
                  
            }
        }
    }

    /// <summary>
    /// アンチウイルスパズルを十字方向すべてに生成する
    /// </summary>
    /// <param name="Pieces">現在のパズルの盤面</param>
    public void Attack_Cross_Piece(Piece[,] Pieces)
    {
        for (var i = 0; i < _cols; i++)
        {
            for (var k = 0; k < _rows; k++)
            {
                Pieces[3, k].GetPieceState = Piece_Type.BLACK;

                var criatePos = _VectorReturn.GetPieceWorldPos(new Vector2(3, k), _Width);
                _Particles.PlayerDestroyParticles(criatePos, _ParticleTransform);

                var criatePos1 = _VectorReturn.GetPieceWorldPos(new Vector2(k, 3), _Width);
                Pieces[k, 3].GetPieceState = Piece_Type.BLACK;

                _Particles.PlayerDestroyParticles(criatePos, _ParticleTransform);
                _Particles.PlayerDestroyParticles(criatePos1, _ParticleTransform);
            }
        }
    }

    /// <summary>
    /// アンチウイルスパズルを×字方向すべてに生成する
    /// </summary>
    /// <param name="Pieces">現在のパズルの盤面</param>
    public void Attack_ObliqueCross(Piece[,] Pieces) 
    {
        for (var i = 0; i < _rows; i++)
        {
            for (var k = 0; k < _cols; k++)
            {
                var col = _cols - 1;
                var criatePos = _VectorReturn.GetPieceWorldPos(new Vector2(k, k), _Width);
                var criatePos2 = _VectorReturn.GetPieceWorldPos(new Vector2(col - i, k), _Width);

                _Particles.EnemyAttackParticles(criatePos, _ParticleTransform);
                _Particles.EnemyAttackParticles(criatePos2, _ParticleTransform);

                Pieces[k, k].GetPieceState = Piece_Type.BLACK;
                Pieces[col - i, i].GetPieceState = Piece_Type.BLACK;
            }
        }
    }


}
    
















