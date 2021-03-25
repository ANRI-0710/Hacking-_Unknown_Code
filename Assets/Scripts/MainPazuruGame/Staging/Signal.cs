using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  //スタート・クリア・第一層~3層までの合図アニメーション管理クラス
/// </summary>

public class Signal : MonoBehaviour
{

    //スタート・クリア・第一層~3層までの合図アニメーション
    [SerializeField]
    private GameObject _SignalObject;

    //[SerializeField]
    //private Text _Text;

    public GameObject SignalObject(string str, RectTransform transforms) {

        var obj =  Instantiate(_SignalObject);
        var objchild = obj.transform.GetChild(1).gameObject;
        objchild.GetComponent<Text>().text = str;

        obj.transform.position = new Vector3(Screen.width, Screen.height, 0);
        obj.transform.SetParent(transforms);
        return obj;

    }




}
