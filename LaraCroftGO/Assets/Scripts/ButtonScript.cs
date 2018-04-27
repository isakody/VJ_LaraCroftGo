using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text text;
    public bool isOver = false;
    public Color startColor = Color.black;
    // Use this for initialization
    void Start () {
        text = gameObject.GetComponent<Text>();
        startColor = text.color;
    }

    // Update is called once per frame
    void Update () {
        if (isOver)
        {
            text.color = Color.black;
        }
        else if (!isOver)
        {
            text.color = startColor;
        }
        
            

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
    }

}
