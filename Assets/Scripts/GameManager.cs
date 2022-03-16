using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerManager PlayerManager;

    public Color Point1Color;
    public Color Point2Color;
    public Color Point3Color;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerManager = new PlayerManager(Application.persistentDataPath);
        PlayerManager.LoadPlayers();

        Point1Color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        Point2Color = new Color(0.5f, 0.5f, 0.65f, 1.0f);
        Point3Color = new Color(0.5f, 0.5f, 0.80f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

[Serializable]
public class Player
{
    public string Name;
    public long Score;

    public Player(string name)
    {
        Name = name;
        Score = 0;
    }

    internal string GetNameAndScore()
    {
        return $"{Name}: {Score}";
    }

    internal void SetNewScore(long score)
    {
        if (score > Score)
        {
            Score = score;
        }
    }
}

[Serializable]
public class PlayerManager
{
    private const string PLAYERS_SAVE_FILE_NAME = "/playerManager.json";
    private readonly string savePath;

    public List<Player> Players;
    public Player ActivePlayer;

    public PlayerManager(string path)
    {
        savePath = path + PLAYERS_SAVE_FILE_NAME;
    }

    public void SavePlayers()
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(savePath, json);
    }

    public void LoadPlayers()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerManager pm = JsonUtility.FromJson<PlayerManager>(json);
            Players = pm.Players;
            ActivePlayer = pm.ActivePlayer;

            if (Players == null)
            {
                Players = new List<Player>();
            }
        }
        else
        {
            Players = new List<Player>();
            ActivePlayer = null;
        }
    }

    public void SetActivePlayer(string name)
    {
        Player player = FindPlayer(name);
        if (player == null)
        {
            player = new Player(name);
            Players.Add(player);
        }
        ActivePlayer = player;
    }

    private Player FindPlayer(string name)
    {
        foreach (Player player in Players)
        {
            if (player.Name.Equals(name))
            {
                return player;
            }
        }

        return null;
    }

    internal int GetActivePlayerIndex()
    {
        if (ActivePlayer == null)
        {
            return -1;
        }

        for (int i = 0; i < Players.Count; i++)
        {
            Player player = Players[i];
            if (player.Name.Equals(ActivePlayer.Name))
            {
                return i;
            }
        }

        return -1;
    }

    internal void SelectActivePlayer(int index)
    {
        if ((index < 0) || (index >= Players.Count))
        {
            return;
        }
        ActivePlayer = Players[index];
        SavePlayers();
    }

    public Player GetBestPlayer()
    {
        if (Players.Count == 0)
        {
            return null;
        }

        Player selected = Players[0];
        foreach (Player player in Players)
        {
            if (player.Score > selected.Score)
            {
                selected = player;
            }
        }

        return selected;
    }
}
