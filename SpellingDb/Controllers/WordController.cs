using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpellingDb.Models;

namespace SpellingDb.Controllers
{
    public class WordController : Controller
    {
        WordDataAccessLayer wordAccess = new WordDataAccessLayer();
        byte currentGrade = 1;
        string currentList = "";

        public IActionResult Index()
        {
            List<Word> words = new List<Word>();
            words = wordAccess.GetAllWords().ToList();

            return View(words);
        }

        [HttpGet]
        public IActionResult Create(Word newWord)
        {
            return View(newWord);
        }

        [HttpPost]
        public IActionResult Search([Bind] Word word)
        {
            var wordRetrieval = new OnlineWordRetrieval();
            var words = wordRetrieval.GetWordGoogle(word.Name).ToList();
            var newWord = new Word();

            // words[0], words[1], etc. are the words
            // words[0].Word == name of the word
            // words[0].Meaning dictionary of meanings
            var firstWord = words.First();
            newWord.Name = firstWord.Word;
            newWord.List = word.List;
            newWord.Grade = word.Grade;

            var partOfSpeech = firstWord.Meaning.First();
            newWord.PartOfSpeech = partOfSpeech.Key;

            var definition = partOfSpeech.Value.First(); // Get first definition/example, then capitalize first letters and check they end with period.
            newWord.Definition = definition.Definition.First().ToString().ToUpper() + definition.Definition.Substring(1);
            newWord.Definition = newWord.Definition.EndsWith(".") ? newWord.Definition : newWord.Definition + ".";

            if (definition.Example != null)
            {
                newWord.Example = definition.Example.First().ToString().ToUpper() + definition.Example.Substring(1);
                newWord.Example = newWord.Example.EndsWith(".") ? newWord.Example : newWord.Example + ".";
            }

            if (definition.Synonyms != null)
            {
                newWord.Synonyms = String.Join(", ", definition.Synonyms.Take(7));
            }

            return RedirectToAction("Create", newWord);
        }

        [HttpPost]
        public IActionResult SearchOxford([Bind] Word word)
        {
            var wordRetrieval = new OnlineWordRetrieval();
            wordRetrieval.GetWordOxford(word.Name);

            var newWord = new Word
            {
                Name = "boat",
                PartOfSpeech = "noun",
                Definition = "A small vessel propelled on water by oars, sails, or an engine.",
                Example = "A fishing boat drifted in the lake.",
                Synonyms = "vessel, craft, watercraft, ship ",
                List = "1st - 2018 - words 18-34 ",
                Grade = 1
            };

            return RedirectToAction("Create", newWord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToDb([Bind] Word word)
        {
            if (ModelState.IsValid)
            {
                wordAccess.AddWord(word);
                word.Name = "";
                word.PartOfSpeech = "";
                word.Definition = "";
                word.Example = "";
                word.Synonyms = "";
                currentList = word.List;
                currentGrade = word.Grade;
            }
            return RedirectToAction("Create", word);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Word word = wordAccess.GetWordDetails(id);

            if (word == null)
            {
                return NotFound();
            }
            return View(word);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] Word word)
        {
            if (id != word.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                wordAccess.UpdateWord(word);
                return RedirectToAction("Index");
            }
            return View(word);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Word word = wordAccess.GetWordDetails(id);

            if (word == null)
            {
                return NotFound();
            }
            return View(word);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Word word = wordAccess.GetWordDetails(id);

            if (word == null)
            {
                return NotFound();
            }
            return View(word);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            wordAccess.DeleteWord(id);
            return RedirectToAction("Index");
        }

    }
}