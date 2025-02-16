using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRepository
    {
        public Label AddLabel(LabelModel model, int userId);

        public List<Label> GetAllLabels(int userId);

        public bool UpdateLabel(int userId, int labelId, string labelName);

        public bool DeleteLabel(int userId, int labelId);
    }
}