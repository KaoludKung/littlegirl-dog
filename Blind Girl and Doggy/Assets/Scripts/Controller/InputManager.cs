using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private Dictionary<string, KeyCode> buttonMapping;
    private float moveDelay = 0.25f;
    private float joystickThreshold = 0.6f;
    private string lastConnectedDevice = "";
    private string currentDevice;

    private float checkInterval = 2.5f;
    private float timeSinceLastCheck = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Set up a controller first
        MappingButton(1);
    }

    void Update()
    {
        currentDevice = GetCurrentInputDevice();
        if (lastConnectedDevice != currentDevice)
        {
            lastConnectedDevice = currentDevice;
            Debug.Log("Input device switched to: " + currentDevice);
        }

        timeSinceLastCheck += Time.unscaledDeltaTime;

        if (timeSinceLastCheck >= checkInterval)
        {
            timeSinceLastCheck = 0f;

            string[] connectedJoysticks = Input.GetJoystickNames();
            string activeJoystickName = null;

            foreach (string joystick in connectedJoysticks)
            {
                if (!string.IsNullOrEmpty(joystick))
                {
                    activeJoystickName = joystick.Trim().ToLower();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(activeJoystickName))
            {
                if (activeJoystickName.Contains("wireless controller") || activeJoystickName.Contains("sony") || activeJoystickName.Contains("playstation"))
                {
                    MappingButton(2);
                }
                else if (activeJoystickName.Contains("xbox") || activeJoystickName.Contains("microsoft"))
                {
                    MappingButton(1);
                }
                else
                {
                    MappingButton(1);
                }
            }
        }
    }

    string GetCurrentInputDevice()
    {
        string[] joystickNames = Input.GetJoystickNames();
        bool isJoystickConnected = false;
        foreach (var joystickName in joystickNames)
        {
            if (!string.IsNullOrEmpty(joystickName))
            {
                isJoystickConnected = true;
                break;
            }
        }
        return isJoystickConnected ? "Joystick" : "Keyboard";
    }

    public void MappingButton(int controller)
    {
        switch (controller)
        {
            //Mapping Xbox
            case 1:
                Debug.Log("Mapping Controller To Xbox");
                buttonMapping = new Dictionary<string, KeyCode>{
                    { "Z", KeyCode.JoystickButton0 },
                    { "X", KeyCode.JoystickButton1 },
                    { "Q", KeyCode.JoystickButton4 },
                    { "E", KeyCode.JoystickButton5 },
                    { "Enter", KeyCode.JoystickButton7 },
                    { "Shift", KeyCode.JoystickButton6 }
                };
                break;
            // Mapping PS4
            case 2:
                Debug.Log("Mapping Controller To PS4");
                buttonMapping = new Dictionary<string, KeyCode>{
                    { "Z", KeyCode.JoystickButton1 },
                    { "X", KeyCode.JoystickButton2 },
                    { "Q", KeyCode.JoystickButton4 },
                    { "E", KeyCode.JoystickButton5 },
                    { "Enter", KeyCode.JoystickButton9 },
                    { "Shift", KeyCode.JoystickButton8 }
                };
                break;
        }
    }

    public bool IsZPressed()
    {
        return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(buttonMapping["Z"]);
    }

    public bool IsXPressed()
    {
        return Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(buttonMapping["X"]);
    }

    public bool IsQPressed()
    {
        return Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(buttonMapping["Q"]);
    }

    public bool IsEPressed()
    {
        return Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(buttonMapping["E"]);
    }

    public bool IsEnterPressed()
    {
        return Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(buttonMapping["Enter"]);
    }

    public bool IsShiftPressed()
    {
        return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(buttonMapping["Shift"]);
    }

    public bool IsUpPressed(ref float lastMoveTime)
    {
        if (currentDevice == "Keyboard" && Input.GetKeyDown(KeyCode.UpArrow))
        {
            return true;
        }

        if (Time.unscaledTime - lastMoveTime > moveDelay)
        {
            if (currentDevice == "Joystick" && Input.GetAxisRaw("Vertical") > joystickThreshold)
            {
                lastMoveTime = Time.unscaledTime;
                return true;
            }
        }
        return false;
    }

    public bool IsDownPressed(ref float lastMoveTime)
    {
        if (currentDevice == "Keyboard" && Input.GetKeyDown(KeyCode.DownArrow))
        {
            return true;
        }

        if (Time.unscaledTime - lastMoveTime > moveDelay)
        {
            if (currentDevice == "Joystick" && Input.GetAxisRaw("Vertical") < -joystickThreshold)
            {
                lastMoveTime = Time.unscaledTime;
                return true;
            }
        }
        return false;
    }

    public bool IsLeftPressed(ref float lastMoveTime)
    {
        if (currentDevice == "Keyboard" && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return true;
        }

        if (Time.unscaledTime - lastMoveTime > moveDelay)
        {
            if (currentDevice == "Joystick" && Input.GetAxisRaw("Horizontal") < -joystickThreshold)
            {
                lastMoveTime = Time.unscaledTime;
                return true;
            }
        }
        return false;
    }

    public bool IsRightPressed(ref float lastMoveTime)
    {
        if (currentDevice == "Keyboard" && Input.GetKeyDown(KeyCode.RightArrow))
        {
            return true;
        }

        if (Time.unscaledTime - lastMoveTime > moveDelay)
        {
            if (currentDevice == "Joystick" && Input.GetAxisRaw("Horizontal") > joystickThreshold)
            {
                lastMoveTime = Time.unscaledTime;
                return true;
            }
        }
        return false;
    }

    //for dog movement
    public bool IsWalkingLeft()
    {
        if (currentDevice == "Keyboard" && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return true;
        }
        else if (currentDevice == "Joystick" && Input.GetAxis("Horizontal") < -joystickThreshold)
        {
            return true;
        }
        return false;
    }

    public bool IsWalkingRight()
    {
        if (currentDevice == "Keyboard" && Input.GetKeyDown(KeyCode.RightArrow))
        {
            return true;
        }
        else if (currentDevice == "Joystick" && Input.GetAxis("Horizontal") > joystickThreshold)
        {
            return true;
        }
        return false;
    }
}
