using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

public partial struct CollectAppleSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerScore>();
        state.RequireForUpdate<SimulationSingleton>();
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var appleCount = new NativeArray<byte>(1, Allocator.TempJob);

        state.Dependency = new CollisionJob
        {
            AppleLookup = SystemAPI.GetComponentLookup<AppleTag>(true),
            BasketLookup = SystemAPI.GetComponentLookup<BasketTag>(true),
            OrangeLookup = SystemAPI.GetComponentLookup<OrangeTag>(true),
            OliveLookup = SystemAPI.GetComponentLookup<OliveTag>(true),
            ECB = ecb,
            AppleCount = appleCount
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

        state.Dependency.Complete();

        if (appleCount[0] == 1)
        {
            var playerScore = SystemAPI.GetSingleton<PlayerScore>();
            playerScore.Value += 100;
            SystemAPI.SetSingleton(playerScore);
        }

        appleCount.Dispose();
    }

    [BurstCompile]
    private struct CollisionJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<AppleTag> AppleLookup;
        [ReadOnly] public ComponentLookup<BasketTag> BasketLookup;
        [ReadOnly] public ComponentLookup<OrangeTag> OrangeLookup;
        [ReadOnly] public ComponentLookup<OliveTag> OliveLookup;
        public EntityCommandBuffer ECB;
        public NativeArray<byte> AppleCount;

        public void Execute(CollisionEvent collisionEvent)
        {
            var entityA = collisionEvent.EntityA; // basket
            var entityB = collisionEvent.EntityB; // apple
            
            if (AppleLookup.HasComponent(entityA) && BasketLookup.HasComponent(entityB))
            {
                ECB.DestroyEntity(entityA);
                AppleCount[0] = 1;
            }
            else if (AppleLookup.HasComponent(entityB) && BasketLookup.HasComponent(entityA))
            {
                ECB.DestroyEntity(entityB);
                AppleCount[0] = 1;
            }
            else if (OrangeLookup.HasComponent(entityA) && BasketLookup.HasComponent(entityB))
            {
                ECB.DestroyEntity(entityB); // Destroy the basket if entityB is the basket
                AppleCount[0] = 1;
            }
            else if (OrangeLookup.HasComponent(entityB) && BasketLookup.HasComponent(entityA))
            {
                ECB.DestroyEntity(entityA); // Destroy the basket if entityA is the basket
                AppleCount[0] = 1;
            }
            else if (OliveLookup.HasComponent(entityA) && BasketLookup.HasComponent(entityB))
            {
                ECB.DestroyEntity(entityB); // Destroy the basket if entityB is the basket
                AppleCount[0] = 1;
            }
            else if (OliveLookup.HasComponent(entityB) && BasketLookup.HasComponent(entityA))
            {
                ECB.DestroyEntity(entityA); // Destroy the basket if entityA is the basket
                AppleCount[0] = 1;
            }
        }
    }
}
