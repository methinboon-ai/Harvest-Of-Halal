using System.Collections.Generic;
using UnityEngine;



public abstract class Plant : MonoBehaviour
{
    [SerializeField] List<Mesh> PlantMeshStages;
    [SerializeField] public float MaxStageUnit { get; private set; }
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

        Debug.Log($"Current Stage: {currentStage}, Progress: {progress:P0}, Index: {targetIndex}");
    }


    public void Harvesting(Player Player)
    {
        GameObject newPlanter = Instantiate(Planter,transform.parent);
        newPlanter.transform.parent = transform;
        Destroy(this.gameObject);
    }
}
