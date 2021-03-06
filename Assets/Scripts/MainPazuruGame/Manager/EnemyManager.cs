using UnityEngine;
using System.Collections;

/// <summary>
/// 敵の管理クラス
/// </summary>

public class EnemyManager : MonoBehaviour
{
    //敵の生成専用クラス（ここから敵を生成し、リターンでBaseEnemyに戻す）  
    [SerializeField]
    private EnemyFactory _EnemyFactroy;

    //敵のステータス（敵データが格納された構造体をそれぞれのクラス・変数に代入する）
    [SerializeField]
    private Enemy_Status _EnemyStatus;
    public Enemy_Status GetEnemyStatus { get => _EnemyStatus;}

    //敵の攻撃パターンを管理するクラスへのアクセス
    [SerializeField]
    private EnemyAttack _enemyAttack;

    //敵クラスへのアクセス
    private Enemy _enemyConroll;
    public Enemy GetenemyConroll { get => _enemyConroll; set { _enemyConroll = value; } }

    //敵の出現回数（全3体、敵を倒すとCountが1プラスされる）
    private int _enemyCount = 1;
    public int GetenemyCount { get => _enemyCount; }

    //敵の総数分の敵ステータスの参照用配列
    //private EnemyStatus[] Enemys = new EnemyStatus[enemyCount];
    private EnemyStatus[] Enemys;

    //敵を撃破しなければならない時間制限をカウントするコルーチン
    private IEnumerator EnemyCountCol;
    public IEnumerator GetEnemyCountCol { get => EnemyCountCol; set { EnemyCountCol = value; } }

    [SerializeField]
    private RectTransform _EnemyTransform; //敵の表示位置の固定

    //敵が攻撃するか否か
    private bool isEnemyAttackTurn = false;
    public bool GetisEnemyAttackTurn{ get => isEnemyAttackTurn; }

    private void Awake()
    {
        Enemys = new EnemyStatus[_EnemyStatus.GetEnemyRowsCount];
    }

    /// <summary>
    /// 敵のHPを確認し、0になったらEnemyを消す
    /// </summary>
    /// <returns></returns>
    public IEnumerator Is_Enemy_Check()
    {
        //敵のHPがゼロだったら
        if (_enemyConroll.Get_NowHpPoint <= 0)
        {
            _enemyConroll.GetEnemyAttackCount = 0;
            GameManager.Instance.GetParticle.EnemyDestory(GameManager.Instance.GetParticle.GetParticleTransform);

            //タイムリミットを停止
            StopCoroutine(EnemyCountCol);
            EnemyCountCol = null;
            PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyDestroy);
            yield return new WaitForSeconds(2.0f);
            //敵の削除
            Destroy(_enemyConroll.gameObject);
            yield return new WaitForSeconds(1.0f);

            //敵の出現カウントが確認し、EnemyCount以下だったら次の敵を出現させる
            if (_enemyCount < _EnemyStatus.GetEnemyRowsCount -1)
            {
                _enemyCount++;
                GameManager.Instance.GetParticle.EnemyPop(GameManager.Instance.GetParticle.GetParticleTransform);

                var displayenemycount = _EnemyStatus.GetEnemyRowsCount - 1;
                GameManager.Instance.GetUIManager.SignalPopup("Stage"+ _enemyCount.ToString() + "/" + displayenemycount.ToString(), GameManager.Instance.GetUIManager.GetrectTransform);
                PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyAppearance);
                yield return new WaitForSeconds(3.0f);

                //敵の構造体を所得
                Enemys[_enemyCount] = _EnemyStatus.SetEnemyStatus(_enemyCount);
                //敵の生成
                InitEnemys(_enemyCount);
                yield return null;
                //敵のタイムカウントを開始する
                yield return null;
                StartCoroutine(EnemyCountCol);
                //yield return EnemyAttack();
            }//出現カウントを越えたらクリア画面へ
            else { yield return GameManager.Instance.GetUIManager.Clear(); }
           
        }
        else if (_enemyConroll.GetEnemyTimeOver)
        {
            //秒数カウント以内に倒せなかったらGameOver
            yield return GameManager.Instance.GetUIManager.GameOver();
        }
    }

    /// <summary>
    /// 敵カウントが0になったら敵からの攻撃処理を行う
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemyAttack()
    {
        if (_enemyConroll.GetHindrancePiece == _enemyConroll.GetEnemyAttackCount)
        {
            _enemyAttack.EnemyAttackType((Enemy_Attack_Type)_enemyConroll.GetAttackEnemyType,GameManager.Instance.GetPieceManager.GetPieces);
            GameManager.Instance.GetParticle.EnemyAll(GameManager.Instance.GetParticle.GetParticleTransform);
            yield return new WaitForSeconds(3.0f);
            _enemyConroll.GetEnemyAttackCount = 0;
            
        }
    }

    /// <summary>
    /// 敵の初期化処理
    /// </summary>
    public void InitEnemys(int num)
    {
        //敵生成クラスを生成する
        _EnemyFactroy = Instantiate(_EnemyFactroy);

        //入力データと何体目の敵なのかを判定し、該当の敵の構造体を受け渡す
        Enemys[_enemyCount] = _EnemyStatus.SetEnemyStatus(_enemyCount);

        //受け渡した構造体を敵クラスに代入する
        _enemyConroll = _EnemyFactroy.EnemyInstance(Enemys[num]);
        _enemyConroll.transform.SetParent(_EnemyTransform);

        Instantiate(_EnemyStatus);
        EnemyCountCol = _enemyConroll.TimeCount(_enemyConroll.GetEnemyTimeLimit);
        StartCoroutine(EnemyCountCol);

    }

}
