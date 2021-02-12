using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticlePlayer
{
    private static List<GameObject> _currentParticleList;

    /// <summary>
    /// Create an instance of a GameObject containing 1 one or more particle system and destroy it when it stops
    /// </summary>
    /// <param name="pParticleToPlay"> GameObject to instantiate </param>
    /// <param name="pPosition"> instance position </param>
    public static void playParticle(GameObject pParticleToPlay, Vector3 pPosition)
    {
        GameObject lParticleInstance = MonoBehaviour.Instantiate(pParticleToPlay, pPosition, Quaternion.identity) as GameObject;

        if (lParticleInstance.TryGetComponent<ParticleSystem>(out ParticleSystem particleSystem))
        {
            _currentParticleList.Add(lParticleInstance);
            MonoBehaviour.Destroy(lParticleInstance, particleSystem.main.duration);
        }
    }

    /// <summary>
    /// Clean every particle system instance in the scene
    /// </summary>
    public static void cleanParticles()
    {
        foreach (GameObject lParticle in _currentParticleList)
            if(lParticle != null) MonoBehaviour.Destroy(lParticle);

        _currentParticleList.Clear();
    }
}
