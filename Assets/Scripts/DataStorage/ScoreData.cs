using System;

[Serializable]
public class ScoreData
    {
        public int wallsDestroyed;
        public int time;

        public ScoreData(int s,int t)
        {
            wallsDestroyed = s;
            time = t;
        }
    }
