using UnityEngine;
using UnityEngine.EventSystems;

public class InputFieldEvents : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        GameOver.Instance.ToggleKeyboard();
    }

    public void OnDeselect(BaseEventData eventData)
    {
    }
}