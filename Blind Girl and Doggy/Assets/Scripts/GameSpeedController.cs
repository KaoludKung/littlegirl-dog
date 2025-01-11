using UnityEngine;

public class GameSpeedController : MonoBehaviour
{
    [SerializeField] private KeyCode toggleSpeedKey = KeyCode.F; // ?????????????????????? (????????????????)
    [SerializeField] private float normalSpeed = 1.0f; // ????????????
    [SerializeField] private float fastSpeed = 3.0f; // ?????????????????

    private bool isFast = false;

    void Update()
    {
        if (Input.GetKeyDown(toggleSpeedKey))
        {
            ToggleGameSpeed();
        }
    }

    private void ToggleGameSpeed()
    {
        isFast = !isFast;
        Time.timeScale = isFast ? fastSpeed : normalSpeed;

        Debug.Log($"Game speed changed to: {(isFast ? "Fast" : "Normal")}");
    }
}
