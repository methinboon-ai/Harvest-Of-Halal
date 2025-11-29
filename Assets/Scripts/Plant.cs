using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;



public class Plant : MonoBehaviour
{
    [SerializeField] List<Mesh> PlantMeshStages;
    [SerializeField] float MaxStageUnit;
    [SerializeField] GameObject Planter;
    MeshFilter meshFilter;
    public bool Harvestable {  get; private set; }
    public float currentStage { get; private set; } = 0;
    
    

    void Start()
    {
        meshFilter = transform.GetChild(0).GetComponent<MeshFilter>();
        meshFilter.mesh = PlantMeshStages[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Grow()
    {
        currentStage = Mathf.Clamp(currentStage + 1, 0, MaxStageUnit);

        float progress = Mathf.InverseLerp(0, MaxStageUnit, currentStage);

        int meshCount = PlantMeshStages.Count;

        int targetIndex = Mathf.FloorToInt(progress * (meshCount - 1));

        targetIndex = Mathf.Clamp(targetIndex, 0, meshCount - 1);

        Mesh mesh = PlantMeshStages[targetIndex];
        meshFilter.mesh = mesh;

        if (progress >= 100 || currentStage >= MaxStageUnit)
        {
            Harvestable = true;
        }

        //Debug.Log($"{transform.name}: {currentStage}, Progress: {progress:P0}, Index: {targetIndex}");
    }


    public void Harvesting(Player Player)
    {
        string plant_name = transform.name;
        Vector3 Pos = transform.position;
        Transform Garden = GameObject.Find("Plants").transform;
        GameObject newPlanter = Instantiate(Planter,Garden);
        newPlanter.transform.position = Pos;
        Crop_Class newCrop = new Crop_Class();
        newCrop.SetName(plant_name);
        Player.ItemPickUp(newCrop);
        Destroy(gameObject);
    }
}
