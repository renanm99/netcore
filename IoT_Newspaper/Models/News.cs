namespace IoT_Newspaper.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class News
    {
        public Guid Id { get; set; }

        public Guid Section_Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        public DateTime Date { get; set; }

        [StringLength(100)]
        public string Categories { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        public bool Permanent { get; set; }

        public bool Disabled { get; set; }

        public virtual Section Section { get; set; }
    }
}
