using System;
using System.Collections.Generic;
using Domain.UserInput;

namespace Domain.Users
{
    public class User
    {
        // Added by NG
        // Modified by XV & NVZ
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ZipCode { get; set; }
        public Role Role { get; set; }
        public bool Banned { get; set; }
        public bool Gender { get; set; }
        public bool Active { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<Idea> Ideas { get; set; }
        // NOTE about Interactions. While you would often think the more the merrier this is not the
        // case here, Tree Company specifically asked that the interactions between devices and
        // users should be as anonymous as possible thus we do not want to make this traceable at all.
        // As 'DBA' for the project I think that persisting Interactions as a whole would be quite useless,
        // would love feedback on this. - NVZ
        // public ICollection<Interaction> Interactions { get; set; }
        public int platformID { get; set; }
        
        // Added by EKT
        // Modified by NVZ
        // Methods
        #region
        public User GetUserInfo()
        {
            User info = new User()
            {
                Name = this.Name,
                ZipCode = this.ZipCode,
                Role = this.Role,
                Banned = this.Banned,
                Gender = this.Gender,
                BirthDate = this.BirthDate
            };
            return info;
        }

        public void SetActive(bool active)
        {
            Active = active;
        }

        public void SetBanned(bool banned)
        {
            Banned = banned;
        }

        public void UpdateRole(Role role)
        {
            Role = role;
        }

        public void addAnswer(Answer answer)
        {
            Answers.Add(answer);
        }

        public void addIdea(Idea idea)
        {
            Ideas.Add(idea);
        }
        #endregion
    }
}