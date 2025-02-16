using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace ManagerLayer.Services
{
    public class LabelManager : ILabelManager
    {
        public readonly ILabelRepository label;
        public readonly FundooDBContext context;

        public LabelManager(ILabelRepository label, FundooDBContext context)
        {
            this.label = label;
            this.context = context;
        }

        public Label AddLabel(LabelModel model, int userId)
        {
            return label.AddLabel(model, userId);
        }

        public List<Label> GetAllLabels(int userId)
        {
            return label.GetAllLabels(userId);
        }

        public bool UpdateLabel(int userId, int labelId, string labelName)
        {
            return label.UpdateLabel(userId, labelId, labelName);
        }

        public bool DeleteLabel(int userId, int labelId)
        {
            return label.DeleteLabel(userId, labelId);
        }
    }
}