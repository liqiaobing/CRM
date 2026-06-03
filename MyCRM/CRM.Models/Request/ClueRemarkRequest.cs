using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Models.Request
{
    public class ClueRemarkRequest
    {
        public int Id { get; set; }
        /// <summary>
        /// clueID
        /// </summary>
        public int ClueId { get; set; }

        /// <summary>
        /// 跟踪方式
        /// </summary>
        public int? NoteWay { get; set; }
        /// <summary>
        /// 跟踪内容
        /// </summary>
        public string? NoteContent { get; set; }

    }
}
