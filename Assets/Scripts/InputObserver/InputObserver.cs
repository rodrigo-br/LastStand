using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputObserver : InputControls.IGamePlayActions, InputControls.IComboSequenceActions, InputControls.IUIActions
{
    private readonly InputControls inputControl;
    public event Action OnShootAction;
    public event Action<Vector2> OnCursorMoveAction;
    public event Action<Vector2> OnGamepadStickAction;
    public event Action<int> OnGamepadButtonsAction;
    public event Action OnSelectAction;

    public InputObserver()
    {
        inputControl = new InputControls();
        inputControl.GamePlay.AddCallbacks(this);
        inputControl.ComboSequence.AddCallbacks(this);
        inputControl.UI.AddCallbacks(this);
        inputControl.ComboSequence.Enable();
    }

    public void ActivateComboSequence()
    {
        inputControl.GamePlay.Disable();
        inputControl.UI.Disable();
        inputControl.ComboSequence.Enable();
    }

    public void DeactivateComboSequence()
    {
        inputControl.ComboSequence.Disable();
        inputControl.UI.Disable();
        inputControl.GamePlay.Enable();
    }

    public void DeactivateAll()
    {
        inputControl.ComboSequence.Disable();
        inputControl.UI.Disable();
        inputControl.GamePlay.Disable();
    }

    public void ActivateUI()
    {
        inputControl.ComboSequence.Disable();
        inputControl.GamePlay.Disable();
        inputControl.UI.Enable();
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
        if (context.started)
        {
            OnGamepadButtonsAction?.Invoke((int)context.ReadValue<float>());
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnSelectAction?.Invoke();
        }
    }
}
