namespace Sales.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        [Display(Name = "Imagen del producto")]
        public string ImagePath { get; set; }

        [DisplayFormat(DataFormatString ="{0:C2}", ApplyFormatInEditMode =false)]
        public Decimal Price { get; set; }

        [Display(Name ="Esta en exitencia")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Publicado")]
        [DataType(DataType.Date)]
        public DateTime PublishOn { get; set; }

        public override string ToString()
        {
            return this.Description;
        }
    }
}
