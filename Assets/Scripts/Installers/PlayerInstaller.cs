using System.Collections;
using System.Collections.Generic;
using MaulGrab.Gameplay.Movement;
using MaulGrab.Gameplay.Utility;
using MaulGrab.Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace MaulGrab.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
		public override void InstallBindings()
		{
			Rigidbody body = GetComponent<Rigidbody>();

			Container.Bind<IRigidbody>().To<Rigidbody3D>().AsSingle().WithArguments( body );
			Container.Bind<CharacterMotor>().FromComponentOnRoot().AsSingle();

			Container.Bind<Gun>().FromComponentInChildren().AsSingle();
		}
	}
}
