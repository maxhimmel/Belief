using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace MaulGrab.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private CinemachineBrain _gameplayCameraBrain = default;
        [SerializeField] private ParticleSystem _deathVfx = default;

        public override void InstallBindings()
        {
            Container.BindInstance( _gameplayCameraBrain ).AsSingle();

            // TODO: Create proper death fx service ...
            Container.BindFactory<ParticleSystem, PlaceholderFactory<ParticleSystem>>()
                .FromComponentInNewPrefab( _deathVfx );
        }
    }
}
