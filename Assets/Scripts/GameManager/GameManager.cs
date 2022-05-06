using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mLobbyImage;
    [SerializeField]
    private List<Sprite> mBackImage = new List<Sprite>();
    [SerializeField]
    private GameObject mTitleImage;
    // Start is called before the first frame update
    void Start()
    {
        int ran = Random.Range(0, mBackImage.Count);
        mLobbyImage.GetComponent<Image>().sprite = mBackImage[ran];
        mTitleImage.transform.GetChild(ran).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTestLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


}
