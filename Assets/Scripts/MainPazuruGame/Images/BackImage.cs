using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ピースのバックイメージの調整
/// </summary>
public class BackImage : MonoBehaviour
{
    [SerializeField]
    private VectorReturn vectorReturn;
    void Start()
    {
        //vectorReturn.Size_Adjustment(this.gameObject, 0.6f, 0.6f, 0, -0.15f, 0);
        vectorReturn.Size_Adjustment(this.gameObject, 0.6f, 0.6f, 0, -0.12f, 0);
    }


    
   
}
