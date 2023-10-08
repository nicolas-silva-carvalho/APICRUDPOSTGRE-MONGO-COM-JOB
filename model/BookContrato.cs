using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.Models;

namespace teste.model;

public class BookContrato
{
    public int Id { get; set; }
    public string Erro { get; set; }
    public DateTime DateTime { get; set; }
    public int IdBook { get; set; }
    public virtual Book Book { get; set; }
}
