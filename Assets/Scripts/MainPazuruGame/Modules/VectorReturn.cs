using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 座標変換クラス
/// </summary>
/// 

public class VectorReturn : MonoBehaviour
{
    [SerializeField]
    private Piece _Piece;

    //（UI）ピースをタップするためのポインタ
    [SerializeField]
    PointerEventData pointer;

    private List<RaycastResult> results =  new List<RaycastResult>();
    public List<RaycastResult> GetRaycastResults { get => results; set { results = value; } }

     void Start()
     {
        // ポインタ（マウス/タッチ）イベントに関連するイベントの情報
        pointer = new PointerEventData(EventSystem.current);
     }

    public Piece ReturnRaycastPiece(Piece piece)
    {
        _Piece = piece;
        //results = new List<RaycastResult>();
        // マウスポインタの位置にレイ飛ばし、ヒットしたものを保存
        pointer.position = Input.mousePosition;
        EventSystem.current.RaycastAll(pointer, results);

        // ヒットしたUIの名前
        foreach (RaycastResult target in results)
        {
            if (target.gameObject.tag == "Piece")
            {
                Debug.Log(target.gameObject.name);
                _Piece = target.gameObject.GetComponent<Piece>();
            }
            else 
            {                
              break;             
            }                           
        }
        return _Piece;
    }


    /// <summary>
    /// ワールド座標を返す（RectTransform➡World座標に変換し、インスタンスを生成するため）
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector3 GetPieceWorldPos(Vector2 pos, int width)
    {
        var sizew = (Screen.width) / 2;
        var sizeh = (Screen.height) / 2;
        //return new Vector3(pos.x * Width + (Width / 2), pos.y * Width + (Width / 2), 0);
        return new Vector3(pos.x * width + (width / 2) + sizew, pos.y * width + (width / 2) + sizeh, 0);
    }
  
    /// <summary>
    /// ピースが落下地点のワールド座標を返す
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="num"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public Vector3 FallGetPieceWorldPos(Vector2 pos, int num, int width)
    {
        var sizew = (Screen.width) / 2;
        var sizeh = (Screen.height) / 2;
        return new Vector3(pos.x * width + (width / 2) + sizew, ((pos.y + num) * width) + (width / 2) + sizeh, 0);
    }

    /// <summary>
    /// 画像の初期化
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void Size_Adjustment(GameObject gameObject,float width,float height,float x, float y, float z) 
    {
        var sizew = (Screen.width);
        var sizeh = (Screen.height);
        var obj = gameObject.GetComponent<RectTransform>();
        obj.sizeDelta = new Vector2(sizew*width, sizeh*height);
        obj.transform.localPosition = new Vector3(x* sizew, y * sizeh, z);
    }

}
