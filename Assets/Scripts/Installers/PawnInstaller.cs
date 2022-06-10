using MaulGrab.Gameplay.Utility;
using UnityEngine;
using Zenject;

namespace MaulGrab
{
    public class PawnInstaller : MonoInstaller
    {
		public override void InstallBindings()
		{
			Container.BindInstance( transform ).WithId( InstallerID.Owner ).AsSingle();

			Container.Bind<IRigidbody>().To<Rigidbody3D>()
				.FromMethod( context =>
				{
					var body = GetComponent<Rigidbody>();
					return new Rigidbody3D( body );
				} )
				.AsSingle();
		}
	}
}
