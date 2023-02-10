using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Sprite defaultIcon;

    private Button button;
    private Image icon;

    public void Initialize()
    {
        button = transform.GetChild(0).GetComponent<Button>();
        icon = transform.GetChild(1).GetComponent<Image>();
    }

    public void Refresh()
    {
        button.image.enabled = true;
        icon.sprite = defaultIcon;
        icon.enabled = false;
    }

    public void SetListener(UnityAction<Button> action)
    {
        button.onClick.AddListener(ClickButton);
        button.onClick.AddListener(delegate { action(button); });
    }

    public void ResetListener()
    {
        button.onClick.RemoveAllListeners();
    }

    public void SetInteractable(bool isInteractable)
    {
        button.interactable = isInteractable;
    }

    private void ClickButton()
    {
        button.image.enabled = false;
        button.interactable = false;
        icon.enabled = true;
    }
}
