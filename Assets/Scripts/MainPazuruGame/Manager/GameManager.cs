using System.Collections;
using UnityEngine;

/// <summary>
/// パズルシーンすべての進行管理を担当するクラス
/// </summary>
public class GameManager : MonoBehaviour
{   
    [SerializeField]
    Grid_Manager _GridManager;
       
    public int Num = 0;

    /// <summary>
    /// ゲームコントロール用のインスタンス所得（リバーシ中のみのためシングルトンにしない）
    /// </summary>
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //StartCoroutine(PlayerLoop());
    }

    
}
