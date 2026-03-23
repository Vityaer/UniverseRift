using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HelpFuction
{
    public class GameTimer
    {
        private Action dels;
        private void Register(Action d)
        {
            dels += d;
        }
        private void UnRegister(Action d)
        {
            dels -= d;
        }
        private void PlayFunction()
        {
            if (!isDone)
            {
                if (dels != null)
                {
                    dels();
                }
                isDone = true;
            }
        }
        private float startTime = 0f;
        private float time;
        public float Time { get => time; }
        private bool isDone;
        private bool isLoop;

        public bool IsDone { get => isDone; }
        public bool IsLoop { get => isLoop; set => isLoop = value; }
        public GameTimer(float time, Action function)
        {
            startTime = time;
            this.time = time;
            isDone = false;
            Register(function);
        }
        public void ChangeTime(float deltatime)
        {
            time -= deltatime;
            if (time < 0f)
            {
                PlayFunction();
                if (isLoop)
                {
                    this.time = startTime;
                    isDone = false;
                }
            }
        }
    }

    public class TimerScript : MonoBehaviour
    {
        public static TimerScript Timer;
        public List<GameTimer> listTimer = new List<GameTimer>();
        private Coroutine coroutineTime;

        void Awake()
        {
            if (Timer == null)
            {
                Timer = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public GameTimer StartTimer(float time, Action function)
        {
            GameTimer timer = new GameTimer(time, function);
            if (time > 0f)
            {
                listTimer.Add(timer);
                if (coroutineTime == null)
                    coroutineTime = StartCoroutine(ITimer());
            }
            return timer;
        }

        public GameTimer StartLoopTimer(float time, Action function)
        {
            GameTimer timer = StartTimer(time, function);
            timer.IsLoop = true;
            return timer;
        }

        public void StopTimer(GameTimer timer)
        {
            listTimer.Remove(timer);
        }

        IEnumerator ITimer()
        {
            while (listTimer.Count > 0)
            {
                foreach (GameTimer t in listTimer)
                {
                    t.ChangeTime(Time.deltaTime);
                }

                List<GameTimer> timers = listTimer.Where(x => (x.IsDone && !x.IsLoop)).ToList();
                foreach (GameTimer Item in timers)
                {
                    listTimer.Remove(Item);
                }
                yield return null;
            }
            coroutineTime = null;
        }
    }
}
