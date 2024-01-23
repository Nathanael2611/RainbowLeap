using System.Collections.Generic;
using UnityEngine;

namespace util
{
    
    [RequireComponent(typeof(MeshFilter))]
    [ExecuteInEditMode]
    public class ShapeRenderer : MonoBehaviour
    {
        public Mesh mesh;
        public Vector3[] polygonPoints;
        public int[] polygonTriangles;

        public bool isFilled;
        public int polygonSides;
        public float polygonRadius;
        public float centerRadius;
        
            
        void Start()
        {
            mesh = new Mesh();
            this.GetComponent<MeshFilter>().mesh = mesh;
        }
        
        
        void Update()
        {
            if(isFilled)
            {
                DrawFilled(polygonSides,polygonRadius);
            }
            else
            {
                DrawHollow(polygonSides, polygonRadius, centerRadius);
            }
        }

        public void DrawFilled(int sides, float radius)
        {
            this.polygonPoints = GetCircumferencePoints(sides,radius).ToArray();
            this.polygonTriangles = DrawFilledTriangles(this.polygonPoints);
            this.mesh.Clear();
            this.mesh.vertices = this.polygonPoints;
            this.mesh.triangles = this.polygonTriangles;
        }
     
        public void DrawHollow(int sides, float outerRadius, float innerRadius)
        {
            List<Vector3> pointsList = new List<Vector3>();
            List<Vector3> outerPoints = GetCircumferencePoints(sides,outerRadius);
            pointsList.AddRange(outerPoints);
            List<Vector3> innerPoints = GetCircumferencePoints(sides,innerRadius);
            pointsList.AddRange(innerPoints);
     
            this.polygonPoints = pointsList.ToArray();
            
            this.polygonTriangles = DrawHollowTriangles(polygonPoints);
            this.mesh.Clear();
            this.mesh.vertices = this.polygonPoints;
            this.mesh.triangles = this.polygonTriangles;
        }
     
        public int[] DrawHollowTriangles(Vector3[] points)
        {
            int sides = points.Length/2;
            int triangleAmount = sides*2;
            
            List<int> newTriangles = new List<int>();
            for(int i = 0; i<sides;i++)
            {
                int outerIndex = i;
                int innerIndex = i+sides;
     
                //first triangle starting at outer edge i
                newTriangles.Add(outerIndex);
                newTriangles.Add(innerIndex);
                newTriangles.Add((i+1)%sides);
                
                //second triangle starting at outer edge i
                newTriangles.Add(outerIndex);
                newTriangles.Add(sides+((sides+i-1)%sides));
                newTriangles.Add(outerIndex+sides);
            }
            return newTriangles.ToArray();
        }
        
        List<Vector3> GetCircumferencePoints(int sides, float radius)   
        {
            List<Vector3> points = new List<Vector3>();
            float circumferenceProgressPerStep = (float)1/sides;
            float TAU = 2*Mathf.PI;
            float radianProgressPerStep = circumferenceProgressPerStep*TAU;
            
            for(int i = 0; i<sides; i++)
            {
                float currentRadian = radianProgressPerStep*i;
                points.Add(new Vector3(Mathf.Cos(currentRadian)*radius, Mathf.Sin(currentRadian)*radius,0));
            }
            return points;
        }
        
        int[] DrawFilledTriangles(Vector3[] points)
        {   
            int triangleAmount = points.Length - 2;
            List<int> newTriangles = new List<int>();
            for(int i = 0; i<triangleAmount; i++)
            {
                newTriangles.Add(0);
                newTriangles.Add(i+2);
                newTriangles.Add(i+1);
            }
            return newTriangles.ToArray();
        }
    }
    
}