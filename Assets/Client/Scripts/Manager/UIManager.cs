using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("-----Health-----")]
    [SerializeField] HealthManager healthManager;
    [SerializeField] Image healthSlider;
    [SerializeField] Text healthNum;

    [Header("-----Energy-----")]
    [SerializeField] Image energySlider;
    [SerializeField] Text energyNum;

    [Header("-----GameEvent-----")]
    public Image winGameImage;
    public Image loseGameImage;
    public Text curOwnerMoney;
    public GameObject pointPanel;
    public GameObject gameUI;

    [Header("-----GameSettlement-----")]
    [SerializeField] Text countTimeText;
    [SerializeField] Text beatCountText;
    public Text scoreText;
    public Text totalScoreText;
    public int beatCount;

    [Header("-----Copy-----")]
    public GameObject copySelectionUI;

    [Header("-----CountTime-----")]
    public Text countTIme;

    [Header("-----TryReLoadGame-----")]
    public GameObject tryToReloadPanel;
    public GameObject reloadSuccessPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        winGameImage.gameObject.SetActive(false);
        loseGameImage.gameObject.SetActive(false);
        pointPanel.gameObject.SetActive(false);
    }

    public void UpdateHealth(float health_percentage)
    {
        StartCoroutine(UpdateHealthCoroutine(health_percentage));
    }

    IEnumerator UpdateHealthCoroutine(float health_percentage)
    {
        while(healthSlider.fillAmount > health_percentage)
        {
            healthSlider.fillAmount = Mathf.Lerp(healthSlider.fillAmount, health_percentage, Time.deltaTime);
            healthNum.text = (healthSlider.fillAmount * 100).ToString("0.0") + "%";
            yield return null;
        }
    }

    public void UpdateEnergy(float energy_percentage)
    {
        StartCoroutine(UpdateEnergyCoroutine(energy_percentage));
    }

    IEnumerator UpdateEnergyCoroutine(float energy_percentage)
    {
        while (healthSlider.fillAmount > energy_percentage)
        {
            energySlider.fillAmount = Mathf.Lerp(energySlider.fillAmount, energy_percentage, Time.deltaTime);
            energyNum.text = (energySlider.fillAmount * 100).ToString("0.0") + "%";
            yield return null;
        }
    }

    public void CountTime(float time)
    {
        countTimeText.text = time.ToString("0");
    }

    public void BeatCount(int count)
    {
        beatCount += count;
        Debug.Log("当前击杀的怪物数" + beatCount);
        beatCountText.text = beatCount.ToString();
    }

    public void Score(int score)
    {
        scoreText.text = score.ToString();
    }
}
