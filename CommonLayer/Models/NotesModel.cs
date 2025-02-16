using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class NotesModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Reminder { get; set; }

        public string color { get; set; }

        public string image { get; set; }

        public bool IsArchive { get; set; }

        public bool IsPin { get; set; }

        public bool IsTrash { get; set; }

        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
    }
}