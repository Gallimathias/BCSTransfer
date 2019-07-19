using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BCSTransfer.Core.Model
{
    public class Setting
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
