using System.Collections.Generic;
using UnityEngine;



public abstract class Plant : MonoBehaviour
{
    [SerializeField] List<Mesh> PlantMeshStages;
    [SerializeField] float MaxStageUnit;
    MeshFilter meshFilter;
    public bool FullGrowth {  get; private set; }
    float currentStage = 0;
    
    void Start()
    {
        meshFilter = transform.GetChild(0).GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void Grow()
    {
        currentStage = Mathf.Clamp(currentStage + 1, 0, MaxStageUnit);

        // คำนวณเปอร์เซ็นต์ความคืบหน้า (Progress)
        // progress จะมีค่าระหว่าง 0.0 ถึง 1.0
        float progress = Mathf.InverseLerp(0, MaxStageUnit, currentStage);

        // จำนวนด่าน (Mesh) ที่มีอยู่
        int meshCount = PlantMeshStages.Count;

        // คำนวณ Index ที่จะใช้
        // เราจะใช้ progress คูณกับ (meshCount - 1) เพื่อให้ค่าสูงสุด 1.0 
        // ตรงกับ Index สุดท้าย (meshCount - 1)
        // จากนั้นใช้ Mathf.FloorToInt เพื่อปัดลงให้ได้ Index ที่ถูกต้อง
        int targetIndex = Mathf.FloorToInt(progress * (meshCount - 1));

        // ตรวจสอบให้แน่ใจว่า Index ไม่เกินขอบเขตของ List (ถึงแม้ว่าการคำนวณข้างต้นจะครอบคลุมแล้วก็ตาม)
        targetIndex = Mathf.Clamp(targetIndex, 0, meshCount - 1);

        Mesh mesh = PlantMeshStages[targetIndex];
        meshFilter.mesh = mesh;

        Debug.Log($"Current Stage: {currentStage}, Progress: {progress:P0}, Index: {targetIndex}");
    }
}
