using DG.Tweening.Core.Enums;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ResourceViewer : MonoBehaviour
{
    [SerializeField] private ResourceHolder[] resourceHolders;
    [SerializeField] private ResourceHolder winPiece;
    [SerializeField] private ResourceHolder winResourceAmount;
    [SerializeField] private ResourceHolder multiplyer;

    [Inject] private readonly GlobalEventManager eventManager;
    [Inject] private readonly GlobalResourceManager resourceManager;

    #region MONO
    private void OnEnable()
    {
        eventManager.StopSpinEvent += RefreshResourceView;
        eventManager.StopSpinEvent += RefreshHoldersVisibility;
        eventManager.MultiplyerUpdateEvent += UpdateMultiplyerInfo;
    }

    private void Start()
    {
        BindData(resourceManager.GetResources());
        multiplyer.SetResourceHolder(resourceManager.GetCurrentMultiplyer());
    }

    private void OnDisable()
    {
        eventManager.StopSpinEvent -= RefreshResourceView;
        eventManager.MultiplyerUpdateEvent -= UpdateMultiplyerInfo;
    }
    #endregion

    private void RefreshResourceView(WheelPiece piece)
    {
        WheelPiece currentPiece = (WheelPiece) piece.Clone();

        winPiece.SetResourceHolder(currentPiece);
        int newAmount = resourceManager.MultiplyAmount(currentPiece);
        currentPiece.Amount = newAmount;
        winResourceAmount.SetResourceHolder(currentPiece);
        multiplyer.SetResourceHolder(resourceManager.GetCurrentMultiplyer());
        BindData(resourceManager.GetResources());
    }

    private void RefreshHoldersVisibility(WheelPiece piece)
    {
        winResourceAmount.Hide(false);
        winPiece.Hide(false);
        eventManager.StopSpinEvent -= RefreshHoldersVisibility;
    }

    private void UpdateMultiplyerInfo(CommonMultiplyerInfo newInfo)
    {
        Debug.Log("mult");
        multiplyer.SetResourceHolder(resourceManager.GetCurrentMultiplyer());
    }

    private void BindData(List<WheelPiece> resources)
    {
        for(int i = 0; i < resources.Count; i++)
        {
            resourceHolders[i].SetResourceHolder(resources[i]);
        }
    }
}
