using System;
using System.Collections.Generic;
using Assets.Scripts.Infrastructure.Factory;
using Assets.Scripts.Infrastructure.Services;
using Assets.Scripts.Logic;
using Assets.Scripts.Services;
using Assets.Scripts.Services.PersistentProgress;
using Assets.Scripts.Services.SaveLoad;
using Assets.Scripts.UI.Services;
using Scripts.Infrastructure;

namespace Assets.Scripts.Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),

                [typeof(LoadSceneState)] = new LoadSceneState(this, sceneLoader, loadingCurtain, 
                    services.Single<IGameFactory>(), 
                    services.Single<IPersistentProgressService>(), 
                    services.Single<IStaticDataService>(), 
                    services.Single<IUIFactory>()),
                
                [typeof(LoadProgressState)] = new LoadProgressState(this, 
                    services.Single<IPersistentProgressService>(), 
                    services.Single<ISaveLoadService>()),

                [typeof(GameLoopState)] = new GameLoopState(this)
            };
        }

        public void Enter<TState>() where TState :class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState :class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}