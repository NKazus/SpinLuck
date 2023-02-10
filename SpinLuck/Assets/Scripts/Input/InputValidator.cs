using UnityEngine;

public class InputValidator : MonoBehaviour
{
    [SerializeField] private RectTransform spinActivationArea;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public bool ValidateGameplayInput(Vector2 inputPosition)
    {
        if(RectTransformUtility.RectangleContainsScreenPoint(spinActivationArea, inputPosition, _mainCamera))
        {
            return true;
        }
        return false;
    }

}
