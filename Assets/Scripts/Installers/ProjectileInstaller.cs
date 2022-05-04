using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MaulGrab.Installers
{
    public class ProjectileInstaller : MonoInstaller
    {
		public override void InstallBindings()
		{
			Container.Bind<Rigidbody2D>().FromComponentInChildren().AsSingle();
		}
	}
}
