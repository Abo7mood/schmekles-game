using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _loadingScreen;//the loading screen object
    [SerializeField] GameObject _backGround;//the background for the loading screen
    [SerializeField] Slider _slider;//the slider from 0 to 100
    [SerializeField] TextMeshProUGUI _progressText;// the progress text from 0 to 100
    [SerializeField] TMP_InputField _text;//the input for the name
    [SerializeField] GameObject _warnParent;//warn
  
    private void Awake()
    {//reference 
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            _loadingScreen = GameObject.Find("Canvas").transform.Find("LoadingScreen").gameObject; //ref
            _backGround = _loadingScreen.transform.Find("Slider").transform.Find("Background").gameObject;
            _slider = _loadingScreen.transform.Find("Slider").gameObject.GetComponent<Slider>();
            _progressText = _slider.transform.Find("Fill Area").transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();
        }
       
    }
 
    public void LoadSceneFight(int sceneIndex)
    {
        StartCoroutine(LoadSync(sceneIndex));
        if (_backGround != null)
            _backGround.SetActive(false);
        if (_loadingScreen != null)
            _loadingScreen.SetActive(true);
    }
     IEnumerator OnLoadNext(float time)
    {
        int characters=0;
        foreach (char go in _text.text)
        {
            characters++;
        }
       
        Debug.Log(characters);
        if (string.IsNullOrEmpty(_text.text) )
        {
            StartCoroutine(Warn(1));
         
        }else if (characters <= 2)
        {
            StartCoroutine(Warn(1));
        }
        else
        {
            yield return new WaitForSeconds(time);
            StartCoroutine(LoadSync(SceneManager.GetActiveScene().buildIndex + 1));
            if (_backGround != null)
                _backGround.SetActive(false);
            if (_loadingScreen != null)
                _loadingScreen.SetActive(true);
        }
       
    }
    public void LoadNext(float time) => StartCoroutine(OnLoadNext(time));
    IEnumerator LoadSync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            _slider.value = progress;
            _progressText.text = progress * 100f + "%";

            yield return null;


        }
    }



    public void Menu()
    {
        if(SceneManager.GetActiveScene().name!="MainMenu")
            SceneManager.LoadScene("MainMenu");
    }

    public void Quit() => Application.Quit();

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
   public IEnumerator Warn(float index)
    {
        _warnParent.SetActive(true);
        yield return new WaitForSeconds(index);
        _warnParent.SetActive(false);
    }

}
