using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This Script Is Made By Ahmed Elshazly For The 11th ScoreSpace Jam


[System.Serializable]
public class BoardField
{
    public string PlayerName;
    public int Score;
    public EField boardfield;
    public BoardField(string PlayerName, int Score, EField boardfield)
    {
        this.PlayerName = PlayerName;
        this.Score = Score;
        this.boardfield = boardfield;
    }
}
public class ScoreBoard : MonoBehaviour
{

    public int DesiredNameCount;
    string[] Names =
    {
        "Xxx","Gamer","Killer","Bob","Jake","Hopper","Destroyer","Cod","Terminator","Sebastian","Claude",
        "Zlatan","Genie","Cristiano","Jakoba","Anthony","Melissa","Julius","Daniela","Keith","Roberta","Stephanie",
        "Hacker","German","Mexican","Irish","Italian","German","Russsian","English","brave","fat","creepy",
        "kind","ugly","brown","intelligent","skinny","ruthless","curvy","pretty","fox","puppy","frog",
        "owl","War","Lion","giraffe", "_","-","*"," "," "
    };

    public GameObject emptyfield;

    [HideInInspector]public List<BoardField> AddedBoard;

    [HideInInspector] public List<BoardField> ListedBoard;

    [HideInInspector] public List<GameObject> Allfields;


    public Transform VerticalLayout;

    public Color firstColor;
    public Color PlayerColor;

    public string PlayerName;

    public int Score;


    int PlayerHighestScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Checking if the current score is bigger than the highest score if true new Highest score is equal to current score
        if (Score > PlayerPrefs.GetInt("HighScore")) PlayerPrefs.SetInt("HighScore", Score);
        PlayerHighestScore = PlayerPrefs.GetInt("HighScore");
        AddRandomizedNames();
    }


    void AddRandomizedNames()
    {
        //Creating a name and score for each bot 
        for (int i = 0; i < DesiredNameCount-1; i++)
        {
            int stringcountinname = Random.Range(2, 4);
            string Name = "";
            int Score = 0;
            //Adding more names to the bot's full name
            for(int k = 0; k < stringcountinname; k++)
            {
                int RandName = Random.Range(0, Names.Length);
                Name += Names[RandName];
            }
            //adding random score to the bot
            Score = Random.Range(100, 13000);
            CreateField(Name, Score);
        }
        //Creating a new field for the player
        CreateField(PlayerName, PlayerHighestScore);
        ListBoard();
    }

    void CreateField(string name,int score)
    {
        //Creating a new gameobject to the scene with emptyfield
        GameObject newfieldGO = Instantiate(emptyfield);
        //Parenting our gameobject to the verticle layout
        newfieldGO.transform.parent = VerticalLayout;
        newfieldGO.transform.localScale = Vector3.one;
        //Creating a new empty field class and passing in our newfieldGO gameobject we made earlier
        EField newfield = newfieldGO.GetComponent<EmptyScoreField>().field;
        newfield.name.text= name;
        newfield.score.text = score.ToString();
        //Creating a new Boardfield with the name,score, and empty field we created earlier
        BoardField field = new BoardField(name, score,newfield);
        //Adding the field and the newfieldGO gameobject to lists 
        Allfields.Add(newfieldGO);
        AddedBoard.Add(field);
    }


    void ListBoard()
    {
        //Creating a new int for the highestscore
        int HighestScore = 0;

        //Creating a new int Array for all our board's players scores
        int[] AllBoardScore = new int[AddedBoard.Count];

        //Adding the scores to our AllBoardScore array
        for (int i = 0; i < DesiredNameCount; i++)
        {
            AllBoardScore[i] = AddedBoard[i].Score;
        }

        //Listing the board as a list
        for (int k = 0; k < DesiredNameCount; k++)
        {
            //Checking the max value of our Allboardscore array we made
            HighestScore = Mathf.Max(AllBoardScore);
            //Checking if a field has the same value as our highscore
            foreach (BoardField field in AddedBoard)
            {
                if (field.Score == HighestScore)
                {
                    ListedBoard.Add(field);
                    AddedBoard.Remove(field);
                    //Checking if this field score is equal to one of our ints in allboardscore
                    for (int j = 0; j < AllBoardScore.Length; j++)
                    {
                        if (field.Score == AllBoardScore[j])
                        {
                            AllBoardScore[j] = 0;
                        }
                    }
                    //Resetting our highscore back to 0 so the scoreboard wont have the same player multiple times
                    HighestScore = 0;
                    break;
                }
            }
        }

        //Listing the board to the scoreboard itself
        for (int x = 0; x < Allfields.Count; x++)
        {
            //Creating a new field with the listed field Efield's class 
            EField f = Allfields[x].GetComponent<EmptyScoreField>().field;
            //Setting the field's score and name
            f.name.text = x+1 +"." + ListedBoard[x].PlayerName;
            f.score.text = ListedBoard[x].Score.ToString();
            //making a bool which checks if our player's string is equal to this listed board index
            bool IsPlayer = PlayerName == ListedBoard[x].PlayerName;
            //if it's false and x = 0 which means it's the highest player's on the board set it's text color to golden
            if (x == 0 && !IsPlayer)
            {
                f.name.color = firstColor;
                f.score.color = firstColor;
            }
            //if Its true which means that this index is our player then set the text color to blue
            else if (IsPlayer)
            {
                f.name.color = PlayerColor;
                f.score.color = PlayerColor;
            }
        }
    }
}
