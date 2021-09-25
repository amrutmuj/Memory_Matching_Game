using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject CellPrefab;
    public GameObject Parent;
    int TotalCells = 12;
    public Sprite[] objectSprite;    
    public List<Sprite> PickSprite = new List<Sprite>();
    bool FirstClick = false;
    bool SecondClick = false;
    public string FirstSpriteName, SecondSpriteName;
    public Sprite Cellbg;
    int FirstCellPos, SecondCellPos;
    int Youwon;
    public List<int> RandomizationofCell = new List<int>();
    public TextMeshProUGUI TimerUi;
    public float timeinterval = 20;
    public GameObject GameOverPanel;
    bool CheckifWon = false;
    


    // Start is called before the first frame update
    void Start()
    {
        for ( int i = 0; i<TotalCells; i++)
        {
            GameObject CellInstance = Instantiate(CellPrefab);
            CellInstance.transform.SetParent(Parent.transform);
            CellInstance.gameObject.name = i.ToString();
            ButtonClick(CellInstance);
            RandomizationofCell.Add(i);

        }
        CollectingSprite();


    }

    // Now assigning Sprite to GameObject Card 

    public void  CollectingSprite()
    {
        int index = 0;
        for (int i = 0; i < TotalCells; i++)
        {
            if (i == TotalCells/2)
            {
                index = 0;
            }
            PickSprite.Add(objectSprite[index]);
            index++;    
        }
        Randomization();
    }


    // Randomization of Cells or Avatars

    public void Randomization()
    {
        RandomizationofCell = RandomizationofCell.OrderBy(Out => System.Guid.NewGuid()).ToList();
    }


    // Button clicked, i.e, when a Card is being Clicked

    void ButtonClick(GameObject CellInstance)
    {
        CellInstance.GetComponent<Button>().onClick.AddListener(DetectClick);

    }

    // Detect Click
    public void DetectClick()
    {
        // Applying Logic so that only two clicks are allowed atmost
        if (!FirstClick)
        {
            FirstCellPos = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            FirstClick = true;
            Parent.transform.GetChild(FirstCellPos).GetComponent<Image>().sprite = PickSprite[RandomizationofCell[FirstCellPos]];
            
            FirstSpriteName = Parent.transform.GetChild(FirstCellPos).GetComponent<Image>().sprite.name;
            Debug.Log("Object Pressed : " + UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            Parent.transform.GetChild(FirstCellPos).GetComponent<Button>().enabled = false;

        }

        else if (!SecondClick)
        {
            SecondCellPos = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            SecondClick = true;
            Parent.transform.GetChild(SecondCellPos).GetComponent<Image>().sprite = PickSprite[RandomizationofCell[SecondCellPos]];
            
            SecondSpriteName = Parent.transform.GetChild(SecondCellPos).GetComponent<Image>().sprite.name;
            Debug.Log("Object Pressed : " + UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            Parent.transform.GetChild(SecondCellPos).GetComponent<Button>().enabled = false;
            Invoke("Match", 0.4f);

        }
    }

    // Does the first ad Second Matched or not

    void Match()
    {
        if(FirstSpriteName == SecondSpriteName)
        {
            FirstClick = false;
            SecondClick = false;
            Youwon++;
            if(Youwon == TotalCells/2)
            {
                Debug.Log("Won");
                CheckifWon = true;
                GameOverPanel.SetActive(true);
                GameOverPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You Won";
            }
        }

        else
        {
            FirstClick = false;
            SecondClick = false;
           
            Parent.transform.GetChild(FirstCellPos).GetComponent<Image>().sprite = Cellbg;
            Parent.transform.GetChild(SecondCellPos).GetComponent<Image>().sprite = Cellbg;
            Parent.transform.GetChild(FirstCellPos).GetComponent<Button>().enabled = true;
            Parent.transform.GetChild(SecondCellPos).GetComponent<Button>().enabled = true;
            GameOverPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You Won";

        }
    }
            
    // Update is called once per frame
    void Update()
    {
        if (!CheckifWon)
        {
            timeinterval = timeinterval - Time.deltaTime;
            TimerUi.text = "Timer: " + timeinterval.ToString("0");
            if (timeinterval < 0)
            {
                TimerUi.text = "Timer: 0";
                GameOverPanel.SetActive(true);
                GameOverPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You Lost";
            }
        }
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
