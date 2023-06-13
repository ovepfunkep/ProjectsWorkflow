﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
