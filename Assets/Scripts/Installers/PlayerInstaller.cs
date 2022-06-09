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

		public override void InstallBindings()
		{
			Rigidbody body = GetComponent<Rigidbody>();

			var body = GetComponent<Rigidbody>();
			Container.Bind<IRigidbody>().To<Rigidbody3D>().AsSingle().WithArguments( body );

			BindAimAssist();

			Container.Bind<CharacterMotor>().FromComponentOnRoot().AsSingle();

			Container.Bind<Gun>().FromComponentInChildren().AsSingle();
		}

		private void BindAimAssist()
		{
			var visionBounds = GetComponentInChildren<VisionBounds>();
			Container.BindInstance<VisionBounds>( visionBounds ).AsSingle();

			Container.Bind<IGameObjectProvider>()
				.To<SphereOverlapProvider>()
				.AsSingle().WithArguments( _aimAssistData );

			var aimAssist = GetComponentInChildren<AimAssist>();
			Container.BindInstance<AimAssist>( aimAssist ).AsSingle();
		}
	}
}
