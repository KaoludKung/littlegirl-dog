using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private float moveDelay = 0.25f;
    private float joystickThreshold = 0.6f;
    private string lastConnectedDevice = "";
    private string currentDevice;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        currentDevice = GetCurrentInputDevice();

        if (lastConnectedDevice != currentDevice)
        {
            lastConnectedDevice = currentDevice;
            //Input.ResetInputAxes();
            Debug.Log("Input device switched to: " + currentDevice);
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

    public bool IsZPressed()
    {
        return Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.JoystickButton1);
    }

    public bool IsXPressed()
    {
        return Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton2);
    }

    public bool IsQPressed()
    {
        return Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4);
    }

    public bool IsEPressed()
    {
        return Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5);
    }

    public bool IsEnterPressed()
    {
        return Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton9);
    }

    public bool IsShiftPressed()
    {
        return Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.JoystickButton8);
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
