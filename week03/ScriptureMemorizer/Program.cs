using System;
using System.Collections.Generic;
using System.Linq;

namespace ScriptureMemorizer
{
    public class Reference
    {
        private readonly string _book;
        private readonly int _chapter;
        private readonly int _verse;
        private readonly int _endVerse;

        public Reference(string book, int chapter, int verse)
        {
            _book = book;
            _chapter = chapter;
            _verse = verse;
            _endVerse = verse;
        }

        public Reference(string book, int chapter, int startVerse, int endVerse)
        {
            _book = book;
            _chapter = chapter;
            _verse = startVerse;
            _endVerse = endVerse;
        }

        public string GetDisplayText()
        {
            return _endVerse == _verse
                ? $"{_book} {_chapter}:{_verse}"
                : $"{_book} {_chapter}:{_verse}-{_endVerse}";
        }
    }

    public class Word
    {
        private readonly string _text;
        private bool _isHidden;

        public Word(string text)
        {
            _text = text;
            _isHidden = false;
        }

        public void Hide()
        {
            _isHidden = true;
        }

        public void Show()
        {
            _isHidden = false;
        }

        public bool IsHidden()
        {
            return _isHidden;
        }

        public string GetDisplayText()
        {
            if (!_isHidden) return _text;
            var chars = _text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                if (char.IsLetter(chars[i])) chars[i] = '_';
            return new string(chars);
        }
    }

    public class Scripture
    {
        private readonly Reference _reference;
        private readonly List<Word> _words;
        private readonly Random _rng = new Random();

        public Scripture(Reference reference, string text)
        {
            _reference = reference;
            _words = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(t => new Word(t)).ToList();
        }

        public void HideRandomWords(int numberToHide)
        {
            if (numberToHide <= 0 || _words.Count == 0) return;
            for (int k = 0; k < numberToHide; k++)
            {
                int idx = _rng.Next(0, _words.Count);
                _words[idx].Hide();
            }
        }

        public string GetDisplayText()
        {
            string body = string.Join(' ', _words.Select(w => w.GetDisplayText()));
            return $"{_reference.GetDisplayText()}\n{body}";
        }

        public bool IsCompletelyHidden()
        {
            return _words.All(w => w.IsHidden());
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var reference = new Reference("Proverbs", 3, 5, 6);
            string text = "Trust in the Lord with all thine heart; and lean not unto thine own understanding. In all thy ways acknowledge him, and he shall direct thy paths.";
            var scripture = new Scripture(reference, text);

            const int wordsPerStep = 3;

            while (true)
            {
                Console.Clear();
                Console.WriteLine(scripture.GetDisplayText());
                Console.WriteLine();
                if (scripture.IsCompletelyHidden()) return;

                Console.Write("Press Enter to continue or type 'quit' to finish: ");
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input) &&
                    input.Trim().Equals("quit", StringComparison.OrdinalIgnoreCase)) return;

                scripture.HideRandomWords(wordsPerStep);
            }
        }
    }
}