using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; private set; }

    [Header("Map settings")]
    public int controlPointCount = 15;
    public float yStep = 2f;
    public float xRange = 5f;
    public float mapWidth { get; set; } = 2f;
    public float screenBottomY { get; set; } = -5f;

    public List<Vector3> controlPoints = new List<Vector3>();

    public LineRenderer lineRenderer;
    public int splineResolution = 500; 

    private float mapOffsetY = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        GenerateInitialPoints();
        DrawSpline();
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        
    }

    void Update()
    {
        ScrollMap();
        CheckAndGenerateMorePoints();
        DrawSpline();
        Debug.Log(GetHardness());
    }

    void GenerateInitialPoints()
    {
        controlPoints.Clear();
        Vector3 point = Vector3.zero;

        for (int i = 0; i < controlPointCount; i++)
        {
            point = GenerateNewPoint(point.y, this.yStep, point.x);
            controlPoints.Add(point);
        }
    }

    void CheckAndGenerateMorePoints()
    {
        if (Camera.main.WorldToViewportPoint(controlPoints[1]).y < -0.6f)
        {
            controlPoints.RemoveAt(0);
            Vector3 last = controlPoints[controlPoints.Count - 1];
            Vector3 next = GenerateNewPoint(last.y, this.yStep, last.x);
            controlPoints.Add(next);
        }
    }

    Vector3 GenerateNewPoint(float y, float yStep, float x)
    {
        Vector3 point = Vector3.zero;
        point.x += UnityEngine.Random.Range(-xRange, xRange);
        point.x = Math.Clamp(point.x, -this.mapWidth, this.mapWidth);
        Debug.Log(point.x);
        point.y = y + yStep;
        return point;
    }

    void ScrollMap()
    {
        float scrollSpeed = -2f * Time.deltaTime;
        mapOffsetY += scrollSpeed;

        for (int i = 0; i < controlPoints.Count; i++)
        {
            controlPoints[i] += new Vector3(0, scrollSpeed, 0);
        }
    }

    void DrawSpline()
    {
        List<Vector3> splinePoints = GenerateBSplinePoints(controlPoints, splineResolution);
        lineRenderer.positionCount = splinePoints.Count;
        lineRenderer.SetPositions(splinePoints.ToArray());
    }

    List<Vector3> GenerateBSplinePoints(List<Vector3> controlPoints, int resolution)
    {
        int degree = 3;
        int n = controlPoints.Count - 1;
        int knotCount = n + degree + 2;


        List<float> knots = new List<float>();
        for (int i = 0; i < knotCount; i++)
        {
            knots.Add(i);
        }

        List<Vector3> curvePoints = new List<Vector3>();

        float tStart = knots[degree];
        float tEnd = knots[n + 1];

        for (float t = tStart; t <= tEnd; t += (tEnd - tStart) / resolution)
        {
            Vector3 point = Vector3.zero;
            for (int i = 0; i <= n; i++)
            {
                float b = CoxDeBoor(t, i, degree, knots);
                point += b * controlPoints[i];
            }
            curvePoints.Add(point);
        }

        return curvePoints;
    }

    public float GetHardness()
    {
        var splinePoints = this.GenerateBSplinePoints(this.controlPoints, this.splineResolution);
        var splinePointsAboveScreen = splinePoints.Where(p => p.y > this.screenBottomY).ToList();
        return Vector3.Distance(splinePointsAboveScreen[0], splinePointsAboveScreen[1]);
    }


    float CoxDeBoor(float t, int i, int k, List<float> knots)
    {
        if (k == 0)
        {
            return (knots[i] <= t && t < knots[i + 1]) ? 1f : 0f;
        }

        float denom1 = knots[i + k] - knots[i];
        float denom2 = knots[i + k + 1] - knots[i + 1];

        float term1 = denom1 != 0 ? ((t - knots[i]) / denom1) * CoxDeBoor(t, i, k - 1, knots) : 0f;
        float term2 = denom2 != 0 ? ((knots[i + k + 1] - t) / denom2) * CoxDeBoor(t, i + 1, k - 1, knots) : 0f;

        return term1 + term2;
    }
}
