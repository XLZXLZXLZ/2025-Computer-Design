using Clipper2Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ColliderCutterHelper : MonoBehaviour
{
    private void Start()
    {
        Paths64 subj = new Paths64();
        Paths64 clip = new Paths64();
        subj.Add(Clipper.MakePath(new int[] { 0, 0, 0, 2, 2, 2, 2, 0 }));
        clip.Add(Clipper.MakePath(new int[] { 0, 0, 0, 1, 1, 1, 1, 0, 1, -1, 0, -1 }));
        Paths64 r1 = Clipper.Intersect(subj, clip, FillRule.NonZero);
        Paths64 r2 = Clipper.Union(subj, clip, FillRule.NonZero);
        Paths64 r3 = Clipper.Difference(subj, clip, FillRule.NonZero);
        Paths64 r4 = Clipper.Xor(subj, clip, FillRule.NonZero);
        Trace.WriteLine(r1.ToString());//1,0  1,1  0,1  0,0 
        Trace.WriteLine(r2.ToString());//1,-1 , 1,0 , 2,0 , 2,2 , 0,2 , 0,0 , 0,-1 
        Trace.WriteLine(r3.ToString());//0,1 , 1,1 , 1,0 , 2,0 , 2,2 , 0,2 
        Trace.WriteLine(r4.ToString());
    }

    public static Vector2[][] Intersect(Vector2[][] subj, Vector2[][] clip)
    {
        PathsD res = Clipper.Intersect(ConvertVectorArrayToPathsD(subj), ConvertVectorArrayToPathsD(clip), FillRule.NonZero);
        return ConvertPathsDArrayToVectors(res);
    }

    public static Vector2[][] Union(Vector2[][] subj, Vector2[][] clip)
    {
        PathsD res = Clipper.Union(ConvertVectorArrayToPathsD(subj), ConvertVectorArrayToPathsD(clip), FillRule.NonZero);
        return ConvertPathsDArrayToVectors(res);
    }

    public static Vector2[][] Difference(Vector2[][] subj, Vector2[][] clip)
    {
        PathsD res = Clipper.Difference(ConvertVectorArrayToPathsD(subj), ConvertVectorArrayToPathsD(clip), FillRule.NonZero);
        return ConvertPathsDArrayToVectors(res);
    }

    public static Vector2[][] Xor(Vector2[][] subj, Vector2[][] clip)
    {
        PathsD res = Clipper.Xor(ConvertVectorArrayToPathsD(subj), ConvertVectorArrayToPathsD(clip), FillRule.NonZero);
        return ConvertPathsDArrayToVectors(res);
    }

    public static PathsD ConvertVectorArrayToPathsD(Vector2[][] inputs)
    {
        var path = new PathsD();

        foreach(var vectors in inputs)
        {
            var arr = new double[vectors.Length * 2];
            for(int i = 0; i < vectors.Length; i++)
            {
                arr[2 * i] = vectors[i].x;
                arr[2 * i + 1] = vectors[i].y;
            }
            path.Add(Clipper.MakePath(arr));
        }

        return path;
    }

    public static Vector2[][] ConvertPathsDArrayToVectors(PathsD paths)
    {
        var arr = new Vector2[paths.Count][];
        
        for(int i = 0; i < paths.Count; i++)
        {
            var path = paths[i];
            arr[i] = new Vector2[path.Count];
            for(int j = 0; j < path.Count; j++)
            {
                arr[i][j] = new Vector2(Convert.ToSingle(path[j].x), Convert.ToSingle(path[j].y));
            }
        }

        return arr;
    }

}

