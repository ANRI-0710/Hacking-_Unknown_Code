using UnityEngine;

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

/// <summary>
/// パズルシーンのBGMとSEを管理するクラス
/// </summary>
public class PuzzleSoundManager : MonoBehaviour
{
    public static PuzzleSoundManager Instance { get; private set; }

   /// <summary>
   /// BGM・及びSE
   /// </summary>
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

   
    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }
   
    /// <summary>
    /// SE用のenum変数に応じたSE・BGMを再生する
    /// </summary>
    /// <param name="se"></param>
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
