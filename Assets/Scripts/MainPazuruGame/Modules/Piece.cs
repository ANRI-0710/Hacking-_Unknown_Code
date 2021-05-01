using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public enum Piece_Type //ピースタイプ
{
    RED,
    BLUE,
    GREEN,
    YELLOW,
    WHITE,
    BLACK,
    FEVER_4,
    FEVER_5,
    FALL,
    EMPTY,
}

//３つ以上揃う場合削除するブロックの位置を保存するクラス
public class Blocks
{
    private int x;   
    private int y;  
    private Piece_Type pieceType;

    public int Getx() { return x; }
    public int Gety() { return y; }
    public Piece_Type Getboxtype() { return pieceType; }

    //コンストラクタ1
    public Blocks(int x, int y)
    {
        this.x = x;
        this.y = y;
    }    
    //コンストラクタ
    public Blocks(int x, int y, Piece_Type pieceType)
    {
        this.x = x;
        this.y = y;
        this.pieceType = pieceType;
    }

<<<<<<< HEAD
    internal int indexof()
    {
        throw new NotImplementedException();
    }
}

=======
>>>>>>> f061488f36f51569aa254191514fb7a1a159ec79
/// <summary>
/// パズルピース（1個単位）クラス
/// </summary>
public class Piece : MonoBehaviour
{
    //ピースクラス
    [SerializeField]
    private Sprite[] Sprites;    
    
    //ピースの画像
    [SerializeField]
    private Image[] Images;
    private Image _pieceImage;

    //ピースタイプ（エディタ上で確認するため）
    [SerializeField]
    private Piece_Type PieceColorState;

    //ピースタイプを受け取り、enumが変わった場合、イメージを変更する
    public Piece_Type GetPieceState
    {
        get { return PieceColorState; }
        set
        {
            PieceColorState = value;
            ChangePieceColor();
        }
    }

    //座標所得
    private RectTransform _rectTransForm;
    public RectTransform GetRectTransForm
    {
        get { return _rectTransForm; }
        set
        {
            _rectTransForm = value;
        }

    }

    //ピースの座標位置及び配列番号の登録をしてコントロールをする
    private int x;
    private int y;
    public int GetPieceX { get { return x; } set { x = value; } }
    public int GetPieceY { get { return y; } set { y = value; } }

    private void Awake()
    {
        _rectTransForm = this.GetComponent<RectTransform>();
        _pieceImage = this.GetComponent<Image>();        
    }

    /// <summary>
    /// ピースの画像を変更する
    /// </summary>
    private void ChangePieceColor()
    {
        switch (PieceColorState)
        {
            case Piece_Type.RED:              
                StartCoroutine(ChangePiece(Piece_Type.RED));
                break;
            case Piece_Type.GREEN:               
                StartCoroutine(ChangePiece(Piece_Type.GREEN));
                break;
            case Piece_Type.BLUE:               
                StartCoroutine(ChangePiece(Piece_Type.BLUE));
                break;
            case Piece_Type.YELLOW:               
                StartCoroutine(ChangePiece(Piece_Type.YELLOW));
                break;
            case Piece_Type.WHITE:               
                StartCoroutine(ChangePiece(Piece_Type.WHITE));
                break;
            case Piece_Type.BLACK:               
                StartCoroutine(ChangePiece(Piece_Type.BLACK));
                break;
            case Piece_Type.FEVER_4:                
                StartCoroutine(ChangePiece(Piece_Type.FEVER_4));
                break;
            case Piece_Type.FEVER_5:               
                StartCoroutine(ChangePiece(Piece_Type.FEVER_5));
                break;
            case Piece_Type.EMPTY:
                this.GetComponent<Image>().enabled = false;
                break;

        }
    }

    /// <summary>
    /// 落下演出のため、0.1秒間を置いて、イメージ画像を変更する
    /// </summary>
    /// <param name="piecetype"></param>
    /// <returns></returns>
    IEnumerator ChangePiece(Piece_Type piecetype)
    {      
        this.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.1f);   
        _pieceImage.sprite = Sprites[(int)piecetype];  

    }

    /// <summary>
    /// 画面サイズに併せてピースのサイズを変更する
    /// </summary>
    /// <param name="size"></param>
    public void SetSize(int size) { this._rectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * size; }


}
