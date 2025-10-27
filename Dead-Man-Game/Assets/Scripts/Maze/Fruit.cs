using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Fruit : Pellet
{
    //[SerializeField] private FruitType fruitType;
    [SerializeField] private List<Sprite> fruitIcons = new List<Sprite>();
    private SpriteRenderer renderer;

    protected override void Awake(){
        renderer = GetComponent<SpriteRenderer>();

        switch (GameManager.Instance.currentLevel){
            case 1:
                points = 100;
                renderer.sprite = fruitIcons[0];
                break;
            case 2:
                points = 300;
                renderer.sprite = fruitIcons[1];
                break;
            case 3 or 4:
                points = 500;
                renderer.sprite = fruitIcons[2];
                break;
            case 5 or 6:
                points = 700;
                renderer.sprite = fruitIcons[3];
                break;
            case 7 or 8:
                points = 1000;
                renderer.sprite = fruitIcons[4];
                break;
            case 9 or 10:
                points = 2000;
                renderer.sprite = fruitIcons[5];
                break;
            case 11 or 12:
                points = 3000;
                renderer.sprite = fruitIcons[6];
                break;
            case > 12:
                points = 5000;
                renderer.sprite = fruitIcons[7];
                break;
        }
    }

    protected override void Eat()
    {
        GameManager.Instance.PelletEaten(this);
        AudioManager.Instance.PlaySound("FruitEaten");
        FruitManager.Instance.RemoveFruit();
        Debug.Log($"fruit eaten: {points} points");
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman")) {
            Eat();
            AudioManager.Instance.PlaySound("PelletEaten");
        }
    }
}
