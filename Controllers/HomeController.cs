using Google.Apis.Gmail.v1.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThesisProject.Models;
using ThesisProject.Services;
using ThesisProject.Services.EmailOrganizer.Services;

namespace ThesisProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserHome()
        {
            JSONService<UserPreference> jsonReader = new JSONService<UserPreference>();

            string path;
            path = System.IO.Path.GetDirectoryName(
               System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string localPath = new Uri(path).LocalPath;

            //UserPreference up = jsonReader.ConvertJSONToObject(localPath + @"\Data\UserPreference.json");

            UserPreference up = jsonReader.ConvertJSONToObject("C:/Users/swapn/Desktop/Thesis/ThesisProject/ThesisProject/Data/UserPreference.json");

            return View(up);
        }
       
        [HttpGet]
        public ActionResult CreateDictionary()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateDictionary(string s)
        {
            string path;
            path = System.IO.Path.GetDirectoryName(
               System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string localPath = new Uri(path).LocalPath;

            string userPreferencesFile = "C:/Users/swapn/Desktop/Thesis/ThesisProject/ThesisProject/Data/UserPreference.json";
            string trainingSetFile = "C:/Users/swapn/Desktop/Thesis/ThesisProject/ThesisProject/Data/TrainingSet.json";

            JSONService<UserPreference> userPreferenceReader = new JSONService<UserPreference>();
            UserPreference up = userPreferenceReader.ConvertJSONToObject(userPreferencesFile);

            JSONService<List<TrainingSet>> trainingSetReader = new JSONService<List<TrainingSet>>();

            List<TrainingSet> ts = trainingSetReader.ConvertJSONToObject(trainingSetFile);

            Dictionary<string, List<string>> keywordDict = new Dictionary<string, List<string>>();

            foreach (string item in up.ShoppingDomain)
            {
                foreach (TrainingSet t in ts)
                {
                    if (t.From.Contains(item))
                    {
                        List<string> subjectWords = new List<string>(t.Subject.Split(' '));
                        List<string> bodyWords = new List<string>(t.Body.Split(' '));
                        List<string> allWords = new List<string>();

                        subjectWords = (from x in subjectWords
                                        where IsWordAStopWord(x) == false
                                        select x).ToList();
                        bodyWords = (from x in bodyWords
                                     where IsWordAStopWord(x) == false
                                     select x).ToList();
                        allWords.AddRange(subjectWords);
                        allWords.AddRange(bodyWords);
                        List<string> mostFreqWords = GetWordsWithMostFrequency(allWords, 1);
                        if (keywordDict.ContainsKey("ShoppingDomain"))
                        {
                            List<string> keywordList = keywordDict["ShoppingDomain"];
                            keywordList.AddRange(mostFreqWords);

                        }
                        else // no key exists...create a key and assign the value
                        {
                            keywordDict["ShoppingDomain"] = mostFreqWords;
                        }

                    }
                }

            }

            string keywordjson = JsonConvert.SerializeObject(keywordDict);
            //System.IO.File.Delete(localPath + @"\Data\KeywordDictFile.json");
            System.IO.File.WriteAllText("C:/Users/swapn/Desktop/Thesis/ThesisProject/ThesisProject/Data/KeywordDictFile.json", keywordjson);

            Dictionary<string, List<string>> key = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(keywordjson);
            return View();
        }

        private bool IsWordAStopWord(string word)
        {
            List<string> stopWordList = new List<string>();
            stopWordList.Add("the");
            stopWordList.Add("for");
            stopWordList.Add("and");
            stopWordList.Add("to");

            return stopWordList.Contains(word);


        }

        private List<string> GetWordsWithMostFrequency(List<string> inputlist, int numberOfWords)
        {
            List<string> outputList = new List<string>();

            // group words by count in descending order
            var wordCount = from word in inputlist
                            group word by word into g
                            orderby g.Count() descending
                            select new { g.Key, Count = g.Count() };

            // Get the list of word counts
            List<int> wordCounts = (from word in wordCount
                                    orderby word.Count descending
                                    select word.Count).ToList<int>();

            // Get distinct word counts
            HashSet<int> distinctWordCounts = new HashSet<int>(wordCounts);

            // Iterate through word counts and get only as many word counts as specified by value of numberOfWords input parameter
            foreach (var x in distinctWordCounts)
            {
                if (outputList.Count < numberOfWords)
                {
                    var selectWords = from word in wordCount
                                      where word.Count == x
                                      select word.Key;
                    outputList.AddRange(selectWords);
                }
                else
                {
                    break;
                }

            }

            return outputList;
        }

        [HttpGet]
        public ActionResult ClassifyEmails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ClassifyEmails(string s)
        {

            EmailService es = new EmailService();
            IList<Label> labels = es.GetLabels();
            IList<Message> messages = es.GetMessages("Warner");

            Message msg = es.GetMessageDetail(messages[0].Id);
            string snippet = msg.Snippet;
           // string decodedString = null;
            //if (msg.Payload.Parts[0].Parts != null)
            //{
             //   decodedString = es.Base64Decode(msg.Snippet);
            //}
            //else
            //{
            //    decodedString = es.Base64Decode(msg.Payload.Body.Data);
            //}

            return View();
        }

    }

}