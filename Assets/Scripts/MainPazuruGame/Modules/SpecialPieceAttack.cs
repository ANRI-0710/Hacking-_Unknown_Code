using UnityEngine;

public enum E_SpecialAttack //リミット技パターン
{
    SP_Red,
    SP_Blue,
    SP_Yellow,
    SP_Green,
    SP_White,
    SP_HorizontalOneArray,
    SP_VerticalOneArray,
    SP_Destroy_Cross,
    SP_ObliqueCross_Cross,
    SP_BlackPiece_Destroy    
}

/// <summary>
/// リミット技によって起こるピースの破壊・エフェクト・サウンドを管理するクラス
/// </summary>
public class SpecialPieceAttack : MonoBehaviour
{  
   
    [SerializeField]
    private Particle _Particles;
    [SerializeField]
    private RectTransform _ParticleTransform;

    [SerializeField]
    private VectorReturn vectorReturn;

    private int Width;
    private const int Cols = 7;
    private const int Rows = 7;
    private const int Piece_Attack_Count = 10;

    void Start()
    {
        //スクリーンサイズの所得
        Width = (Screen.width / Cols);
    }

    /// <summary>
    /// 登録したリミット技を発動させる
    /// </summary>
    /// <param name="specialAttack">リミット技の攻撃タイプ</param>
    /// <param name="Pieces">ピースの盤面</param>
    public void SpecialAttack(E_SpecialAttack specialAttack, Piece[,] Pieces) 
    {
        switch (specialAttack) 
        {
            case E_SpecialAttack.SP_Red:
                Special_Attack_SP_Red(Pieces);
                break;
            case E_SpecialAttack.SP_Blue:
                Special_Attack_SP_Blue(Pieces);
                break;
            case E_SpecialAttack.SP_Yellow:
                Special_Attack_SP_Yellow(Pieces);
                break;
            case E_SpecialAttack.SP_Green:
                Special_Attack_SP_Green(Pieces);
                break;
            case E_SpecialAttack.SP_White:
                Special_Attack_SP_Pink(Pieces);
                break;
            case E_SpecialAttack.SP_HorizontalOneArray:
                Special_Attack_SP_HorizontalOneArray(Pieces);
                break;
            case E_SpecialAttack.SP_VerticalOneArray:
                Special_Attack_SP_VerticalOneArray(Pieces);
                break;
            case E_SpecialAttack.SP_Destroy_Cross:
                Special_Attack_SP_Destroy_Cross(Pieces);
                break;
            case E_SpecialAttack.SP_ObliqueCross_Cross:
                Special_Attack_SP_ObliqueCross_Cross(Pieces);
                break;
            case E_SpecialAttack.SP_BlackPiece_Destroy:
                Special_Attack_SP_Black_Destroy(Pieces);
                break;
        }       
    }

    /// <summary>
    /// 赤属性のピースを10個増やす
    /// </summary>
    /// <param name="Pieces">ピースの盤面</param>
    public void Special_Attack_SP_Red(Piece[,] Pieces)
    {
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
        for (var i = 0; i < Piece_Attack_Count; i++)
            {
                var x = Random.Range(0, Cols);
                var y = Random.Range(0, Rows);
                
                var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(x, y), Width);  
                _Particles.Color_10_Piece(criatePos, _ParticleTransform);
                Pieces[x, y].GetPieceState = Piece_Type.RED;
            }        
    }

    /// <summary>
    /// 青属性のピースを10個増やす
    /// </summary>
    /// <param name="Pieces">ピースの盤面</param>
    public void Special_Attack_SP_Blue(Piece[,] Pieces)
    {
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
        for (var i = 0; i < Piece_Attack_Count; i++)
        {
            var x = Random.Range(0, Cols);
            var y = Random.Range(0, Rows);

            var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(x, y), Width);
            _Particles.Color_10_Piece(criatePos, _ParticleTransform);
            Pieces[x, y].GetPieceState = Piece_Type.BLUE;
        }
    }

    /// <summary>
    /// 黄属性のピースを10個増やす
    /// </summary>
    /// <param name="Pieces">ピースの盤面</param>
    public void Special_Attack_SP_Yellow(Piece[,] Pieces)
    {
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
        for (var i = 0; i < Piece_Attack_Count; i++)
        {
            var x = Random.Range(0, Cols);
            var y = Random.Range(0, Rows);

            var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(x, y), Width);
            _Particles.Color_10_Piece(criatePos, _ParticleTransform);
            Pieces[x, y].GetPieceState = Piece_Type.YELLOW;
            
        }
    }

    /// <summary>
    /// 緑属性のピースを10個増やす
    /// </summary>
    /// <param name="Pieces">ピースの盤面</param>
    public void Special_Attack_SP_Green(Piece[,] Pieces)
    {
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
        for (var i = 0; i < Piece_Attack_Count; i++)
        {
            var x = Random.Range(0, Cols);
            var y = Random.Range(0, Rows);

            var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(x, y), Width);
            _Particles.Color_10_Piece(criatePos, _ParticleTransform);
            Pieces[x, y].GetPieceState = Piece_Type.GREEN;
        }
    }

    /// <summary>
    /// ハート型ウイルス属性のピースを10個増やす
    /// </summary>
    /// <param name="Pieces">ピースの盤面</param>
    public void Special_Attack_SP_Pink(Piece[,] Pieces)
    {
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
        for (var i = 0; i < Piece_Attack_Count; i++)
        {
            var x = Random.Range(0, Cols);
            var y = Random.Range(0, Rows);

            var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(x, y), Width);
            _Particles.Color_10_Piece(criatePos, _ParticleTransform);
            Pieces[x, y].GetPieceState = Piece_Type.WHITE;
        }
    }

    /// <summary>
    /// アンチウイルスピースをすべて抹消する
    /// </summary>
    /// <param name="Pieces">ピースの盤面</param>
    public void Special_Attack_SP_Black_Destroy(Piece[,] Pieces)
    {
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                if (Pieces[i, k].GetPieceState == Piece_Type.BLACK) 
                {
                    var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(k, 3), Width);
                    _Particles.Burning_Piece(criatePos, _ParticleTransform);
                    Pieces[i, k].GetPieceState = Piece_Type.EMPTY;
                }
                
               
            }
        }
    }

    /// <summary>
    /// 4行目のすべてのピースを破壊する
    /// </summary>
    /// <param name="Pieces">ピースの盤面</param>
    public void Special_Attack_SP_HorizontalOneArray(Piece[,] Pieces)
    {
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.LimitCross);
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(k, 3), Width);
                _Particles.Burning_Piece(criatePos, _ParticleTransform);
                Pieces[k, 3].GetPieceState = Piece_Type.EMPTY;
            }
        }
    }


    /// <summary>
    /// 4列目のすべてのピースを破壊する
    /// </summary>
    /// <param name="Pieces">ピースの盤面</param>
    public void Special_Attack_SP_VerticalOneArray(Piece[,] Pieces)
    {
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.LimitCross);
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(3, k), Width);
                _Particles.Burning_Piece(criatePos, _ParticleTransform);
                Pieces[3, k].GetPieceState = Piece_Type.EMPTY;
            }
        }
    }

    /// <summary>
    /// ×方向すべてのピースを破壊する
    /// </summary>
    /// <param name="Pieces">ピースの盤面</param>
    public void Special_Attack_SP_ObliqueCross_Cross(Piece[,] Pieces)
    {
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.LimitCross);
        for (var i = 0; i < Rows; i++)
        {
            for (var k = 0; k < Cols; k++)
            {
                var col = Cols - 1;
                var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(k, k), Width);
                var criatePos2 = vectorReturn.GetPieceWorldPos(new Vector2(col - i, k), Width);

                _Particles.PlayerDestroyParticles(criatePos, _ParticleTransform);
                _Particles.PlayerDestroyParticles(criatePos2, _ParticleTransform);
                _Particles.SpecialAttack_Tate(_ParticleTransform);
                Pieces[k, k].GetPieceState = Piece_Type.EMPTY;
                Pieces[col - i, i].GetPieceState = Piece_Type.EMPTY;
            }
        }
    }

    /// <summary>
    /// ＋方向すべてのピースを破壊する
    /// </summary>
    /// <param name="Pieces">ピースの盤面</param>
    public void Special_Attack_SP_Destroy_Cross(Piece[,] Pieces)
    {
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.LimitCross);
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                Pieces[3, k].GetPieceState = Piece_Type.EMPTY;                
                var criatePos = vectorReturn.GetPieceWorldPos(new Vector2(3, k), Width);                
                _Particles.Burning_Piece(criatePos, _ParticleTransform);

                var criatePos1 = vectorReturn.GetPieceWorldPos(new Vector2(k, 3), Width);
                Pieces[k, 3].GetPieceState = Piece_Type.EMPTY;                                              
                _Particles.PlayerDestroyParticles(criatePos1, _ParticleTransform);
                _Particles.SpecialAttack_Tate(_ParticleTransform);
            }
        }
    }


}



