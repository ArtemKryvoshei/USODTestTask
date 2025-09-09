using Content.Features.GameStateMachine.Scripts;
using Content.Features.SessionTimer.Scripts;
using Core.EventBus;
using Core.PrefabFactory;
using Core.SaveLoadSystem;
using UnityEngine;
using Zenject;


namespace Content.Global.Scripts
{
    public class ContextInstaller : MonoInstaller
    {
        [SerializeField] private GameObject gameStateMachinePrefab;
        
        private IPrefabFactory factory;
        
        public override void InstallBindings()
        {
            Container.Bind<IEventBus>()
                .To<EventBus>()
                .AsSingle();
            
            Container.Bind<ISaveLoadSystem>()
                .To<SaveLoadSystem>()
                .AsSingle();
            
            Container.Bind<IPrefabFactory>()
                .To<BasicPrefabFactory>()
                .AsSingle();
            
            factory = Container.Resolve<IPrefabFactory>();
            var gsmObj = factory.SpawnPrefab(gameStateMachinePrefab, Vector3.zero, Quaternion.identity);
            GameStateMachine stateMachine = gsmObj.GetComponent<GameStateMachine>();
            Container.InjectGameObject(gsmObj);

            Container.Bind<IGameStateMachine>()
                .To<GameStateMachine>()
                .FromInstance(stateMachine);
            
            Container.BindInterfacesAndSelfTo<SessionTimer>().AsSingle();
            
            Debug.Log("[GlobalContextInstaller] Context init!");
        }
    }
}