using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModiManager : MonoBehaviour
{
    [SerializeField] private Toggle fasterGameToggle;
    [SerializeField] private Toggle slowerGameToggle;
    [SerializeField] private Toggle noFailToggle;

    public bool IsFasterGameSelected()
    {
        return fasterGameToggle != null && fasterGameToggle.isOn;
    }

    public bool IsSlowerGameSelected()
    {
        return slowerGameToggle != null && slowerGameToggle.isOn;
    }

    public bool IsNoFailModeSelected()
    {
        return noFailToggle != null && noFailToggle.isOn;
    }
}
