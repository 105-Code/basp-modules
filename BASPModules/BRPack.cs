using UnityEngine;
using HarmonyLib;
using static SFS.World.SpaceCenter;

[CreateAssetMenu(fileName = "BRPack", menuName = "BR-Apollo Mod", order = 1)]
public class BRPack : ScriptableObject
{
    public static BRPack Main;
    public const string ModIdPatching = "brapollo.brioche.github";
    public Building vab, launchPad;

    public BRPack()
    {
        Main = this;
    }

    public void OnEnable()
    {
        if (Application.isEditor)
        {
            return;
        }

        Debug.Log("patch functions BR Apollo!");
        Harmony harmony = new Harmony(ModIdPatching);
        harmony.PatchAll();
        Debug.Log("BR Apollo Ready!");
    }

}
