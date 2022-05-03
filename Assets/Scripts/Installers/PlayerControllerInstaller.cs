using System.Collections;
using System.Collections.Generic;
using SooperDooper.Gameplay.Movement;
using UnityEngine;
using Zenject;

namespace SooperDooper.Installers
{
    public class PlayerControllerInstaller : MonoInstaller
    {
		public override void InstallBindings()
		{
			Container.Bind<Rigidbody2D>().FromComponentInChildren().AsSingle();
			Container.Bind<CharacterMotor>().FromComponentInChildren().AsSingle();
		}
	}
}
