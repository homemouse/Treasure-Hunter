using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

using UnityEngine.UI;

public class DialogBoxItemCtrl : MonoBehaviour
{
    [SerializeField] Image DialogBoxbackground;
    [SerializeField] Text contentText;

    public string text
    {
        set {_text = value; Textchange_envet();  }
        get {return _text;}
    }
    [SerializeField] private string _text = "";

    void Textchange_envet()
    {
        if (contentText.text == "")
        {
            DialogBoxbackground.rectTransform.localScale = Vector3.zero;
            return;
        }
        DialogBoxbackground.rectTransform.localScale = Vector3.one;
        float i = contentText.rectTransform.sizeDelta.y;
        DialogBoxbackground.rectTransform.sizeDelta = new Vector2(DialogBoxbackground.rectTransform.sizeDelta.x, i + 40);
    }

    private void Start()
    {
        DialogBoxbackground.rectTransform.localScale = Vector3.zero;
    }

}
