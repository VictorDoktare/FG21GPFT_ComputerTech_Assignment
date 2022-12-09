using Components;
using Unity.Entities;

public partial class GameBoundarySystem : SystemBase
{
    // Y1: 7, Y2: -2, X1: 3 X2: -3
    protected override void OnCreate()
    {
        Entities
            .ForEach((ref Boundary boundary) =>
            {
            }).ScheduleParallel();
    }

    protected override void OnUpdate()
    {
        
    }
}
 