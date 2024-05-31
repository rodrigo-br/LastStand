using System;
using UnityEngine.InputSystem;

public class InputObserver : InputControls.IGamePlayActions
{
    private readonly InputControls inputControl;
    public event Action OnShootAction;

    public InputObserver()
    {
        inputControl = new InputControls();
        inputControl.GamePlay.AddCallbacks(this);
        inputControl.Enable();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnShootAction?.Invoke();
        }
    }
}
