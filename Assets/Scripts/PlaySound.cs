using UnityEngine;

public interface IPlayerSoundable
{ 
    void PlantSound(AudioSource PlantSound)
    {
        PlantSound.Play();
    }
    void HarvestSound(AudioSource HarvestSound)
    {
        HarvestSound.Play();
    }
}