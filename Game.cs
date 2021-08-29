using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public GameObject myPrefab;
    Snake player = new Snake();
    Food food = new Food();
    string pressed = "r"; // l - left; r - right; u - up; d - down - movement direction
    public static bool dead = false;
    public static int score = 0;
    public Text scoreLabel;

    public GameObject endGame;

    void Start()
    {
        player.SetPrefab(myPrefab);
        player.CreateFirstThreeBodyParts();
        food.SetGameObject(GameObject.Find("Food"));
        food.PlaceFood(player.x, player.y);
        dead = false;
        endGame.SetActive(false);
        score = 0;

        InvokeRepeating("GameUpdate", 0, 0.5f);
    }

    void Update()
    {
        if (Input.GetKeyDown("w") && pressed != "d")
        {
            pressed = "u";
        }
        if (Input.GetKeyDown("d") && pressed != "l")
        {
            pressed = "r";
        }
        if (Input.GetKeyDown("s") && pressed != "u")
        {
            pressed = "d";
        }
        if (Input.GetKeyDown("a") && pressed != "r")
        {
            pressed = "l";
        }
    }

    void GameUpdate() {
        if(!dead)
        {
            player.Movement(pressed, food);
            scoreLabel.text = "" + score;
        }
        else
        {
            endGame.SetActive(true);
        }

    }

    public static void ScoreUpdate()
    {
        score++;
    }
}

public class Snake : MonoBehaviour
{

    public Queue<int> x = new Queue<int>();
    public Queue<int> y = new Queue<int>();
    private GameObject prefab;
    private int headX = -2;
    private int headY = 0;

    public void SetPrefab(GameObject pfb){
        prefab = pfb;   
    }

    public void CreateFirstThreeBodyParts()
    {
        CollisionCheckWithNewCoordinates(0, 0);
        CollisionCheckWithNewCoordinates(1, 0);
        CollisionCheckWithNewCoordinates(1, 0);
    }

    public void CollisionCheckWithNewCoordinates(int xCoord, int yCoord)
    {
        headX += xCoord;
        headY += yCoord;

        if (CollisionCheck())
        {
            Game.dead = true;
            return;
        }
        CreateNewHead();
    }

    public void CreateNewHead() 
    {
        x.Enqueue(headX);
        y.Enqueue(headY);

        GameObject bodyPart = (GameObject)Instantiate(prefab, new Vector3(headX, headY, 0), Quaternion.identity);
        bodyPart.name = "BodyPartX=" + headX + ";Y=" + headY;
    }

    public void DeleteOldTail()
    {
        GameObject bodyPart = GameObject.Find("BodyPartX=" + x.Peek() + ";Y=" + y.Peek());
        if (bodyPart)
        {
            Destroy(bodyPart.gameObject);
        }
        x.Dequeue();
        y.Dequeue();
    }

    public void Movement(string pressed, Food food)
    {


        if (pressed == "r")
        {
            CollisionCheckWithNewCoordinates(1, 0);
        }
        else if(pressed == "l")
        {
            CollisionCheckWithNewCoordinates(-1, 0);
        }
        else if (pressed == "u")
        {
            CollisionCheckWithNewCoordinates(0, 1);
        }
        else if (pressed == "d")
        {
            CollisionCheckWithNewCoordinates(0, -1);
        }

        

        if (headX == food.x && headY == food.y)
        {
            food.PlaceFood(x, y);
            Game.ScoreUpdate();
            return;
        }

        DeleteOldTail();
    }

    private bool CollisionCheck()
    {
        if (CollisionWithItself())
        {
            return true;
        }
        if (CollisionWithBorders())
        {
            return true;
        }
        return false;
    }

    private bool CollisionWithItself()
    {
        if(GameObject.Find("BodyPartX=" + headX + ";Y=" + headY))
        {
            return true;
        }
        return false;
    }
    
    private bool CollisionWithBorders()
    {
        if (headX >= 6 || headX <= -6 || headY >= 6 || headY <= -6)
        {
            return true;
       }
        return false;
    }

}

public class Food : MonoBehaviour
{
    public int x;
    public int y;
    private GameObject food;

    public void SetGameObject(GameObject foodobj)
    {
        food = foodobj;
    }

    public void PlaceFood(Queue<int> playerX, Queue<int> playerY) {

        CheckIfNewPositionFreeAndSetCoordinates(playerX, playerY);
        food.transform.position = new Vector3(x, y, 0);
    }

    public void CheckIfNewPositionFreeAndSetCoordinates(Queue<int> playerX, Queue<int> playerY)
    {
        x = Random.Range(-5, 5);
        y = Random.Range(-5, 5);

        int[] xArr = playerX.ToArray();
        int[] yArr = playerY.ToArray();

        for (int i = 0; i < xArr.Length; i++)
        {
            if (xArr[i] == x && yArr[i] == y)
            {
                x = Random.Range(-5, 5);
                y = Random.Range(-5, 5);
                i = 0;
            }
        }
    }

}