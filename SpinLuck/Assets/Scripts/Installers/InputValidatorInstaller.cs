using UnityEngine;
using Zenject;

public class InputValidatorInstaller : MonoInstaller
{
    [SerializeField] private InputValidator inputValidator;
    public override void InstallBindings()
    {
        Container.Bind<InputValidator>().FromInstance(inputValidator).AsSingle().NonLazy();
    }
}