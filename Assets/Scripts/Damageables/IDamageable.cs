using UnityEngine;

namespace MaulGrab.Gameplay
{
    public interface IDamageable
    {
        Transform Transform { get; }

        float TakeDamage( DamageEvent damageEvent, Transform instigator, Transform causer );
    }
}
