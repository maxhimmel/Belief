using MaulGrab.Extensions;
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
			Container.Bind<IRigidbody>().To<Rigidbody3D>()
				.FromMethod( context =>
				{
					var body = GetComponent<Rigidbody>();
					return new Rigidbody3D( body );
				} )
				.AsSingle();

			Container.BindInterfacesTo<Projectile>().FromComponentOnRoot().AsSingle();

			BindColliders();
			BindHitBox();
		}

		private void BindColliders()
		{
			Container.Bind<Collider>().FromMethodMultiple( context =>
			{
				var rootBody = GetComponent<Rigidbody>();
				return rootBody.GetCompositeColliders();
			} ).AsSingle();
		}

		private void BindHitBox()
		{
			Container.Bind<HitBox>().FromComponentsChildedTo( gameObject ).AsSingle();

			Container.Bind<HitBoxService>().AsTransient();
		}
	}
}
