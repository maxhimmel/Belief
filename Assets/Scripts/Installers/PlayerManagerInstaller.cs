using UnityEngine;
using Zenject;

namespace SooperDooper.Installers
{
    public class PlayerManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Rewired.Player>().FromMethod( GetFirstPlayer ).AsSingle();
        }

        private Rewired.Player GetFirstPlayer()
        {
            const int inputId = 0;
            return Rewired.ReInput.players.GetPlayer( inputId );
        }
    }
}