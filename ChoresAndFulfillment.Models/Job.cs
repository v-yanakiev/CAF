﻿using ChoresAndFulfillment.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChoresAndFulfillment.Models
{
    public class Job
    {
        public Job()
        {
            this.JobState = JobState.Active;
            this.EmployerConfirmedFinished = false;
            this.WorkerConfirmedFinished = false;
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int JobCreatorId { get; set; }
        public EmployerAccount JobCreator { get; set; }
        [Required]
        public decimal PayUponCompletion { get; set; }
        public List<WorkerAccountApplication> Applicants { get; set; }
        public int? HiredUserId { get; set; }
        public WorkerAccount HiredUser { get; set; }
        public bool EmployerConfirmedFinished { get; set; }
        public bool WorkerConfirmedFinished{ get; set; }
        public JobState JobState { get; set; }
    }
}