using System;

namespace bdotnet
{
    public class Task {
        public Guid Id { get; set; }
        public int? Order { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool? Completed { get; set; }
    }
}