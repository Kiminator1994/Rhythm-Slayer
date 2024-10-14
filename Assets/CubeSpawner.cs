using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private float noteSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCube());
    }

    private IEnumerator SpawnCube()
    {
        Debug.Log("SpawnCube called....");
        yield return new WaitForSeconds(2);
        GameObject note = Instantiate(cube, this.gameObject.transform.position, Quaternion.identity);
        Rigidbody rb = note.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = new Vector3(0, 0, -noteSpeed); // Move the note toward the player (-Z direction)
        }
        Debug.Log("Cube spawned");
        StartCoroutine(SpawnCube());
    }
}
