using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class bottons : MonoBehaviour
{

    public Sprite musicOn, musicOff; //Sprite означает тип данных спрайт

    private void Start() {
        if (PlayerPrefs.GetString("music") == "on" && gameObject.name == "Music")
        {
            PlayerPrefs.SetString("music", "on");
            GetComponent<Image>().sprite = musicOn;
        }
    }

    public void RestartGame() {
        //SceneManager.LoadScene(0);
        if (PlayerPrefs.GetString("music") == "on")
        {
            GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 
    
    public void openInstagram() {
        if (PlayerPrefs.GetString("music") == "on")
        {
            GetComponent<AudioSource>().Play();
        }
        Application.OpenURL("https://www.instagram.com/ilya_bon");
    }    
       
    public void openShop() {
        if (PlayerPrefs.GetString("music") == "on")
        {
            GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene("Shop");
    }    
        
    
    public void closeShop() {
        if (PlayerPrefs.GetString("music") == "on")
        {
            GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene("Main");
    }    

    public void musicWork() {

        if (PlayerPrefs.GetString("music") == "off")
        {
            GetComponent<AudioSource>().Play();
        }

        if (PlayerPrefs.GetString("music")=="off") {
            PlayerPrefs.SetString("music", "on");
            GetComponent<Image>().sprite = musicOn;
        } else {
            PlayerPrefs.SetString("music", "off");
            GetComponent<Image>().sprite = musicOff;
        }
    }

}
