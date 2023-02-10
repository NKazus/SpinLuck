using Zenject;

public class NonMonobehInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        EventManagerInstaller.Install(Container);
        SaveManagerInstaller.Install(Container);
    }
}