using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace WebApplication1.Models
{
    public class ExportModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets CSV file.
        /// </summary>
        [Required]
        [Display(Name = "Import CSV File")]
        public HttpPostedFileBase FileAttach { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether CSV has header or not.
        /// </summary>
        [Required]
        [Display(Name = "Has Header")]
        public bool HasHeader { get; set; }

        /// <summary>
        /// Gets or sets Data table.
        /// </summary>
        public DataTable Data { get; set; }

        #endregion
    }
}