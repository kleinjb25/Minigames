using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphGenerator : MonoBehaviour
{
    public GameObject vertexPrefab;
    public GameObject edgePrefab;
    public Transform canvasTransform;
    public int numVertices = 10;
    public int weightMin = -100;
    public int weightMax = 100;
    public static int weight;
    public float weightTextWidth = 10f;
    public float weightTextHeight = 10f;
    public Text ruleText;
    public string[] rules = { "the total edge weight is " + weight, "the total edge weight is an odd number",
        "the total edge weight is an even number", "there are no odd vertices in your connection", "there are no even vertices in your connection",
        "there are no even edge weights in your connection", "there are no odd edge weights in your connection"};
    public static List<GameObject> vertices = new List<GameObject>();
    public static List<GameObject> edges = new List<GameObject>();
    private void Start()
    {
        GenerateGraph();
        int randomIndex1 = Random.Range(0, numVertices - 1);
        int randomIndex2 = Random.Range(0, numVertices - 1);
        while (randomIndex1 == randomIndex2)
            randomIndex2 = Random.Range(0, numVertices - 1);
        string randomRule = rules[Random.Range(0, rules.Length)];
        while (randomRule.Equals(""))
            randomRule = rules[Random.Range(0, rules.Length)];
        ruleText.text = "Connect vertex " + randomIndex1 + " to vertex " + randomIndex2 + " in such a way that " + randomRule + ".";
    }
    public void GenerateGraph()
    {

        for (int i = 0; i < numVertices; i++)
        {
            GameObject vertex = Instantiate(vertexPrefab, canvasTransform);
            vertex.SetActive(true);
            vertex.transform.localPosition = GetRandomPositionOnCanvas(i);
            vertex.GetComponentInChildren<Text>().text = i.ToString();
            vertices.Add(vertex);
        }

        HashSet<string> connectedPairs = new HashSet<string>();
        for (int i = 0; i < numVertices - 1; i++)
        {
            GameObject startVertex = vertices[i];
            for (int j = i + 1; j < numVertices; j++)
            {
                GameObject endVertex = vertices[j];
                string vertexPairKey = $"{i}-{j}";
                if (connectedPairs.Contains(vertexPairKey))
                    continue; 
                bool intersectsVertex = CheckEdgeIntersectsVertex(startVertex.transform.localPosition, endVertex.transform.localPosition);
                if (intersectsVertex)
                    continue;

                GameObject edge = Instantiate(edgePrefab, canvasTransform);
                edge.SetActive(true);
                
                edge.transform.localPosition = (startVertex.transform.localPosition + endVertex.transform.localPosition) / 2f;
                
                edge.transform.right = (endVertex.transform.localPosition - startVertex.transform.localPosition).normalized;
                float edgeLength = Vector3.Distance(startVertex.transform.localPosition, endVertex.transform.localPosition);
                edge.transform.localScale = new Vector3(edgeLength, 1f, 1f);
                
                weight = Random.Range(weightMin, weightMax + 1);
                GameObject textObject = new GameObject("WeightText");
                textObject.transform.SetParent(edge.transform);
                textObject.transform.localPosition = new Vector3(0f, 10f, 0f);
                textObject.transform.localRotation = Quaternion.identity;
                Text textComponent = textObject.AddComponent<Text>();
                textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                textComponent.alignment = TextAnchor.MiddleCenter;
                textComponent.text = weight.ToString();
                textComponent.rectTransform.sizeDelta = new Vector2(10f, 10f);
                textComponent.rectTransform.localScale = new Vector3(0.005f, (edge.transform.localScale.x *.005f), 1f);
                //textObject.transform.localRotation = Quaternion.Euler(180f, 180f, 0f);
                textComponent.horizontalOverflow = HorizontalWrapMode.Overflow;
                textComponent.verticalOverflow = VerticalWrapMode.Overflow;
                //edge.transform.localRotation = Quaternion.Euler(180f, 180f, edge.transform.localRotation.z);
                edges.Add(edge);
                connectedPairs.Add(vertexPairKey);
            }
        }
    }

    private Vector3 GetRandomPositionOnCanvas(int vertexIndex)
    {
        float canvasWidth = canvasTransform.GetComponent<RectTransform>().rect.width;
        float canvasHeight = canvasTransform.GetComponent<RectTransform>().rect.height;
        float vertexRadius = vertexPrefab.GetComponent<RectTransform>().sizeDelta.x / 2f;
        float x = Random.Range(-canvasWidth / 2f + vertexRadius, canvasWidth / 2f - vertexRadius);
        float y = Random.Range(-canvasHeight / 2f + vertexRadius, canvasHeight / 2f - vertexRadius);
        while (y >= 420f)
        {
            x = Random.Range(-canvasWidth / 2f + vertexRadius, canvasWidth / 2f - vertexRadius);
            y = Random.Range(-canvasHeight / 2f + vertexRadius, canvasHeight / 2f - vertexRadius);
        }
        for (int i = 0; i < vertexIndex; i++)
        {
            float distance = Vector2.Distance(new Vector2(x, y), vertices[i].transform.localPosition);
            float minDistance = 2f * vertexRadius; // Minimum distance to avoid overlapping
            if (distance < minDistance)
            {
                x += (minDistance - distance) * (x - vertices[i].transform.localPosition.x) / distance;
                y += (minDistance - distance) * (y - vertices[i].transform.localPosition.y) / distance;
            }
        }
        return new Vector3(x, y, 0f);
    }

    private static bool CheckEdgeIntersectsVertex(Vector3 start, Vector3 end)
    {
        Vector2 rayStart = start;
        Vector2 rayDirection = end - start;
        float rayDistance = rayDirection.magnitude;
        rayDirection /= rayDistance;

        foreach (GameObject vertex in vertices)
        {
            Collider2D vertexCollider = vertex.GetComponent<Collider2D>();
            if (vertexCollider != null)
            {
                Vector2 start2D = start;
                Vector2 end2D = end;
                RaycastHit2D hit = Physics2D.Linecast(start2D, end2D, LayerMask.GetMask("Default"));
                if (hit.collider != null && hit.collider.gameObject == vertex)
                {
                    return true; 
                }
            }
        }

        return false;
    }

}
