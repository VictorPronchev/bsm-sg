using UnityEngine;

public class ProtonAndNeutronStructure : MonoBehaviour
{
    public GameObject prismPrefab; // Префаб за субелементарна частица (призма)

    // Настройки за протона (форма на ∞)
    public int numPrismsProton = 50;
    public float loopRadiusProton = 1.0f;
    public float separationProton = 0.5f;

    // Настройки за неутрона (форма на тороид)
    public int numPrismsNeutron = 100;
    public float torusRadius = 1.0f;
    public float tubeRadius = 0.3f;
    public float torusHeight = 1.0f; // Нов параметър за височина на торуса

    void Start()
    {
        CreateProton(new Vector3(-2, 0, 0)); // Протонът е разположен вляво
        CreateNeutron(new Vector3(2, 0, 0)); // Неутронът е разположен вдясно
    }

    void CreateProton(Vector3 centerPosition)
    {
        // Създаване на двете примки на протона във форма ∞
        for (int i = 0; i < numPrismsProton; i++)
        {
            float angle = i * Mathf.PI * 2 / numPrismsProton;

            // Лява примка
            Vector3 leftLoopPosition = new Vector3(
                Mathf.Cos(angle) * loopRadiusProton - separationProton,
                0,
                Mathf.Sin(angle) * loopRadiusProton
            ) + centerPosition;

            // Дясна примка
            Vector3 rightLoopPosition = new Vector3(
                Mathf.Cos(angle) * loopRadiusProton + separationProton,
                0,
                Mathf.Sin(angle) * loopRadiusProton
            ) + centerPosition;

            // Създаване на призми
            Instantiate(prismPrefab, leftLoopPosition, Quaternion.identity, transform);
            Instantiate(prismPrefab, rightLoopPosition, Quaternion.identity, transform);
        }
    }

    void CreateNeutron(Vector3 centerPosition)
    {
        // Създаване на тороидалната форма за неутрона с възможност за регулиране на височината
        for (int i = 0; i < numPrismsNeutron; i++)
        {
            float angle = i * Mathf.PI * 2 / numPrismsNeutron;

            // Позиция на частиците върху торуса
            Vector3 torusPosition = new Vector3(
                (torusRadius + Mathf.Cos(angle) * tubeRadius) * Mathf.Cos(angle),
                Mathf.Sin(angle) * torusHeight, // Височината се контролира от torusHeight
                (torusRadius + Mathf.Cos(angle) * tubeRadius) * Mathf.Sin(angle)
            ) + centerPosition;

            // Завъртане на всяка частица
            Quaternion rotation = Quaternion.LookRotation(torusPosition - centerPosition);

            // Създаване на призма
            Instantiate(prismPrefab, torusPosition, rotation, transform);
        }
    }
}
