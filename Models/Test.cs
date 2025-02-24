﻿namespace WebServer.Models
{
    public class Test
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string Author { get; set; }
        public required string DataJson { get; set; }

        public List<Comment> Comments { get; set; } = new();
    }

    public class TestRequest
    {
        public required string Name { get; set; }
        public required string Author { get; set; }
        public required string DataJson { get; set; }
    }
}
