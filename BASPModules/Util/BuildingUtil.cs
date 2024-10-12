using SFS.World;
using SFS.WorldBase;
using UnityEngine;


public class BuildingUtil
{

    public static void AddBuildingToWorld(GameObject building, Planet planet, Double2 position)
        {
            WorldLocation location = building.GetComponent<WorldLocation>();
            location.Value = new Location(planet, position, default(Double2));
        }
}
