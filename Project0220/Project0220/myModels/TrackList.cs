using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Project0220.myModels
{
    public class TrackList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrackID { get; set; }

 
        public int CustomerID { get; set; }

    
        public int ProductID { get; set; }
    }
}
