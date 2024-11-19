using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetSelection : MonoBehaviour
{
    public void SelectPlanet(string planetName)
    {
        PlayerPrefs.SetString("SelectedPlanet", planetName); // Store the selected planet name
        SceneManager.LoadScene("Earth_quiz"); // Load the quiz scene
    }
}