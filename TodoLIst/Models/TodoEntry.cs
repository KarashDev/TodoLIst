using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoLIst.Models
{
    public class TodoEntry
    {
        public int Id { get; set; }

        public string Text { get; set; }
        public bool isActive { get; set; }
    }
}
