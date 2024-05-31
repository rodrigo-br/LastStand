using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    protected Canvas canvas;

    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        //canvas.enabled = false;
    }

    public virtual void Enable()
    {
        canvas.enabled = true;
    }

    public virtual void Disable()
    {
        canvas.enabled = false;
    }
}
