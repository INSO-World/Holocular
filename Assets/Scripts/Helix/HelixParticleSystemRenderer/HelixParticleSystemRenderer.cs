using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HelixParticleSystemRenderer: MonoBehaviour
{
    public static Dictionary<string, Dictionary<string, HelixParticleSystemElement>> particleSystemElements = new Dictionary<string, Dictionary<string, HelixParticleSystemElement>>();
    ParticleSystem particleSystem;
    private UnityAction updateParticleSystemListener;
    private static bool updateRequired = false;
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        updateParticleSystemListener = new UnityAction(UpdateParticleSystem);
        EventManager.StartListening("updateParticleSystem", updateParticleSystemListener);
    }

    private void Update()
    {
        if (updateRequired)
        {
            UpdateParticleSystem();
            updateRequired = false;
        }
    }

    public static void UpdateElement(string sha,string fullFilePath, HelixParticleSystemElement element)
    {
        if (!particleSystemElements.ContainsKey(sha))
        {
            particleSystemElements[sha] = new Dictionary<string, HelixParticleSystemElement>();
        }
        particleSystemElements[sha][fullFilePath] = element;
        updateRequired = true;
    }
    
    public static void UpdateCommitPosition(string sha, Vector3 position)
    {
        Main.actionQueue.Enqueue((() =>
        {
            if (particleSystemElements.ContainsKey(sha))
            {
                Debug.Log("UpdateCommitParticlePositions");
                foreach (KeyValuePair<string, HelixParticleSystemElement> element in particleSystemElements[sha])
                {
                    element.Value.position.z = position.z;
                }

                updateRequired = true;
            }
        }));

    }

    public static void RemoveCommit(string sha)
    {
        particleSystemElements.Remove(sha);
        updateRequired = true;
    }
    
    public static void RemoveFile(string sha,string fullFilePath)
    {
        particleSystemElements[sha].Remove(fullFilePath);
        updateRequired = true;
    }

    private void UpdateParticleSystem()
    {
        HelixParticleSystemElement[] elements = getAllElements(particleSystemElements);
        RuntimeDebug.Log("ParticleSystem rendering "+elements.Length+" elements");
        ParticleSystem.Particle[] cloud = new ParticleSystem.Particle[elements.Length];

        for (int i = 0; i < elements.Length; i++)
        {
            cloud[i].position = elements[i].position;			
            cloud[i].startSize = GlobalSettings.fileSize;	
            cloud[i].startColor = elements[i].color;
        }
            
        particleSystem.maxParticles = cloud.Length;
        particleSystem.SetParticles(cloud, cloud.Length);
    }

    private HelixParticleSystemElement[] getAllElements(
        Dictionary<string, Dictionary<string, HelixParticleSystemElement>> dictElements)
    {
        List<HelixParticleSystemElement> allValues = new List<HelixParticleSystemElement>();
        foreach (Dictionary<string, HelixParticleSystemElement> dict in dictElements.Values)
        {
            foreach (HelixParticleSystemElement value in dict.Values)
            {
                allValues.Add(value);
            }
        }
        return allValues.ToArray();
    }
}
