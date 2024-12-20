using System.Collections.Generic;
using UnityEngine;

public class CosmicLattice : MonoBehaviour
{
    [Header("Node and Connection Prefabs")]
    public GameObject CLNode; // Prefab for lattice nodes (e.g., a sphere)
    public GameObject CLConn; // Prefab for lattice connections (e.g., a cylinder)

    [Header("Lattice Settings")]
    public int gridSize = 5; // Size of the lattice grid
    public float spacing = 2.0f; // Spacing between nodes
    public bool useHexagonalPattern = true;

    [Header("Physics Settings")]
    public float springForce = 10f; // Force of the spring
    public float damping = 1f; // Damping to reduce oscillations

    [Header("Offsets for Odd Layers")]
    public float xOffsetOdd = 0f; // Хоризонтален офсет по X за нечетни слоеве
    public float zOffsetOdd = 0f; // Хоризонтален офсет по Z за нечетни слоеве
    public float yOffsetOdd = 0f; // Вертикален офсет за нечетни слоеве
    public float yOffsetEven = 0f; // Вертикален офсет за четни слоеве

    [Header("Vibration Settings")]
    public float vibrationAmplitude = 0.2f; // Amplitude of node vibration
    public float vibrationSpeed = 2.0f; // Speed of vibration

    [Header("Energy Flow Settings")]
    public float energyPropagationSpeed = 1.0f; // Speed of energy propagation

    private GameObject[,,] nodes; // 3D array to store nodes
    private List<GameObject> allNodes = new List<GameObject>(); // Flat list for easy access

    void Start()
    {
        if (useHexagonalPattern)
            CreateHexagonalLattice();
        else
            CreateLattice();
    }

    void FixedUpdate()
    {
        ApplyForces();
    }

    //void Update()
    //{
    //    AnimateLattice();
    //    PropagateEnergy();
    //    HandleNodeSelection();
    //}

    void Update()
    {
        AnimateLattice();
        PropagateEnergy();
        HandleNodeSelection();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Node selectedNode = hit.collider.GetComponent<Node>();
                if (selectedNode != null)
                {
                    Debug.Log("Node Selected: " + selectedNode.name);
                    selectedNode.UpdateEnergy(1.0f); // Example interaction
                }
            }
        }
    }
    #region Lattice Creation
    void CreateLattice()
    {
        nodes = new GameObject[gridSize, gridSize, gridSize];

        for (int y = 0; y < gridSize; y++) // За всеки слой (y)
        {
            float yOffset = (y % 2 == 0) ? yOffsetEven : yOffsetOdd; // Избор на вертикален офсет
            float xOffset = (y % 2 == 0) ? 0 : xOffsetOdd; // Избор на X офсет за нечетните слоеве
            float zOffset = (y % 2 == 0) ? 0 : zOffsetOdd; // Избор на Z офсет за нечетните слоеве

            for (int z = 0; z < gridSize; z++) // За всяка редица в слоя (z)
            {
                for (int x = 0; x < gridSize; x++) // За всеки нод в редицата (x)
                {
                    // Изчисляване на позицията
                    Vector3 position = new Vector3(x * spacing + xOffset, y * spacing + yOffset, z * spacing + zOffset);

                    // Създаване на нод
                    //GameObject node = Instantiate(CLNode, position, Quaternion.identity, transform);
                    GameObject node = Instantiate(CLNode, position, Quaternion.identity);
                    node.tag = "LatticeNode";

                    // Приложи ориентация на базата на слоя (четен/нечетен)
                    if (y % 2 == 0)
                    {
                        node.transform.rotation = Quaternion.Euler(0, 0, 0); // Няма завъртане
                    }
                    else
                    {
                        node.transform.rotation = Quaternion.Euler(180, 0, 0); // Завъртане на 180 градуса
                    }

                    // Запази нода в масива и списъка
                    nodes[x, y, z] = node;
                    allNodes.Add(node);

                    // Свържи текущия нод с предишния (ако има такъв)
                    if (x > 0) CreateConnection(nodes[x - 1, y, z], node); // Връзка по X
                    if (z > 0) CreateConnection(nodes[x, y, z - 1], node); // Връзка по Z
                    if (y > 0) CreateConnection(nodes[x, y - 1, z], node); // Връзка по Y
                }
            }
        }
    }

    void CreateConnection(GameObject startNode, GameObject endNode)
    {
        // Създаване на връзка между два нода
        Vector3 start = startNode.transform.position;
        Vector3 end = endNode.transform.position;
        Vector3 midPoint = (start + end) / 2;

        GameObject line = Instantiate(CLConn, midPoint, Quaternion.identity, transform);
        float length = Vector3.Distance(start, end);
        line.transform.localScale = new Vector3(0.1f, length / 2, 0.1f); // Скалиране на линията
        line.transform.up = end - start; // Подравняване на линията
    }

    #endregion

    #region Alternate Lattice Creation

    void CreateAlternateLattice()
    {
        nodes = new GameObject[gridSize, gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    // Instantiate node
                    Vector3 position = new Vector3(x, y, z) * spacing;
                    GameObject node = Instantiate(CLNode, position, Quaternion.identity, transform);
                    nodes[x, y, z] = node;
                    allNodes.Add(node);

                    // Connect nodes with lines
                    if (x > 0) CreateConnection(nodes[x - 1, y, z], node);
                    if (y > 0) CreateConnection(nodes[x, y - 1, z], node);
                    if (z > 0) CreateConnection(nodes[x, y, z - 1], node);
                }
            }
        }
    }

    void CreateHexagonalLattice()
    {
        float hexRadius = spacing;
        int rows = gridSize;
        int cols = gridSize;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                float xOffset = (row % 2 == 0) ? 0 : hexRadius * Mathf.Sqrt(3) / 2;
                Vector3 position = new Vector3(col * hexRadius * 1.5f, 0, row * hexRadius * Mathf.Sqrt(3)) + new Vector3(xOffset, 0, 0);
                GameObject node = Instantiate(CLNode, position, Quaternion.identity, transform);
                allNodes.Add(node);
            }
        }
    }

    //void CreateConnection(GameObject startNode, GameObject endNode)
    //{
    //    Vector3 start = startNode.transform.position;
    //    Vector3 end = endNode.transform.position;
    //    Vector3 midPoint = (start + end) / 2;

    //    GameObject line = Instantiate(Cylinder, midPoint, Quaternion.identity, transform);
    //    float length = Vector3.Distance(start, end);
    //    line.transform.localScale = new Vector3(0.1f, length / 2, 0.1f); // Scale the line
    //    line.transform.up = end - start; // Align the line
    //}

    #endregion

    #region Physics Simulation

    //void ApplyForces()
    //{
    //    foreach (GameObject node in allNodes)
    //    {
    //        if (node.TryGetComponent(out Rigidbody rb))
    //        {
    //            Vector3 force = Vector3.zero;

    //            // Calculate spring forces for neighboring nodes
    //            foreach (GameObject neighbor in GetNeighbors(node))
    //            {
    //                force += SpringForce(node, neighbor);
    //            }

    //            rb.AddForce(force);
    //        }
    //    }
    //}

    void ApplyForces()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    GameObject node = nodes[x, y, z];
                    if (node != null && node.TryGetComponent(out Rigidbody rb))
                    {
                        Vector3 force = Vector3.zero;

                        // Check neighbors and apply spring force
                        if (x > 0) force += SpringForce(node, nodes[x - 1, y, z]);
                        if (y > 0) force += SpringForce(node, nodes[x, y - 1, z]);
                        if (z > 0) force += SpringForce(node, nodes[x, y, z - 1]);

                        rb.AddForce(force);
                    }
                }
            }
        }
    }

    Vector3 SpringForce(GameObject a, GameObject b)
    {
        Vector3 direction = b.transform.position - a.transform.position;
        float distance = direction.magnitude;
        direction.Normalize();

        // Hooke's Law: F = -kx
        float spring = -springForce * (distance - spacing);

        // Add damping
        Vector3 velocityDiff = b.GetComponent<Rigidbody>().velocity - a.GetComponent<Rigidbody>().velocity;
        float damp = damping * Vector3.Dot(velocityDiff, direction);

        return (spring + damp) * direction;
    }

    List<GameObject> GetNeighbors(GameObject node)
    {
        // Returns the neighboring nodes for a given node
        List<GameObject> neighbors = new List<GameObject>();

        foreach (GameObject otherNode in allNodes)
        {
            if (Vector3.Distance(node.transform.position, otherNode.transform.position) <= spacing * 1.1f && node != otherNode)
            {
                neighbors.Add(otherNode);
            }
        }

        return neighbors;
    }

    #endregion

    #region Lattice Animation

    void AnimateLattice()
    {
        foreach (GameObject node in allNodes)
        {
            if (node != null)
            {
                Vector3 originalPosition = node.transform.position;
                float offset = Mathf.Sin(Time.time * vibrationSpeed + originalPosition.sqrMagnitude) * vibrationAmplitude;
                node.transform.position = originalPosition + new Vector3(0, offset, 0);
            }
        }
    }

    #endregion

    #region Energy Propagation

    //void PropagateEnergy()
    //{
    //    foreach (GameObject node in allNodes)
    //    {
    //        if (node.TryGetComponent(out Node nodeScript))
    //        {
    //            float energy = Mathf.Sin(Time.time * energyPropagationSpeed + node.transform.position.sqrMagnitude);
    //            nodeScript.UpdateEnergy(energy);
    //        }
    //    }
    //}

    void PropagateEnergy()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    Node node = nodes[x, y, z].GetComponent<Node>();
                    if (node != null)
                    {
                        float energy = Mathf.Sin(Time.time + x + y + z);
                        node.UpdateEnergy(energy);
                    }
                }
            }
        }
    }

    #endregion

    #region Interactivity

    void HandleNodeSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Node selectedNode = hit.collider.GetComponent<Node>();
                if (selectedNode != null)
                {
                    Debug.Log("Node Selected: " + selectedNode.name);
                    selectedNode.UpdateEnergy(1.0f); // Example interaction
                }
            }
        }
    }

    #endregion
    #region UIControls
    public void SetGridSize(float size)
    {
        gridSize = Mathf.RoundToInt(size);
        //ClearLattice();
        CreateLattice();
    }

    public void SetVibrationSpeed(float speed)
    {
        vibrationSpeed = speed;
    }

    public void SetSpringForce(float force)
    {
        springForce = force;
    }
    #endregion

    void VisualizeVibrations()
    {
        foreach (GameObject node in nodes)
        {
            if (node != null)
            {
                LineRenderer lr = node.GetComponent<LineRenderer>();
                lr.SetPosition(0, node.transform.position);
                lr.SetPosition(1, node.transform.position + Vector3.up * Mathf.Sin(Time.time));
            }
        }
    }

}
public class Node : MonoBehaviour
{
    public float mass = 1.0f;
    public float charge = 0.0f;
    public Color nodeColor;

    void Start()
    {
        GetComponent<Renderer>().material.color = nodeColor;
    }

    public void UpdateEnergy(float energy)
    {
        nodeColor = Color.Lerp(Color.blue, Color.red, energy);
        GetComponent<Renderer>().material.color = nodeColor;
    }
}
