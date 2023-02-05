using SFS.Parts;
using SFS.Parts.Modules;
using SFS.World;
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
    private Part part;

    // list of priority parts to detach before the default detach module
    public string[] partToDetach = new string[0];
    // default detach module.
    public DetachModule detachModule;
    
    private void Awake()
    {
        this.part = this.GetComponent<Part>();
    }

    /**
     * <summary>
     *  Search the partToDetach list if the part exists and return the priority in the list
     * </summary>
     */
    private bool isInList(string partName, out short priority)
    {
        for(short i = 0; i < this.partToDetach.Length; i++)
        {
            if (partName == this.partToDetach[i])
            {
                priority = i;
                return true;
            }
        }
        priority = 100;
        return false;
    }

    /**
     * <summary>
     *  Call this method to find and detach the part with the highest priority.
     * </summary>
     */
    public void OnDetach(UsePartData data)
    {
        DetachModule maxPriorityDetachModule = this.detachModule;
        short maxPriorityValue = 100;
        short priority;
        bool exist;
       
        if (this.partToDetach != null)
        {
            // search the part to detach
            foreach (PartJoint joint in this.Rocket.jointsGroup.GetConnectedJoints(this.part))
            {
                Part otherPart = joint.GetOtherPart(this.part);
                exist = this.isInList(otherPart.name, out priority);
                if (exist && priority < maxPriorityValue)
                {
                    maxPriorityValue = priority;
                    maxPriorityDetachModule = otherPart.GetComponent<DetachModule>();
                }
            }
        }

        // detach the part
        maxPriorityDetachModule.Detach(data);
    }
}
