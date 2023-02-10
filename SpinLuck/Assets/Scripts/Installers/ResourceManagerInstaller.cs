using UnityEngine;
using Zenject;

public class ResourceManagerInstaller : MonoInstaller
{
    [SerializeField] private GlobalResourceManager resourceManager;
    public override void InstallBindings()
    {
        Container.Bind<GlobalResourceManager>().FromInstance(resourceManager).AsSingle().NonLazy();
    }
}