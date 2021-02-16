using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Text healthText;

    private float currentHealth;

    private void Start() {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
        healthText.text = currentHealth.ToString();
        if (currentHealth <= 0) {
            // Restart scene if dead
            Invoke("RestartScene", 1f);
        }
    }

    private void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
