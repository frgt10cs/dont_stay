using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Пользовательский таймер
/// </summary>
public class Timer : MonoBehaviour
{

    #region Статичные поля

    /// <summary>
    /// Создаёт новый game object, который отсчитывает время и вызывает заданный метод
    /// </summary>
    /// <param name="name">Имя создаваемого GameObject'a</param>
    /// <param name="duration">Задержка таймера до вызова метода</param>
    /// <param name="countOfRepeat">Число повторений (1 - таймер крутит один раз, -1 - бесконечность)</param>
    /// <param name="method">Метод, который будет вызываться</param>
    /// <returns>Объект с таймером</returns>
    public static GameObject StartNewTimer(string name, float duration, int countOfRepeat, Action method)
    {
        GameObject obj = new GameObject(name + "_Timer");
        obj.transform.parent = GameObject.Find("Timers").transform;
        Timer timer = obj.AddComponent<Timer>();
        timer.duration = duration;
        timer.countOfRepeat = countOfRepeat;
        timer.method = method;
        timer.n = duration;
        return obj;
    }

    /// <summary>
    /// Создаёт новый game object, который отсчитывает время и вызывает заданный метод.
    /// Второй метод вызывается каждый кадр, когда таймер работает
    /// </summary>
    /// <param name="name">Имя создаваемого GameObject'a</param>
    /// <param name="duration">Задержка таймера до вызова метода</param>
    /// <param name="countOfRepeat">Число повторений (1 - таймер крутит один раз, -1 - бесконечность)</param>
    /// <param name="method">Метод, который будет вызываться</param>
    /// <param name="integral">Метод, вызывающийся каждый кадр (параметр - время рендра кадра)</param>
    /// <returns>Объект с таймером</returns>
    public static GameObject StartNewTimer(string name, float duration, int countOfRepeat, Action method, Action<float, Timer> integral)
    {
        GameObject obj = new GameObject(name + "_Timer");
        obj.transform.parent = GameObject.Find("Timers").transform;
        TimerWithIntegralSimple timer = obj.AddComponent<TimerWithIntegralSimple>();
        timer.duration = duration;
        timer.countOfRepeat = countOfRepeat;
        timer.method = method;
        timer.n = duration;
        timer.integral = integral;
        return obj;
    }

    /// <summary>
    /// Создаёт новый game object, который отсчитывает время и вызывает заданный метод.
    /// Второй метод вызывается каждый кадр, когда таймер работает
    /// </summary>
    /// <param name="name">Имя создаваемого GameObject'a</param>
    /// <param name="duration">Задержка таймера до вызова метода</param>
    /// <param name="countOfRepeat">Число повторений (1 - таймер крутит один раз, -1 - бесконечность)</param>
    /// <param name="method">Метод, который будет вызываться</param>
    /// <param name="integral">Метод, вызывающийся каждый кадр (первый параметр - время рендра кадра, второй - сколько осталось времени, третий - общее время таймера)</param>
    /// <returns>Объект с таймером</returns>
    public static GameObject StartNewTimer(string name, float duration, int countOfRepeat, Action method, Action<float, float, float, Timer> integral)
    {
        GameObject obj = new GameObject(name + "_Timer");
        obj.transform.parent = GameObject.Find("Timers").transform;
        TimerWithIntegral timer = obj.AddComponent<TimerWithIntegral>();
        timer.duration = duration;
        timer.countOfRepeat = countOfRepeat;
        timer.method = method;
        timer.n = duration;
        timer.integral = integral;
        return obj;
    }

    /// <summary>
    /// Приостановить таймер
    /// </summary>
    /// <param name="name">Имя таймера, который вы хотите приостановить</param>
    public static void StopTimer(string name) { GameObject.Find(name + "_Timer").GetComponent<Timer>().stop = true; }

    /// <summary>
    /// Приостановить таймер
    /// </summary>
    /// <param name="timer">Игровой объект с  таймером, который вы хотите приостановить</param>
    public static void StopTimer(GameObject timer) { timer.GetComponent<Timer>().stop = true; }

    /// <summary>
    /// Возабновить таймер (начинается с момента его остановки)
    /// </summary>
    /// <param name="name">Имя таймера, который вы хотите запустить</param>
    public static void ReStartTimer(string name) { GameObject.Find(name + "_Timer").GetComponent<Timer>().stop = false; }

    /// <summary>
    /// Возабновить таймер (начинается с момента его остановки)
    /// </summary>
    /// <param name="timer">Игровой объект с  таймером, который вы хотите запустить</param>
    public static void ReStartTimer(GameObject timer) { timer.GetComponent<Timer>().stop = false; }

    #endregion

    public float duration;
    public int countOfRepeat;
    public Action method;
    public bool stop;
    protected float n;

    protected virtual void Update()
    {
        if (stop == false)
        {
            if (n < 0f) n = 0;
            else if (n == 0)
            {
                method();
                countOfRepeat--;
                if (countOfRepeat == 0) Destroy(this.gameObject);
                else n = duration;
            }
            else n -= Time.deltaTime;
        }
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}

/// <summary>
/// Таймер с вызовом промежуточного метода(Time.deltatime)
/// </summary>
public class TimerWithIntegralSimple : Timer
{
    public Action<float, Timer> integral;

    protected override void Update()
    {
        if (stop == false)
        {
            if (n < 0f)
                n = 0;
            else if (n == 0)
            {
                if ((object)method != (object)null)
                    method();
                countOfRepeat--;
                if (countOfRepeat == 0)
                    Destroy(this.gameObject);
                else
                    n = duration;
            }
            else { n -= Time.deltaTime; integral(Time.deltaTime, this); }
        }
    }
}

/// <summary>
/// Таймер с вызовом промежуточного метода(Time.deltatime, n, duration)
/// </summary>
public class TimerWithIntegral : Timer
{
    public Action<float, float, float, Timer> integral;

    protected override void Update()
    {
        if (stop == false)
        {
            if (n < 0f) n = 0;
            else if (n == 0)
            {
                if ((object)method != (object)null) method();
                countOfRepeat--;
                if (countOfRepeat == 0) Destroy(this.gameObject);
                else n = duration;
            }
            else { n -= Time.deltaTime; integral(Time.deltaTime, n, duration, this); }
        }
    }
}