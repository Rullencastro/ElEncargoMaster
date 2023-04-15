using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class WrapperHighScores
{
    public List<ScoreData> scores;

    public WrapperHighScores()
    {
        scores = new List<ScoreData>();
    }
    
    public void AddScore(ScoreData newScore)
    {
        scores.Add(newScore);
        scores.Sort(new ScoreComparer());
        if (scores.Count > 10)
        {
            scores.RemoveAt(scores.Count - 1);
        }
    }

    public class ScoreComparer : IComparer<ScoreData>
    {
        public int Compare(ScoreData x, ScoreData y)
        {
            if (x.wallsDestroyed != y.wallsDestroyed)
            {
                // Ordenar por score de forma descendente
                return x.wallsDestroyed.CompareTo(y.wallsDestroyed);
            }
            else
            {
                // Si los scores son iguales, ordenar por tiempo de forma ascendente
                return x.time.CompareTo(y.time);
            }
        }
    }

    public void SaveScores(string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                string jsonData = JsonUtility.ToJson(this);
                writer.Write(jsonData);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Se ha producido un error: " + ex.Message);
        }
    }

    public void LoadScores(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                JsonUtility.FromJsonOverwrite(json,this);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Se ha producido un error: " + ex.Message);
        }
    }
}