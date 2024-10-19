using UnityEngine;

public class FollowController : MonoBehaviour
{
    public Transform controller;
    
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    void Update()
    {
        if (controller != null)
        {
            transform.position = controller.position + controller.TransformVector(positionOffset);

            Quaternion offsetRotation = Quaternion.Euler(rotationOffset);
            transform.rotation = controller.rotation * offsetRotation;
        }
    }
}
