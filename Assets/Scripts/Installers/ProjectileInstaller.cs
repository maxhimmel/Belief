using System.Collections;
using System.Collections.Generic;
using MaulGrab.Gameplay.Utility;
using MaulGrab.Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace MaulGrab.Installers
{
    public class ProjectileInstaller : MonoInstaller
    {
		[SerializeField] private float _lifetime = 1;

		public override void InstallBindings()
		{
			Container.Bind<Rigidbody2D>().FromComponentOnRoot().AsSingle();
			Container.BindInterfacesTo<Projectile>().FromComponentOnRoot().AsSingle();

			Container.Bind<LifetimeService>().AsSingle().WithArguments( _lifetime );
		}
	}
}
