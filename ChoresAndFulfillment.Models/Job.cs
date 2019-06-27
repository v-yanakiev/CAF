using ChoresAndFulfillment.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChoresAndFulfillment.Models
{
    public class Job
    {
        public Job()
        {
            this.JobState = JobState.Hiring;
            this.EmployerConfirmedFinished = false;
            this.WorkerConfirmedFinished = false;
        }
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(250)]
        public string Description { get; set; }
        public int JobCreatorId { get; set; }
        public EmployerAccount JobCreator { get; set; }
        [Required]
        [Range(1,25000)]
        public decimal PayUponCompletion { get; set; }
        public List<WorkerAccountApplication> Applicants { get; set; }
        public int? HiredUserId { get; set; }
        public WorkerAccount HiredUser { get; set; }
        public bool EmployerConfirmedFinished { get; set; }
        public bool WorkerConfirmedFinished{ get; set; }
        public JobState JobState { get; set; }
    }
}