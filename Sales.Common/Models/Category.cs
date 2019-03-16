namespace Sales.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        [Display(Name ="Image")]
        public string ImagePath { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; } /*Key*/
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "noproduct";
                }
                return $"https://salesunanmanagua2019.azurewebsites.net{this.ImagePath.Substring(1)}";
            }
        }
    }
}
