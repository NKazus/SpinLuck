using UnityEngine;
using Zenject;

public class UIManagerInstaller : MonoInstaller
{
    [SerializeField] private UIManager uiManager;
    public override void InstallBindings()
    {
        Container.Bind<UIManager>().FromInstance(uiManager).AsSingle().NonLazy();
    }
}