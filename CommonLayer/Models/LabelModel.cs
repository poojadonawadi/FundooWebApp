using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;

namespace CommonLayer.Models
{
    public class LabelModel
    {
        public int LabelId { get; set; }

        public string LabelName { get; set; }

        public int NotesId { get; set; }

    }
}