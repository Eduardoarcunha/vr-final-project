using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [Header("Ray Interactors")]
    [SerializeField] private GameObject leftRayInteractor;
    [SerializeField] private GameObject rightRayInteractor;

    [Header("Input Actions")]
    [SerializeField] private InputActionProperty menuButton;

    public bool menuPressed { get; private set; }

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
            UIManager.instance.SetCanvasState(CanvasEnum.Collection, UIStateEnum.Toggle);
            leftRayInteractor.SetActive(!leftRayInteractor.activeSelf);
            rightRayInteractor.SetActive(!rightRayInteractor.activeSelf);
        }
    }

}
