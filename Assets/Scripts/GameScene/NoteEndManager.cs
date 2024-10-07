using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEndManager : MonoBehaviour
{
    public event Action OnNoteMiss;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 10)
        {
            OnNoteMiss?.Invoke();
            Destroy(other.gameObject);
        }
    }
}
