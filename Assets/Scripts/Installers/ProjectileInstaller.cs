using MaulGrab.Gameplay;
using MaulGrab.Gameplay.Utility;
using MaulGrab.Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace MaulGrab.Installers
{
    public class ProjectileInstaller : MonoInstaller
    {
		public override void InstallBindings()
		{
			var body = GetComponent<Rigidbody>();
			Container.Bind<IRigidbody>().To<Rigidbody3D>().AsSingle().WithArguments( body );

			Container.BindInterfacesTo<Projectile>().FromComponentOnRoot().AsSingle();

			BindColliders( body );
			BindHitBox();
		}

		private void BindColliders( Rigidbody rootBody )
		{
			Container.Bind<Collider>().FromMethodMultiple( context =>
			{
				return rootBody.GetCompositeColliders();
			} ).AsSingle();
		}

		private void BindHitBox()
		{
			Container.Bind<HitBox>().FromMethodMultiple( context =>
			{
				return GetComponentsInChildren<HitBox>();
			} ).AsSingle();

			Container.Bind<HitBoxService>().AsTransient();
		}
	}
}
