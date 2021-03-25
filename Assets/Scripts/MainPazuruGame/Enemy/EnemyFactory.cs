using UnityEngine;

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
      
       // EnemyConroll.GetComponent<RectTransform>().transform.localPosition = new Vector3(0, 150, 0);
        return EnemyConroll;
    }

}
