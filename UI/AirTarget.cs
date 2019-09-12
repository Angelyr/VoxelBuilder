using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AirTarget : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(AirTargetMode);
    }

    private void AirTargetMode()
    {
        References.player.ToggleAirTarget();
    }
}