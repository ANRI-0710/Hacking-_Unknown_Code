using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パーティクル（エフェクトの管理）クラス
/// </summary>
/// 

public enum Particle_Type 
{
    PlayerParticle,
    EnemyParticle
}

public class Particle : MonoBehaviour
{
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
    private ParticleSystem _SpecialComboCross_Yoko;

    [SerializeField]
    private ParticleSystem _SpecialComboCross_Tate;

    [SerializeField]
    private ParticleSystem _PlayerAttackEffect;

    [SerializeField]
    VectorReturn vectorReturn;

    [SerializeField]
    private ParticleSystem _Bunung;

    //パーティクルの生成
    [SerializeField]
    private RectTransform _ParticleTransform;

    private int Width;
    private const int Cols = 7;

    void Start()
    {        
        Width = (Screen.width / Cols);
    }

    public void PlayerDestroyParticles(Vector2 pos, RectTransform recttransform) 
    {
        var particle = Instantiate(_GreenParticle, pos, Quaternion.identity);
        particle.transform.SetParent(recttransform);         
    }

    public void EnemyAttackParticles(Vector2 pos, RectTransform recttransform)
    {
        var particle = Instantiate(_EnemyAttackPartcle, pos, Quaternion.identity);
        particle.transform.SetParent(recttransform);
    }

    //爆発処理
    public void Burning_Piece(Vector2 pos, RectTransform recttransform)
    {
        var particle = Instantiate(_Bunung, pos, Quaternion.identity);
        particle.transform.SetParent(recttransform);
    }

    //タイプからー増殖時の攻撃
    public void Color_10_Piece(Vector2 pos, RectTransform recttransform)
    {
        var particle = Instantiate(_EnemyDestroy, pos, Quaternion.identity);
        particle.transform.SetParent(recttransform);
    }


    //敵の攻撃演出
    public void EnemyAll(RectTransform recttransform) 
    {
        var particle = Instantiate(_SpecialComboCross_Yoko,new Vector3(Screen.width, Screen.height, 0),Quaternion.identity);
        particle.transform.SetParent(recttransform);
    }

    public void EnemyDestory(RectTransform recttransform) 
    {
        var particle = Instantiate(_EnemyDestroy, new Vector3(Screen.width, Screen.height + Screen.height/2, 0), Quaternion.identity);
        particle.transform.SetParent(recttransform);
    }

    public void EnemyPop(RectTransform recttransform) 
    {
        var criatePos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        var particle = Instantiate(_EnemyPop); 
        particle.transform.localPosition = new Vector3(Screen.width, Screen.height+(Screen.height /3), 0);
        particle.transform.SetParent(recttransform);      
    }

    public void PlayerAttackToEnemy(RectTransform recttransform)
    {
        var criatePos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        var particle = Instantiate(_PlayerAttackEffect);
        particle.transform.localPosition = new Vector3(Screen.width, Screen.height + (Screen.height / 3), 0);
        particle.transform.SetParent(recttransform);
    }

    public void SpecialAttack_Tate( RectTransform recttransform)
    {       
        var particle = Instantiate(_SpecialComboCross_Tate);
        particle.transform.localPosition = new Vector3(Screen.width, Screen.height - (Screen.height / 4f), 10);
        particle.transform.SetParent(recttransform);
    }

  



}
