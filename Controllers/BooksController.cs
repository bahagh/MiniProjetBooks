using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

                var path = @"C:\\Users\\Asus\\Downloads\\New folder\\Books\\{0}.txt";
                var pathWithTitles = string.Format(path, model.Titles);

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

        public class BookCreationModel
        {
            public string Content { get; set; }
            public string Titles { get; set; }
        }


        [HttpGet("GetTitles", Name = "GetTitles")]
        public List<string> GetTitles()
        {
            var books = GetBooksTitles();

            return books;
        }

        private List<string> GetBooksTitles()
        {
            var path = @"C:\\Users\\Asus\\Downloads\\New folder\\Books";

            string[] books = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            List<string> _books = new List<string>();

            foreach (var book in books)
            {

                _books.Add(book.Replace(path, string.Empty));
            }

            return _books;
        }


        [HttpGet("GetTopTenWords/{id}", Name = "GetTopTenWords")]
        public List<string> GetTopTenWords(int id)
        {
            return GetTopWords(id);
        }
        private List<string> GetTopWords(int id)
        {
            var words = ReadWordsFromFile(id);
            var topTenWords = GetOcurrentWords(words);

            return topTenWords;
        }
        private string[] ReadWordsFromFile(int id)
        {
            var path = @"C:\\Users\\Asus\\Downloads\\New folder\\Books";
            string[] books = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);
            var filePath = books[id];

            string content;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                byte[] contentBytes = new byte[fs.Length];
                fs.Read(contentBytes, 0, contentBytes.Length);
                content = System.Text.Encoding.UTF8.GetString(contentBytes);

            }

            var words = content.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            return words;
        }
        private List<string> GetOcurrentWords(string[] words)
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

            var topTenWords = new List<string>();
            foreach (var word in _words)
            {
                topTenWords.Add(word.Key);
            }

            return topTenWords;
        }


        [HttpGet("IsMatchingWord/{id}/{word}", Name = "IsMatchingWord")]
        public bool IsMatchingWord(int id, string word)
        {
            var topTenWords = GetTopWords(id);
            foreach (var _word in topTenWords)
            {
                if (_word.ToLower() == word.ToLower())
                {
                    return true;
                }
            }

            return false;
        }

        [HttpGet("FindSubstring/{id}/{subWord}", Name = "FindSubstring")]
        public List<string> FindSubtringInWords(int id, string subWord)
        {
            var _words = new List<string>();

            var words = ReadWordsFromFile(id);
            foreach (var word in words)
            {
                if (word.Contains(subWord))
                {
                    _words.Add(word);
                }
            }
            return _words;
        }

    }
}