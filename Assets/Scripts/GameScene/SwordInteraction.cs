using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SwordInteraction : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] [Range(0, 1)] private float hapticIntensity = 0.2f;
    [SerializeField] private float hapticDuration = 0.1f;
    [SerializeField] private bool isLeftSword;
    private ActionBasedController controllerLeft;
    private ActionBasedController controllerRight;
    public event Action OnCubeNoteHit;


    private void Start()
    {
        controllerLeft = GameObject.FindWithTag("LeftHandController")?.GetComponent<ActionBasedController>();
        controllerRight = GameObject.FindWithTag("RightHandController")?.GetComponent<ActionBasedController>();

        if (controllerLeft == null || controllerRight == null)
        {
            Debug.LogError("Could not find XR controllers. Left: " + controllerLeft + ", Right: " + controllerRight);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 10) // CubeNote
        {
            Destroy(other.gameObject);
            OnCubeNoteHit?.Invoke();
            InvokeHapticImpulse();
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    void InvokeHapticImpulse()
    {
        if (isLeftSword && controllerLeft != null)
        {
            controllerLeft.SendHapticImpulse(hapticIntensity, hapticDuration);
        }

        else if (!isLeftSword && controllerRight != null)
        {
            controllerRight.SendHapticImpulse(hapticIntensity, hapticDuration);
        }
    }
}
