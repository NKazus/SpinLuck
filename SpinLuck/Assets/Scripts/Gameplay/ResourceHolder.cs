using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHolder : MonoBehaviour
{
    private Transform localTransform;
    private Image localImage;
    private TextMeshProUGUI localText;

    private bool isHidden;

    private void Awake()
    {
        localTransform = GetComponent<Transform>();
        localImage = localTransform.GetChild(0).GetComponent<Image>();
        localText = localTransform.GetChild(1).GetComponent<TextMeshProUGUI>();
        isHidden = !localImage.enabled;
    }

    public void SetResourceHolder(WheelPiece piece)
    {
        localImage.sprite = piece.Icon;
        localText.text = piece.Amount.ToString();
    }

    public void SetResourceHolder(CommonMultiplyerInfo multiplyerInfo)
    {
        if ((multiplyerInfo.spinInfo.Spins == 0 && !isHidden) || (multiplyerInfo.spinInfo.Spins > 0 && isHidden))
        {
            Hide(!isHidden);
        }
        else if(!isHidden)
        {            
            localImage.sprite = multiplyerInfo.baseInfo.Icon;
            localText.text = multiplyerInfo.spinInfo.Spins.ToString() + "/" + multiplyerInfo.spinInfo.SpinsMax.ToString();
        }        
    }

    public void Hide(bool shouldHide)
    {
        localImage.enabled = localText.enabled = !shouldHide;
        isHidden = shouldHide;
    }
}
