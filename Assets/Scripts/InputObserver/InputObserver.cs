using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputObserver : InputControls.IGamePlayActions, InputControls.IComboSequenceActions
{
    private readonly InputControls inputControl;
    public event Action OnShootAction;
    public event Action<Vector2> OnCursorMoveAction;
    public event Action<Vector2> OnGamepadStickAction;

    public InputObserver()
    {
        inputControl = new InputControls();
        inputControl.GamePlay.AddCallbacks(this);
        inputControl.ComboSequence.AddCallbacks(this);
        inputControl.GamePlay.Enable();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnShootAction?.Invoke();
        }
    }

    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        OnCursorMoveAction?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnGamepadStick(InputAction.CallbackContext context)
    {
        OnGamepadStickAction?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnGamepadButtons(InputAction.CallbackContext context)
    {
        Debug.Log((int)context.ReadValue<float>());
    }
}
