using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using MaulGrab.Gameplay.Animation;
using MaulGrab.Gameplay.Utility;
using MaulGrab.Extensions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MaulGrab.Gameplay.Weapons
{
    public class Gun : MonoBehaviour
    {
        public bool IsMagazineEmpty => _currentAmmoCount <= 0;

        [SerializeField] private Transform _shotOrigin = default;

        [BoxGroup( "Spread" )]
        [SerializeField, Min( 1 )] private int _bulletsPerShot = 1;
        [BoxGroup( "Spread" )]
        [SerializeField, Range( 0, 360 )] private float _shotSpread = 0;

        [BoxGroup( "Ammo" )]
        [SerializeField] private bool _useAmmo = true;
        [BoxGroup( "Ammo" )]
        [SerializeField, Min( 1 ), ShowIf( "_useAmmo" )] private int _magazineSize = 6;
        [BoxGroup( "Ammo" )]
        [SerializeField, Min( 1 ), ShowIf( "_useAmmo" )] private int _maxAmmo = 36;

        [BoxGroup( "Misc" )]
        [SerializeField, Min( 0 )] private float _fireRate = 0.25f;
        [BoxGroup( "Misc" )]
        [SerializeField] private float _shotForce = 10;
        [BoxGroup( "Misc" )]
        [SerializeField, MinMaxSlider( 0, 20, true )] private Vector2 _shotTorqueRange = new Vector2( 6, 7 );
        [BoxGroup( "Misc" )]
        [SerializeField] private float _blowbackForce = 2;

        private IRigidbody _ownerBody;
		private Projectile.Factory _projectileFactory;
		private IGunAnimator _gunAnimator;
		private AimAssist _aimAssist;

		private bool _isFiringRequested = false;
        private float _fireCountdown = 0;
        private int _currentAmmoCount = 0;
        private int _heldAmmoCount = 0;

        [Inject]
		public void Construct( IRigidbody ownerBody,
            Projectile.Factory projectileFactory,
            IGunAnimator gunAnimator,
            AimAssist aimAssist )
		{
            _ownerBody = ownerBody;
            _projectileFactory = projectileFactory;
            _gunAnimator = gunAnimator;
            _aimAssist = aimAssist;

            _currentAmmoCount = _magazineSize;
            _heldAmmoCount = _maxAmmo;

            _shotOrigin = _shotOrigin ?? transform;
		}

        public void StartFiring()
		{
            if ( !_isFiringRequested )
            {
                _isFiringRequested = true;
                _gunAnimator.OnFiringStart();
            }
		}

        public void StopFiring()
        {
            if ( _isFiringRequested )
            {
                _isFiringRequested = false;
                _gunAnimator.OnFiringEnd();
            }
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

            _gunAnimator.OnReloaded();
		}

        public void AddAmmo( int ammo )
		{
            _heldAmmoCount = Mathf.Min( _maxAmmo, _heldAmmoCount + ammo );
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
            Quaternion aimAssistRotation = _aimAssist.GetTargetDirection();
            Vector3 forwardDir = aimAssistRotation * Vector3.forward;
            Vector3 rightDir = aimAssistRotation * Vector3.right;
            Vector3 normal = aimAssistRotation* Vector3.up;
            Quaternion rotationOffset = Quaternion.AngleAxis( -_shotSpread / 2f, normal );

            float stepAngle = _shotSpread / (_bulletsPerShot - 1);
            for ( float angle = 0; angle <= _shotSpread; angle += stepAngle )
            {
                Vector3 bulletDir = forwardDir * Mathf.Cos( angle * Mathf.Deg2Rad ) + rightDir * Mathf.Sin( angle * Mathf.Deg2Rad );
                bulletDir = rotationOffset * bulletDir;

                Projectile newProjectile = _projectileFactory.Create();
                newProjectile.transform.SetPositionAndRotation( _shotOrigin.position, _shotOrigin.rotation );

                newProjectile.Fire( bulletDir * _shotForce, _shotTorqueRange.RandomRange() );
            }

            OnFired();
		}

        private void OnFired()
        {
            _fireCountdown = _fireRate;

            if ( _useAmmo )
            {
                _currentAmmoCount = Mathf.Max( --_currentAmmoCount, 0 );
            }

            Vector3 blowbackDir = -_shotOrigin.forward;
            _ownerBody.AddForce( blowbackDir * _blowbackForce, ForceType.Impulse );

            _gunAnimator.OnFired();
        }

#if UNITY_EDITOR
		[Header( "Editor/Tools" )]
        [SerializeField] private float _drawRange = 5;
        [SerializeField] private Color _drawColor = Color.red;

		private void OnDrawGizmosSelected()
		{
            Gizmos.color = _drawColor;
            Handles.color = _drawColor;

            Transform shotOrigin = _shotOrigin ?? transform;
            Vector3 origin = shotOrigin.position;
            Vector3 forwardDir = shotOrigin.forward;
            Vector3 rightDir = shotOrigin.right;
            Vector3 normal = shotOrigin.up;
            Quaternion rotationOffset = Quaternion.AngleAxis( -_shotSpread / 2f, normal );

            Vector3 arcStart = rotationOffset * forwardDir;
            Handles.DrawWireArc( origin, normal, arcStart, _shotSpread, _drawRange );

            float stepAngle = _shotSpread / (_bulletsPerShot - 1);
            for ( float angle = 0; angle <= _shotSpread; angle += stepAngle )
			{
				Vector3 bulletDir = forwardDir * Mathf.Cos( angle * Mathf.Deg2Rad ) + rightDir * Mathf.Sin( angle * Mathf.Deg2Rad );
                bulletDir = rotationOffset * bulletDir;

				Gizmos.DrawRay( origin, bulletDir * _drawRange );

                if ( _shotSpread == 0 )
				{
                    // Stop infinite loop ...
                    break;
				}
			}
        }
#endif
    }
}
