using UnityEngine;

/// <summary>
///GridManagerから生成の指示を受け取り敵を生成を管理するクラス
/// </summary>
public class EnemyFactory : MonoBehaviour
{
    [SerializeField]
    private BaseEnemy _Enemy;  
    private BaseEnemy EnemyConroll;

    [SerializeField]
    private RectTransform _EnemyTransform;

    [SerializeField]
    private Sprite[] _EnemyImageManagaer;

    public BaseEnemy EnemyInstance(int num,EnemyStatus status)
    {
        EnemyConroll =  Instantiate(_Enemy);
        EnemyConroll = EnemyConroll.GetComponent<BaseEnemy>();
        EnemyConroll.EnemyInitImage(_EnemyImageManagaer[status.GetenemyImageNum]);
        EnemyConroll.EnemyInit(status.GetenemyHp, status.GetenemyHp, 0.6f, 0.03f, 100f, status.GetPiece_Type, status.Gethindrance_Piece,status.GetEnemyAttackType,status.GetEnemyTimeLimit); 
        return EnemyConroll;
    }

}
