using System.Collections;
using UnityEngine;

public class TimeScaleUI : MonoBehaviour
{
    public float SpeedUpRatio = 5;
    public float SpeedMax = 10;

    private float _normalSpeed = 1;

    private void Awake()
    {
        _normalSpeed = Time.timeScale;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void PlayAtNormalSpeed()
    {
        Time.timeScale = 1 * _normalSpeed;
    }

    public void SpeedUp()
    {
        Time.timeScale = Mathf.Min(SpeedMax, (Time.timeScale <= 0 ? 1 : Time.timeScale) * SpeedUpRatio);
    }
}