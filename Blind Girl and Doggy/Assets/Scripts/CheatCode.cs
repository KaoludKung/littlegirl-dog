using System.Collections;
using UnityEngine;

public class CheatCode : MonoBehaviour
{
    private string cheatCode = "UP,UP,DOWN,DOWN,LEFT,RIGHT,LEFT,RIGHT,B,A,";
    private string currentInput = "";
    private float inputDelay = 0.2f;  // ???????????????????? (???????????????)

    void Update()
    {
        // ??????????????????????????????
        if (Time.time % inputDelay < 0.1f)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                AddInput("UP");
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                AddInput("DOWN");
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                AddInput("LEFT");
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                AddInput("RIGHT");
            else if (Input.GetKeyDown(KeyCode.B))
                AddInput("B");
            else if (Input.GetKeyDown(KeyCode.A))
                AddInput("A");
            else if (Input.GetKeyDown(KeyCode.Return))
                AddInput("START");
        }
    }

    void AddInput(string key)
    {
        currentInput += key + ",";
        Debug.Log(currentInput);

        // ?????????????????????????????????????
        if (currentInput == cheatCode)
        {
            ActivateCheat();
            currentInput = "";  // ?????????????????????????
        }
        else if (!cheatCode.StartsWith(currentInput))  // ?????????? ??????
        {
            currentInput = "";
            Debug.Log("Reset Try Again");
        }
    }

    void ActivateCheat()
    {
        Debug.Log("Cheat Activated!");
    }
}
