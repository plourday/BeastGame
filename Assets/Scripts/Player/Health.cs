using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }

    private void Awake() 
    {
        currentHealth = startingHealth;
    }

    private void Update() 
    {
        
    }

    public void TakeDamage(float _damage) 
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
         
        if (currentHealth > 0)
        {
            // Player Hurt

        }
        else 
        {
            //player dead
        }
    }
}
