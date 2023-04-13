using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemButtonSelect : MonoBehaviour,ISelectHandler,IDeselectHandler
{
    public Texture selectTex;
    public RawImage itemTexBillboard;

    public void Update()
    {
        itemTexBillboard = GameObject.Find("itemTexBillboard").GetComponent<RawImage>() ;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("selected");
        itemTexBillboard.texture = selectTex;
        itemTexBillboard.color = new Color(1, 1, 1, 1);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("NOselected");
        itemTexBillboard.texture = null;
        itemTexBillboard.color = new Color(1,1,1,0);
    }

    
}
