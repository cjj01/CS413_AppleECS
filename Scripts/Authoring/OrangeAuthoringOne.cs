using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
// Empty components can be


// Empty components can be used to tag entities
public struct OrangeTag : IComponentData
{
}

public struct OrangeBottomY : IComponentData
{
    // If you have only one field in a component, name it "Value"

    public float Value;
}
public class OrangeAuthoringOne : MonoBehaviour
{
    [SerializeField] private float bottomY = -14f;

    private class OrangeAuthoringBaker : Baker<OrangeAuthoringOne>
    {
        public override void Bake(OrangeAuthoringOne authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<OrangeTag>(entity);
            AddComponent(entity, new OrangeBottomY { Value = authoring.bottomY });
        }
    }
}