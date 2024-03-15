using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project0220.myModels
{
    public class TrackList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主鍵值是由資料庫自動產生的，通常是透過自動增量（例如自動增長的整數）來生成的。
        public int TrackID { get; set; }

        public int CustomerID { get; set; }

        public int ProductID { get; set; }
    }
}
