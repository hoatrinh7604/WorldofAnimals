using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] int score;
    [SerializeField] int highscore;
    public Color[] template = { new Color32(255, 81, 81, 255), new Color32(255, 129, 82, 255), new Color32(255, 233, 82, 255), new Color32(163, 255, 82, 255), new Color32(82, 207, 255, 255), new Color32(170, 82, 255, 255) };

    private int currentTarget = 0;
    [SerializeField] Image colorImage;
    private int nextTarget = 0;
    [SerializeField] Image colorNextImage;
    private UIController uiController;

    private float time;
    [SerializeField] float timeToChangeColor;
    [SerializeField] float timeOfGame;

    [SerializeField] BackgroundController bgController;
    [SerializeField] AnimalSearching animalSearching;
    private int currentMapIndex;

    private int remainingAnimals;

    // Start is called before the first frame update
    void Start()
    {
        uiController = GetComponent<UIController>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        UpdateSlider();

        if(time < 0)
        {
            GameOver();
        }
    }

    public void UpdateSlider()
    {
        uiController.UpdateSlider(time);
    }

    public void SetSlider()
    {
        uiController.SetSlider(timeOfGame);
    }

    public void OnPressHandle(int index)
    {
        if(index == currentTarget)
        {
            remainingAnimals--;
            UpdateScore();
            if (remainingAnimals == 0)
            {
                GameOver();
            }

            bgController.UpdateListIDSpawn(currentTarget);
            SetCurrentTarget();
            if(currentTarget == -1)
            {
                GameOver();
            }
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        uiController.GameOver();
        Reset();
    }

    public void UpdateScore()
    {
        score = remainingAnimals;
        uiController.UpdateScore(score);
    }

    public void SetCurrentTarget()
    {
        currentTarget = bgController.GetRandomIDAnimalExist();
        animalSearching.SetTarget(currentTarget);
    }

    public void RandomMap()
    {
        currentMapIndex = bgController.HandleShowBG();
        int amountAllAnimal = animalSearching.GetAmountAnimals();
        int maxAnimalInMap = bgController.GetMaxAnimal();
        int max = (amountAllAnimal > maxAnimalInMap) ? maxAnimalInMap : amountAllAnimal;
        remainingAnimals = Random.Range(10, max);
        bgController.SpawAnimals(remainingAnimals);
        SetCurrentTarget();
    }

    public void Reset()
    {
        Time.timeScale = 1;

        RandomMap();
        
        time = timeOfGame;
        SetSlider();
        score = remainingAnimals;
        uiController.UpdateScore(score);
        //uiController.UpdateHighScore(highscore);
    }

}
