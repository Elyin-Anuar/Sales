﻿namespace Sales.Common.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int CategoryId { get; set; } /*Key*/

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

        [Required]
        [StringLength(128)]
        public string UserId { get; set; } /*Key ASP.Net User*/

        public double Latitud { get; set; } /*Mapa*/
        public double Longitud { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "images";
                }
              return $"https://salesapi20190306034232.azurewebsites.net{this.ImagePath.Substring(1)}";
            }
        }
        public override string ToString()
        {
            return this.Description;
        }
    }
}
