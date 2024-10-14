using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollissionTest : MonoBehaviour
{

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 10)
        {
            Debug.Log("Cube collision");
            Destroy(other.gameObject);
        }
    }
}
