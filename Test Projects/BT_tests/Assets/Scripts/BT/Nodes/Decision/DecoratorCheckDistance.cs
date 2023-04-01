using UnityEngine;
using AI.BT;

public class DecoratorCheckDistance : Decorator
{
    Transform origin;
    Transform target;

    float distanceThreshold;

    public DecoratorCheckDistance(INode child, Transform origin, Transform target, float senseDistance)
        : base(child)
    {
        this.origin = origin;
        this.target = target;
        this.distanceThreshold = senseDistance;
    }

    public override bool Run()
    {
        if (Vector3.Distance(origin.position, target.position) < distanceThreshold)
        {
            return child.Run();
        }

        return false;
    }
}
