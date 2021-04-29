using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///�p�Y���s�[�X�̔Ֆʋy�уs�[�X�̏������Ǘ�����N���X 
/// </summary>
public class PieceManager : MonoBehaviour
{
    //�p�Y���s�[�X�̐���   
    [SerializeField]
    private GameObject _PazuruPrefab;

    //�s�[�X�I�u�W�F�N�g�ւ̃A�N�Z�X�i�s�[�X�j�󂷂��Destroy�w���̍ۂɎg�p�j
    private GameObject[,] _pieceGameObjects = new GameObject[Cols, Rows];

    //�s�[�X�N���X�ւ̃A�N�Z�X
    private Piece[,] _pieces = new Piece[Cols, Rows];
    public Piece[,] GetPieces { get => _pieces; set { _pieces = value; } }

    //�^�b�v�����s�[�X�̕ۑ�����
    private Piece _mouseDown;
    private Piece _mouseUp;
    private Piece _inputMemoryPiece;

    //3�}�b�`�`�F�b�N�p�E�����F�̃s�[�X��3�ȏ㑵���Ă��邩�A�㉺���E�Ɋm�F���邽�߂̔z��B���s�[�X�����_�ɏ\�������ɔj��ł������p�Y���̔j�󎞂ɂ��g�p
    private int[] _Same_Block_Check_Direction_X = new int[] { 0, 1, 0, -1 };
    private int[] _Same_Block_Check_Direction_Y = new int[] { 1, 0, -1, 0 };

    //�s�[�X�����_��3�~3�����ɔj��ł������p�Y���̔j�󎞂Ɏg�p����`�F�b�N�z��
    private int[] _Same_5_Block_Cheack_Direction_X = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 };
    private int[] _Same_5_Block_Cheack_Direction_Y = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };

    //�폜����s�[�X�̍��W�ʒu��ۑ�����i3�}�b�`���Ĕj�󂵂��p�Y�������X�g�i���W�ʒu�ۑ��j�ɓ����j
    private List<Blocks> _DestroyPieceList = new List<Blocks>();

    //����A���������������L�^����ϐ�
    private int ComboCount;

    //3�}�b�`�����s�[�X���A�����Ĕj�󂷂�ۂ̑��x�����i�G�f�B�^��Œ����ł���悤��SerializeField�ɂ���j
    [SerializeField]
    private float WaitTime;

    //��[�s�[�X���p�Y���Ղɒ�ʒu�ɍĐ��������ۂ́A�s�[�X�������鑬�x�i�G�f�B�^��Œ����ł���悤��SerializeField�ɂ���j
    [SerializeField]
    private float FallTime;

    //�s�[�X�̍s�E��
    private const int Cols = 7;
    private const int Rows = 7;

    //�s�[�X�̃}�b�`���̒萔
    private const int _piece_Match_3 = 3;
    private const int _piece_Match_4 = 4;
    private const int _piece_Match_5 = 5;

    //�s�[�X�Đ����ɂǂ̒n�_����recttransform��̂ǂ��n�_���琶�����邩�̒����p�ϐ�
    private int FallNum = 9;

    //��ʔ䗦�T�C�Y
    private int Width;

    //���W�ϊ��N���X�i�C���X�^�G�C�g�̍ۂ�UI���W���烏�[���h���W�ɕϊ�����K�v�����邽�߁j
    [SerializeField]
    private VectorReturn _InputClass;

    //�s�[�X�̕\���ʒu�̌Œ�
    [SerializeField]
    private RectTransform _PieceTrans;

    //�p�[�e�B�N���̐���
    [SerializeField]
    private RectTransform _ParticleTransform;

    public void Awake()
    {
        //��ʃT�C�Y���󂯎��  
        Width = (Screen.width / Cols);
    }

    /// <summary>
    /// �p�Y���{�[�h�Ƀs�[�X�𐶐�����
    /// </summary>
    /// 
    public void InitBoard()
    {       
        Debug.Log(Width);
        //��������s�[�X�̏���ۑ�����C���X�^���X
        var obj = Instantiate(_PazuruPrefab, new Vector3(10, 10, 10), Quaternion.identity);
        _inputMemoryPiece = obj.GetComponent<Piece>();
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                CreatePiece(new Vector2(i, k));
            }
        }
        _mouseDown = _pieces[0, 0];
        _mouseUp = _pieces[6, 6];
    }


    /// <summary>
    /// �s�[�X�̍Đ���
    /// </summary>
    /// <param name="pos">���W�ʒu</param>
    public void CreatePiece(Vector2 pos)
    {
        //�ォ�獂���ɗ��������邽�߁AFallNum����Ƀs�[�X��\��������
        var criatePos = _InputClass.GetPieceWorldPos(pos, Width);

        //�s�[�X�𐶐�����
        _pieceGameObjects[(int)pos.x, (int)pos.y] = Instantiate(_PazuruPrefab, criatePos, Quaternion.identity);
        var piece = _pieceGameObjects[(int)pos.x, (int)pos.y].GetComponent<Piece>();

        piece.GetPieceX = (int)pos.x;
        piece.GetPieceY = (int)pos.y;
        piece.transform.SetParent(_PieceTrans);
        piece.SetSize(Width);

        //�s�[�X��ނ̗���
        var randomNum = Random.Range((int)Piece_Type.RED, (int)Piece_Type.BLACK);
        piece.GetPieceState = (Piece_Type)randomNum;
        piece.name = "piecetest" + (int)pos.x + " " + (int)pos.y;
        _pieces[(int)pos.x, (int)pos.y] = piece;
        _InputClass.GetRaycastResults.Clear();
    }

    /// <summary>
    /// �s�[�X���\�������Ɍ������ꂽ���A���~�b�g�{�^���������ꂽ�������[�v�Ń`�F�b�N����
    /// </summary>
    /// <returns></returns>
    public IEnumerator PieceTapCheckLoop() 
    {
        while (true) 
        {
            ////���~�b�g�{�^���̔����`�F�b�N
            for (var i = 0; i < 2; i++)
            {
                if (GameManager.Instance.GetLimitManager[i])
                {
                    if (i == 0) {GameManager.Instance.GetSpecialAttack.SpecialAttack((E_SpecialAttack)GameManager.Instance.GetSpecialAttack1, _pieces); }
                    if (i == 1) { GameManager.Instance.GetSpecialAttack.SpecialAttack((E_SpecialAttack)GameManager.Instance.GetSpecialAttack2, _pieces); }
                    GameManager.Instance.GetLimitManager[i] = false;
                    GameManager.Instance.GetLimitInvocating = true;
                    yield return null;
                    GameManager.Instance.GetLimitManager.LimitInitValues();
                    yield break;
                }
            }

            //�s�[�X�̃^�b�v���m
            if (Input.GetMouseButtonDown(0))//�������u�Ԃ̃s�[�X�̏�������
            {
                _mouseDown = _InputClass.ReturnRaycastPiece(_pieces[0, 0]);
                //�I�������s�[�X��傫������
                _pieces[_mouseDown.GetPieceX, _mouseDown.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(1.5f, 1.5f) * Width;
                Debug.Log("MDX" + _mouseDown.GetPieceX + "MDY" + _mouseDown.GetPieceY);
            }
            else if (Input.GetMouseButtonUp(0)) //�������Ƃ��̃s�[�X�̏�������
            {
                _mouseUp = _InputClass.ReturnRaycastPiece(_pieces[6, 6]);
                _pieces[_mouseUp.GetPieceX, _mouseUp.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(1.5f, 1.5f) * Width;

                var xtap = (int)_mouseUp.GetPieceX - _mouseDown.GetPieceX;
                var ytap = (int)_mouseUp.GetPieceY - _mouseDown.GetPieceY;
                Vector2 poscheak = new Vector2(xtap, ytap);

                //�s�[�X�̗��������W����s�[�X���ŏ��ɏ����������W�������āA���������W���s�[�X�𒆐S�ɏ㉺���E�����̍��W�ł���΃s�[�X�̌���������
                if (poscheak == new Vector2(1, 0) || poscheak == new Vector2(0, 1) || poscheak == new Vector2(-1, 0) || poscheak == new Vector2(0, -1))
                {
                   GameManager.Instance.GetexchangeSatisfied = true;
                    yield break;
                }
                else
                {
                    //������Ȃ���΃^�b�v�����s�[�X�̑傫�������ɖ߂�
                    _pieces[_mouseDown.GetPieceX, _mouseDown.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
                    _pieces[_mouseUp.GetPieceX, _mouseUp.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
                }
            }
            yield return null;
        }

    }

    /// <summary>
    ///  �s�[�X����������
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExchangePiece()
    {
        Debug.Log("Exchange Block");
        ComboCount = 0;
        yield return null;
        //�s�[�X�̑傫�������ɖ߂�
        _pieces[_mouseDown.GetPieceX, _mouseDown.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;
        yield return null;
        _pieces[_mouseUp.GetPieceX, _mouseUp.GetPieceY].GetRectTransForm.sizeDelta = new Vector2(0.95f, 0.95f) * Width;

        //mouseDown�̍��W���i�[
        _inputMemoryPiece.GetPieceState = _pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState;
        yield return null;
        //mouseDown ���@mouseUp�ɑ��
        _pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState = _pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState;
        yield return null;
        //mouseDown ���@mouseUp�ɑ��
        _pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState = _inputMemoryPiece.GetPieceState;
        yield return null;

        //FEVER4 or FEVER5�̃s�[�X�����������ꍇ�́AFEVER�`�F�b�N��ʂ�             
        if (Fever_Exchange_Check(_mouseDown, _mouseUp))
        {
            GameManager.Instance.GetFeverCheck = true;
            //�G����̍U���܂ł̃J�E���g���X�V
            GameManager.Instance.GetEnemyManager.GetenemyConroll.GetEnemyAttackCount++;
            yield break;        
        }

    }

    /// <summary>
    /// [6]�A���`�E�C���X�p�Y���i�����Piece�j����ԉ���ɂ���ꍇ�A��������������
    /// </summary>
    /// <returns></returns>
    public IEnumerator ObstaclePieceCheck()
    {
        for (var i = 0; i < Rows; i++)
        {
            if (_pieces[i, 0].GetPieceState == Piece_Type.BLACK)
            {
                PuzzleSoundManager.Instance.SE_Selection(SE_Now.EnemyPieceDestroy);
                _pieces[i, 0].GetPieceState = Piece_Type.EMPTY;
                //isDestroy = true;
                GameManager.Instance.GetisDestroy = true;
                //DestroyPiecePaticles(new Vector2(i, 0));
            }
            if (_pieces[i, 0].GetPieceState == Piece_Type.EMPTY) { GameManager.Instance.GetisDestroy = true; }
        }
        yield return null;
    }

    /// <summary>
    ///[7]�S�s�[�X�̏�Ԃ�4�����Ɋm�F�B�R�ȏ㑵���Ă����烊�X�g�ɓ����EMPTY�t���O�i= Destroy�t���O�j�𗧂Ă�
    /// </summary>
    /// <returns></returns>
    public IEnumerator DestroyPieceCheak()
    {
        Piece_Size_Back();
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                for (var p = 0; p < _Same_Block_Check_Direction_X.Length; p++)
                {
                    bool _CanTurn = false;

                    _DestroyPieceList.Clear();

                    //�s�[�X�^�C�v�Ƀs�[�X�̎�ނ���
                    Piece_Type piecetype = _pieces[i, k].GetPieceState;

                    //���X�g�Ɋi�[
                    _DestroyPieceList.Add(new Blocks(i, k, _pieces[i, k].GetPieceState));

                    int pazurux = i;
                    int pazuruy = k;

                    //�㉺���E4�����ɓ����F�̃s�[�X�����邩�ǂ������������A�������ꍇ�̓��X�g�ɓ����
                    while (true)
                    {
                        //4�����̍��W�����ăp�Y���Ղ̔Ֆʂ̒[�܂Ō�������������
                        pazurux += _Same_Block_Check_Direction_X[p];
                        pazuruy += _Same_Block_Check_Direction_Y[p];

                        //�ՖʊO�ł���Ώ������I������
                        if (!(pazurux >= 0 && pazurux < Rows && pazuruy >= 0 && pazuruy < Cols)) break;

                        //���݌������Ă���s�[�X���Apiecetype�̐F�ƈ�v���Ă���A�X�ɃA���`�E�C���X�p�Y���ł͂Ȃ��ꍇ�A���X�g�ɒǉ�����
                        if (_pieces[pazurux, pazuruy].GetPieceState == piecetype && _pieces[pazurux, pazuruy].GetPieceState != Piece_Type.BLACK )
                        {
                            _DestroyPieceList.Add(new Blocks(pazurux, pazuruy, _pieces[pazuruy, pazurux].GetPieceState));

                            //���Ɍ�������s�[�X����
                            int pazuruxplus = pazurux + _Same_Block_Check_Direction_X[p];
                            int pazuruyplus = pazuruy + _Same_Block_Check_Direction_Y[p];

                            //���X�g��3�ȏ�s�[�X������A���Ɍ�������s�[�X���ՊO��������
                            if (_DestroyPieceList.Count >= 3 && !(pazuruxplus >= 0 && pazuruxplus < Rows && pazuruyplus >= 0 && pazuruyplus < Cols))
                            {
                                //�s�[�X�̔j�󉉏o���s��
                                _CanTurn = true;
                                break;
                            }
                            else { continue; }  //�łȂ���Ό�����������
                        }
                        else // ���݌������Ă���s�[�X����v���Ȃ����A���Ƀ��X�g�ɂR�����Ă���ꍇ�͔j�󉉏o�������s��
                        {
                            if (_DestroyPieceList.Count >= 3)
                            {
                                _CanTurn = true;
                                break;
                            }
                            break;
                        }
                    }

                    if (_CanTurn)
                    {
                        if (ComboCount == 0) //�R���{�J�E���g��0��̎��̂݁A�G����̍U���J�E���g�����炷
                        {                            
                            GameManager.Instance.GetEnemyManager.GetenemyConroll.GetEnemyAttackCount++;
                        }
                        if (piecetype != Piece_Type.EMPTY) //�s�[�X���󂶂�Ȃ��ꍇ�A�R���{�����v���X����
                        {
                            ComboCount++;
                        }
                        if (piecetype == Piece_Type.WHITE)
                        {
                            //�n�[�g�^�E�C���X�̏ꍇ�A�G�̐������Ԃ�5�b���΂��������s��
                            GameManager.Instance.GetEnemyManager.GetenemyConroll.GetEnemyTimeLimit += 5;
                            GameManager.Instance.GetUIManager.InitTextRecoveryDisplay(GameManager.Instance.GetUIManager.GetrectTransform);
                            yield return new WaitForSeconds(0.4f);
                            GameManager.Instance.GetUIManager.DestroyRecoveryDisplay();
                        }

                        //�G�ւ̍U������
                        GameManager.Instance.GetEnemyManager.GetenemyConroll.AttackEnemy(piecetype, ComboCount);

                        //�R���{�`�F�b�N�������j���o�s��
                        Pazuru_Combo_Count(_DestroyPieceList, _DestroyPieceList.Count);

                        //�j�󂷂�s�[�X���m�肷�邽�߁A�j�󏈗��̃t���O���I������
                        GameManager.Instance.GetisDestroy = true;                        
                        yield return new WaitForSeconds(0.2f);
                        
                        //���R���{����\������
                        GameManager.Instance.GetUIManager.InitTextComboDisplay(GameManager.Instance.GetUIManager.GetrectTransform, ComboCount);
                        yield return null;
                        GameManager.Instance.GetUIManager.DestroyTextComboDisplay();
                    }
                }

            }

        }
        yield return null;

        if (!GameManager.Instance.GetisDestroy)
        {
            ////�R���{�J�E���g��0�̏ꍇ�A3�����Ă���s�[�X���Ȃ��i���������Ă�3�}�b�`���������Ă��Ȃ��j���߁A�^�b�v�����̃R���[�`���ɖ߂�
            if (ComboCount == 0)
            {
                //���������s�[�X�����ɖ߂�
                _inputMemoryPiece.GetPieceState = _pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState;
                yield return null;
                //mouseDown ���@mouseUp
                _pieces[(int)_mouseUp.GetPieceX, (int)_mouseUp.GetPieceY].GetPieceState = _pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState;
                yield return null;
                _pieces[(int)_mouseDown.GetPieceX, (int)_mouseDown.GetPieceY].GetPieceState = _inputMemoryPiece.GetPieceState;
            }
        }
        yield break;
    }

    /// <summary>
    /// �c��̃s�[�X�����ɋl�߂�
    /// </summary>
    /// <returns></returns>
    public IEnumerator DownPackPiece()
    {
        Debug.Log("DownPackPiece");
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                //�Y���s�[�X���������Ɍ������������A�s�[�X���󂾂�����A�����������s��
                if (_pieces[i, k].GetPieceState == Piece_Type.EMPTY)
                {
                    int pazurux = i;
                    int pazuruy = k;

                    while (pazurux >= 0 && pazurux < Rows && pazuruy >= 0 && pazuruy < Cols)
                    {
                        var exchangepazuru = _pieces[pazurux, pazuruy].GetPieceState;
                        if (exchangepazuru != Piece_Type.EMPTY)
                        {
                            _pieces[i, k].GetPieceState = exchangepazuru;
                            _pieces[pazurux, pazuruy].GetPieceState = Piece_Type.EMPTY;
                            break;
                        }
                        pazurux += _Same_Block_Check_Direction_X[0];
                        pazuruy += _Same_Block_Check_Direction_Y[0];
                        yield return new WaitForSeconds(WaitTime);
                    }
                }
            }
        }
        yield break;
    }

    /// <summary>
    /// �폜�t���O�������Ă���s�[�X���폜����
    /// </summary>
    /// <returns></returns>
    public IEnumerator DestroyPiece()
    {
        Debug.Log("DestroyPiece");
        _DestroyPieceList.Clear();
        for (var i = 0; i < Cols; i++)
        {
            for (var k = 0; k < Rows; k++)
            {
                if (_pieces[i, k].GetPieceState == Piece_Type.EMPTY)
                {
                    Destroy(_pieceGameObjects[i, k]);
                    yield return new WaitForSeconds(0.01f);
                    _DestroyPieceList.Add(new Blocks(i, k));
                }
            }
        }
    }

    /// <summary>
    /// �s�[�X���폜�������ƐV�����s�[�X�𐶐�����
    /// </summary>
    /// <returns></returns>
    public IEnumerator CreateNewPiece()
    {
        Debug.Log("CreateNewPiece()");
        foreach (var i in _DestroyPieceList)
        {
            var pos = _InputClass.FallGetPieceWorldPos(new Vector2(i.Getx(), i.Gety()), FallNum, Width);
            CreatePiece(new Vector2(i.Getx(), i.Gety()));

            _pieces[i.Getx(), i.Gety()].GetRectTransForm.position = pos;
            var pos2 = _InputClass.GetPieceWorldPos(new Vector2(i.Getx(), i.Gety()), Width);
            while (pos2.y <= pos.y)
            {
                pos.y -= FallTime;
                _pieces[i.Getx(), i.Gety()].GetRectTransForm.position = pos;
                yield return null;
            }
            _pieces[i.Getx(), i.Gety()].GetRectTransForm.position = pos2;
        }
    }

    /// <summary>
    /// �^�b�v���Č��������s�[�X��FEVER4�A5���������ꍇ�́A�폜�Ɣ��j�A�j���[�V�������Đ�����
    /// </summary>
    /// <param name="pazuru1">��������2�̃s�[�X</param>
    /// <returns></returns>
    public bool Fever_Exchange_Check(params Piece[] pazuru1)
    {
        bool isConbo = false;
        for (var i = 0; i < pazuru1.Length; i++)
        {
            var p = pazuru1[i].GetComponent<Piece>();
            switch (_pieces[(int)p.GetPieceX, (int)p.GetPieceY].GetPieceState)
            {
                case Piece_Type.FEVER_4:
                    DestroyCheak((int)p.GetPieceX, (int)p.GetPieceY, _Same_Block_Check_Direction_X, _Same_Block_Check_Direction_Y);
                    isConbo = true;
                    break;
                case Piece_Type.FEVER_5:
                    DestroyCheak((int)p.GetPieceX, (int)p.GetPieceY, _Same_5_Block_Cheack_Direction_X, _Same_5_Block_Cheack_Direction_Y);
                    isConbo = true;
                    break;
                default:
                    break;
            }
        }
        return isConbo;
    }

    /// <summary>
    /// �s�[�X�����}�b�`���Ă��邩�`�F�b�N���A4�R���{�̏ꍇ��Piece_Type��FEVER4�ɁA5�R���{�̏ꍇFEVER5�ɂ���
    /// </summary>
    /// <param name="destroylist">�}�b�`���Ă���s�[�X�̃��X�g</param>
    /// <param name="listcount">���X�g�̐�</param>
    private void Pazuru_Combo_Count(List<Blocks> destroylist, int listcount)
    {
        Debug.Log("Pazuru_Combo_Count");
        if (listcount == _piece_Match_5)
        {
            foreach (var i in destroylist)
            {

                if (i == destroylist.First())
                {
                    _pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.FEVER_5;
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                    GameManager.Instance.GetParticle.PlayerAttackToEnemy(_ParticleTransform);
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
                }
                else
                {
                    _pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.EMPTY;
                    GameManager.Instance.GetParticle.PlayerAttackToEnemy(_ParticleTransform);
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                }
                GameManager.Instance.GetLimitManager.LimitPlus();
            }

        }
        else if (listcount == _piece_Match_4)
        {
            foreach (var i in destroylist)
            {
                if (i == destroylist.First())
                {
                    _pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.FEVER_4;
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.SpecialAttackColor);
                }
                else
                {
                    _pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.EMPTY;
                    GameManager.Instance.GetParticle.PlayerAttackToEnemy(_ParticleTransform);
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                }
                GameManager.Instance.GetLimitManager.LimitPlus();

            }
        }
        else if (listcount == _piece_Match_3)
        {
            foreach (var i in destroylist)
            {
                if (i == destroylist.First())
                {
                    DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                    GameManager.Instance.GetParticle.PlayerAttackToEnemy(_ParticleTransform);
                    PuzzleSoundManager.Instance.SE_Selection(SE_Now.PuzzleDestroy);
                }
                _pieces[i.Getx(), i.Gety()].GetPieceState = Piece_Type.EMPTY;
                GameManager.Instance.GetParticle.PlayerAttackToEnemy(_ParticleTransform);
                DestroyPiecePaticles(new Vector2(i.Getx(), i.Gety()));
                GameManager.Instance.GetLimitManager.LimitPlus();
            }
        }

    }


    /// <summary>
    /// FEVER�s�[�X�����������ꍇ�A��������4or8�����ɑ΂��č폜�t���O�Ɣ��j�A�j���[�V�������s��
    /// </summary>
    /// <param name="i">piece�z���x</param>
    /// <param name="k">piece�z���y</param>
    /// <param name="arrayx">���j����x�̍��W</param>
    /// <param name="arrayy">���j����y�̍��W</param>
    private void DestroyCheak(int i, int k, int[] arrayx, int[] arrayy)
    {
        for (var p = 0; p < arrayx.Length; p++)
        {
            int px = i + arrayx[p];
            int py = k + arrayy[p];
            if (!(px >= 0 && px < Rows && py >= 0 && py < Cols)) continue;
            _pieces[px, py].GetPieceState = Piece_Type.EMPTY;
        }
        DestroyPiecePaticles(new Vector2(i, k));
        PuzzleSoundManager.Instance.SE_Selection(SE_Now.PuzzleDestroy);
        _pieces[i, k].GetPieceState = Piece_Type.EMPTY;
    }

    /// <summary>
    /// �s�[�X�̔j��p�[�e�B�N��
    /// </summary>
    /// <param name="pos"></param>
    private void DestroyPiecePaticles(Vector2 pos)
    {
        var criatePos = _InputClass.GetPieceWorldPos(pos, Width);
       GameManager.Instance.GetParticle.PlayerDestroyParticles(criatePos, _ParticleTransform);
    }

    /// <summary>
    /// �s�[�X�̃T�C�Y�����ɖ߂�
    /// </summary>
    private void Piece_Size_Back()
    {
        for (var i = 0; i < Rows; i++)
        {
            for (var k = 0; k < Cols; k++)
            {
                _pieces[i, k].SetSize(Width);
            }
        }
    }


}
