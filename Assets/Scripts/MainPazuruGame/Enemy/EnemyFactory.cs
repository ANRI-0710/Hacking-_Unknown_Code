using UnityEngine;

/// <summary>
///敵を生成するクラス
/// </summary>
public class EnemyFactory : MonoBehaviour
{
    //敵クラス
    [SerializeField]
<<<<<<< HEAD
    private Enemy _Enemy;  
    private Enemy EnemyConroll;
=======
    private BaseEnemy _Enemy;  
    private BaseEnemy EnemyConroll;
>>>>>>> f061488f36f51569aa254191514fb7a1a159ec79
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
<<<<<<< HEAD
    public Enemy EnemyInstance(EnemyStatus status)
    {
        EnemyConroll = Instantiate(_Enemy);
        EnemyConroll = EnemyConroll.GetComponent<Enemy>();
=======
    public BaseEnemy EnemyInstance(EnemyStatus status)
    {
        EnemyConroll = Instantiate(_Enemy);
        EnemyConroll = EnemyConroll.GetComponent<BaseEnemy>();
>>>>>>> f061488f36f51569aa254191514fb7a1a159ec79
        EnemyConroll.EnemyInitImage(_EnemyImageManagaer[status.GetenemyImageNum]);
        EnemyConroll.EnemyInit(status.GetenemyHp, status.GetenemyHp, 0.6f, 0.03f, status.GetPiece_Type, status.Gethindrance_Piece, status.GetEnemyAttackType, status.GetEnemyTimeLimit);
        return EnemyConroll;
    }

}
