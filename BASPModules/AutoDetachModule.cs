using SFS.Parts;
using SFS.Parts.Modules;
using SFS.World;
using System.Collections.Generic;
using UnityEngine;
using static SFS.World.Rocket;

/**
 * Author: dani0105
 * created At: 05/02/2023
 * <summary>
 *  Automatic separation of parts connected with this module.
 * </summary>
 */
public class AutoDetachModule : MonoBehaviour, INJ_Rocket
{
    public Rocket Rocket { set; private get; }
    public Part Part;

    // list of priority parts to detach before the default detach module
    public string[] partToDetach = new string[0];
    // default detach module.
    public DetachModule detachModule;

    private short _currentStage;
    private List<Part> _visited;
    private const short _max_search_level = 3;
    
    private void Awake()
    {
        this._currentStage = 0;
        this._visited = new List<Part>();
    }

    private Part SearchPart(string partName, Part StartPoint, int level)
    {
        this._visited.Add(StartPoint);
        if(level >= _max_search_level)
        {
            return null;
        }

        foreach (PartJoint joint in this.Rocket.jointsGroup.GetConnectedJoints(StartPoint))
        {
            Part otherPart = joint.GetOtherPart(StartPoint);
            if (this.AlreadyVisited(otherPart))
            {
                continue;
            }

            if(otherPart.name == partName)
            {
                return otherPart;
            }

            otherPart = this.SearchPart(partName, otherPart, level + 1);

            if(otherPart != null)
            {
                return otherPart;
            }
        }
        return null;
    }

    private bool AlreadyVisited(Part part)
    {
        return this._visited.Exists(e=> e == part);
    }

    /**
     * <summary>
     *  Call this method to find and detach the part with the highest priority.
     * </summary>
     */
    public void OnDetach(UsePartData data)
    {
        if (data.sharedData.fromStaging)
        {
            if (this._currentStage+1 == this.partToDetach.Length || this.partToDetach.Length == 0)
            {
                this.detachModule.Detach(data);
            }
            this._currentStage += 1;
            return;
        }
       
        if (this.partToDetach == null)
        {
            return;
        }

        string partToDetach = this.partToDetach[this._currentStage];
        Part otherPart = this.SearchPart(partToDetach, this.Part, 0);
        this._visited = new List<Part>();//clear visited list for the next call
        this._currentStage += 1;
        DetachModule[] otherPartModule = otherPart.GetModules<DetachModule>();

        if(otherPartModule == null || otherPartModule.Length == 0)
        {
            Debug.Log("Other part don't have  DetachModule");
            return;
        }

        foreach(DetachModule detachmodule in otherPartModule)
        {
            detachmodule.Detach(data);
        }
    }
}
