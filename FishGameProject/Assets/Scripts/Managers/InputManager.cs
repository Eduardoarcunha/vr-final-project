using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [SerializeField] private InputActionProperty menuButton;

    public bool menuPressed { get; private set; }
    public bool rightPrimaryPressed { get; private set; }

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
        menuPressed = menuButton.action.WasPressedThisFrame() ? true : false;

        if (menuPressed)
        {
            UIManager.instance.SetCollectionCanvasState(UIStateEnum.Toggle);
        }
    }

}
