using UnityEngine;
using UnityEngine.EventSystems;

public class JumpUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController player;

    public void OnPointerDown(PointerEventData eventData)
    {
        player.StartJumpHold();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.EndJumpHold();
    }
}
