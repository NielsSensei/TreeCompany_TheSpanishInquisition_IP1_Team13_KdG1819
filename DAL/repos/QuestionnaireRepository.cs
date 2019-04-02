using System;
using System.Collections.Generic;
using System.Data;
using Domain;

namespace DAL
{
    public class QuestionnaireRepository : IRepository<Questionnaire>
    {
        // Added by DM
        private List<Questionnaire> questionnaires;
        
        // Added by NVZ
        // Modified by XV
        public QuestionnaireRepository()
        {
            Seed seed = new Seed();
            questionnaires = new List<Questionnaire>();
            questionnaires.Add(seed.questionnaire);
        }

        // Added by NVZ
        // Questionnaire CRUD
        #region
        public Questionnaire Create(Questionnaire obj)
        {
            if (!questionnaires.Contains(obj))
            {
                questionnaires.Add(obj);
            }
            throw new DuplicateNameException("This Questionnaire already exists!");
        }
        
        public Questionnaire Read(int id)
        {
            Questionnaire q = questionnaires.Find(qu => qu.Id == id);
            if (q != null)
            {
                return q;
            } else throw new KeyNotFoundException("This Questionnaire can't be found!");

        }

        public void Update(Questionnaire obj)
        {
            Delete(obj.Id);
            Create(obj);
        }

        public void Delete(int id)
        {
            Questionnaire q = Read(id);
            if (q != null)
            {
                questionnaires.Remove(q);
            }
        }
        
        public IEnumerable<Questionnaire> ReadAll()
        {
            return questionnaires;
        }

        public IEnumerable<Questionnaire> ReadAll(int projectID)
        {
            return questionnaires.FindAll(q => q.ParentPhase.Project.Id == projectID);
        }
        #endregion   
    }
}