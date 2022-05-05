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

        [BoxGroup( "Spread" )]
        [SerializeField, MinValue( 1 )] private int _bulletsPerShot = 1;
        [BoxGroup( "Spread" )]
        [SerializeField, Range( 0, 360 )] private float _shotSpread = 0;

        [BoxGroup( "Ammo" )]
        [SerializeField] private bool _useAmmo = true;
        [BoxGroup( "Ammo" )]
        [SerializeField, MinValue( 1 ), ShowIf( "_useAmmo" )] private int _magazineSize = 6;
        [BoxGroup( "Ammo" )]
        [SerializeField, MinValue( 1 ), ShowIf( "_useAmmo" )] private int _maxAmmo = 36;

        [BoxGroup( "Misc" )]
        [SerializeField, MinValue( 0 )] private float _fireRate = 0.25f;
        [BoxGroup( "Misc" )]
        [SerializeField] private float _shotForce = 10;

        private Projectile.Factory _projectileFactory;
        private bool _isFiringRequested = false;
        private float _fireCountdown = 0;
        private int _currentAmmoCount = 0;
        private int _heldAmmoCount = 0;

        [Inject]
		public void Construct( Projectile.Factory projectileFactory )
		{
            _projectileFactory = projectileFactory;

            _currentAmmoCount = _magazineSize;
            _heldAmmoCount = _maxAmmo;
		}

        public void StartFiring()
		{
            _isFiringRequested = true;
		}

        public void StopFiring()
        {
            _isFiringRequested = false;
        }

        public void Reload()
		{
            if ( !_useAmmo )
			{
                return;
			}

            int ammoRequestAmount = _magazineSize - _currentAmmoCount;
            int ammoReceiveAmount = Mathf.Min( _heldAmmoCount, ammoRequestAmount );

            _heldAmmoCount -= ammoReceiveAmount;
            _currentAmmoCount += ammoReceiveAmount;
		}

		private void FixedUpdate()
        {
            UpdateState();

            if ( CanFire() )
            {
                Fire();
            }
		}

        private void UpdateState()
        {
            _fireCountdown -= Time.deltaTime;
        }

        private bool CanFire()
		{
            if ( !_isFiringRequested )
			{
                return false;
			}
            if ( _fireCountdown > 0 )
			{
                return false;
			}
            if ( _useAmmo && _currentAmmoCount <= 0 )
			{
                return false;
			}

            return true;
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

            if ( _useAmmo )
            {
                _currentAmmoCount = Mathf.Max( --_currentAmmoCount, 0 );
            }
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
