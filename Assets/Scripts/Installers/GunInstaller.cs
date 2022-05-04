using System.Collections;
using System.Collections.Generic;
using MaulGrab.Gameplay.Weapons;
using UnityEngine;
using Zenject;

namespace MaulGrab.Installers
{
    public class GunInstaller : MonoInstaller
    {
		[SerializeField] private Projectile _projectilePrefab = default;

		public override void InstallBindings()
		{
			Container.BindFactory<Projectile, Projectile.Factory>().FromComponentInNewPrefab( _projectilePrefab );
		}
	}
}
