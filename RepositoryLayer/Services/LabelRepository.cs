using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class LabelRepository : ILabelRepository
    {
        private readonly FundooDBContext context;
        public LabelRepository(FundooDBContext context)
        {
            this.context = context;
        }

        public Label AddLabel(LabelModel model, int userId)
        {
            //var labelexist = context.Labels.FirstOrDefault(x => x.LabelName == model.LabelName);
            var labelExist = context.Label.FirstOrDefault(x => x.LabelName == model.LabelName && x.UserId == userId && x.NotesId == model.NotesId);
            if (labelExist == null)
            {
                Label label = new Label();
                label.LabelName = model.LabelName;
                label.UserId = userId;
                label.NotesId = model.NotesId;
                context.Label.Add(label);
                context.SaveChanges();
                return label;
            }
            else
            {
                Label newLabel = new Label();
                newLabel.LabelName = labelExist.LabelName;
                newLabel.NotesId = model.NotesId;
                newLabel.UserId = userId;
                context.Add(newLabel);
                context.SaveChanges();
                return newLabel;
            }
        }

        public List<Label> GetAllLabels(int userId)
        {
            try
            {
                List<Label> labelList = new List<Label>();
                var labels = context.Label.Select(x => x).Where(y => y.UserId == userId);
                if (labels != null)
                {
                    foreach (Label label in labels)
                    {
                        labelList.Add(label);
                    }
                    return labelList;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateLabel(int userId, int labelId, string labelName)
        {
            try
            {
                var label = context.Label.FirstOrDefault(x => x.UserId == userId && x.LabelId == labelId);
                if (label != null)
                {
                    label.LabelName = labelName;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteLabel(int userId, int labelId)
        {
            try
            {
                var label = context.Label.FirstOrDefault(x => x.UserId == userId && x.LabelId == labelId);
                if (label != null)
                {
                    context.Remove(label);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}