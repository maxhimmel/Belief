using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MaulGrab.Gameplay.Weapons
{
    public class Gun : MonoBehaviour
    {
        private Vector3 ShotOriginPosition => _shotOrigin != null ? _shotOrigin.position : transform.position;
        private Quaternion ShotOriginRotation => _shotOrigin != null ? _shotOrigin.rotation : transform.rotation;

        [SerializeField] private Transform _shotOrigin = default;
        [SerializeField, MinValue( 1 )] private int _bulletsPerShot = 1;
        [SerializeField, MinValue( 1 )] private int _magazineSize = 6;
        [SerializeField, MinValue( 0 )] private float _fireRate = 0.25f;
        [SerializeField] private float _shotForce = 10;
        [SerializeField, Range( 0, 360 )] private float _shotSpread = 0;

        private Projectile.Factory _projectileFactory;
        private bool _isFiringRequested = false;
        private float _fireCountdown = 0;

        [Inject]
		public void Construct( Projectile.Factory projectileFactory )
		{
            _projectileFactory = projectileFactory;
		}

        public void StartFiring()
		{
            _isFiringRequested = true;
		}

        public void StopFiring()
        {
            _isFiringRequested = false;
        }

		private void FixedUpdate()
        {
            _fireCountdown -= Time.deltaTime;
            if ( _fireCountdown > 0 )
			{
                return;
			}

            if ( !_isFiringRequested )
			{
                return;
			}

            Fire();
		}

        private void Fire()
        {
            Vector3 upDir = ShotOriginRotation * Vector3.up;
            Vector3 rightDir = ShotOriginRotation * Vector3.right;
            Vector3 normal = ShotOriginRotation * Vector3.forward;
            Quaternion rotationOffset = Quaternion.AngleAxis( _shotSpread / 2f, normal );

            float stepAngle = _shotSpread / (_bulletsPerShot - 1);
            for ( float angle = 0; angle <= _shotSpread; angle += stepAngle )
            {
                Vector3 bulletDir = upDir * Mathf.Cos( angle * Mathf.Deg2Rad ) + rightDir * Mathf.Sin( angle * Mathf.Deg2Rad );
                bulletDir = rotationOffset * bulletDir;

                Projectile newProjectile = _projectileFactory.Create();
                newProjectile.Fire( bulletDir * _shotForce );
            }

            _fireCountdown = _fireRate;
		}

#if UNITY_EDITOR
		[Header( "Editor/Tools" )]
        [SerializeField] private float _drawRange = 5;
        [SerializeField] private Color _drawColor = Color.red;

		private void OnDrawGizmosSelected()
		{
            Gizmos.color = _drawColor;
            Handles.color = _drawColor;

            Vector3 origin = ShotOriginPosition;
            Vector3 upDir = ShotOriginRotation * Vector3.up;
            Vector3 rightDir = ShotOriginRotation * Vector3.right;
            Vector3 normal = ShotOriginRotation * Vector3.forward;
            Quaternion rotationOffset = Quaternion.AngleAxis( _shotSpread / 2f, normal );

            Vector3 arcStart = rotationOffset * upDir;
            Handles.DrawWireArc( origin, -normal, arcStart, _shotSpread, _drawRange );

            float stepAngle = _shotSpread / (_bulletsPerShot - 1);
            for ( float angle = 0; angle <= _shotSpread; angle += stepAngle )
			{
				Vector3 bulletDir = upDir * Mathf.Cos( angle * Mathf.Deg2Rad ) + rightDir * Mathf.Sin( angle * Mathf.Deg2Rad );
                bulletDir = rotationOffset * bulletDir;

				Gizmos.DrawRay( origin, bulletDir * _drawRange );
			}
        }
#endif
    }
}
