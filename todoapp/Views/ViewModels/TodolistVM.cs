using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todoapp.Views.ViewModels
{
    public class TodolistVM
    {
        public int id { get; set; }
        public string Name { get; set; }
        public DateTime createdate { get; set; } = DateTime.Now;

        public int NotStartedCount { get; set; }
        public int Completed { get; set; }

        public int InProgresCount { get; set; }

    }
}
