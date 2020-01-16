using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public Button[] playerInputs = { null, null, null, null };
    public bool[] activeInputs = { true, true, true, true };
    // Start is called before the first frame update
    void Start()
    {
        ToggleButtonList();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ToggleButtonList()
    {
        for (int i = 0; i < playerInputs.Length; i++)
        {
            ToggleButton(playerInputs[i], activeInputs[i]);
        }
    }
    void ToggleButton(Button button, bool active)
    {
        if (button == null) return;
        button.interactable = active;
    }
}
