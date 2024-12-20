using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalacticEgg : MonoBehaviour
{
    public GameObject particlePrefab; // Префаб за частиците
    public int numParticles = 1000; // Брой частици
    public float radius = 5.0f; // Радиус на сферата
    public bool enableCrystallization = true; // Активиране на кристализация
    public float crystallizationSpeed = 1.0f; // Скорост на кристализацията

    private List<GameObject> particles = new List<GameObject>();
    private Vector3[] targetPositions;

    void Start()
    {
        // Генерира сферична структура
        GenerateGalacticEgg();
        if (enableCrystallization)
        {
            StartCoroutine(CrystallizeParticles());
        }
    }

    void GenerateGalacticEgg()
    {
        for (int i = 0; i < numParticles; i++)
        {
            // Създаване на частица
            GameObject particle = Instantiate(particlePrefab, Random.onUnitSphere * radius, Quaternion.identity);
            particle.transform.localScale *= 0.03f; // Намалете размера на частиците
            particle.transform.SetParent(transform); // Групирайте частиците от двата вида лефтхенден и райтхендед!!!
            particles.Add(particle);
        }
    }

    IEnumerator CrystallizeParticles()
    {
        // Генерирайте кристална структура (мрежа)
        GenerateCrystalTargets();

        // Постепенно движение на частиците към целевите позиции
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * crystallizationSpeed;

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].transform.position = Vector3.Lerp(
                    particles[i].transform.position,
                    targetPositions[i],
                    progress
                );
            }

            yield return null;
        }
    }

    void GenerateCrystalTargets()
    {
        targetPositions = new Vector3[numParticles];
        int index = 0;

        // Генерирайте подредена решетка в зададен обем
        float spacing = Mathf.Pow((radius * radius * radius) / numParticles, 1f / 3f);
        for (float x = -radius; x < radius; x += spacing)
        {
            for (float y = -radius; y < radius; y += spacing)
            {
                for (float z = -radius; z < radius; z += spacing)
                {
                    if (index >= numParticles) return;
                    Vector3 position = new Vector3(x, y, z);
                    if (position.magnitude <= radius)
                    {
                        targetPositions[index] = position;
                        index++;
                    }
                }
            }
        }
    }
}
