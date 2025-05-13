using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAnimator))]
    public class AnimateAlongAgent : MonoBehaviour
    {
        private const float MinimalVelocity = 0.1f;

        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private NavMeshAgent _agent;

        private void Update()
        {
            if (ShouldMove())
                _animator.Move(_agent.velocity.magnitude);
            else
                _animator.StopMoving();
        }

        private bool ShouldMove() =>
            _agent.velocity.magnitude > MinimalVelocity && _agent.remainingDistance > _agent.radius;
    }
}