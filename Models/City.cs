using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesManager.WebAPI.Models
{
    public class City
    {
        [Key]
        public Guid CityId { get; set; }
        public string? CityName { get; set; }
    }
}
