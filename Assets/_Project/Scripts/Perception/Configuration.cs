using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Perception
{
    public class Configuration
    {
        [XmlAttribute("name")]
        public string TypeString { get; set; }
        public StimulusTypes Type => (StimulusTypes)Enum.Parse(typeof(StimulusTypes), TypeString);
        public float Peak { get; set; }
        public float Attack { get; set; }
        public float Decay { get; set; }
        public float SustainPercentage { get; set; }
        public float Sustain { get; set; }
        public float Release { get; set; }
        public float Modifier { get; set; }
        public float Agressive { get; set; }
        public float Threatning { get; set; }
        public float Interesting { get; set; }

        public static List<Configuration> GetConfigurations()
        {
            var deserializer = new XmlSerializer(typeof(List<Configuration>), new XmlRootAttribute("PerceptionConfiguration"));
            var textReader = new StreamReader("Assets/_Project/Scripts/Perception/PerceptionconfigData.xml");
            List<Configuration> configs;
            configs = (List<Configuration>)deserializer.Deserialize(textReader);
            textReader.Close();
            return configs;
        }

        public float GetPerception(float life)
        {
            if (life < Attack)
            {
                return GetGraphY(life, 0f, Attack, 0f, Peak);
            }
            else if (life < Decay)
            {
                return GetGraphY(life, Attack, Decay, Peak, Peak * SustainPercentage);
            }
            else if (life < Sustain)
            {
                return Peak * SustainPercentage;
            }
            else if (life < Release)
            {
                return GetGraphY(life, Sustain, Release, Peak * SustainPercentage, 0f);
            }
            else
            {
                return -1f;
            }
        }

        private float GetGraphY(float x, float x1, float x2, float y1, float y2)
        {
            float m = (y2 - y1) / (x2 - x1);
            return m * (x - x1) + y1;
        }
    }
}