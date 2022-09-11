using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace No_Brainer
{
    public class Statistic
    {

        List<List<int>> games;
        List<List<float>> score;

        int current_day;

        DateTime start_date;

        int passed_days;

        public Statistic()
        {
            games = new List<List<int>>();
            score = new List<List<float>>();

            current_day = -1;
        }

        public int DaysPlayed
        {
            get { return current_day + 1; }
        }

        public void AddDay()
        {
            games.Add(new List<int>());
            score.Add(new List<float>());

            current_day += 1;
        }

        public float FirstScore(int game_id)
        {
            for (int day = 0; day < current_day; day++)
            {
                for (int game = 0; game < score[day].Count; game++)
                {
                    if (games[day][game] == game_id)
                    {
                        return score[day][game];
                    }
                }
            }

            return 0f;
        }

        public float GetHighestScore(int game_id)
        {
            float highest = 0f;

            for(int day = 0; day < current_day; day++)
            {
                for (int game = 0; game < score[day].Count; game++)
                {
                    if (games[day][game] == game_id)
                    {
                        if (highest < score[day][game])
                            highest = score[day][game];
                    }
                }
            }

            return highest;
        }

        public int GameCount(int day)
        {
            return games[day].Count;
        }

        public int GameID(int day, int nr)
        {
            return games[day][nr];
        }

        public float Score(int day, int nr)
        {
            return score[day][nr];
        }

        public void AddGameScore(int game_id, float s)
        {
            int size = games.Count - 1;
            games[size].Add(game_id);
            score[size].Add(s);
        }

        public void SetScore(int id, float s)
        {
            int size = games.Count - 1;
            score[size][id] = s;
        }

        public bool DaySkipped(int day)
        {
            for (int i = 0; i < score[day].Count; i++)
            {
                if (score[day][i] > 0f)
                    return false;
            }

            return true;
        }

        public DateTime StartDate
        {
            get { return start_date; }
            set { start_date = value; }
        }

    }
}
