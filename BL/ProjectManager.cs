using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;
using Domain.Projects;
using Domain.Users;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BL
{
    public class ProjectManager
    {
        // Added by NG
        // Modified by NVZ
        private ProjectRepository ProjectRepo { get; set; }
        public ModuleManager ModuleMan { get; }

        // Added by NG
        // Modified by NVZ
        public ProjectManager()
        {
            ProjectRepo = new ProjectRepository();
            ModuleMan = new ModuleManager();
        }

        // Added by NG
        // Modified by NVZ & XV
        //Project 

        #region

        /*
        * Setter method, we might need this for certain properties but
        * certainly not all of them. Please make a difference between
        * properties you need and the ones you do not. - NVZ
        * 
        */
        public Project MakeProject(Project project)
        {
            Project newProject = ProjectRepo.Create(project);

            List<Phase> savedPhases = new List<Phase>();

            foreach (var phase in project.Phases)
            {
                phase.Project = newProject;
                Phase savedPhase = MakePhase(phase, newProject.Id);
                savedPhases.Add(savedPhase);
                ProjectRepo.Update(newProject);
            }

            newProject.Phases = savedPhases;


            // ProjectRepo.

            return GetProject(newProject.Id, false);
        }

        public void EditProject(Project project)
        {
            ProjectRepo.Update(project);
        }

        /*
         * Simple getter to get information about our Project. - NVZ
         */


        public Project GetProject(int projectId, bool details)
        {
            Project project = ProjectRepo.Read(projectId, details);

            List<Phase> phases = new List<Phase>();
            phases = ProjectRepo.ReadAllPhases(projectId).ToList();

            project.Phases = phases;

            return project;
        }

        /*
         * Might need this for initialisation. - NVZ
         */

        public void RemoveProject(int projectId)
        {
            ProjectRepo.Delete(projectId);
        }

        public IEnumerable<Project> GetAllProjectsForPlatform(int platformId)
        {
            return ProjectRepo.ReadAllForPlatform(platformId);
        }

        #endregion

        // Added by NG
        // Modified by NVZ
        //Phase 

        #region

        /*
        * Setter method, we might need this for certain properties but
        * certainly not all of them. Please make a difference between
        * properties you need and the ones you do not. - NVZ
        * 
        */
        public void EditPhase(Phase phase)
        {
            ProjectRepo.Update(phase);
        }

        /*
         * Might need this for initialisation - NVZ
         * 
         */
        public IEnumerable<Phase> GetAllPhases(int projectId)
        {
            return ProjectRepo.ReadAllPhases(projectId);
        }

        public Phase MakePhase(Phase newPhase, int projectId)
        {
            Phase savedPhase = ProjectRepo.Create(newPhase);
            var alteredProject = ProjectRepo.Read(projectId, false);
            alteredProject.Phases.Add(savedPhase);
            ProjectRepo.Update(alteredProject);
            if (newPhase.Module != null)
            {
                var moduleType = newPhase.Module.GetType() == typeof(Questionnaire);
                var alteredModule = ModuleMan.GetModule(newPhase.Module.Id, false, moduleType);
                alteredModule.Phases.Add(newPhase);
                ModuleMan.EditModule(alteredModule);
            }

            return savedPhase;
        }


        public void RemovePhase(int projectId, int phaseId)
        {
            var removedPhase = ProjectRepo.ReadPhase(phaseId, false);
            var alteredProject = ProjectRepo.Read(projectId, false);
            alteredProject.Phases.Remove(removedPhase);
        
            if (removedPhase.Module != null)
            {
                var moduleType = removedPhase.Module.GetType() == typeof(Questionnaire);
                var alteredModule = ModuleMan.GetModule(removedPhase.Module.Id, false, moduleType);
                alteredModule.Phases.Remove(removedPhase);
                ModuleMan.EditModule(alteredModule);
            }
            else
            {
                ProjectRepo.DeletePhase(removedPhase.Id);
            }

            //TODO: Enkel de phase moet verwijdert worden project niet? 
            // ProjectRepo.Delete(projectId);
        }

        public Phase GetPhase(int phaseId)
        {
            return ProjectRepo.ReadPhase(phaseId, false);
        }

        #endregion

        // Added by NVZ
        // Other Methods

        #region

        /*
        private bool VerifyProjectEditable(int projectId)
        {
            throw new NotImplementedException("Out of scope!");
        } */

        /*
         *  In case we want to show the projectpage for the POC. -NVZ
         
        public List<Module> GetModules(int projectId, bool details)
        {
            throw new NotImplementedException("I might need this!");
        } */

        /*
         * We have two options with this method:
         * 
         * 1. Either any call to this class is via this method.
         * 2. Either only calls outside of the ProjectController are for
         * this method so that it can delegate to the moduleManager
         * if it can't solve the problem.
         *
         * This method is conceived to be modular towards microservices,
         * if we have the time I'll explain why. - NVZ
         *
        public void HandleProjectAction(int projectId, string actionName)
        {
            throw new NotImplementedException("I need this!");
        } */

        #endregion

        // Added by XV
        // Methods for Platform

        #region PlatformMethods

        public IEnumerable<Project> GetPlatformProjects(Platform platform)
        {
            return ProjectRepo.ReadAllForPlatform(platform.Id);
        }

        #endregion
    }
}