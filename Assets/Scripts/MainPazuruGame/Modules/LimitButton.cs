using UnityEngine;
using UnityEngine.UI;

public class LimitButton : MonoBehaviour
{
    [SerializeField]
    private Sprite[] Sprite;

    [SerializeField]
    private Button _Button;
    private Image _AttackImage;

    private bool isButtonPress;
    public bool GetisButtonPress { get => isButtonPress; set { isButtonPress = value; } }
      
    private E_SpecialAttack SpecialAttackType;
    public E_SpecialAttack GetSpecialAttackType
    {
        get => SpecialAttackType;
        set 
        {
            SpecialAttackType = value;
            SetSpecialAttackImage(SpecialAttackType);
        }     
    }

     void Awake()
    {

        var obj = transform.GetChild(0).gameObject;
        _AttackImage = obj.GetComponent<Image>();
        _Button = this.GetComponent<Button>();
        _Button.onClick.AddListener(ButtonPressed);
    }

    private void SetSpecialAttackImage(E_SpecialAttack SpecialAttackType) 
    {
        switch (SpecialAttackType) 
        {            
            case E_SpecialAttack.SP_Red:
                _AttackImage.sprite = Sprite[(int)E_SpecialAttack.SP_Red];
                break;
            case E_SpecialAttack.SP_Blue:
                _AttackImage.sprite = Sprite[(int)E_SpecialAttack.SP_Blue];
                break;

            case E_SpecialAttack.SP_Yellow:
                _AttackImage.sprite = Sprite[(int)E_SpecialAttack.SP_Yellow];
                break;

            case E_SpecialAttack.SP_Green:
                _AttackImage.sprite = Sprite[(int)E_SpecialAttack.SP_Green];
                break;

            case E_SpecialAttack.SP_White:
                _AttackImage.sprite = Sprite[(int)E_SpecialAttack.SP_White];
                break;

            case E_SpecialAttack.SP_HorizontalOneArray:
                _AttackImage.sprite = Sprite[(int)E_SpecialAttack.SP_HorizontalOneArray];
                break;

            case E_SpecialAttack.SP_VerticalOneArray:
                _AttackImage.sprite = Sprite[(int)E_SpecialAttack.SP_VerticalOneArray];
                break;

            case E_SpecialAttack.SP_Destroy_Cross:
                _AttackImage.sprite = Sprite[(int)E_SpecialAttack.SP_Destroy_Cross];
                break;

            case E_SpecialAttack.SP_ObliqueCross_Cross:
                _AttackImage.sprite = Sprite[(int)E_SpecialAttack.SP_ObliqueCross_Cross];
                break;

            case E_SpecialAttack.SP_BlackPiece_Destroy:
                _AttackImage.sprite = Sprite[(int)E_SpecialAttack.SP_BlackPiece_Destroy];
                break;
        }
    }

    public void ButtonPressed() 
    {
        GetisButtonPress = true;
        Debug.Log("GetisButtonPress" + GetisButtonPress);

    } 














}
