using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;


namespace Backend.Models.Request
{

    public class Page
    {
        [Required]
        public int pageSize { get; set; }
        [Required]
        public int pageIndex { get; set; }
    }

}
