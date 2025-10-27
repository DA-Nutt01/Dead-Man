using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    // Define Event
    public static event Action<Pellet> OnPelletEaten;

    public int points;

    protected virtual void Awake(){
        points = 10;
    }

    protected virtual void Eat()
    {
        OnPelletEaten?.Invoke(this);
        GameManager.Instance.PelletEaten(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman")) {
            Eat();
            AudioManager.Instance.PlaySound("PelletEaten");
        }
    }

}
