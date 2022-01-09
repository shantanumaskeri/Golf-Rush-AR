using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerJoystickMovement : MonoBehaviour
{

    public FixedJoystick MoveJoystick;
    public FixedButton JumpButton;
    public FixedTouchField TouchField;

    private RigidbodyFirstPersonController fps;

    private void Start()
    {
        fps = GetComponent<RigidbodyFirstPersonController>();
    }

    private void Update()
    {
        fps.RunAxis = MoveJoystick.Direction;
        fps.JumpAxis = JumpButton.Pressed;
        fps.mouseLook.LookAxis = TouchField.TouchDist;
    }
}
