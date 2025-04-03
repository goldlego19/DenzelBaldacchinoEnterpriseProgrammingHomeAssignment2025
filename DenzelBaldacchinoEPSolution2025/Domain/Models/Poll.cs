﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Poll
    {
        [Key]
        public int PollId { get; set; }

        public string Title {  get; set; }

        public string Option1Text { get; set; }
        public string Option2Text { get; set; }
        public string Option3Text { get; set; }
        public int Option1VotesCount { get; set; }
        public int Option2VotesCount { get; set; }
        public int Option3VotesCount { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<string> VotedUserIds { get; set; } = new List<string>();
    }
}
