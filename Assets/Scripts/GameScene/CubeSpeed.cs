using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpeed : MonoBehaviour
{
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameManager.Instance.OnPauseStart += SetSpeedZero;
        GameManager.Instance.OnPauseEnd += SetSpeed;
        SetSpeed();
    }

    private void SetSpeedZero()
    {
        rb.velocity = Vector3.zero;
        Debug.Log("CubeNoteSpeed paused: " + rb.velocity);
    }

    public void SetSpeed()
    {
        rb.velocity = Vector3.back * GameManager.Instance.GetNoteSpeed();
        Debug.Log("CubeNoteSpeed: " + rb.velocity);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPauseStart -= SetSpeedZero;
        GameManager.Instance.OnPauseEnd -= SetSpeed;
    }
}
