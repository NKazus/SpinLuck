using UnityEngine;
using Zenject;

public class UpdateManagerInstaller : MonoInstaller
{
    [SerializeField] private GlobalUpdateManager updateManager;
    public override void InstallBindings()
    {
        Container.Bind<GlobalUpdateManager>().FromInstance(updateManager).AsSingle().NonLazy();
    }
}