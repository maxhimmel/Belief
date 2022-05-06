using System.Collections;
using System.Collections.Generic;
using MaulGrab.Gameplay.Movement;
using MaulGrab.Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace MaulGrab.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
		public override void InstallBindings()
		{
			Container.Bind<Rigidbody2D>().FromComponentOnRoot().AsSingle();
			Container.Bind<CharacterMotor>().FromComponentOnRoot().AsSingle();

			Container.Bind<Gun>().FromComponentInChildren().AsSingle();
		}
	}
}
