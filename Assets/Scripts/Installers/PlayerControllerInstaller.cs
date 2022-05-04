using System.Collections;
using System.Collections.Generic;
using MaulGrab.Gameplay.Movement;
using MaulGrab.Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace MaulGrab.Installers
{
    public class PlayerControllerInstaller : MonoInstaller
    {
		public override void InstallBindings()
		{
			Container.Bind<Rigidbody2D>().FromComponentInChildren().AsSingle();
			Container.Bind<CharacterMotor>().FromComponentInChildren().AsSingle();

			Container.Bind<Gun>().FromComponentInChildren().AsSingle();
		}
	}
}
