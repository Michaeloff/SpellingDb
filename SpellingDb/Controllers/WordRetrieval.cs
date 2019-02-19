using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using SpellingDb.Models;

namespace SpellingDb.Controllers
{
    public class DataObjectGoogle
    {
        public string Word { get; set; }
        public string Phonetic { get; set; }
        public Dictionary<String, List<Definitions>> Meaning { get; set; }
    }

    public class Definitions
    {
        public string Definition { get; set; }
        public string Example { get; set; }
        public List<string> Synonyms { get; set; }
    }

    public class DataObjectDataMuse
    {
        public string Word { get; set; }
        public int Score { get; set; }
        public int NumSyllables { get; set; }
        public string[] Tags { get; set; }
        public string[] Defs { get; set; }
    }

    public class OnlineWordRetrieval
    {
        static HttpClient client = new HttpClient();
        WordDataAccessLayer wordAccess = new WordDataAccessLayer();

        private const string URLDataMuse = "https://api.datamuse.com/words"; // https://sub.domain.com/objects.json
        private const string URLGoogle = "https://googledictionaryapi.eu-gb.mybluemix.net/";
        private const string URLOxford = "https://od-api.oxforddictionaries.com/api/v1/";
        //private string urlParameters = "?sp=cry&md=dps&max=1";       // ?api_key=123

        public IEnumerable<DataObjectGoogle> GetWordGoogle(string word)
        {
            var urlParameters = "?define=" + word + "&lang=en";
            IEnumerable<DataObjectGoogle> definition = null;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLGoogle);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                definition = response.Content.ReadAsAsync<IEnumerable<DataObjectGoogle>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                foreach (var wordData in definition)
                {
                    Console.WriteLine("{0}", wordData.Word);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();
            return definition;
        }

        public void GetWordOxford(string word)
        {
            var urlParameters = "entries/en/" + word;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLOxford);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
            client.DefaultRequestHeaders.Add("app_id", "5f343be9");
            client.DefaultRequestHeaders.Add("app_key", "b4322fd3c5cb5899ec7a36e9d6d029fb");

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
            }
        }

        public string GetWordDataMuse(string word)
        {
            var urlParameters2 = "?sp=" + word + "&md=dps&max=1";
            var definition = "not found";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLDataMuse);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters2).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObjectDataMuse>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                foreach (var d in dataObjects)
                {
                    Console.WriteLine("{0}", d.Word);
                    definition = d.Defs[0];
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            //Make any other calls using HttpClient here.

            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();

            return definition;
        }
    }
}


/*  DataMuse return packet from "https://api.datamuse.com/words?sp=cry&md=dps&max=1"
[{
"word":"cry",
"score":1708,
"numSyllables":1,
"tags":["n","v"],
"defs":[
    "n\ta fit of weeping",
    "n\ta loud utterance; often in protest or opposition",
    "n\ta loud utterance of emotion (especially when inarticulate)",
    "n\tthe characteristic utterance of an animal",
    "n\ta slogan used to rally support for a cause",
    "v\tshed tears because of sadness, rage, or pain",
    "v\tbring into a particular state by crying",
    "v\tproclaim or announce in public",
    "v\tutter a characteristic sound",
    "v\tdemand immediate action",
    "v\tutter aloud; often with surprise, horror, or joy","v\tutter a sudden loud cry"
    ]
}]
*/
/* Google return packet from https://googledictionaryapi.eu-gb.mybluemix.net/?define=dog&lang=en
[{
"word": "dog",
"phonetic": "/dɔɡ/",
"meaning": {
    "noun": [
        {
            "definition": "A domesticated carnivorous mammal that typically has a long snout, an acute sense of smell, nonretractable claws, and a barking, howling, or whining voice.",
            "synonyms": ["hound","canine","mongrel","cur","tyke"]
        },
        {
            "definition": "A person regarded as unpleasant, contemptible, or wicked (used as a term of abuse)",
            "example": "come out, Michael, you dog!"
        },
        {
            "definition": "A mechanical device for gripping."
        },
        {
            "definition": "Feet.",
            "example": "if only I could sit down and rest my tired dogs",
            "synonyms": ["tootsie","trotter"]
        },
        {
            "definition": "short for firedog"
        }
    ],
    "verb": [
        {
            "definition": "Follow (someone or their movements) closely and persistently.",
            "example": "photographers seemed to dog her every step",
            "synonyms": ["pursue","follow","stalk","track","trail","shadow","hound"]
        },
        {
            "definition": "Act lazily; fail to try one's hardest."
        },
        {
            "definition": "Grip (something) with a mechanical device.",
            "example": "she has dogged the door shut"
        }
    ]
    }
}]
*/
