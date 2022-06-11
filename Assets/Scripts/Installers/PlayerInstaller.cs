using MaulGrab.Extensions;
using MaulGrab.Gameplay;
using MaulGrab.Gameplay.Movement;
using MaulGrab.Gameplay.Utility;
using MaulGrab.Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace MaulGrab.Installers
{
    public class PlayerInstaller : MonoInstaller
	{
		[SerializeField] private CollisionQueryData _aimAssistData = default;
		[SerializeField] private DashData _dashData = default;

		public override void InstallBindings()
		{
			BindAimAssist();

			Container.Bind<CharacterMotor>().FromComponentOnRoot().AsSingle();

			Container.BindInstance( _dashData ).AsSingle();
			Container.Bind<DashController>().AsSingle();

			Container.Bind<Gun>().FromComponentInChildren().AsSingle();
		}

		private void BindAimAssist()
		{
			Container.Bind<VisionBounds>().FromComponentChildedTo( gameObject ).AsSingle();

			Container.Bind<IGameObjectProvider>()
				.To<SphereOverlapProvider>()
				.AsSingle().WithArguments( _aimAssistData );

			Container.Bind<AimAssist>().FromComponentChildedTo( gameObject ).AsSingle();
		}
	}
}
