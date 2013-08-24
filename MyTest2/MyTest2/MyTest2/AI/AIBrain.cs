using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace MyTest2.AI
{
    class AIBrain
    {
        private static AIBrain instance;

        Random r;
        String[] messages;

        public AIBrain()
        {
            r = new Random();
            messages = new String[5];

            messages[0] = "UP#";
            messages[1] = "DOWN#";
            messages[2] = "RIGHT#";
            messages[3] = "LEFT#";
            messages[4] = "SHOOT#";

        }

        public static AIBrain getAI
        {
            get
            {
                if (instance == null)
                {
                    instance = new AIBrain();
                }
                return instance;
            }
        }

        public void play()
        {

            int x = r.Next()%5;
            GameManager.getGameManager.sendMessage(messages[x]);

        }

        public void starter()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            this.play();
        }
    }
}
