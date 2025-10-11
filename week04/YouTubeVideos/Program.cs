using System;
using System.Collections.Generic;


namespace YoutubeTracker
{
    public class Comment
    {
        private string _author;
        private string _text;
        public string Author => _author;
        public string Text => _text;
        public Comment(string author, string text)
        {
            _author = author ?? throw new ArgumentNullException(nameof(author));
            _text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public class Video
        {

            private string _title;
            private string _author;
            private int _lengthInSeconds;

            private readonly List<Comment> _comments = new List<Comment>();

            public string Title
            {
                get => _title;
                set => _title = value ?? throw new ArgumentNullException(nameof(value));
            }

            public string Author
            {
                get => _author;
                set => _author = value ?? throw new ArgumentNullException(nameof(value));
            }

            public int LengthSeconds
            {
                get => _lengthInSeconds;
                set
                {
                    if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                    _lengthInSeconds = value;
                }
            }

            public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

            public Video(string title, string author, int lengthSeconds)
            {
                Title = title;
                Author = author;
                LengthSeconds = lengthSeconds;
            }

            public void AddComment(Comment comment)
            {
                if (comment is null) throw new ArgumentNullException(nameof(comment));
                _comments.Add(comment);
            }

            public int GetCommentCount() => _comments.Count;

            public string GetFormattedLength()
            {
                var ts = TimeSpan.FromSeconds(LengthSeconds);
                return ts.Hours > 0 ? ts.ToString(@"hh\:mm\:ss") : ts.ToString(@"mm\:ss");

            }
        }
        internal class Program
        {
            private static void Main()
            {
                var videos = new List<Video>
                {
                    new Video("Intro to PTT with ECG & PPG", "BioSignals Lab", 485),
                    new Video("C# OOP: Abstraction Explained", "CodeAcademyX", 732),
                    new Video("ESP32 + MQTT Real-Time Dashboard", "AdriánDC", 615),
                    new Video("Django Channels: Live Charts", "DevClinic", 401)
                };

                videos[0].AddComment(new Comment("Maria", "Excellent explanation of the PTT concept!"));
                videos[0].AddComment(new Comment("Luis", "Helped me understand the ECG-PPG synchronization."));
                videos[0].AddComment(new Comment("Carla", "Could you upload the sample code?"));

                videos[1].AddComment(new Comment("Pedro", "Now the abstraction is clear to me."));
                videos[1].AddComment(new Comment("Nadia", "Difference between abstraction and encapsulation?"));
                videos[1].AddComment(new Comment("Gustavo", "Good pace and practical examples."));
                videos[1].AddComment(new Comment("Sofia", "Thanks for the class-responsibility diagram."));

                videos[2].AddComment(new Comment("Ivan", "What is the total latency of the pipeline?"));
                videos[2].AddComment(new Comment("Marcos", "Great use of MQTT and separate topics."));
                videos[2].AddComment(new Comment("Elena", "Can you show the WebSockets part?"));

                videos[3].AddComment(new Comment("Roxana", "Plotly in real time was fantastic."));
                videos[3].AddComment(new Comment("Fabian", "How do you handle client reconnections?"));
                videos[3].AddComment(new Comment("Andrea", "More examples with multiple series, please."));

                foreach (var v in videos)
                {
                    Console.WriteLine(new string('=', 60));
                    Console.WriteLine($"Title : {v.Title}");           
                    Console.WriteLine($"Author: {v.Author}");
                    Console.WriteLine($"Length: {v.GetFormattedLength()} ({v.LengthSeconds} s)");
                    Console.WriteLine($"Comments count: {v.GetCommentCount()}");
                    Console.WriteLine("Comments:");
                    int idx = 1;
                    foreach (var c in v.Comments)
                    {
                        Console.WriteLine($"  {idx++}. {c.Author}: {c.Text}");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine(new string('=', 60));
                Console.WriteLine("End. Press ENTER to exit...");
                Console.ReadLine();
            }

        }
    }
}


