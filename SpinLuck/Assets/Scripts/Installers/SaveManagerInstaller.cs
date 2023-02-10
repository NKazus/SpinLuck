using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Zenject;

public class SaveManagerInstaller : Installer<SaveManagerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ISaveManager>().To<SaveManager>().AsSingle().NonLazy();
    }
}