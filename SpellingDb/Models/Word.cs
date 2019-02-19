using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellingDb.Models
{
    public class Word
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PartOfSpeech { get; set; }
        [Required]
        public string Definition { get; set; }
        public string Example { get; set; }
        public string Synonyms { get; set; }
        [Required]
        public Byte Grade { get; set; }
        [Required]
        public string List { get; set; }
    }
}
