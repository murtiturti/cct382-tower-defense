using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour
{
    private Transform objectTrans;
    private Vector3 origScale;

    private void Start()
    {
        objectTrans = gameObject.transform;
        origScale = objectTrans.localScale;
    }

    public void HoverButton()
    {
        objectTrans.localScale = new Vector3(1.15f * origScale.x, 1.15f * origScale.y, origScale.z);
    }

    public void HoverOffButton()
    {
        objectTrans.localScale = origScale;
    }
}
