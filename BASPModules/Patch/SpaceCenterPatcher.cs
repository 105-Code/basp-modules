using HarmonyLib;
using SFS.World;
using SFS;
using SFS.WorldBase;
using UnityEngine;
using SFS.Parts.Modules;

[HarmonyPatch(typeof(SpaceCenter), "Start")]
class SpaceCenterPatcher
{
    [HarmonyPostfix]
    public static void Postfix(SpaceCenter __instance)
    {
        __instance.vab.building.gameObject.SetActive(false);
        __instance.launchPad.building.gameObject.SetActive(false);


        SpaceCenterData spaceCenter = Base.planetLoader.spaceCenter;

        // VAB building
        GameObject vabGameObject = GameObject.Instantiate(BRPack.Main.vab.building.gameObject);
        vabGameObject.transform.parent = __instance.transform;
        vabGameObject.SetActive(true);

        BuildingUtil.AddBuildingToWorld(vabGameObject, spaceCenter.address.GetPlanet(), spaceCenter.LaunchPadLocation.position + new Double2(-750.0, -9.0));
        foreach (I_InitializePartModule initialize in vabGameObject.GetComponentsInChildren<I_InitializePartModule>())
        {
            initialize.Initialize();
        }
        foreach (BaseMesh mesh in vabGameObject.GetComponentsInChildren<BaseMesh>())
        {
            mesh.GenerateMesh();
        }

        WorldLocation vabLocation = vabGameObject.GetComponent<WorldLocation>();
        __instance.vab.building.location.position.Value =  vabLocation.position.Value + new Double2(80,0);


       // Launch pad building
        GameObject launchPadGameObject = GameObject.Instantiate(BRPack.Main.launchPad.building.gameObject);
        launchPadGameObject.transform.parent = __instance.transform;
        launchPadGameObject.SetActive(true);

        BuildingUtil.AddBuildingToWorld(launchPadGameObject, spaceCenter.address.GetPlanet(), spaceCenter.LaunchPadLocation.position + new Double2(-130, -10.0));
        foreach (I_InitializePartModule initialize in launchPadGameObject.GetComponentsInChildren<I_InitializePartModule>())
        {
            initialize.Initialize();
        }
        foreach (BaseMesh mesh in launchPadGameObject.GetComponentsInChildren<BaseMesh>())
        {
            mesh.GenerateMesh();
        }

    }
}

