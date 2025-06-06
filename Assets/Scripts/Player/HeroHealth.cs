using System;
using Assets.Scripts.Data;
using Assets.Scripts.Logic;
using Assets.Scripts.Services.PersistentProgress;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
    {
        private State _state;
        private HeroAnimator _animator;

        public event Action HealthChanged;

        private void Awake()
        {
            _animator = GetComponent<HeroAnimator>();
        }

        public float Current
        {
            get => _state.CurrentHP;
            set
            {
                if (_state.CurrentHP != value)
                {
                    _state.CurrentHP = value;
                    HealthChanged?.Invoke();
                }
            }

        }

        public float Max
        {
            get => _state.MaxHP;
            set => _state.MaxHP = value;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHP = Current;
            progress.HeroState.MaxHP = Max;
        }

        public void TakeDamage(float damage)
        {
            if (Current <= 0)
                return;

            Current -= damage;
            _animator.PlayHit(); 
        }
    }
}