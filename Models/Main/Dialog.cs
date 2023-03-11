using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace GoodJoker.Models
{
    public class Dialog
    {
        public int Id { get; set; }

        [DefaultValue(false)]
        public bool DelAuthor { get; set; }

        [DefaultValue(false)]
        public bool DelInvitation { get; set; }

        public int AuthorId { get; set; }
        public int InvitationId { get; set; }

        [ForeignKey("AuthorId")]
        [InverseProperty("AuthorDialog")]
        public virtual User Author { get; set; }

        [ForeignKey("InvitationId")]
        [InverseProperty("InvitationDialog")]
        public virtual User Invitation { get; set; }

        public virtual ICollection<MessageDialog> Messages { get; set; }

        public class DialogMapping : EntityTypeConfiguration<Dialog>
        {
            public DialogMapping()
            {
                HasRequired(d => d.Author).WithMany(u => u.AuthorDialog).HasForeignKey(u => u.AuthorId).WillCascadeOnDelete(false);
                HasRequired(d => d.Invitation).WithMany(u => u.InvitationDialog).HasForeignKey(u => u.InvitationId).WillCascadeOnDelete(false);
            }
        }
    }

    public class MessageDialog
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime DateCreate { get; set; }

        public bool View { get; set; }

        public virtual Dialog Dialog { get; set; }
        
        public virtual User Author { get; set; }
    }
}