using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    [SerializeField] private float amplitude = 10f;
    [SerializeField] private float frequency = 1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Berechne die neue Position basierend auf der Sinusfunktion
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Wende die neue Position an
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}
