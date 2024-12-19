using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class SwordInteraction : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] [Range(0, 1)] private float hapticIntensity = 0.2f;
    [SerializeField] [Range(0, 1)] private float hapticIntensityWronghit = 0.4f;
    [SerializeField] private float hapticDuration = 0.1f;
    [SerializeField] private float hapticDurationWrongHit = 0.15f;
    [SerializeField] private bool isLeftSword;
    [SerializeField] private float minPitch = 0.7f;
    [SerializeField] private float maxPitch = 1.3f;
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 10) // CubeNote
        {
            CorrectHit correctHit = other.gameObject.GetComponent<CorrectHit>();
            if (correctHit.GetCorrectHit() == true)
            {
                if (isLeftSword)
                {
                    if (other.gameObject.tag == "BlueCube" && other.gameObject)
                    {
                        OnCubeNoteHit?.Invoke();
                        InvokeHapticImpulse();
                    }
                    else
                    {
                        Debug.Log("Wrong Color!");
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
                        Debug.Log("Wrong Color!");
                        OnWrongHit?.Invoke();
                        InvokeHapticImpulseWrongHit();
                    }
                }
            }
            else
            {
                Debug.Log("Wrong Hit!");
                OnWrongHit?.Invoke();
                InvokeHapticImpulseWrongHit();
            }
            PlayRandomPitchedSlashEffect();
            Destroy(other.gameObject);
        }
    }

    private void InvokeHapticImpulse()
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

    private void InvokeHapticImpulseWrongHit()
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

    private void PlayRandomPitchedSlashEffect()
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
