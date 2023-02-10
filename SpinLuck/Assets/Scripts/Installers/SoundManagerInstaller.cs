using UnityEngine;
using Zenject;

public class SoundManagerInstaller : MonoInstaller
{
    [SerializeField] private SoundManager soundManager;

    public override void InstallBindings()
    {
        Container.Bind<SoundManager>().FromInstance(soundManager).AsSingle().NonLazy();
    }
}