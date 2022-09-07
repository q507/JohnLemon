using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    private enum CurrentTime
    {
        Morning,
        Afternoon,
        Evening
    }

    private Camera mainCamera;

    public Light enviormentLight;

    public Color morning;
    public Color afternoon;
    public Color evening;

    public Color morningLight;
    public Color afternoonLight;
    public Color eveningLight;

    //切换天空颜色时间间隔
    public float onDayTime;

    private Color currentColor;
    private Color currentLightColor;

    private CurrentTime currentTime;

    //RGB
    private float red;
    private float green;
    private float blue;

    //RGB
    private float redLight;
    private float greenLight;
    private float blueLight;

    private void Start()
    {
        mainCamera = Camera.main;

        currentColor = mainCamera.backgroundColor;
        currentLightColor = enviormentLight.color;

        currentTime = CurrentTime.Afternoon;

        //获取RGB数值
        red = currentColor.r;
        green = currentColor.g;
        blue = currentColor.b;

        redLight = currentLightColor.r;
        greenLight = currentLightColor.g;
        blueLight = currentLightColor.b;

        StartCoroutine(CountTime());
    }

    private void Update()
    {
        _ChangeSky();
    }

    private void _ChangeSky()
    {
        switch (currentTime)
        {
            //晚—早
            case CurrentTime.Morning:
                red = Mathf.Lerp(red, morning.r, 0.5f * Time.deltaTime);
                blue = Mathf.Lerp(blue, morning.b, 0.5f * Time.deltaTime);
                green = Mathf.Lerp(green, morning.g, 0.5f * Time.deltaTime);

                redLight = Mathf.Lerp(redLight, morningLight.r, 0.5f * Time.deltaTime);
                blueLight = Mathf.Lerp(blueLight, morningLight.b, 0.5f * Time.deltaTime);
                greenLight = Mathf.Lerp(greenLight, morningLight.g, 0.5f * Time.deltaTime);

                enviormentLight.color = new Color(redLight, greenLight, blueLight);
                mainCamera.backgroundColor = new Color(red, green, blue);
                break;

            //早—午
            case CurrentTime.Afternoon:
                red = Mathf.Lerp(red, afternoon.r, 0.5f * Time.deltaTime);
                blue = Mathf.Lerp(blue, afternoon.b, 0.5f * Time.deltaTime);
                green = Mathf.Lerp(green, afternoon.g, 0.5f * Time.deltaTime);

                redLight = Mathf.Lerp(redLight, afternoonLight.r, 0.5f * Time.deltaTime);
                blueLight = Mathf.Lerp(blueLight, afternoonLight.b, 0.5f * Time.deltaTime);
                greenLight = Mathf.Lerp(greenLight, afternoonLight.g, 0.5f * Time.deltaTime);

                enviormentLight.color = new Color(redLight, greenLight, blueLight);
                mainCamera.backgroundColor = new Color(red, green, blue);
                break;

            //午—晚
            case CurrentTime.Evening:
                red = Mathf.Lerp(red, evening.r, 0.5f * Time.deltaTime);
                blue = Mathf.Lerp(blue, evening.b, 0.5f * Time.deltaTime);
                green = Mathf.Lerp(green, evening.g, 0.5f * Time.deltaTime);

                redLight = Mathf.Lerp(redLight, eveningLight.r, 0.5f * Time.deltaTime);
                blueLight = Mathf.Lerp(blueLight, eveningLight.b, 0.5f * Time.deltaTime);
                greenLight = Mathf.Lerp(greenLight, eveningLight.g, 0.5f * Time.deltaTime);

                enviormentLight.color = new Color(redLight, greenLight, blueLight);
                mainCamera.backgroundColor = new Color(red, green, blue);
                break;

            default:
                break;
        }
    }

    IEnumerator CountTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(onDayTime);

            if(currentTime == CurrentTime.Afternoon)
            {
                currentTime = CurrentTime.Evening;
            }
            else if(currentTime == CurrentTime.Evening)
            {
                currentTime = CurrentTime.Morning;
            }
            else if(currentTime == CurrentTime.Morning)
            {
                currentTime = CurrentTime.Afternoon;
            }
        }
    }
}
