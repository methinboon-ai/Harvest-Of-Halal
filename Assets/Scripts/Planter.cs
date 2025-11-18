using System.Collections.Generic;
using UnityEngine;

public class Planter : Plant
{
    List<GameObject> PlantPrefabs;

    public virtual void Planting()
    {
        GameObject plantingPlant;
        foreach (var plant in PlantPrefabs)
    }
}
