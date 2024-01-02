using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        [HttpPost(Name = "Create")]
        public ActionResult Create([FromBody] BookCreationModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model?.Titles))
                {
                    return BadRequest("Titles cannot be empty.");
                }

                // Check if a book with the same title already exists
                if (BookWithTitleExists(model.Titles))
                {
                    return BadRequest($"A book with the title '{model.Titles}' already exists.");
                }

                int index = GetNextIndex();
                var path = @".\Books\{0} - {1}.txt";
                var pathWithTitles = string.Format(path, index, model.Titles);

                using (FileStream fs = new FileStream(pathWithTitles, FileMode.Create))
                {
                    byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(model.Content);
                    fs.Write(contentBytes, 0, contentBytes.Length);
                }

                return Ok("Book created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating book: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        private bool BookWithTitleExists(string title)
        {
            var dir = @".\Books\";
            var existingFiles = Directory.GetFiles(dir, "*.txt");

            foreach (var filePath in existingFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var existingTitle = fileName.Split('-').Last().Trim();
                if (existingTitle.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private int GetNextIndex()
        {
            var dir = @".\Books\";
            int currentIndex = Directory.GetFiles(dir, "*.txt").Length + 1;

            return currentIndex;
        }






        [HttpGet("", Name = "books")]
        public List<string> GetTitles()
        {
            var books = GetBooksTitles();

            return books;
        }

        private List<string> GetBooksTitles()
        {
            var path = @".\Books";

            string[] books = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            List<string> _books = new List<string>();

            foreach (var book in books)
            {

                var title = book.Replace(path, string.Empty).TrimStart('\\').TrimEnd(".txt".ToCharArray());

                _books.Add(title);
            }

            return _books;
        }


        [HttpGet("{id}", Name = "GetTopTenWords")]
        public List<KeyValuePair<string, int>> GetTopTenWords(int id)
        {
            return GetTopWords(id);
        }

        private List<KeyValuePair<string, int>> GetTopWords(int id)
        {
            var words = ReadWordsFromFile(id);
            var topTenWords = GetOcurrentWords(words);

            return topTenWords;
        }
        private string[] ReadWordsFromFile(int id)
        {
            var path = @".\\Books";
            string[] books = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);

            
            var matchingFile = books.FirstOrDefault(file =>
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                return fileNameWithoutExtension.StartsWith(id.ToString() + " - ");
            });

            if (matchingFile == null)
            {
                throw new FileNotFoundException($"File with ID {id} not found.");
            }

            var filePath = matchingFile;

            string content;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                byte[] contentBytes = new byte[fs.Length];
                fs.Read(contentBytes, 0, contentBytes.Length);
                content = System.Text.Encoding.UTF8.GetString(contentBytes);
            }

            var words = content.Split(new char[] { ' ', '\t', '\n', '\r' , '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '{', '}', '[', ']', '|', '\\', ';', ':', '\'', '"', ',', '.', '/', '<', '>', '?' }, StringSplitOptions.RemoveEmptyEntries);

            return words;
        }

        private List<KeyValuePair<string, int>> GetOcurrentWords(string[] words)
        {
            var wordCount = new Dictionary<string, int>();

            foreach (string word in words)
            {
                if (word.Length > 5)
                {
                    string cleanedWord = word.ToLower();
                    if (wordCount.ContainsKey(cleanedWord))
                    {
                        wordCount[cleanedWord]++;
                    }
                    else
                    {
                        wordCount[cleanedWord] = 1;
                    }
                }
            }

            var _words = wordCount.OrderByDescending(w => w.Value).Take(10);

            var topTenWords = new List<KeyValuePair<string, int>>();
            foreach (var word in _words)
            {
                topTenWords.Add(new KeyValuePair<string, int>(word.Key, word.Value));
            }

            return topTenWords;
        }


        [HttpGet("{id}/{word}", Name = "IsMatchingWord")]
        public bool IsMatchingWord(int id, string word)
        {
            var topTenWords = GetTopWords(id);

            // Check if the provided word is in the list of top ten words
            return topTenWords.Any(wordInfo => wordInfo.Key.ToLower() == word.ToLower());
        }

        [HttpGet("{id}/search/{subWord}", Name = "FindSubstring")]
        public List<KeyValuePair<string, int>> FindSubstringInWords(int id, string subWord)
        {
            if (subWord.Length < 3)
            {
                throw new ArgumentException("The subword must have a minimum length of 3 characters.", nameof(subWord));
            }

            string[] words;
            try
            {
                words = ReadWordsFromFile(id);
            }
            catch (FileNotFoundException ex)
            {
                throw new ArgumentException(ex.Message);
            }

            var matchingWords = new Dictionary<string, int>();

            foreach (var word in words)
            {
                if (word.ToLower().StartsWith(subWord.ToLower()))
                {
                    string cleanedWord = word.ToLower();
                    if (matchingWords.ContainsKey(cleanedWord))
                    {
                        matchingWords[cleanedWord]++;
                    }
                    else
                    {
                        matchingWords[cleanedWord] = 1;
                    }
                }
            }

            var result = matchingWords.OrderByDescending(w => w.Value)
                                .Select(w => new KeyValuePair<string, int>(w.Key, w.Value))
                                .ToList();

            return result;
        }


    }
}