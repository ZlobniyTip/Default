using Assets.Scripts.Logic;
using System;
using UnityEngine;

public class HeroAnimator : MonoBehaviour, IAnimationStateReader
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] public Animator _animator;

    private static readonly int MoveHash = Animator.StringToHash("Speed");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DieHash = Animator.StringToHash("Die");

    private readonly int _idleStateHash = Animator.StringToHash("Idle");
    private readonly int _idleStateFullHash = Animator.StringToHash("Base Layer.Idle");
    private readonly int _attackStateHash = Animator.StringToHash("Attack");
    private readonly int _walkingStateHash = Animator.StringToHash("Run");
    private readonly int _deathStateHash = Animator.StringToHash("Die");

    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public AnimatorState State { get; private set; }
    public bool IsAttacking => State == AnimatorState.Attack;

    private void Update()
    {
        _animator.SetFloat(MoveHash, _characterController.velocity.magnitude, 0.1f, Time.deltaTime);
    }

    public void PlayHit()
    {
        _animator.SetTrigger(HitHash);
    }

    public void PlayAttack()
    {
        _animator.SetTrigger(AttackHash);
    }

    public void PlayDeath()
    {
        _animator.SetTrigger(DieHash);
    }

    public void ResetToIdle()
    {
        _animator.Play(_idleStateHash, -1);
    }

    public void EnteredState(int stateHash)
    {
        State = StateFor(stateHash);
        StateEntered?.Invoke(State);
    }

    public void ExitedState(int stateHash)
    {
        StateExited?.Invoke(StateFor(stateHash));
    }

    private AnimatorState StateFor(int stateHash)
    {
        AnimatorState state;
        if (stateHash == _idleStateHash)
        {
            state = AnimatorState.Idle;
        }
        else if (stateHash == _attackStateHash)
        {
            state = AnimatorState.Attack;
        }
        else if (stateHash == _walkingStateHash)
        {
            state = AnimatorState.Walking;
        }
        else if (stateHash == _deathStateHash)
        {
            state = AnimatorState.Died;
        }
        else
        {
            state = AnimatorState.Unknown;
        }

        return state;
    }
}
