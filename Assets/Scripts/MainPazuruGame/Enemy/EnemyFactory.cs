using UnityEngine;

/// <summary>
///敵を生成するクラス
/// </summary>
public class EnemyFactory : MonoBehaviour
{
    //敵クラス
    [SerializeField]
    private Enemy _Enemy;  
    private Enemy EnemyConroll;
    [SerializeField]
    private RectTransform _EnemyTransform;

   //敵の画像（この配列番号に合わせた敵画像を表示する）
    [SerializeField]
    private Sprite[] _EnemyImageManagaer;

    /// <summary>
    /// 敵の構造体データをBaseEnemyに渡し、リターンをする
    /// </summary>
    /// <param name="num"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Enemy EnemyInstance(EnemyStatus status)
    {
        EnemyConroll = Instantiate(_Enemy);
        EnemyConroll = EnemyConroll.GetComponent<Enemy>();
        EnemyConroll.EnemyInitImage(_EnemyImageManagaer[status.GetenemyImageNum]);
        EnemyConroll.EnemyInit(status.GetenemyHp, status.GetenemyHp, 0.6f, 0.03f, status.GetPiece_Type, status.Gethindrance_Piece, status.GetEnemyAttackType, status.GetEnemyTimeLimit);
        return EnemyConroll;
    }

}
