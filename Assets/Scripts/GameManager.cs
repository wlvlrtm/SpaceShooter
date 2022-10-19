using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    [SerializeField] private List<Transform> points = new List<Transform>();
    [SerializeField] private List<GameObject> monsterPool = new List<GameObject>();
    [SerializeField] private GameObject monster;
    [SerializeField] private int maxMonsters = 10;
    [SerializeField] private float createTime = 3.0f;
    [SerializeField] private TMP_Text scoreText;
    private int totScore = 0;

    private bool isGameOver;
        public bool IsGameOver {
            get{ return isGameOver; }
            set{
                isGameOver = value;
                if (isGameOver) {
                    CancelInvoke("CreateMonster");
                }
            }
        }
    public static GameManager instance = null;


    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        CreateMonsterPool();
        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;
        
        foreach(Transform point in spawnPointGroup) {
            points.Add(point);
        }

        InvokeRepeating("CreateMonster", 2.0f, createTime);
        totScore = PlayerPrefs.GetInt("TOT_SCORE", 0);
        DisplayScore(0);
    }

    void CreateMonster() {
        int idx = Random.Range(0, points.Count);
        
        GameObject _monster = GetMonsterInPool();
        _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);
        _monster?.SetActive(true);
    }

    void CreateMonsterPool() {
        for(int i = 0; i < maxMonsters; i++) {
            var _monster = Instantiate<GameObject>(monster);
            _monster.name = $"Monster_{i:00}";
            _monster.SetActive(false);
            monsterPool.Add(_monster);
        }
    }

    public GameObject GetMonsterInPool() {
        foreach(var _monster in monsterPool) {
            if (_monster.activeSelf == false) {
                return _monster;
            }
        }
        return null;
    }

    public void DisplayScore(int score) {
        totScore += score;
        scoreText.text = $"<color=#00ff00>Score :</color> <color=#ff0000>{totScore:#,##0}</color>";
        PlayerPrefs.SetInt("TOT_SCORE", totScore);
    }
}