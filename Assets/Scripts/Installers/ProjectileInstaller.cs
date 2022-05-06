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
			Container.Bind<Rigidbody2D>().FromComponentOnRoot().AsSingle();
			Container.BindInterfacesTo<Projectile>().FromComponentOnRoot().AsSingle();
		}
	}
}
