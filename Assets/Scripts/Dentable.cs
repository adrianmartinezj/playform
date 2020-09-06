using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dentable : MonoBehaviour
{
    Mesh originalMesh;
    Mesh clonedMesh;
    MeshFilter meshFilter;

    [HideInInspector]
    public int targetIndex;

    [HideInInspector]
    public Vector3 targetVertex;

    [HideInInspector]
    public Vector3[] originalVertices;

    [HideInInspector]
    public Vector3[] modifiedVertices;

    [HideInInspector]
    public Vector3[] normals;

    [HideInInspector]
    public bool isMeshReady = false;
    public bool showTransformHandle = true;
    public List<int> selectedIndices = new List<int>();
    public float pickSize = 0.01f;

    public float radiusOfEffect = 0.3f; //1 
    //public float pullValue = 0.3f; //2
    public float pullValue = 0.3f; //2
    public float duration = 1.2f; //3
    int currentIndex = 0; //4
    bool isAnimate = false;
    float startTime = 0f;
    float runTime = 0f;
    private Vector3 m_ValidGizmo = Vector3.zero;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        meshFilter = GetComponent<MeshFilter>();
        isMeshReady = false;

        currentIndex = 0;

        originalMesh = meshFilter.sharedMesh;
        clonedMesh = new Mesh();
        clonedMesh.name = "clone_" + originalMesh.name;
        clonedMesh.vertices = originalMesh.vertices;
        clonedMesh.triangles = originalMesh.triangles;
        clonedMesh.normals = originalMesh.normals;
        meshFilter.mesh = clonedMesh;
        originalVertices = clonedMesh.vertices;

        modifiedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            modifiedVertices[i] = originalVertices[i];
        }

        normals = clonedMesh.normals;
        Gizmos.color = Color.green;

    }

    public void DentAtPosition(Vector3 point)
    {
        targetVertex = point; //1
        m_ValidGizmo = point;
        StartDisplacement();
    }

    private void OnDrawGizmos()
    {
        if (m_ValidGizmo != Vector3.zero)
        {
            Gizmos.DrawSphere(m_ValidGizmo, .1f);
        }
    }

    public void StartDisplacement()
    {
        startTime = Time.time; //2
        isAnimate = true;
    }

    protected void FixedUpdate() //1
    {
        if (!isAnimate) //2
        {
            return;
        }

        runTime = Time.time - startTime; //3

        if (runTime < duration)  //4
        {
            Vector3 targetVertexPos =
                meshFilter.transform.InverseTransformPoint(targetVertex);
            DisplaceVertices(targetVertexPos, pullValue, radiusOfEffect);
        }
        else //5
        {
            currentIndex++;
            if (currentIndex < selectedIndices.Count) //6
            {
                StartDisplacement();
            }
            else //7
            {
                originalMesh = GetComponent<MeshFilter>().mesh;
                isAnimate = false;
                isMeshReady = true;
            }
        }
    }

    void DisplaceVertices(Vector3 targetVertexPos, float force, float radius)
    {
        Vector3 currentVertexPos = Vector3.zero;
        float sqrRadius = radius * radius; //1

        for (int i = 0; i < modifiedVertices.Length; i++) //2
        {
            currentVertexPos = modifiedVertices[i];
            float sqrMagnitude = (currentVertexPos - targetVertexPos).sqrMagnitude; //3
            if (sqrMagnitude > sqrRadius)
            {
                continue; //4
            }
            float distance = Mathf.Sqrt(sqrMagnitude); //5
            float falloff = GaussFalloff(distance, radius);
            Vector3 translate = (currentVertexPos * force) * falloff; //6
            translate.z = 0f;
            Quaternion rotation = Quaternion.Euler(translate);
            Matrix4x4 m = Matrix4x4.TRS(translate, rotation, Vector3.one);
            modifiedVertices[i] = m.MultiplyPoint3x4(currentVertexPos);
        }
        clonedMesh.vertices = modifiedVertices; //7
        clonedMesh.RecalculateNormals();
    }

    public void ClearAllData()
    {
        selectedIndices = new List<int>();
        targetIndex = 0;
        targetVertex = Vector3.zero;
    }

    #region HELPER FUNCTIONS

    static float LinearFalloff(float dist, float inRadius)
    {
        return Mathf.Clamp01(0.5f + (dist / inRadius) * 0.5f);
    }

    static float GaussFalloff(float dist, float inRadius)
    {
        return Mathf.Clamp01(Mathf.Pow(360, -Mathf.Pow(dist / inRadius, 2.5f) - 0.01f));
    }

    static float NeedleFalloff(float dist, float inRadius)
    {
        return -(dist * dist) / (inRadius * inRadius) + 1.0f;
    }

    #endregion
}
