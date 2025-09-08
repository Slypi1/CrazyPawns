using UnityEngine;

public class InputManager
{
   public bool MouseLeftDown => Input.GetMouseButtonDown(0);
   public bool MouseLeftUp => Input.GetMouseButtonUp(0);
   public bool MouseRightDown => Input.GetMouseButtonDown(1);
   public bool MouseRightUp => Input.GetMouseButtonUp(1);
   public float MouseScrollWheel => Input.GetAxis("Mouse ScrollWheel");
}
