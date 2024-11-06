using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public struct OrangeTreeTag : IComponentData
{
}

public struct OrangeTreeSpeed : IComponentData
{
    public float Value;
}

public struct OrangeTreeBounds : IComponentData
{
    public float Left;
    public float Right;
}

public struct OrangeTreeDirectionChangeChance : IComponentData
{
    public float Value;
}

public struct OrangeTreeRandom : IComponentData
{
    public Random Value;
}

public class OrangeTreeAuthoring : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float leftAndRightEdge = 24f;
    [SerializeField] private float directionChangeChance = 0.1f;

    private class OrangeTreeAuthoringBaker : Baker<OrangeTreeAuthoring>
    {
        public override void Bake(OrangeTreeAuthoring authoring)
        {
            // TransformUsageFlags specifies how the entity's transform will be used.
            // - None: The entity will not have a transform component.
            // - Renderable: The entity will have a transform component but will not be updated during runtime.
            // - Dynamic: The entity will have a transform component and will be updated during runtime.
            // - WorldSpace: Indicates that the entity's transform is in world space.
            // - NonUniformScale: Indicates that the entity's transform has non-uniform scale.
            // - ManualOverride: Use it only if you want to take control of the entity's transform.

            // The apple tree entity will have a transform component and will be updated during runtime.
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<OrangeTreeTag>(entity);

            AddComponent(entity, new OrangeTreeSpeed { Value = authoring.speed });
            AddComponent(entity, new OrangeTreeBounds
            {
                Left = -authoring.leftAndRightEdge,
                Right = authoring.leftAndRightEdge
            });

            AddComponent(entity, new OrangeTreeDirectionChangeChance
            {
                Value = authoring.directionChangeChance
            });
            AddComponent(entity, new OrangeTreeRandom
            {
                Value = Random.CreateFromIndex((uint)entity.Index)
            });
        }
    }
}
