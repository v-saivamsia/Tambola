using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tambola.Models
{
    public class Player
    {
        [Required]
        [MaxLength(20, ErrorMessage = "max length of the name cannot be more than 20 characters")]
        public string Name { get; set; }
        [Required]
        public int NumberOfTickets { get; set; } =1;

    }
    public enum SelectNumberOfTickets
    {
        one = 1, two, three, four, five, six
    }
}
