using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    [SerializeField] GameObject goAnim;
    [SerializeField] GameObject threeAnim;
    [SerializeField] GameObject twoAnim;
    [SerializeField] GameObject oneAnim;
    [SerializeField] GameManager gameManager;

    private void Start()
    {
        goAnim.gameObject.SetActive(false);
        threeAnim.gameObject.SetActive(false);
        twoAnim.gameObject.SetActive(false);
        oneAnim.gameObject.SetActive(false);
    }

    public void gameBeginCountDown()
    {
        StartCoroutine(countDownCoroutine());
    }

    IEnumerator countDownCoroutine()
    {
        //3
        threeAnim.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        threeAnim.gameObject.SetActive(false);
        //2
        twoAnim.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        twoAnim.gameObject.SetActive(false);
        //1
        oneAnim.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        oneAnim.gameObject.SetActive(false);
        //go
        gameManager.isStartGame = true;
        goAnim.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        goAnim.gameObject.SetActive(false);
        //¿ªÊ¼ÓÎÏ·
        UIManager.Instance.gameUI.SetActive(true);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
