using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaConnect_mobile.DataModel
{
    class TodoItem
    {
        public TodoItem()
        {
        }

        private String id;
        private String name;
        private String victim;
        private String distance;
        private String date;
        private Boolean complete;

        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "victim")]
        public string Victim { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public string Distance { get; set; }

        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        public TodoItem(String id, String name, String victim, String distance, String date, Boolean complete)
        {
            this.id = id;
            this.name = name;
            this.victim = victim;
            this.distance = distance;
            this.date = date;
            this.complete = complete;
        }

    }
}
