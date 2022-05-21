using MaulGrab.Gameplay;
using MaulGrab.Gameplay.Utility;
using Zenject;

namespace MaulGrab.Installers
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<World>().AsSingle();
            Container.Bind<Rewired.Player>().FromMethod( GetFirstPlayer ).AsSingle();
        }

        private Rewired.Player GetFirstPlayer()
        {
            const int inputId = 0;
            return Rewired.ReInput.players.GetPlayer( inputId );
        }
    }
}