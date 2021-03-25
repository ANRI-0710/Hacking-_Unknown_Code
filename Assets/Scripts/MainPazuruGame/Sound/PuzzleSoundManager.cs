using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パズルシーンのBGMとSEを管理するクラス
/// </summary>
/// 
public enum SE_Now 
{    
    PuzzleDestroy,
    Alert,
    EnemyAppearance,
    LimitCross,
    LimitMax,
    EnemyDestroy,
    EnemyPieceDestroy,
    SpecialAttackColor,

}

public class PuzzleSoundManager : MonoBehaviour
{
    public static PuzzleSoundManager Instance { get; private set; }

    [SerializeField]
    private AudioClip BGM_Pazzle;

    [SerializeField]
    private AudioClip SE_PuzzleDestruction;

    [SerializeField]
    private AudioClip SE_Alert;

    [SerializeField]
    private AudioClip SE_EnemyAppearance;

    [SerializeField]
    private AudioClip SE_LimitCross;

    [SerializeField]
    private AudioClip SE_LimitMax;

    [SerializeField]
    private AudioClip SE_SpecialAttackColor;

    [SerializeField]
    private AudioClip SE_EnemyDestroy;

    [SerializeField]
    private AudioClip SE_EnemyPieceDestroy;



    private AudioSource _audioSource;

    private SE_Now SE_Play_Now;
    public SE_Now Get_SE__Play_Now 
    {
        get => SE_Play_Now;
        set 
        {
            SE_Play_Now = value;
            SE_Selection(SE_Play_Now);
        }    
    }

    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }
   
    public void SE_Selection(SE_Now se) 
    {
        switch (se) 
        {
            case SE_Now.PuzzleDestroy:
                _audioSource.PlayOneShot(SE_PuzzleDestruction);
                break;
            case SE_Now.Alert:
                _audioSource.PlayOneShot(SE_Alert);
                break;
            case SE_Now.EnemyAppearance:
                _audioSource.PlayOneShot(SE_EnemyAppearance);
                break;
            case SE_Now.LimitCross:
                _audioSource.PlayOneShot(SE_LimitCross);
                break;
            case SE_Now.LimitMax:
                _audioSource.PlayOneShot(SE_LimitMax);
                break;
            case SE_Now.EnemyDestroy:
                _audioSource.PlayOneShot(SE_EnemyDestroy);
                break;
            case SE_Now.EnemyPieceDestroy:
                _audioSource.PlayOneShot(SE_EnemyPieceDestroy);
                break;
            case SE_Now.SpecialAttackColor:
                _audioSource.PlayOneShot(SE_SpecialAttackColor);
                break;

            default:
                break;
        }    
    }


}
