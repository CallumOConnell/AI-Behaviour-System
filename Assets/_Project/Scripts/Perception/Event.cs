using UnityEngine;

namespace Perception
{
    public class Event : Stimulus
    {
        public Configuration Config;
        public float EventPerceptionValue;
        public float Life;
        public float Birth;
        public bool MarkEventForDeletion;

        public Event(Stimulus stimulus, Configuration config)
        {
            Type = stimulus.Type;
            Source = stimulus.Source;
            Location = stimulus.Location;
            Direction = stimulus.Direction;
            Radius = stimulus.Radius;
            Config = config;
            Birth = Time.time;
            Life = 0f;
            MarkEventForDeletion = false;
        }

        public void Process()
        {
            Life = Time.time - Birth;
            EventPerceptionValue = Config.GetPerception(Life);
            
            if (EventPerceptionValue < 0f)
            {
                MarkEventForDeletion = true;
            }
        }

        // Stimulation has been resent so update the event life
        public void Update()
        {
            // Don't change life if in attack or decay phase
            if (Life > Config.Decay)
            {
                // Reset the life to the start of the sustain period.
                Birth = Time.time - Config.Decay;
            }
        }
    }
}