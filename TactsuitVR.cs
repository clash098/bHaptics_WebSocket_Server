using Bhaptics.SDK2;
using System;
using System.Threading;

namespace bHaptics
{
    public class TactsuitVR
    {
        public bool tactsuitInit = false;
        // Event to start and stop the heartbeat thread
        private static ManualResetEvent HeartBeat_mrse = new ManualResetEvent(false);

        public int heartbeatCount = 0;
        public int heartbeatMax = 4;

        public void HeartBeatFunc()
        {
            while (true)
            {
                // Check if reset event is active
                HeartBeat_mrse.WaitOne();
                PlaybackHaptics("HeartBeat");
                if (heartbeatCount > heartbeatMax)
                {
                    StopHeartBeat();
                }
                heartbeatCount++;
                Thread.Sleep(600);
            }
        }
        public void LOG(string logStr)
        {
            Console.WriteLine(logStr);
        }

        public TactsuitVR()
        {
            LOG("Initializing suit");
            var res = BhapticsSDK2.Initialize("", "", "");

            if (res > 0)
            {
                LOG("Failed to do bhaptics initialization...");
                return;
            }
            LOG("Starting HeartBeat thread...");
            Thread HeartBeatThread = new Thread(HeartBeatFunc);
            HeartBeatThread.Start();
            PlaybackHaptics("HeartBeat");
            tactsuitInit = true;
        }

        public void PlaybackHaptics(String key, float intensity = 1.0f, float duration = 1.0f, float xzAngle = 0f, float yShift = 0f)
        {
            int res;
            res = BhapticsSDK2.Play(key.ToLower(), intensity, duration, xzAngle, yShift);
        }

        public void StartHeartBeat()
        {
            HeartBeat_mrse.Set();
        }

        public void StopHeartBeat()
        {
            HeartBeat_mrse.Reset();
            heartbeatCount = 0;
        }

        public bool IsPlaying(String effect)
        {
            return BhapticsSDK2.IsPlaying(effect.ToLower());
        }

        public void StopHapticFeedback(String effect)
        {
            BhapticsSDK2.Stop(effect.ToLower());
        }

        public void StopAllHapticFeedback()
        {
            StopThreads();
            BhapticsSDK2.StopAll();
        }

        public void StopThreads()
        {
            StopHeartBeat();
        }
    }
}
