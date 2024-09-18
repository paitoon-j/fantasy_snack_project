using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Helper
{
    public static float GetNumberWithComma(float number)
    {
        float round = Mathf.Round(number * 100.0f) * 0.01f; // 2 decimal
        return round;
    }

    public static bool GetRandomWithPercent(int valid)
    {
        int randomNumber = Random.Range(0, 100);
        if (randomNumber <= valid)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int GetRandomRang(int min, int max)
    {
        int randomRang = Random.Range(min, 1 + max);
        return randomRang;
    }

    public static async UniTask DelayAsync(float delay)
    {
        await UniTask.Delay((int)(delay * 1000));
    }

    public static void Exit()
    {
        Application.Quit();
    }

    public static void LoadSceneGame()
    {
        SceneManager.LoadScene(1);
    }
}