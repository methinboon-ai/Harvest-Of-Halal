using System.Collections.Generic;
using UnityEngine;

public class PlantHandler : MonoBehaviour
{
    
    public void GrowAll()
    {
        foreach (Transform t in transform)
        {
            Plant plant = t.GetComponent<Plant>();
            if (plant != null) {
                plant.Grow();
            }
        }
    }
}
