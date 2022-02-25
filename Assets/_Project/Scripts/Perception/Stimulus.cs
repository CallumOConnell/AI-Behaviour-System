using UnityEngine;

namespace Perception
{
    public class Stimulus
    {
        public StimulusTypes Type;
        public GameObject Source;
        public GameObject SecondarySource;
        public Vector3 Location;
        public Vector3 Direction;
        public float Radius;

        public Stimulus() { }

        public Stimulus(StimulusTypes type, GameObject source, Vector3 location, Vector3 direction, float radius, GameObject secondarySource)
        {
            Type = type;
            Source = source;
            SecondarySource = secondarySource;
            Location = location;
            Direction = direction;
            Radius = radius;
        }
    }
}