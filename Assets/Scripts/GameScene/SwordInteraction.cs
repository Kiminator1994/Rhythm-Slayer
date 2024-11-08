using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SwordInteraction : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] [Range(0, 1)] private float hapticIntensity = 0.2f;
    [SerializeField][Range(0, 1)] private float hapticIntensityWronghit = 0.4f;
    [SerializeField] private float hapticDuration = 0.1f;
    [SerializeField] private float hapticDurationWrongHit = 0.15f;
    [SerializeField] private bool isLeftSword;
    private ActionBasedController controllerLeft;
    private ActionBasedController controllerRight;
    public event Action OnCubeNoteHit;
    public event Action OnWrongHit;


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
            if (isLeftSword) 
            {
                if (other.gameObject.tag == "BlueCube")
                {
                    OnCubeNoteHit?.Invoke();
                    InvokeHapticImpulse();
                }
                else 
                {
                    OnWrongHit?.Invoke();
                    InvokeHapticImpulseWrongHit();
                }
            }
            else
            {
                if (other.gameObject.tag == "RedCube")
                {
                    OnCubeNoteHit?.Invoke();
                    InvokeHapticImpulse();
                }
                else
                {
                    OnWrongHit?.Invoke();
                    InvokeHapticImpulseWrongHit();
                }
            }
            audioSource.PlayOneShot(audioSource.clip);
            Destroy(other.gameObject);
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

    void InvokeHapticImpulseWrongHit()
    {
        if (isLeftSword && controllerLeft != null)
        {
            controllerLeft.SendHapticImpulse(hapticIntensityWronghit, hapticDurationWrongHit);
        }

        else if (!isLeftSword && controllerRight != null)
        {
            controllerRight.SendHapticImpulse(hapticIntensityWronghit, hapticDurationWrongHit);
        }
    }
}
