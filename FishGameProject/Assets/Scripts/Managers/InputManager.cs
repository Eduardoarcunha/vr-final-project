using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public InputActionProperty toggleCollectionPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {
        if (toggleCollectionPanel.action.WasPressedThisFrame())
        {
            UIManager.instance.ToggleCollectionPanel();
        }
    }

}
