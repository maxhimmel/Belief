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
		public override void InstallBindings()
		{
			var body = GetComponent<Rigidbody>();
			Container.Bind<IRigidbody>().To<Rigidbody3D>().AsSingle().WithArguments( body );

			Container.BindInterfacesTo<Projectile>().FromComponentOnRoot().AsSingle();

			Container.Bind<Collider>().FromComponentsInChildren().AsSingle();
		}
	}
}
