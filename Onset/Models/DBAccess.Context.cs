//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Onset.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ONSETDBEntities : DbContext
    {
        public ONSETDBEntities()
            : base("name=ONSETDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ADMIN> ADMINS { get; set; }
        public virtual DbSet<CHATMESSAGE> CHATMESSAGES { get; set; }
        public virtual DbSet<CHATROOM> CHATROOMS { get; set; }
        public virtual DbSet<EMPLOYEE> EMPLOYEES { get; set; }
        public virtual DbSet<EVENT> EVENTS { get; set; }
        public virtual DbSet<EVENTUSER> EVENTUSERS { get; set; }
        public virtual DbSet<MANAGER> MANAGERS { get; set; }
        public virtual DbSet<PROGRESSEMPLOYEE> PROGRESSEMPLOYEES { get; set; }
        public virtual DbSet<PROGRESS> PROGRESSES { get; set; }
        public virtual DbSet<PROGRESSFILE> PROGRESSFILES { get; set; }
        public virtual DbSet<REPORT> REPORTS { get; set; }
        public virtual DbSet<REPORTUSER> REPORTUSERS { get; set; }
        public virtual DbSet<ROOM> ROOMS { get; set; }
        public virtual DbSet<TASKEMPLOYEE> TASKEMPLOYEES { get; set; }
        public virtual DbSet<TASKFILE> TASKFILES { get; set; }
        public virtual DbSet<TASKMANAGER> TASKMANAGERS { get; set; }
        public virtual DbSet<TASKPROGRESS> TASKPROGRESSES { get; set; }
        public virtual DbSet<TASK> TASKS { get; set; }
        public virtual DbSet<USERBIO> USERBIOS { get; set; }
        public virtual DbSet<USERPIC> USERPICS { get; set; }
        public virtual DbSet<USER> USERS { get; set; }
    }
}
