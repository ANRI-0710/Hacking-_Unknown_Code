using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//３つ以上揃う場合削除するブロックの位置を保存する
public class Blocks
{
    private int x;
    private int y;
    private Piece_Type pieceType;

    public int Getx() { return x; }
    public int Gety() { return y; }
    public Piece_Type Getboxtype() { return pieceType; }

    public Blocks() { }

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
}

public enum Piece_Type
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

public class Piece : MonoBehaviour
{
    [SerializeField]
    private Sprite[] Sprites;

    [SerializeField]
    private Image[] Images;

    [SerializeField]
    private Piece_Type PieceColorState;

    public Piece_Type GetPieceState
    {
        get { return PieceColorState; }
        set
        {
            PieceColorState = value;
            ChangePieceColor();
        }
    }

    private Image _Image;
    private ParticleSystem _ParticleGameObject;

    private Animator _Animator;

    private RectTransform _RectTransForm;
    public RectTransform _GetRectTransForm
    {
        get { return _RectTransForm; }
        set
        {
            _RectTransForm = value;
        }

    }

    //private _PatricleObject;

    //配列番号所得用
    private int x;
    private int y;

    public int GetPieceX { get { return x; } set { x = value; } }
    public int GetPieceY { get { return y; } set { y = value; } }

    private void Awake()
    {
        _RectTransForm = this.GetComponent<RectTransform>();
        _Image = this.GetComponent<Image>();
        
        //Sprites = this.GetComponent<Sprite>();
        
    }

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

    IEnumerator ChangePiece(Piece_Type piecetype)
    {      
        this.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        //_Image = _Image[(int)piecetype];       

        _Image.sprite = Sprites[(int)piecetype];  
        //var p = this.GetComponent<Image>();
        //p = Images[(int)piecetype];
    }

    public void SetSize(int size) 
    {
        //this._RectTransForm.sizeDelta = Vector2.one * size;  
        this._RectTransForm.sizeDelta = new Vector2(0.95f,0.95f) * size;
    }

    public void PieceAnimationTrue() 
    {
        _Animator = GetComponent<Animator>();
        _Animator.SetBool("Piece_Select", true);
    }

    public void PieceAnimationFalse()
    {
        _Animator = GetComponent<Animator>();
        _Animator.SetBool("Piece_Select", false);
    }

}
