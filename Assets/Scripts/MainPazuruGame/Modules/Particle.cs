using UnityEngine;

/// <summary>
/// パーティクル（エフェクトの管理）クラス
/// </summary>
/// 
public class Particle : MonoBehaviour
{
   
   //パーティクルの種類
    [SerializeField]
    private ParticleSystem _Particle;
    [SerializeField]
    private ParticleSystem _GreenParticle;
    [SerializeField]
    private ParticleSystem _EnemyAttackPartcle;
    [SerializeField]
    private ParticleSystem _EnemyDestroy;
    [SerializeField]
    private ParticleSystem _EnemyPop;
    [SerializeField]
    private ParticleSystem _EnemyBackParticle;
    [SerializeField]
    private ParticleSystem _SpecialComboCross_Yoko;
    [SerializeField]
    private ParticleSystem _SpecialComboCross_Tate;
    [SerializeField]
    private ParticleSystem _PlayerAttackEffect;
    [SerializeField]
    private ParticleSystem _Bunung;

    //座標位置クラス
    [SerializeField]
    VectorReturn vectorReturn;
    //パーティクルの生成
    [SerializeField]
    private RectTransform _ParticleTransform;
    public RectTransform GetParticleTransform { get => _ParticleTransform; }

    /// <summary>
    /// 3マッチした際のピースの破壊アニメーション
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="recttransform"></param>
    public void PlayerDestroyParticles(Vector2 pos, RectTransform recttransform) 
    {
        var particle = Instantiate(_GreenParticle, pos, Quaternion.identity);
        particle.transform.SetParent(recttransform);         
    }

    /// <summary>
    /// 敵が登場演出
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="recttransform"></param>
    public void EnemyAttackParticles(Vector2 pos, RectTransform recttransform)
    {
        var particle = Instantiate(_EnemyPop, pos, Quaternion.identity);
        particle.transform.SetParent(recttransform);
    }

   /// <summary>
   /// ×方向やリミット技の際の爆発演出
   /// </summary>
   /// <param name="pos"></param>
   /// <param name="recttransform"></param>
    public void Burning_Piece(Vector2 pos, RectTransform recttransform)
    {
        var particle = Instantiate(_Bunung, pos, Quaternion.identity);
        particle.transform.SetParent(recttransform);
    }

    /// <summary>
    /// リミット技でピースタイプの属性を増やす際のパーティクル演出
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="recttransform"></param>
    public void Color_10_Piece(Vector2 pos, RectTransform recttransform)
    {
        var particle = Instantiate(_EnemyDestroy, pos, Quaternion.identity);
        particle.transform.SetParent(recttransform);
    }

   /// <summary>
   /// 敵の攻撃の際に背景演出
   /// </summary>
   /// <param name="recttransform"></param>
    public void EnemyAll(RectTransform recttransform) 
    {
        var particle = Instantiate(_EnemyBackParticle, new Vector3(Screen.width, Screen.height, 0),Quaternion.identity);
        particle.transform.SetParent(recttransform);
    }

    /// <summary>
    /// 敵を倒して消える際の演出
    /// </summary>
    /// <param name="recttransform"></param>
    public void EnemyDestory(RectTransform recttransform) 
    {
        var particle = Instantiate(_EnemyDestroy, new Vector3(Screen.width, Screen.height + Screen.height/2, 0), Quaternion.identity);
        particle.transform.SetParent(recttransform);
    }

    /// <summary>
    /// 敵の登場時演出
    /// </summary>
    /// <param name="recttransform"></param>
    public void EnemyPop(RectTransform recttransform) 
    {       
        var particle = Instantiate(_EnemyPop); 
        particle.transform.localPosition = new Vector3(Screen.width, Screen.height+(Screen.height /3), 0);
        particle.transform.SetParent(recttransform);      
    }

    /// <summary>
    /// 敵への攻撃演出
    /// </summary>
    /// <param name="recttransform"></param>
    public void PlayerAttackToEnemy(RectTransform recttransform)
    {
        var particle = Instantiate(_PlayerAttackEffect);
        particle.transform.localPosition = new Vector3(Screen.width, Screen.height + (Screen.height / 3), 0);
        particle.transform.SetParent(recttransform);
    }

    /// <summary>
    /// ×方向やリミット技の際の光線演出
    /// </summary>
    /// <param name="recttransform"></param>
    public void SpecialAttack_Tate( RectTransform recttransform)
    {       
        var particle = Instantiate(_SpecialComboCross_Tate);
        particle.transform.localPosition = new Vector3(Screen.width, Screen.height - (Screen.height / 4f), 10);
        particle.transform.SetParent(recttransform);
    }

}
