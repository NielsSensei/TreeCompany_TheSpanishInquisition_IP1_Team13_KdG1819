using System.Collections.Generic;
using System.Data;
using System.Linq;
using Domain.UserInput;

namespace DAL.repos
{
    public class IdeationQuestionsRepository : IRepository<IdeationQuestion>
    {
        // Added by DM
        // Modified by NVZ
        private List<IdeationQuestion> ideationQuestions;
        private List<Idea> ideas;
        
        // Added by NVZ
        public IdeationQuestionsRepository()
        {
            //TODO: Initalisatie
        }
        
        // Added by NVZ
        // IdeationQuestion CRUD
        #region
        public IdeationQuestion Create(IdeationQuestion obj)
        {
            if (!ideationQuestions.Contains(obj))
            {
                ideationQuestions.Add(obj);
                return obj; 
            }
            throw new DuplicateNameException("This IdeationQuestion already exist!");
        }
        
        public IdeationQuestion Read(int id)
        {
            IdeationQuestion iq = ideationQuestions.Find(q => q.Id == id);
            if (iq != null)
            {
                return iq;
            }
            throw new KeyNotFoundException("This IdeationQuestion can't be found!");
        }

        public void Update(IdeationQuestion obj)
        {
            Delete(obj.Id);
            Create(obj);
        }

        //TODO: set op [deleted]
        public void Delete(int id)
        {
            IdeationQuestion iq = Read(id);
            if (iq != null)
            {
                ideationQuestions.Remove(iq); 
            }  
        }

        public IEnumerable<IdeationQuestion> ReadAll()
        {
            return ideationQuestions;
        }
        #endregion
     
        // Added by NVZ
        // Idea CRUD
        #region
        public Idea Create(Idea idea)
        {
            if (!ideas.Contains(idea))
            {
                ideas.Add(idea);
                IdeationQuestion iq = Read(idea.Id);
                iq.Ideas.Add(idea);
                Update(iq);
                return idea; 
            }
            throw new DuplicateNameException("This Idea already exist!");
        }

        public Idea Read(int questionID, int ideaID)
        {
            Idea i = Read(questionID).Ideas.First(idea => idea.Id == ideaID);
            if (i != null)
            {
                return i;
            }
            throw new KeyNotFoundException("This Idea can't be found!");
        }

        public Idea ReadIdea(int IdeaId)
        {
            Idea i = ideas.Find(idea => idea.Id == IdeaId);
            if (i != null)
            {
                return i;
            }
            throw new KeyNotFoundException("This Idea can't be found!");
        }
        
        public void Update(Idea obj)
        {
            Delete(ideationQuestions.Find(iq => iq.Ideas.Contains(obj)).Id, obj.Id);
            Create(obj);
        }

        //TODO: set op [deleted]
        public void Delete(int questionID, int ideaID)
        {
            Idea toDelete = Read(questionID, ideaID);
            if (toDelete != null)
            {
                ideas.Remove(toDelete);
                Read(questionID).Ideas.Remove(toDelete);
            }
        }

        public IEnumerable<Idea> ReadAll(int questionID)
        {
            return Read(questionID).Ideas;
        }

        public IEnumerable<Idea> ReadAllChilds(int parentId)
        {
            return ideas.FindAll(idea => idea.ParentIdea.Id == parentId);
        }
        #endregion
    }
}